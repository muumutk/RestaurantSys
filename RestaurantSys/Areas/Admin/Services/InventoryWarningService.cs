using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;

namespace RestaurantSys.Areas.Admin.Services
{
    public class InventoryWarningService
    {
        private readonly RestaurantSysContext _context;
        public InventoryWarningService(RestaurantSysContext context)
        {
            _context = context;
        }

        public async Task<List<string>> CheckAndLogExpiringBatchesAsync(string employeeID)
        {
            var today = DateTime.Today;
            var warningDate = today.AddDays(5);

            var expiringBatches = await _context.StockBatch
                .Where(b => b.ExpiryDate >= today && b.ExpiryDate <= warningDate).ToListAsync();

            var warnings = new List<string>();

            foreach (var batch in expiringBatches)
            {
                bool alreadyWarned = await _context.StockBatchWarningLog
                    .AnyAsync(log => log.EmployeeID == employeeID && log.BatchID == batch.BatchID);

                if(!alreadyWarned)
                {
                    var item = await _context.Stock.FindAsync(batch.ItemID);
                    string itemName = item?.ItemName ?? "未知物品";

                    warnings.Add($"物品「{itemName}」的批次（編號：{batch.BatchNo}）將於 {batch.ExpiryDate:yyyy/MM/dd} 到期！");

                    _context.StockBatchWarningLog.Add(new Models.StockBatchWarningLog
                    {
                        EmployeeID = employeeID,
                        BatchID = batch.BatchID,
                        WarningSentDate = DateTime.Now
                    });
                }
            }

            if(warnings.Any())
                await _context.SaveChangesAsync();

            return warnings;

        }

    }
}

using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;
using RestaurantSys.Models;

namespace RestaurantSys.Areas.Backend.Services
{
    public class EmployeeService
    {
        private readonly RestaurantSysContext _context;

        public EmployeeService(RestaurantSysContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 非同步新增員工，包含自動產生編號的邏輯。
        /// </summary>
        /// <param name="newEmployee">待新增的員工物件。</param>
        public async Task AddEmployeeAsync(Employee newEmployee)
        {
            // 核心業務邏輯：生成員工編號
            string newEmployeeId = await GenerateEmployeeIDAsync(newEmployee.HireDate);
            newEmployee.EmployeeID = newEmployeeId;

            // 非同步資料庫操作
            await _context.Employee.AddAsync(newEmployee);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 私有方法：非同步產生員工編號。
        /// </summary>
        private async Task<string> GenerateEmployeeIDAsync(DateTime hireDate)
        {
            string year = hireDate.ToString("yy");
            string month = hireDate.ToString("MM");
            string prefix = $"E{year}{month}";

            string latestEmployeeID = await _context.Employee
                                                    .Where(e => e.EmployeeID.StartsWith(prefix))
                                                    .OrderByDescending(e => e.EmployeeID)
                                                    .Select(e => e.EmployeeID)
                                                    .FirstOrDefaultAsync();

            int serialNumber = 1;
            if (!string.IsNullOrEmpty(latestEmployeeID))
            {
                string serialPart = latestEmployeeID.Substring(prefix.Length);
                if (int.TryParse(serialPart, out int currentSerial))
                {
                    serialNumber = currentSerial + 1;
                }
            }

            return $"{prefix}{serialNumber.ToString("D3")}";
        }
    }
}

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;
using RestaurantSys.Models;
using System.Data;

namespace RestaurantSys.Areas.Admin.Services
{
    public class EmployeeService
    {
        private readonly RestaurantSysContext _context;

        public EmployeeService(RestaurantSysContext context)
        {
            _context = context;
        }

        public async Task CreateEmployeeAsync(Employee newEmployee)
        {
            var hireDateParam = new SqlParameter("@HireDate", newEmployee.HireDate);
            var newEmployeeIDParam = new SqlParameter
            {
                ParameterName = "@NewEmployeeID",
                SqlDbType = SqlDbType.VarChar,
                Size = 20,
                Direction = ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                //OUT 關鍵字明確標示 @NewEmployeeID 是一個輸出參數
                "EXEC GetNextEmployeeID @HireDate, @NewEmployeeID OUT",
                //將前面建立的兩個參數傳入命令中
                hireDateParam, newEmployeeIDParam);

            string newEmployeeID = (string)newEmployeeIDParam.Value;

            //將從資料庫預存程序中獲得的新員工編號，賦值給 newEmployee 物件的 EmployeeID 屬性
            newEmployee.EmployeeID = newEmployeeID;

            await _context.Employee.AddAsync(newEmployee);
            await _context.SaveChangesAsync();
        }

    }
}
      
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;
using RestaurantSys.Areas.Admin.Services;
using RestaurantSys.Models;
using RestaurantSys.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace RestaurantSys.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "EmployeeLogin")]
    public class EmployeesController : Controller
    {
        private readonly RestaurantSysContext _context;

        private readonly EmployeeService _employeeService;

        public EmployeesController(RestaurantSysContext context , EmployeeService employeeService)
        {
            _context = context;
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Employee.ToListAsync());
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Backend/Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EName,EmployeeTel,Address,Birthday,HireDate,IsEmployed,EEmail,Password")] Employee newEmployee)
        {
            ModelState.Remove("EmployeeID");

            if (!ModelState.IsValid)
            {
                return View(newEmployee);
            }
            
            try
            {
                newEmployee.Password = HashService.HashPasswordSHA256(newEmployee.Password);

                await _employeeService.AddEmployeeAsync(newEmployee);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("","無法新增員工: " + ex.Message);
                return View(newEmployee);
            }

        }

        // GET: Backend/Employees/Edit/5
        public async Task<IActionResult> Edit(string id , DateTime? employeeBDay)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("EmployeeID,EName,EmployeeTel,Address,Birthday,HireDate,IsEmployed,EEmail,Password")] Employee employee)
        //{
        //    if (id != employee.EmployeeID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            // 從資料庫載入現有員工資料，以確保密碼不會被清空
        //            var existingEmployee = await _context.Employee.AsNoTracking().FirstOrDefaultAsync(e => e.EmployeeID == id);
        //            if (existingEmployee == null)
        //            {
        //                return NotFound();
        //            }

        //            if (!string.IsNullOrEmpty(employee.Password))
        //            {
        //                existingEmployee.Password = HashService.HashPasswordSHA256(employee.Password);
        //            }
        //            else
        //            {
        //                // 如果沒有新密碼，就保留舊的雜湊密碼
        //                employee.Password = existingEmployee.Password;
        //            }

        //            // 更新其他欄位
        //            _context.Entry(existingEmployee).CurrentValues.SetValues(employee);
        //            _context.Update(employee);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!EmployeeExists(employee.EmployeeID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(employee);
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("EmployeeID,EName,EmployeeTel,Address,HireDate,IsEmployed,EEmail")] Employee employee)
        {
            if (id != employee.EmployeeID)
            {
                return NotFound();
            }

            //不驗證密碼
            ModelState.Remove("Password");

            // 檢查模型驗證是否通過
            if (ModelState.IsValid)
            {
                try
                {
                    // 從資料庫中載入現有的員工實體。
                    // 使用 FindAsync()，它會追蹤物件，並返回一個完整的物件。
                    var existingEmployee = await _context.Employee.FindAsync(id);
                    if (existingEmployee == null)
                    {
                        return NotFound();
                    }

                    // 將表單提交的屬性值複製到現有實體上。
                    // 這裡只會複製在 [Bind] 中指定的欄位，
                    // 且不會影響 existingEmployee 的 Birthday 和 Password。
                    existingEmployee.EName = employee.EName;
                    existingEmployee.EmployeeTel = employee.EmployeeTel;
                    existingEmployee.Address = employee.Address;
                    existingEmployee.HireDate = employee.HireDate;
                    existingEmployee.IsEmployed = employee.IsEmployed;
                    existingEmployee.EEmail = employee.EEmail;

                    // 更新追蹤中的實體，並儲存變更。
                    // existingEmployee 已經被追蹤且其狀態會被自動更新
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // 如果模型驗證失敗，返回視圖並傳回包含錯誤的物件
            return View(employee);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var employee = await _context.Employee.FindAsync(id);
            if (employee != null)
            {
                _context.Employee.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool EmployeeExists(string id)
        {
            return _context.Employee.Any(e => e.EmployeeID == id);
        }
    }
}

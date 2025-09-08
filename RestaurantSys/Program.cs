using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;
using RestaurantSys.Areas.Admin.Services;
using RestaurantSys.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<RestaurantSysContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RestaurantSysConnection")));

builder.Services.AddDistributedMemoryCache(); // 添加分散式記憶體快取服務
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // 設定 Session 過期時間
    options.Cookie.IsEssential = true; // 讓 Session Cookie 成為必要的
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "MemberLogin";
})
.AddCookie("MemberLogin", options =>
{
    options.LoginPath = "/MemberLogin/Login";
    options.LogoutPath = "/Logout/Logout";
    options.AccessDeniedPath = "/Menu/Index";
})
.AddCookie("EmployeeLogin", options =>
{
    options.LoginPath = "/Admin/EmployeeLogin/Login";
    options.LogoutPath = "/Admin/EmployeeLogout/Logout";
    options.AccessDeniedPath = "/Admin/AdminHome/Index";
});



builder.Services.AddScoped<EmployeeService>();

builder.Services.AddScoped<InventoryWarningService>();

builder.Services.AddScoped<HashService>();


/////////////////////////////////////////////////////////////////
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


//app.MapControllerRoute(
//        name: "admin",
//        pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

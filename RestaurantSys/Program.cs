using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;
using RestaurantSys.Areas.Admin.Services;
using RestaurantSys.Areas.User.Services;
using RestaurantSys.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
        .AddJsonOptions(options =>
        {
                                                             //告訴序列化器，當它發現一個循環參考時，直接忽略它，不再繼續處理。
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

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

builder.Services.AddScoped<IOrderService, OrderService>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

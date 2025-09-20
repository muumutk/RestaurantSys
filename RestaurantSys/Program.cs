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
                                                             //�i�D�ǦC�ƾ��A���o�{�@�Ӵ`���ѦҮɡA�����������A���A�~��B�z�C
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

builder.Services.AddDbContext<RestaurantSysContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RestaurantSysConnection")));

builder.Services.AddDistributedMemoryCache(); // �K�[�������O����֨��A��
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // �]�w Session �L���ɶ�
    options.Cookie.IsEssential = true; // �� Session Cookie �������n��
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

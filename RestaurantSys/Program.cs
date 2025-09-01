using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;
using RestaurantSys.Areas.Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<RestaurantSysContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RestaurantSysConnection")));

builder.Services.AddAuthentication("MemberLogin").AddCookie("MemberLogin", options =>
{
    options.LoginPath = "/Login/Login"; 
    options.LogoutPath = "/Login/Logout"; 
    options.AccessDeniedPath = "/Menu/Index"; 
});


builder.Services.AddScoped<EmployeeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

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

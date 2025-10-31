var builder = WebApplication.CreateBuilder(args);
//Add services needed to handle MVC Controlers and Views
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.MapDefaultControllerRoute();
//The abolve line is equivalent to this:
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

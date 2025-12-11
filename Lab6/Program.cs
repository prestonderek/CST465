using Lab6.Config;
using Lab6.Data;
using Lab6.Logic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(
    "Config/BlogConfig.json",
    optional: false,
    reloadOnChange: true);

builder.Services.Configure<BlogConfig>(
    builder.Configuration.GetSection("BlogConfig"));

var connectionString = builder.Configuration.GetConnectionString("DB_BlogPosts");

builder.Services.AddMemoryCache();

builder.Services.AddScoped<BlogRepo>();

builder.Services.AddScoped<IBlogRepo>(sp =>
{
    var inner = sp.GetRequiredService<BlogRepo>();
    var cache = sp.GetRequiredService<IMemoryCache>();
    return new BlogRepoCached(inner, cache);
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Blog}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

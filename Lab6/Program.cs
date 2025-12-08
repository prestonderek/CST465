using Lab6.Config;
using Lab6.Logic;
using Lab6.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DB_BlogPosts");

builder.Configuration.AddJsonFile
    (
        "Config/BlogConfig.json",
        optional: false,
        reloadOnChange: true
    );

builder.Services.Configure<BlogConfig>
    (
        builder.Configuration.GetSection("BlogConfig")
    );

builder.Services.AddMemoryCache();

builder.Services.AddScoped<BlogRepo>();

builder.Services.AddScoped<IBlogRepo>(sp =>
{
    var repo = sp.GetRequiredService<IBlogRepo>();
    var cache = sp.GetRequiredService<IMemoryCache>();
    return new BlogRepoCached(repo, cache);
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Blog}/{action=Index}/{id?}")
    .WithStaticAssets();


app.MapRazorPages();


app.Run();

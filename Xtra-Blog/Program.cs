using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using XtraBlog.Data;
using XtraBlog.Models;
using XtraBlog.Services.Implementations;
using XtraBlog.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
    {
        optionsBuilder.UseSqlServer(
            builder.Configuration.GetConnectionString("Default"),
            sqlOptions => sqlOptions.MigrationsHistoryTable("Migrations"));
    });

    builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 8;
        options.Lockout.AllowedForNewUsers = false;
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

    builder.Services.AddScoped<IBlogService, BlogManager>();
}

var app = builder.Build();
{
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
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Blog}/{action=Index}/{id?}");

    app.Run();
}

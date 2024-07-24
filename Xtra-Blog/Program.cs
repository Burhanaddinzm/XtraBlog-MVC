using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using XtraBlog.Data;
using XtraBlog.Enums;
using XtraBlog.Models;
using XtraBlog.Services.Implementations;
using XtraBlog.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllersWithViews();
    builder.Services.AddHttpContextAccessor();
    builder.Services.ConfigureApplicationCookie(cookieAuthOptions =>
    {
        cookieAuthOptions.LoginPath = "/auth/login";
    });

    builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
    {
        optionsBuilder.UseSqlServer(
            builder.Configuration.GetConnectionString("Default"),
            sqlOptions => sqlOptions.MigrationsHistoryTable("Migrations"));
    });

    builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedEmail = true;
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
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<ITagService, TagManager>();

    builder.Services.AddTransient<IEmailSender, EmailSender>();
}

var app = builder.Build();
{
    if (!app.Environment.IsDevelopment())
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

    using (var scope = app.Services.CreateScope())
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string Username = "Admin";
        string Email = "admin@admin.com";
        string Password = "Admin123";

        if (await userManager.FindByEmailAsync(Email) == null)
        {
            var user = new AppUser
            {
                UserName = Username,
                Email = Email,
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(user, Password);
            if (result.Succeeded)
            {
                foreach (var role in Enum.GetValues(typeof(Roles)))
                {
                    if (!await roleManager.RoleExistsAsync(role.ToString()!))
                        await roleManager.CreateAsync(new IdentityRole(role.ToString()!));
                }

                await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            }
            else
            {
                throw new Exception("Failed to create admin and/or roles!");
            }
        }
    }

    app.Run();
}

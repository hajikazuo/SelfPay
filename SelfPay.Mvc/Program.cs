using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SelfPay.Common.Data;
using SelfPay.Common.Models.Users;
using SelfPay.Common.Repositories.Implementation;
using SelfPay.Common.Repositories.Interface;
using SelfPay.Common.Services.Implementation;
using SelfPay.Common.Services.Interface;
using SelfPay.Mvc.Services.Implementation;
using SelfPay.Mvc.Services.Interface;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUserRepository, UsuarioRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

builder.Services.AddScoped<ISeedService, SeedService>();
builder.Services.AddScoped<IUploadService, UploadService>();

builder.Services.AddSingleton(RT.Comb.Provider.Sql);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SelfPayContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<User, Role>()
           .AddEntityFrameworkStores<SelfPayContext>()
           .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);

    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.LogoutPath = "/Account/Logout";
    options.SlidingExpiration = true;
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SelfPayContext>();
    db.Database.Migrate();
}

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

using (var scope = app.Services.CreateScope())
{
    var seedUserService = scope.ServiceProvider.GetRequiredService<ISeedService>();
    seedUserService.Seed();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

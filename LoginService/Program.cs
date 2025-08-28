using LoginService.Data;
using LoginService.Data.Entities;
using LoginService.Data.Repositories;
using LoginService.Helpers;
using LoginService.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("staging"), sqlServerOptions => sqlServerOptions.EnableRetryOnFailure())
);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddAuthentication("AuthCookie")
//    .AddCookie("AuthCookie", options =>
//    {
//        options.Cookie.Name = "auth_session";   // Name of your cookie
//        options.Cookie.HttpOnly = true;         // Prevent JS access
//        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS only
//        options.Cookie.SameSite = SameSiteMode.Strict; // Prevent CSRF
//        options.LoginPath = "/Login";           // Redirect if not logged in
//        options.LogoutPath = "/Logout";
//        options.ExpireTimeSpan = TimeSpan.FromHours(1); // Lifetime
//        options.SlidingExpiration = true;       // Extend if active
//    });
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

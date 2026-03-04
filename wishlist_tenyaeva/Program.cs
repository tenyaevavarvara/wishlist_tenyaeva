using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using wishlist_tenyaeva.Data;
using wishlist_tenyaeva.Interfaces;
using wishlist_tenyaeva.Services;

var builder = WebApplication.CreateBuilder(args);

// Добавление контекста базы данных
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Настройка JWT аутентификации
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Регистрация сервисов
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IWishlistService, WishlistService>();
builder.Services.AddScoped<IWishService, WishService>();
builder.Services.AddScoped<IBookingService, BookingService>();

// Добавляем контроллеры
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 403 && !context.Response.HasStarted)
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"message\":\"Доступ запрещен\"}");
    }
    else if (context.Response.StatusCode == 401 && !context.Response.HasStarted)
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"message\":\"Требуется авторизация\"}");
    }
});


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
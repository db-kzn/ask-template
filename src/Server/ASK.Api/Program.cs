using ASK.Api.Endpoints;
using ASK.Api.Middleware;
using ASK.Application;
using ASK.Application.Interfaces;
using ASK.Application.Services;
using ASK.Domain.Entities;
using ASK.Infrastructure.Data;
using ASK.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

builder.Services.AddApplicationDbContext(builder.Configuration);

// Регистрируем интерфейс → реализацию
builder.Services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<AppDbContext>());

// Mediator
//builder.Services.AddMediator();
builder.Services.AddApplication();

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole<string>>(options =>
{
  options.SignIn.RequireConfirmedAccount = false;
  options.Password.RequireDigit = true;
  options.Password.RequiredLength = 8;
  options.Password.RequireNonAlphanumeric = true;
  options.Password.RequireUppercase = true;
  options.Password.RequireLowercase = true;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();



// Кастомные сервисы
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<TokenService>();

// Аутентификация + JWT Bearer
builder.Services.AddAuthentication()
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
          Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
      };
    });

builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
});

// Подключаем middleware
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<TenantMiddleware>();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

// OpenAPI (Swagger)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapHealthChecks("/health");

// После app.UseAuthentication(), app.UseAuthorization()
//app.MapDefaultEndpoints();

// Маршрутизация эндпоинтов
app.MapAuthEndpoints();

// Fallback для Blazor WASM
app.MapFallbackToFile("index.html");

// Seed данных
//await SeedData.SeedAsync(app.Services);

await app.RunAsync().ConfigureAwait(false);

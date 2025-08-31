using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NewsPortal.Application.Services;
using NewsPortal.Auth;
using NewsPortal.DataAccess;
using NewsPortal.DataAccess.Repositories;
using System.Text;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "News Portal API",
        Version = "v1",
        Description = "API for News Portal application"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddControllers();

builder.Services.AddDbContext<NewsPortalDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(NewsPortalDbContext)));
});

builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<JwtService>();
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));
builder.Services.AddAuth(builder.Configuration);

builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

var app = builder.Build();

// Static Files
app.UseStaticFiles();

// Static files for Images folder
var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "NewsPortal.Core", "Images");
if (!Directory.Exists(imagesPath))
{
    Directory.CreateDirectory(imagesPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagesPath),
    RequestPath = "/images"
});

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "News Portal API v1");
});

app.UseHttpsRedirection();

// CORS
app.UseCors(x =>
{
    x.WithHeaders().AllowAnyHeader();
    x.WithOrigins("http://localhost:4200", "https://localhost:4200");
    x.WithMethods().AllowAnyMethod();
    x.AllowCredentials();
});

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Controllers
app.MapControllers();

// Start Angular dev server in development mode
if (app.Environment.IsDevelopment())
{
    StartAngularDevServer();
}

// SPA fallback
app.MapFallbackToFile("index.html");

app.Run();

void StartAngularDevServer()
{
    try
    {
        var frontendPath = Path.Combine(Directory.GetCurrentDirectory(), "NewsPortal.Frontend");

        if (Directory.Exists(frontendPath))
        {
            // Запускаем Angular в фоновом режиме
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                WorkingDirectory = frontendPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = "/c npm start -- --port 4200"
            };

            var process = new Process { StartInfo = processStartInfo };


            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data) &&
                    (e.Data.Contains("error", StringComparison.OrdinalIgnoreCase) ||
                     e.Data.Contains("compiled", StringComparison.OrdinalIgnoreCase) ||
                     e.Data.Contains("4200", StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("Angular: " + e.Data);
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    Console.WriteLine("Angular ERROR: " + e.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            Console.WriteLine("Angular development server starting...");
        }
        else
        {
            Console.WriteLine("Frontend folder not found. Please start Angular manually:");
            Console.WriteLine("cd NewsPortal.Frontend && npm start");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Note: Angular server not started: " + ex.Message);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;
using StockAvaibleTest_API.Data;
using StockAvaibleTest_API.Interfaces;
using StockAvaibleTest_API.Repositories;
using StockAvaibleTest_API.Services;
using StockAvaibleTest_API.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssemblyContaining<ProductValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Stock Available API",
        Version = "v1",
        Description = "API para gestión de inventario y stock disponible",
        Contact = new OpenApiContact
        {
            Name = "Tegra",
            Email = "support@tegra.com"
        }
    });

    // Incluir comentarios XML para Swagger
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories and unit of work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBoxService, BoxService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stock Available API V1");
        c.RoutePrefix = string.Empty; // Hacer Swagger UI la página principal
    });
}

app.UseHttpsRedirection();

// Configure CORS for development
app.UseCors(builder => builder
    .AllowAnyOrigin()     // Permite peticiones desde cualquier origen
    .AllowAnyMethod()     // Permite todos los métodos HTTP (GET, POST, PUT, DELETE, etc.)
    .AllowAnyHeader());   // Permite todos los headers

// Production CORS configuration
/*
// Configure CORS for production
app.UseCors(builder => builder
    .WithOrigins(
        "http://yourdomain.com",
        "https://yourdomain.com",
        "http://localhost:3000"    // Si necesitas mantener acceso local
    )
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()           // Si necesitas enviar cookies o auth headers
    .SetIsOriginAllowedToAllowWildcardSubdomains() // Permite subdominios si usas un wildcard en WithOrigins
);
*/

app.UseAuthorization();
app.MapControllers();

app.Run();

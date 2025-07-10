using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UnistreamService.AutoMapper.Profiles;
using UnistreamService.Data;
using UnistreamService.Middleware;
using UnistreamService.Model;
using UnistreamService.Repositories;
using UnistreamService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<TransactionProfile>();
});
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddControllers();
builder.Services.Configure<TransactionSettings>(
    builder.Configuration.GetSection("TransactionSettings"));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Transaction Service API",
        Version = "v1",
        Description = "API для создания и получения транзакций"
    });
    var dir = new DirectoryInfo(AppContext.BaseDirectory);

    foreach (var fi in dir.EnumerateFiles("*.xml"))
    {
        var doc = XDocument.Load(fi.FullName);
        c.IncludeXmlComments(() => new XPathDocument(doc.CreateReader()), true);
    }
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transaction API V1");
        c.RoutePrefix = string.Empty; // чтобы Swagger открывался по адресу `/`
    });
}
// app.UseHttpsRedirection();
app.MapControllers();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.Run();

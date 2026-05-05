using Microsoft.EntityFrameworkCore;
using Payment_Back.Application.Interfaces;
using Payment_Back.Application.Services;
using Payment_Back.Infrastructure.Data;
using Payment_Back.Infrastructure.PaymentProviders;
using Payment_Back.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// AJOUT DB CONTEXT ICI
builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// DI
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentProvider, PaypalProvider>();
builder.Services.AddScoped<PaymentProviderFactory>();
builder.Services.AddScoped<PayPalClientFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

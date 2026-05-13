using Microsoft.EntityFrameworkCore;
using Payment_Back.Application.Interfaces;
using Payment_Back.Application.Services;
using Payment_Back.Infrastructure.Data;
using Payment_Back.Infrastructure.PaymentProviders;
using Payment_Back.Infrastructure.Repositories;
using Payment_Back.Infrastructure.Messaging;

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
builder.Services.AddSingleton<KafkaConsumer>();
builder.Services.AddCors(options =>

{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

var kafkaConsumer = app.Services.GetRequiredService<KafkaConsumer>();
kafkaConsumer.Start();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthorization();

app.MapControllers();

app.Run();

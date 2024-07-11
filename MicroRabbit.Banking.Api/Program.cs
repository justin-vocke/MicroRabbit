using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Infrastructure.IoC;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<BankingDbContext>(

    options => options.UseSqlServer(builder.Configuration.GetConnectionString("BankingDbConnection"))
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
         Title = "v1",
         Description = "Banking Microservice" 
    });
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
RegisterServices(builder.Services);

void RegisterServices(IServiceCollection services)
{
    DependencyContainer.RegisterServices(services);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Banking Microservice V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

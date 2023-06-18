using CQRS.Ordering.Domain.AggregatesModel.BuyerAggregate;
using CQRS.Ordering.Domain.AggregatesModel.OrderAggregate;
using CQRS.Ordering.Infrastructure.DataContexts;
using CQRS.Ordering.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using FluentValidation.AspNetCore;
using CQRS.Ordering.Infrastructure.Models;
using CQRS.Ordering.Application.Behaviors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddFluentValidation(cfg =>
cfg.RegisterValidatorsFromAssemblyContaining<Program>());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(c =>
    c.RegisterServicesFromAssembly(typeof(CQRS.Ordering.Application.Commands.CreateOrderCommand).Assembly)
    );
//.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>)));
builder.Services.AddDbContext<OrderingContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionString"]);
});
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IBuyerRepository, BuyerRepository>();
builder.Services.AddTransient<OrderingContextSeed>();


var connectionString = new
ConnectionStringModel(builder.Configuration["ConnectionString"]);
builder.Services.AddSingleton(connectionString);
builder.Services.AddScoped(typeof(IPipelineBehavior<,>),typeof(TransactionBehavior<,>));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetService<OrderingContextSeed>().SeedAsync().Wait();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

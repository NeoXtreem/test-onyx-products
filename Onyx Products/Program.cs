using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onyx.Products.Data;
using Onyx.Products.Models;
using Onyx.Products.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer();

builder.Services.AddDbContext<StoreContext>(options => options.UseInMemoryDatabase("products"));
builder.Services.AddScoped<StoreService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<StoreContext>();
    context.Database.EnsureCreated();

    if (!context.Products.Any())
    {
        foreach (var colour in new[] {Color.Red, Color.Green, Color.Blue})
        {
            await context.Products.AddRangeAsync(Enumerable.Range(0, colour.Name.Length).Select(i => new Product(Guid.NewGuid().ToString(), $"Product{i}", colour)));
        }
    }

    context.SaveChanges();
}


app.UseHttpsRedirection();

app.MapGet("/", () => "OK");

app.MapGet(
        "/api/products",
        (StoreService storeService, [FromQuery(Name = "colour")] string? colour) => storeService.GetProducts(colour is null ? null : Color.FromName(colour)))
    .RequireAuthorization(p => p.RequireRole("auditor").RequireClaim("scope", "products"));

app.Run();

public partial class Program { }
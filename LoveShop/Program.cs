using LoveShop.DTOs.Category;
using LoveShop.DTOs.Product;
using LoveShop.Models;
using LoveShop.Persistence;
using LoveShop.Services;
using LoveShop.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services
	.AddScoped<IGenericCrudService<Product, ProductDTO, ProductCreateDTO, ProductUpdateDTO>, ProductService>();

builder.Services
	.AddScoped<IGenericCrudService<Category, CategoryDTO, CategoryCreateDTO, CategoryUpdateDTO>, CategoryService>();

builder.Host.UseSerilog();

builder.Services.AddDbContext<LoveShopDbContext>(opt =>
	opt.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<LoveShopDbContext>();
	dbContext.Database.Migrate();
}

app.UseSerilogRequestLogging();

app.Run();
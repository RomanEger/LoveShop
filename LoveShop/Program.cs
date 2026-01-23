using LoveShop.DTOs.Category;
using LoveShop.DTOs.Product;
using LoveShop.Models;
using LoveShop.Persistence;
using LoveShop.Services;
using LoveShop.Services.Contracts;
using Microsoft.AspNetCore.Identity;
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

builder.Services.AddDbContext<IdentityDbContext>(opt =>
	opt.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddAuthentication();

builder.Services
	.AddIdentityCore<User>(options =>
	{
		options.User.RequireUniqueEmail = true;

		options.Password.RequireDigit = false;
		options.Password.RequireLowercase = false;
		options.Password.RequireUppercase = false;
		options.Password.RequireNonAlphanumeric = false;
	})
	.AddEntityFrameworkStores<IdentityDbContext>()
	.AddDefaultTokenProviders();

builder.Services.AddIdentityApiEndpoints<User>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapIdentityApi<User>();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<LoveShopDbContext>();
	dbContext.Database.Migrate();

	var identityDbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
	identityDbContext.Database.Migrate();
}

app.UseSerilogRequestLogging();

app.Run();
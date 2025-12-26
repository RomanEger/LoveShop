using LoveShop.Persistence;
using LoveShop.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddScoped<ProductService, ProductService>();

string? host = builder.Configuration["POSTGRES_HOST"];
string? port = builder.Configuration["POSTGRES_PORT"];
string? db = builder.Configuration["POSTGRES_DB"];
string? user = builder.Configuration["POSTGRES_USER"];
string? password = builder.Configuration["POSTGRES_PASSWORD"];

string connectionString = $"Host={host};Port={port};Database={db};User={user};Password={password}";

builder.Services.AddDbContext<LoveShopDbContext>(opt =>
    opt.UseNpgsql(connectionString));

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

app.Run();
using WebapiSales.DataAccess.Interfaces;
using WebapiSales.DataAccess.Repositories;
using WebapiSales.Providers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSingleton<DbConnectionProvider>();
builder.Services.AddScoped<IDistrictRepository, DistrictRepository>();
builder.Services.AddScoped<ISalesPersonRepository, SalesPersonRepository>();
builder.Services.AddScoped<ISecondarySalesPersonRepository, SecondarySalesPersonRepository>();
// Add HttpClient for SalesApi
var salesApiHost = builder.Configuration["StoreApiHost"];
if (string.IsNullOrEmpty(salesApiHost))
{
    throw new InvalidOperationException("StoreApiHost is not configured");
}
builder.Services.AddHttpClient("Store", c =>
{
    c.BaseAddress = new Uri(salesApiHost);
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseAuthorization();

app.MapControllers();

app.Run();

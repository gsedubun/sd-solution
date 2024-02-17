using WebapiStores.DataAccess.Interfaces;
using WebapiStores.DataAccess.Repositories;
using WebapiStores.Providers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSingleton<DbConnectionProvider>();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
// Add HttpClient for SalesApi
var salesApiHost = builder.Configuration["SalesApiHost"];
if(string.IsNullOrEmpty(salesApiHost))
{
    throw new InvalidOperationException("SalesApiHost is not configured");
}
builder.Services.AddHttpClient("Sales", c =>
{
    c.BaseAddress = new Uri(salesApiHost);
});
var localhost = "dev-site";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: localhost,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
        });
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
app.UseCors(localhost);

app.UseAuthorization();

app.MapControllers();

app.Run();

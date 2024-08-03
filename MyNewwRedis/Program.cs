using Microsoft.EntityFrameworkCore;
using MyNewwRedis.Data;
using MyNewwRedis.MiddleWares;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DbContextClass>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IConnectionMultiplexer>(provider =>
{
    var host = builder.Configuration.GetValue<string>("RedisOption:Host");
    var port = builder.Configuration.GetValue<int>("RedisOption:Port");
    var option = new ConfigurationOptions
    {
        EndPoints = { $"{host}:{port}" },
    };
    var connection = ConnectionMultiplexer.Connect(option);
    return connection;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<SlidingWindowRateLimiterMiddleware>();
app.UseMiddleware<MessageCenterSubMiddleware>();
app.Run();

using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("GameStore");

// Add services to the container.
builder.Services.AddSqlite<GameStoreContext>(connectionString); // Dependency Injection Mechanism -- Registering bcontext into service provider (service container)

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "My API",
//        Version = "v1"             
//    });
//});


var app = builder.Build();



//app.MapGet("/", () => "Hello C# Developers");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseSwaggerUI(c =>
    //{
    //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
    //});
}

app.MapGamesEndpoints();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.MigrateDb();

app.Run();

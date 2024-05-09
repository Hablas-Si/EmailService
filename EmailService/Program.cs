using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EmailService.Services;
using EmailService.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

var builder = WebApplication.CreateBuilder(args);

// BsonSeralizer... fortæller at hver gang den ser en Guid i alle entiteter skal den serializeres til en string. 
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

//MongoDB
builder.Services.Configure<MongoDbSettings>(options =>
{
    options.ConnectionURI = Environment.GetEnvironmentVariable("ConnectionURI");
    options.DatabaseName = Environment.GetEnvironmentVariable("DatabaseName");
    options.CollectionName = Environment.GetEnvironmentVariable("CollectionName");
});
builder.Services.AddSingleton<Logging>();


// Add services to the container.

builder.Services.AddControllers();

// smpt Email
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<EmailSender>();
builder.Services.AddSingleton<ILoggingService, Logging>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Console.WriteLine("Starting application...");

var app = builder.Build();
Console.WriteLine("Application built, configuring middleware...");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
Console.WriteLine("Application is running...");
app.Run();

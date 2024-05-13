using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EmailService.Services;
using EmailService.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using EmailService;

var builder = WebApplication.CreateBuilder(args);

// BsonSerializer configuration
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// MongoDB configuration
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<Logging>();

// Email settings and services
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<EmailSender>();

// Register RabbitReceiver with a factory to handle scoped dependencies
builder.Services.AddSingleton<RabbitReceiver>(serviceProvider =>
    new RabbitReceiver(() => serviceProvider.CreateScope().ServiceProvider.GetRequiredService<EmailSender>()));

builder.Services.AddSingleton<ILoggingService, Logging>();

// Add controllers
builder.Services.AddControllers();

// Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware configuration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

Console.WriteLine("Application is starting...");

// Start listening for RabbitMQ messages
var receiver = app.Services.GetRequiredService<RabbitReceiver>();
// Ensure this runs in a background thread or as a hosted service
app.Lifetime.ApplicationStarted.Register(() => receiver.ReceiveMessages());

app.Run();

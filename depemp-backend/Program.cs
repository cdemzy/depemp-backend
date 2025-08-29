using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// JSON Serializer
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options => 
        options.SerializerSettings
               .ReferenceLoopHandling=Newtonsoft.Json.ReferenceLoopHandling.Ignore).AddNewtonsoftJson(options => 
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver());

var app = builder.Build();

// Enable CORS
app.UseCors(options => options
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

// Photo Directory
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Photos")),
    RequestPath = "/Photos"
});

app.Run();

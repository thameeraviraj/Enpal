using AppointmentBooking.Converters;
using AppointmentBooking.Data;
using AppointmentBooking.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Register services
builder.Services.AddScoped<AppointmentService>();

// Add Controllers & Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Swagger Support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Appointment Booking API",
        Version = "v1"
    });
});

// Enable CORS 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter());
});

var app = builder.Build();

// Enable Swagger in Development Mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Appointment Booking API v1");
        c.RoutePrefix = "/swagger";
    });
}

// Enable CORS
app.UseCors("AllowAllOrigins");

// Enable HTTPS Redirection 
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
using DoctorAppointmentSystem.Data;
using DoctorAppointmentSystem.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);


builder.Services.AddScoped<AvailabilityService>();
builder.Services.AddScoped<AppointmentService>();


builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
//namespace DoctorAppointmentSystem
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add services to the container.
//            builder.Services.AddAuthorization();

//            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//            builder.Services.AddOpenApi();

//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (app.Environment.IsDevelopment())
//            {
//                app.MapOpenApi();
//            }

//            app.UseHttpsRedirection();

//            app.UseAuthorization();

//            var summaries = new[]
//            {
//                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//            };

//            app.MapGet("/weatherforecast", (HttpContext httpContext) =>
//            {
//                var forecast = Enumerable.Range(1, 5).Select(index =>
//                    new WeatherForecast
//                    {
//                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//                        TemperatureC = Random.Shared.Next(-20, 55),
//                        Summary = summaries[Random.Shared.Next(summaries.Length)]
//                    })
//                    .ToArray();
//                return forecast;
//            })
//            .WithName("GetWeatherForecast");

//            app.Run();
//        }
//    }
//}

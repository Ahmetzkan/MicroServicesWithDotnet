using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetryMicro1.API;
using OpenTelemetryMicro1.API.Model;
using OpenTelemetryMicro1.API.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<Micro2Service>(x => { });
builder.Services.AddDbContext<AppDbContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});


builder.Services.AddOpenTelemetry().WithTracing(options =>
{
    options.SetSampler(new AlwaysOnSampler());


    options.AddSource("ActivitySourceProvider");


    options.ConfigureResource(x => x.AddService("order.api", "1.0v"));


    options.AddAspNetCoreInstrumentation(o =>
    {
        o.RecordException = true;


        o.Filter = (context) =>
        {
            var url = context.Request.Path.Value!;
            Console.WriteLine(url);
            return url.Contains("api");
        };


        o.EnrichWithHttpRequest = (activity, request) =>
        {
            var userId = 200;
            activity.AddTag("userId", 200);
        };
    });
    options.AddEntityFrameworkCoreInstrumentation(o =>
    {
        o.EnrichWithIDbCommand = (activity, command) => { activity.AddTag("commandText", command.CommandText); };

        o.SetDbStatementForStoredProcedure = true;
        o.SetDbStatementForText = true;
    });
    options.AddHttpClientInstrumentation();


    options.AddConsoleExporter();
    options.AddOtlpExporter();
}).WithLogging(options =>
{
    options.ConfigureResource(x => x.AddService("order.api", "1.0v"));

    options.AddConsoleExporter();
    options.AddOtlpExporter();
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};


app.MapGet("api/order", async (ILogger<Program> logger, AppDbContext context, Micro2Service micro2Service) =>
{
    context.Orders.Add(new Order { Code = "123" });
    await context.SaveChangesAsync();
    Activity.Current?.AddTag("orderCode", "123");

    using (var activity = ActivitySourceProvider.Instance.StartActivity("File Write Operation", ActivityKind.Server))
    {
        System.IO.File.WriteAllText("example.txt", "Hello World");
    }


    var userId = 30;
    logger.LogInformation("Siparis endpoint is worked");
    logger.LogInformation("Siparis is created, userId={userId}", userId);


    var response = await micro2Service.GetMicro2Data();

    return Results.Ok(response);
});


app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddOpenTelemetry().WithTracing(options =>
{
    options.SetSampler(new AlwaysOnSampler());


    options.ConfigureResource(x => x.AddService("stock.api", "1.0v"));


    options.AddAspNetCoreInstrumentation(o =>
    {
        o.RecordException = true;


        o.Filter = (context) =>
        {
            var url = context.Request.Path.Value!;
            Console.WriteLine(url);
            return url.Contains("api");
        };
    });


    options.AddConsoleExporter();
    options.AddOtlpExporter();
}).WithLogging(options =>
{
    options.ConfigureResource(x => x.AddService("basket.api", "1.0v"));

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


app.MapPost("/api/products", (Product product, IHttpContextAccessor contextAccessor) =>
{
    throw new Exception("error");
    var header = contextAccessor.HttpContext.Request.Headers;
    return Results.Created($"/api/products/{product.Id}", (object?)product);
});


app.Run();


public record Product(int Id, string Name, decimal Price);
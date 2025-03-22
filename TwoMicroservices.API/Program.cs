using MassTransit;
using MicroservicesSecond.API.Consumers;
using MicroservicesSecond.API.Exceptions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserCreatedEventConsumer>();

    x.UsingRabbitMq((context, configure) =>
    {
        configure.UseMessageRetry(r =>
        {
            r.Interval(5, TimeSpan.FromSeconds(10));
            r.Handle<QueueCriticalException>();
            r.Ignore<QueueNormalException>();
        });
        //configure.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5)));

        configure.PrefetchCount = 10;
        configure.ConcurrentMessageLimit = 5;

        configure.UseDelayedRedelivery(x => x.Intervals(TimeSpan.FromHours(1), TimeSpan.FromHours(2)));

        configure.UseInMemoryOutbox(context);

        var connectionString = builder.Configuration.GetConnectionString("RabbitMQ");
        configure.Host(connectionString);

        configure.ReceiveEndpoint("email-microservice.user-created-event.queue",
            e => { e.ConfigureConsumer<UserCreatedEventConsumer>(context); });
    });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
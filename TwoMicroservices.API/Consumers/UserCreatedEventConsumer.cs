using BusShared;
using MassTransit;

namespace MicroservicesSecond.API.Consumers
{
    public class UserCreatedEventConsumer(IPublishEndpoint publishEndpoint) : IConsumer<UserCreatedEvent>
    {
        public Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            Console.WriteLine("Consume methods is worked.");

            try { throw new Exception(); }
            catch (Exception ex) { Console.WriteLine($"Error Message: {ex}"); }

            var message = context.Message;

            Console.WriteLine($"Sms is sended, UserId={message.UserId}");

            return Task.CompletedTask;
        }
    }
}
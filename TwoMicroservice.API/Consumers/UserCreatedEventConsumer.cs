using Bus.Shared;
using MassTransit;

namespace TwoMicroservice.API.Consumers
{
    public class UserCreatedEventConsumer(IPublishEndpoint publishEndpoint) : IConsumer<UserCreatedEvent>
    {
        public Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            Console.WriteLine("Consume method is worked");


            throw new Exception("Error");

            var message = context.Message;


            Console.WriteLine($"Sms is sended, UserId={message.UserId}");

            return Task.CompletedTask;
        }
    }
}
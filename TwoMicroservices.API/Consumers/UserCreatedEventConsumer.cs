using BussShared;
using MassTransit;

namespace TwoMicroservice.API.Consumers
{
    public class UserCreatedEventConsumer(IPublishEndpoint publishEndpoint) : IConsumer<UserCreatedEvent>
    {
        public Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            // transaction

            // queue

            // transaction

            // queue


            Console.WriteLine("Consume methods is worked.");


            var message = context.Message;


            Console.WriteLine($"Sms is sended, UserId={message.UserId}");

            return Task.CompletedTask;
        }
    }
}
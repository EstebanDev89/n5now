using Confluent.Kafka;
using Newtonsoft.Json;

namespace Application.Features.Permissions.Messages
{
    public class PermissionEventPublisher : IPermissionEventPublisher
    {
        private readonly IProducer<Null, string> producer;

        public PermissionEventPublisher(IProducer<Null, string> producer)
        {
            this.producer = producer;
        }

        public async Task ProduceAsync(PermissionMessage message)
        {
            var data = JsonConvert.SerializeObject(message);
            await producer.ProduceAsync(
                "permission-topic",
                new Message<Null, string> { Value = data }
            );
        }
    }
}

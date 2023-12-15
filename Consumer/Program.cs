
using Application.Features.Permissions.Messages;
using Confluent.Kafka;
using Newtonsoft.Json;

var config = new ConsumerConfig
{
    GroupId = "permission-group-id",
    BootstrapServers = "localhost:9092",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

try
{
    using (var consumerBuilder = new ConsumerBuilder<Null, string>(config).Build())
    {
        consumerBuilder.Subscribe("permission-topic");
        var cancelToken = new CancellationTokenSource();

        try
        {
            while (true)
            {
                var consumer = consumerBuilder.Consume(cancelToken.Token);
                var data = JsonConvert.DeserializeObject<PermissionMessage>(consumer.Message.Value);
                Console.WriteLine($"Processing Order Id: {data.Id}, operation: {data.NameOperation}");
            }
        }
        catch (OperationCanceledException)
        {
            consumerBuilder.Close();
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}


using Confluent.Kafka;
using System.Text.Json;
using Payment_Back.Application.Interfaces;
using Payment_Back.Infrastructure.Data;

namespace Payment_Back.Infrastructure.Messaging
{
    public class KafkaConsumer
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public KafkaConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void Start()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "payment-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            consumer.Subscribe(new[]
            {
                "item.paid",
                "item.refunded"
            });

            Task.Run(() =>
            {
                while (true)
                {
                    var result = consumer.Consume();

                    Console.WriteLine(
                        $"Message reçu : {result.Message.Value}"
                    );

                    var topic = result.Topic;

                    var json = JsonDocument.Parse(
                        result.Message.Value
                    );

                    var itemId = json.RootElement
                        .GetProperty("itemId")
                        .GetString();

                    using var scope = _scopeFactory.CreateScope();

                    var context = scope.ServiceProvider
                        .GetRequiredService<PaymentDbContext>();

                    var paymentService = scope.ServiceProvider
                        .GetRequiredService<IPaymentService>();

                    var payment = context.Payments
                        .FirstOrDefault(p => p.ItemId == itemId);

                    if (payment == null)
                    {
                        Console.WriteLine("Payment introuvable");
                        continue;
                    }

                    // ITEM PAID
                    if (topic == "item.paid")
                    {
                        Console.WriteLine(
                            $"Release Escrow pour item {itemId}"
                        );

                        paymentService
                            .ReleaseEscrow(payment.Id)
                            .Wait();
                    }

                    // ITEM REFUNDED
                    if (topic == "item.refunded")
                    {
                        Console.WriteLine(
                            $"Refund Payment pour item {itemId}"
                        );

                        paymentService
                            .RefundPayment(payment.ExternalId)
                            .Wait();
                    }
                }
            });
        }
    }
}
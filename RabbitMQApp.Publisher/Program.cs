using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMQApp.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://myntuobw:GYL7dAkJfH19crSe7jAVI9fjKBwbfrWK@clam.rmq.cloudamqp.com/myntuobw"); // Normalde bu bilgiyi appsetting.json da tutmalıyız

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.ExchangeDeclare("logs-fanout", ExchangeType.Fanout, true);

            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                string message = $"Fanout Log {x}";

                var messageBody = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("logs-fanout", "", null, messageBody);

                Console.WriteLine($"Message Sent - Publisher: { message}");
            });


            Console.ReadLine();
        }
    }
}

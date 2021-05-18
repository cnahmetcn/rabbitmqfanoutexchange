using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitMQApp.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://myntuobw:GYL7dAkJfH19crSe7jAVI9fjKBwbfrWK@clam.rmq.cloudamqp.com/myntuobw"); // Normalde bu bilgiyi appsetting.json da tutmalıyız

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel(); // Bu kanal üzerinden RabbitMQ ile haberleşebiliriz


            channel.ExchangeDeclare("logs-fanout", ExchangeType.Fanout, true);

            var randomQueueName = channel.QueueDeclare().QueueName;

            channel.QueueBind(randomQueueName, "logs-fanout", "", null);
            /*
             * QueueBind ile uygulama her ayağa kalktığında yeni bir kuyruk oluşacak.
             * Uygulama sonlandığında ilgili kuyruk silinecek.
             * QueueDeclare yaparsak uygulama sonlansa bile kuyruk kalır. 
             * Uygulama tekrar ayağa kalktığında ise kaldığı yerden devam eder.
             */

            /* Eğer sabit bir kuyruk olacaksa.
            var randomQueueName = "log-fanout";

            channel.QueueDeclare(randomQueueName, true, false, false);
            */

            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);


            channel.BasicConsume(randomQueueName, false, consumer);
            Console.WriteLine("Loglar Dinleniyor...");

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Thread.Sleep(1500);
                Console.WriteLine("Incoming Message - Subscriber: " + message);

                channel.BasicAck(e.DeliveryTag, false);
            };

            Console.ReadLine();
        }


    }
}

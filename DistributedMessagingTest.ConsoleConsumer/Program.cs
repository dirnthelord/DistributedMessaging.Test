using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Configuration;
using DistributedMessagingTest.Contracts;
using MassTransit;
using MassTransit.Log4NetIntegration.Logging;

namespace DistributedMessagingTest.ConsoleConsumer
{
    class SomethingHappenedConsumer : IConsumer<ISomethingHappened>
    {
        public Task Consume(ConsumeContext<ISomethingHappened> context)
        {
            Console.WriteLine(" text : " + context.Message.What);

            Console.WriteLine(" when : " + context.Message.When);
            Console.Write("  PROCESSED: " + DateTime.Now);
            Console.WriteLine(" (" + System.Threading.Thread.CurrentThread.ManagedThreadId + ")");

            if (context.Message.What == "err")
            {
                throw new ArgumentException("errored");
            }

            return Task.FromResult(0);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var bus = BusInitializer.CreateBus(( host, config) =>
            {
                config.ReceiveEndpoint( "MtPubSubExample_TestSubscriber", e =>
                 e.Consumer<SomethingHappenedConsumer>());
            });
             
            BusHandle busHandle = bus.StartAsync().Result;
            Console.ReadKey();
            busHandle.StopAsync().Wait();
        }
    }
}

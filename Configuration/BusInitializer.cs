using MassTransit;
using MassTransit.BusConfigurators;
using MassTransit.Log4NetIntegration.Logging;
using System;
using MassTransit.RabbitMqTransport;

namespace Configuration
{
    public class BusInitializer
    {
        public static IBusControl CreateBus(  Action< IRabbitMqHost, IRabbitMqBusFactoryConfigurator> moreInitialization)
        {
            Log4NetLogger.Use();
            var bus = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                var host = x.Host(new Uri("rabbitmq://localhost/"), h => { });
                 
                moreInitialization( host, x);
            });
             
            return bus;
        }
    }
}

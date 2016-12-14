using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Configuration;
using DistributedMessagingTest.Contracts;
using DistributedMessagingTest.Web.Hubs;
using MassTransit;
using Microsoft.AspNet.SignalR;

namespace DistributedMessagingTest.Web
{
    class SomethingHappenedConsumer : IConsumer<ISomethingHappened>
    {
        public Task Consume(ConsumeContext<ISomethingHappened> context)
        { 
            Trace.WriteLine(" text : " + context.Message.What);

            Trace.WriteLine(" when : " + context.Message.When);
            Trace.Write("  PROCESSED: " + DateTime.Now);
            Trace.WriteLine(" (" + System.Threading.Thread.CurrentThread.ManagedThreadId + ")");

            if (context.Message.What == "err")
            {
                throw new ArgumentException("errored");
            }

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<Chat>();
            hubContext.Clients.All.addMessage(context.Message, context.Message.Handler);

            return Task.FromResult(0);
        }
    }


    public class WebApiApplication : System.Web.HttpApplication
    {
        public static IBusControl Bus { get; set; }
        private BusHandle _busHandle;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Bus = BusInitializer.CreateBus((host, config) =>
            {
                config.ReceiveEndpoint("MtPubSubExample_TestSubscriber", e =>
                e.Consumer<SomethingHappenedConsumer>());
            });

            _busHandle = Bus.StartAsync().Result;
            
        }
        protected void Application_End()
        {
            _busHandle?.StopAsync().Wait();
            Bus?.StopAsync().Wait();
        }
    }
}

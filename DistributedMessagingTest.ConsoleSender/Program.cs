using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Configuration;
using DistributedMessagingTest.Contracts;
using MassTransit;
using MassTransit.Log4NetIntegration.Logging;

namespace DistributedMessagingTest.ConsoleSender
{

    class SomethingHappenedMessage : ISomethingHappened
    {
        public Handler Handler { get; set; }
        public string What { get; set; }
        public DateTime When { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var bus = BusInitializer.CreateBus((host, config) =>
           {

           });

            var busHandle = bus.StartAsync().Result;
            var text = "";

            while (text != "quit")
            {
                Console.Write("Enter a message: ");
                text = Console.ReadLine() ?? "";
                var handler = GetHandler(ref text);

                var message = new SomethingHappenedMessage()
                {
                    What = text.Trim(),
                    When = DateTime.Now,
                    Handler = handler
                };
                bus.Publish<ISomethingHappened>(message);
            }

            busHandle.StopAsync().Wait();
        }

        private static Handler GetHandler(ref string text)
        {
            if (text.Contains("toastr"))
            {
                return Handler.Toastr;
            }

            if (text.Contains("progress"))
            {
                text = text.Replace("progress", "");
                return Handler.Progress;
            }

            if (text.Contains("log"))
            {
                return Handler.Log;
            }

            if (text.Contains("action"))
            {
                text = text.Replace("action", "");
                return Handler.Action;
            }
             
            return Handler.Content;

        }
    }
}

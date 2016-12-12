using System;
using System.Threading.Tasks;
using DistributedMessagingTest.Contracts;
using Microsoft.AspNet.SignalR;

namespace DistributedMessagingTest.Web.Hubs
{
    public class Chat : Hub
    {
        private static int _id = 0;
        public void Send(string message)
        {
            var msg = string.Format("{0} - {1}", message, _id);
            //Clients.All.addMessage(msg, Context.ConnectionId);
            //Clients.Others.addMessage(string.Format("Some one said - {0} - {1}", message, _id), Context.ConnectionId);
            _id++;


            WebApiApplication.Bus.Publish<ISomethingHappened>(new
            {
                Handler = Handler.Content,
                What = msg,
                When = DateTime.Now
            });
        }

        private const string GroupName = "SecretGroup";
        public void SendGroup(string message)
        {
            Clients.Group(GroupName).addMessage(string.Format("{0} - {1}", message, _id), GroupName);
            _id++;
        }

        public Task JoinGroup(string groupName)
        {
            return Groups.Add(Context.ConnectionId, GroupName);
        }
    }
}
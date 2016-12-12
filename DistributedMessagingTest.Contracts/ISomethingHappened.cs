using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedMessagingTest.Contracts
{
    public enum Handler
    {
        Toastr,
        Progress,
        Log,
        Content,
        Action
    }
    public interface ISomethingHappened
    {
        Handler Handler { get; }
        string What { get; }
        DateTime When { get; }
    }
}

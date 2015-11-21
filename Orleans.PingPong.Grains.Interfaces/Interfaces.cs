using System;
using System.Linq;
using System.Threading.Tasks;
using Orleans.Concurrency;

namespace Orleans.PingPong
{
    [Immutable]
    public class Message
    {}

    public interface IClient : IGrainWithStringKey
    {
        Task Run();
        Task Pong(IDestination from, Message message);
        Task Initialize(IDestination actor, long repeats);
        Task Subscribe(IClientObserver subscriber);
    }

    public interface IClientObserver : IGrainObserver
    {
        void Done(long pings, long pongs);
    }

    public interface IDestination : IGrainWithStringKey
    {
        Task Ping(IClient from, Message message);
    }
}

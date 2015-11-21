using System;
using HelloWorld;

namespace Orleans.PingPong
{
    public static class Program
    {
        static Benchmark benchmark;

        static void Main(string[] args)
        {
            AppDomain hostDomain = AppDomain.CreateDomain("OrleansHost", null, new AppDomainSetup
            {
                AppDomainInitializer = InitSilo,
                AppDomainInitializerArguments = args,
            });
            Console.WriteLine("Make sure that local silo is started. Press Enter to proceed ...");
            Console.ReadLine();

            Console.Write("Enter number of clients: ");
            var numberOfClients = int.Parse(Console.ReadLine() ?? Environment.ProcessorCount.ToString("D"));

            Console.Write("Enter number of repeated pings per client (thousands): ");
            var numberOfRepeatsPerClient = int.Parse(Console.ReadLine() ?? "15");

            GrainClient.Initialize("DevTestClientConfiguration.xml");

            benchmark = new Benchmark(numberOfClients, numberOfRepeatsPerClient * 1000);
            benchmark.Run();

            Console.ReadLine();
        }

#if !USE_INPROC_SILO
        static void InitSilo(string[] args)
        {
            hostWrapper = new OrleansHostWrapper(args);

            if (!hostWrapper.Run())
            {
                Console.Error.WriteLine("Failed to initialize Orleans silo");
            }
        }

        static void ShutdownSilo()
        {
            if (hostWrapper != null)
            {
                hostWrapper.Dispose();
                GC.SuppressFinalize(hostWrapper);
            }
        }

        private static OrleansHostWrapper hostWrapper;
#endif
    }
}

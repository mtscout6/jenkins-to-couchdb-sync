using System;
using System.Timers;

namespace sabatoast_puller
{
    class Program
    {
        static void Main(string[] args)
        {
            var puller = new Puller();

            Console.WriteLine("Starting Sabatoast Puller");

            puller.Start();

            var trigger = new Timer(10000);
            trigger.Elapsed += (sender, eventArgs) => Console.WriteLine("Sabatoast Puller started type 'q' to quit");
            trigger.Start();

            do
            {
                var key = Console.ReadKey(true);

                if ("q".Equals(key.KeyChar.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    break;
                }
            } while (true);

            trigger.Stop();
            Console.WriteLine("Stopping Sabatoast Puller");

            puller.Stop();

            Console.WriteLine("Stopped Sabatoast Puller");
        }
    }
}

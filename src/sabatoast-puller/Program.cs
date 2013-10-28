using System;

namespace sabatoast_puller
{
    class Program
    {
        static void Main(string[] args)
        {
            var puller = new Puller();

            Console.WriteLine("Starting Sabatoast Puller");

            puller.Start();

            do
            {
                Console.WriteLine("Sabatoast Puller started type 'q' to quit");

                var key = Console.ReadKey(true);

                if ("q".Equals(key.KeyChar.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    break;
                }
            } while (true);

            Console.WriteLine("Stopping Sabatoast Puller");

            puller.Stop();

            Console.WriteLine("Stopped Sabatoast Puller");
        }
    }
}

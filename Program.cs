using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Linq;
using static System.Console;

namespace TracerouteGUI
{
    class Program
    {
        static void Main(string[] args)
        {
            string target = args[0];
            const int BUFFER_SIZE = 32;
            const int MAX_TTL = 30;
            const int TIMEOUT = 5_000;

            var validNodeStats = new IPStatus[] { IPStatus.Success, IPStatus.TtlExpired, IPStatus.TimeExceeded };
            List<IPAddress> iPAddresses = new List<IPAddress>();
            IPAddress targetIP = Dns.GetHostAddresses(target)[0];
            PingOptions pingOptions = new PingOptions(1, false);

            WriteLine($"Tracing route for {target} with adresses: {targetIP} ...");

            while (pingOptions.Ttl <= MAX_TTL)
            {
                byte[] buffer = new byte[BUFFER_SIZE];
                new Random().NextBytes(buffer);
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send(targetIP, TIMEOUT, buffer, pingOptions);
                    Out.WriteLine($"IP: {reply.Address}\tTime: {reply.RoundtripTime}ms\tStatus: {reply.Status}");

                    if (validNodeStats.Contains(reply.Status))
                    {
                        iPAddresses.Add(reply.Address);
                    }

                    if (reply.Status == IPStatus.Success) break;
                }

                pingOptions.Ttl++;
            }

            string IPs = string.Join('\n', iPAddresses);
            WriteLine("Done...");
            WriteLine(IPs);
        }
    }
}

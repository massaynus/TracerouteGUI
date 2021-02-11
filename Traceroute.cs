using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace TracerouteGUI
{
    public class Traceroute
    {
        const int BUFFER_SIZE = 32;
        const int MAX_TTL = 30;
        const int TIMEOUT = 5_000;

        readonly IPStatus[] validNodeStats = new IPStatus[] { IPStatus.Success, IPStatus.TtlExpired };
        PingOptions pingOptions = new PingOptions(1, false);

        public IEnumerable<IPAddress> Trace(string target)
        {
            IPAddress targetIP = Dns.GetHostAddresses(target)[0];

            while (pingOptions.Ttl <= MAX_TTL)
            {
                byte[] buffer = new byte[BUFFER_SIZE];
                new Random().NextBytes(buffer);
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send(targetIP, TIMEOUT, buffer, pingOptions);

                    if (validNodeStats.Contains(reply.Status))
                    {
                        yield return reply.Address;
                    }

                    if (reply.Status == IPStatus.Success) break;
                }

                pingOptions.Ttl++;
            }
        }
    }
}
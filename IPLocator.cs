using System;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TracerouteGUI
{
    public class IPLocator
    {
        static readonly HttpClient IPClient = new HttpClient() { BaseAddress = new Uri("https://www.iplocate.io/api/lookup/") };

        public async Task<Location> LocateIP(IPAddress address)
        {
            var res = await IPClient.GetAsync(address.ToString());
            if (res.StatusCode == HttpStatusCode.OK)
            {
                string json = await res.Content.ReadAsStringAsync();
                Location location = JsonConvert.DeserializeObject<Location>(json);
                return location;
            }


            return new Location() { latitude = -1, longitude = -1 };
        }

        public async IAsyncEnumerable<Location> LocateIPRange(IEnumerable<IPAddress> IPs)
        {
            foreach (var ip in IPs)
            {
                yield return await LocateIP(ip);
                await Task.Delay(50);
            }
        }
    }
}
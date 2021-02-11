using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TracerouteGUI
{
    class Program
    {
        public static Settings settings;
        static async Task Main(string[] args)
        {
            settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("appsettings.json"));

            string target = args[0];
            List<Location> locations = new();
            IPLocator locator = new IPLocator();
            Traceroute traceroute = new Traceroute();
            
            Console.WriteLine("Gathering locations...");
            await foreach(var location in locator.LocateIPRange(traceroute.Trace(target)))
            {
                if (location.longitude is null || location.latitude is null )
                    continue;
                    
                locations.Add(location);
            }

            Console.WriteLine("Gathed locations...");
            Console.WriteLine("Initiating GMaps services...");

            GoogleMapsProvider provider = new GoogleMapsProvider(settings, locations);
            
            Console.WriteLine("Making map...");

            Image map = await provider.MakeMap();
            string img_path = Path.Combine(Environment.CurrentDirectory, "map.png");    
            map.Save(img_path);

            Console.WriteLine("Image saved at: " + img_path);
        }
    }
}

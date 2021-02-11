using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Maps;
using Google.Maps.StaticMaps;
using System.Drawing;
using System.Diagnostics;

namespace TracerouteGUI
{
    public class GoogleMapsProvider
    {
        string api_key;
        List<Location> locations;

        public GoogleMapsProvider(Settings settings, List<Location> locations)
        {
            api_key = settings.GOOGLE_API_KEY;
            this.locations = locations;
            GoogleSigned.AssignAllServices(new GoogleSigned(api_key));
        }

        public async Task<Image> MakeMap()
        {
            List<Google.Maps.Location> gLocations = locations.Select(loc => new Google.Maps.Location($"{loc.latitude}, {loc.longitude}")).ToList();
            StaticMapService service = new StaticMapService();
            StaticMapRequest map = new StaticMapRequest();

            map.Path = new Path(gLocations);
            map.Path.Weight = 10;
            map.Path.Encode = false;
            map.Format = GMapsImageFormats.PNG;
            map.MapType = MapTypes.Terrain;
            map.Size = new MapSize(1920, 1080);
            
            return (Image) new ImageConverter().ConvertFrom(await service.GetImageAsync(map));
        }
    }
}
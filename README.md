# Traceroute GUI
a sample console app that traces a route from localhost to ServerX in the internet, takes all the intermediate IPs and geolocates them, then draws a path of all the hops on a map.

# configure
make a **appsettings.json** in root folder of the app before running it
the file should look as follows:

```json
{
    "GOOGLE_API_KEY" : "API_KEY"
}
```

# Usage
just `dotnet restore` and `dotnet run {IP|URL}`


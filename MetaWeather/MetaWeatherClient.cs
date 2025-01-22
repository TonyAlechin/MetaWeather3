using MetaWeather.Models;
using MTCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MetaWeather
{
    public class MetaWeatherClient
    {
        private readonly HttpClient _client;

        public MetaWeatherClient(HttpClient client)
        {
           
            _client = client;                                 
        }
        string apiKey = "57edc836423a459b968223619252101";

        private static readonly JsonSerializerOptions _Options = new()
        {
            Converters =
            {
                new JsonStringEnumConverter(),
                 new JsonCoordinatConverter()
            }
        };

        public async Task<Root> GetLocation(string Name,CancellationToken cancel = default)
        {
            return await _client
               .GetFromJsonAsync<Root>($"/v1/current.json?key={apiKey}&q={Name}&aqi=no", _Options, cancel).ConfigureAwait(false);
              
        }




    }
}

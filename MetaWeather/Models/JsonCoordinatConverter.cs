using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MetaWeather.Models
{
    internal class JsonCoordinatConverter : JsonConverter<(double lat, double lon)>
    {
        public override (double lat, double lon) Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.GetString() is not { Length:>=3 } str) return (double.NaN, double.NaN);
            if (str.Split(',') is not { Length: 2 } component) return (double.NaN, double.NaN);
             if(!double.TryParse(component[0],NumberStyles.Any, CultureInfo.InvariantCulture,out var lat)) return (double.NaN, double.NaN);
             if(!double.TryParse(component[1],NumberStyles.Any, CultureInfo.InvariantCulture,out var lon)) return (double.NaN,double.NaN);

            return(lat, lon);
        }

        public override void Write(Utf8JsonWriter writer, (double lat, double lon) value, JsonSerializerOptions options)
        {
            writer.WriteStringValue($"{value.lat.ToString(CultureInfo.InvariantCulture)}{value.lon.ToString(CultureInfo.InvariantCulture)}");
        }
    }
}

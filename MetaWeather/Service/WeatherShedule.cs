using MetaWeather.Interface;
using MTCL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaWeather.Service
{
    public class WeatherShedule
    {
        private readonly IMetaWeatherClient _metaWeatherClient;

        public WeatherShedule(IMetaWeatherClient metaWeatherClient )
        {
            _metaWeatherClient = metaWeatherClient;
        }

        public async Task GetWeatherTemperatureByLocation(string[] nameCities)
        {
            var weather = new List<Root>();

            foreach (var city in nameCities)
            {
                var resultApiWeather = await _metaWeatherClient.GetLocation(city);

                if (resultApiWeather != null)
                {
                    weather.Add(resultApiWeather);
                }
                else
                {
                    await Console.Out.WriteLineAsync($"{nameCities} = Нет результата по погоде так запрос города не верный");
                }
            }
            var totalResultWeather = weather.Select(x => new
            {
                City = x.location.name,
                Temperature = $"{(Math.Round(x.current.Temperatura_c) > 0 ? "+" + Math.Round(x.current.Temperatura_c) : Math.Round(x.current.Temperatura_c))}",
                Text = x.current.condition.text,
                Time = x.location.localtime
            }) ;

            foreach (var weatherByCity in totalResultWeather)
            {
                await Console.Out.WriteLineAsync($"Погода в {weatherByCity.City}: {weatherByCity.Temperature},{weatherByCity.Text},{weatherByCity.Time}");

            }





        }
    }
}

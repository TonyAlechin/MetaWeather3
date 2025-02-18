using MetaWeather.Interface;
using MetaWeather.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaWeather.Service
{
    public class SportsSheduleDisplay
    {

        private readonly IMetaWeatherClient _weatherClient;

        public SportsSheduleDisplay(IMetaWeatherClient metaWeatherClient)
        {
            _weatherClient = metaWeatherClient;
        }

        public async Task GetSports(string[] city)
        {
            var sportsInfo = new List<SportsSchedule>();
           
            foreach (var item in city)
            {
                var sportsShedule = await _weatherClient.GetLocationSports(item);

                if (sportsShedule != null && sportsShedule.Football.Count > 0)
                {
                    sportsInfo.Add(sportsShedule);
                }
                else
                {
                    Console.WriteLine($"{item} нет событий");

                }
            }

            var allMatchesInfo = sportsInfo.Where(x => x.Football != null && x.Football.Count > 0)
                .SelectMany(x => x.Football).Select(x => $"{x.Country},{x.Tournament},{x.Match},{x.Start}");

            foreach (var item in allMatchesInfo)
            {
                Console.WriteLine(item);
                await Console.Out.WriteLineAsync();
            }


        }
    }
}

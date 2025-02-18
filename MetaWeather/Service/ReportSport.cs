using MetaWeather.Interface;
using MetaWeather.Models;

namespace MetaWeather.Service
{
    public class ReportSport
    {
        private readonly IMetaWeatherClient _weatherClient;

        public ReportSport(IMetaWeatherClient metaWeatherClient)
        {
            _weatherClient = metaWeatherClient;
        }

        public async Task GetReportSports(string[] city)
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
            string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SportsShedule");
            Directory.CreateDirectory(directoryPath);
            string filePath = Path.Combine(directoryPath, "Sportshedule.txt");

            using (var sm = new StreamWriter(filePath))
            {
                foreach (var sport in sportsInfo)
                {
                    foreach (var x in sport.Football)
                    {
                        sm.WriteLineAsync($"{x.Country},{x.Tournament},{x.Match},{x.Start}");
                    }
                    sm.WriteLine();
                }
            }
            await Console.Out.WriteLineAsync("Файл успешно записан!");
        }


    }

}

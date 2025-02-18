using MetaWeather.Interface;
using MetaWeather.Models;
using MetaWeather.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MTCL;
using Polly;
using Polly.Extensions.Http;
using System.Security.Cryptography.X509Certificates;
using static MetaWeather.MetaWeatherClient;

namespace MetaWeather.TestConsole
{
    class Program
    {
        private static IHost _Hosting;

        public static IHost Hosting => _Hosting ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        public static IServiceProvider Services => Hosting.Services;


        public static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .ConfigureServices(ConfigureServices);

        private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
           // var apiKey = host.Configuration["ApiKey"]; // Убедитесь, что ключ API находится в конфигурации

            services.AddHttpClient<MetaWeatherClient>(client =>
                client.BaseAddress = new Uri(host.Configuration["MetaWeather"]))
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy());

         

            services.AddTransient<IMetaWeatherClient, MetaWeatherClient>();

            // Регистрация других сервисов
            services.AddTransient(ss => new SportsSheduleDisplay(ss.GetRequiredService<MetaWeatherClient>()));
            services.AddTransient(sp => new WeatherShedule(sp.GetRequiredService<MetaWeatherClient>()));
            services.AddTransient(sp => new ReportSport(sp.GetRequiredService<MetaWeatherClient>()));
        }
        private static IAsyncPolicy<HttpResponseMessage>GetRetryPolicy()
        {
            var jitter = new Random();
            return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(6, retry_attempt =>
            TimeSpan.FromSeconds(Math.Pow(2, retry_attempt)) +
            TimeSpan.FromMilliseconds(jitter.Next(0, 100)));
        }



        static async Task Main(string[] args)
        {
            using var host = Hosting;
            await host.StartAsync();

            var city = new string[]
            {
               "London",
               "Paris",
               "Moscow",
               "Madrid",
               "Washington",
               "New York",
               "Kolomna",
               "Tokyo",
               "Beijing",
               "Rom"
             };

            await Console.Out.WriteLineAsync("какой сервис использовать? 1 - события спорта,2-расписание погоды,3-запись спорт событий в файл,4 - выход");
            var nameService = Console.ReadLine();

            while(true)
            {
                if (nameService == "1")
                {
                    var sportsDisplay = Services.GetRequiredService<SportsSheduleDisplay>();
                    await sportsDisplay.GetSports(city);
                }
                else if (nameService == "2")
                {
                    var weatherShedule = Services.GetRequiredService<WeatherShedule>();
                    await weatherShedule.GetWeatherTemperatureByLocation(city);
                }
                else if (nameService == "3")
                {
                    var reportSport = Services.GetRequiredService<ReportSport>();
                    await reportSport.GetReportSports(city);
                }
                else { await Console.Out.WriteLineAsync("Неверный код: выход из программы");break; }
                await Console.Out.WriteLineAsync();
                await Console.Out.WriteLineAsync("какой сервис использовать? 1 - события спорта,2-расписание погоды,3-запись спорт событий;*** - любой другой символ выход из программы");
                nameService = Console.ReadLine();
            }
          
            Console.WriteLine("Завершено!");
         
            Console.ReadLine();
            await host.StopAsync();

        }
    }
}

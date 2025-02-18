using MetaWeather.Interface;
using MetaWeather.Models;
using MetaWeather.Service;
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
using System.Xml.Linq;

namespace MetaWeather
{
    public class MetaWeatherClient : IMetaWeatherClient
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

        public async Task<Root> GetLocation(string Name, CancellationToken cancel = default)
        {
            try
            {
                var result = await _client
                     .GetFromJsonAsync<Root>($"/v1/current.json?key={apiKey}&q={Name}&lang=ru&aqi=no", cancel).ConfigureAwait(false);
                if (result == null)
                {
                    await Console.Out.WriteLineAsync($"Ошибка получение города {Name}");
                    return null;
                }
                return result;
            }
            catch (HttpRequestException httpEx) when (httpEx.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                Console.WriteLine($"Город {Name} не найден. Проверьте правильность ввода.");
                // Вернём null или выбросим специальное исключение, если нужно
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Исключение при обработке города {Name}: {ex.Message}");
                return null;
            }
        }
        public async Task<SportsSchedule> GetLocationSports(string NameCity, CancellationToken cancel = default)
        {
            try
            {
                var result = await _client
                     .GetFromJsonAsync<SportsSchedule>($"/v1/sports.json?key={apiKey}&q={NameCity}&aqi=no", cancel).ConfigureAwait(false);
                if (result == null)
                {
                    await Console.Out.WriteLineAsync($"Ошибка получение города {NameCity}");
                    return null;
                }
                return result;
            }
            catch (HttpRequestException httpEx) when (httpEx.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                Console.WriteLine($"Город {NameCity} не найден. Проверьте правильность ввода.");
                // Вернём null или выбросим специальное исключение, если нужно
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Исключение при обработке города {NameCity}: {ex.Message}");
                return null;
            }
        }
          

        


        //var baseUri = new Uri("https://api.weatherapi.com/v1/sports.json");  // Убедитесь, что это правильный базовый адрес
        //                                                                     // Формируем полный URI с учетом API-ключа и параметров
        //var requestUri = new Uri(baseUri, $"?key={apiKey}&q={NameCity}&aqi=no");

        //Console.WriteLine($"Запрос на API: {requestUri}"); // Выводим итоговый URI для проверки

        //try
        //{
        //    // Выполняем запрос
        //    var response = await _client.GetAsync(requestUri.ToString(), cancel).ConfigureAwait(false);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var result = await response.Content.ReadFromJsonAsync<SportsSchedule>(_Options, cancel);

        //        // Проверка на нулевое значение или пустые массивы
        //        if (result == null ||
        //            (result.Football.Count == 0 && result.Cricket.Count == 0 && result.Golf.Count == 0))
        //        {
        //            Console.WriteLine("Получен пустой результат от API.");
        //            return new SportsSchedule(); // Возврат пустого объекта вместо null
        //        }

        //        return result;
        //    }

        //    Console.WriteLine($"Ошибка API: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
        //    return null;
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine($"Исключение при вызове API: {ex.Message}");
        //    return null;


        //try
        //{
        //return await _client
        //   .GetFromJsonAsync<SportsSchedule>($"/v1/sports.json?key={apiKey}&q={Name}&aqi=no", cancel).ConfigureAwait(false);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var result = await response.Content.ReadFromJsonAsync<SportsSchedule>(_Options, cancel);
        //        if (result == null)
        //        {
        //            Console.WriteLine($"Получено null для запроса: {requestUri}");
        //        }
        //        return result;
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Ошибка API: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
        //        return null;
        //    }
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine($"Исключение при вызове API: {ex.Message}");
        //    return null;
    }

}
    

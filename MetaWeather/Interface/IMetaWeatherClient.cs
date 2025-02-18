using MetaWeather.Models;
using MTCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaWeather.Interface
{
    public interface IMetaWeatherClient
    {

        Task<Root> GetLocation(string Name, CancellationToken cancel = default);
        Task<SportsSchedule> GetLocationSports(string Name, CancellationToken cancel = default);
    }
}

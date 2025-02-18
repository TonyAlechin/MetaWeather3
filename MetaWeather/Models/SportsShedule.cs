using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaWeather.Models
{
    public class FootballMatch
    {
        public string Stadium { get; set; }
        public string Country { get; set; }
        public string? Region { get; set; }
        public string Tournament { get; set; }
        public string Start { get; set; }
        public string Match { get; set; }
    }

    public class SportsSchedule
    {
        public List<FootballMatch> Football { get; set; } = new List<FootballMatch>();
        public List<object> Cricket { get; set; } = new List<object>();
        public List<object> Golf { get; set; } = new List<object>();
    }
}

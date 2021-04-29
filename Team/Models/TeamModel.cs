using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team.Models
{
    public class TeamModel
    {
        [JsonProperty("ID")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("teamKey")]
        public string TeamKey { get; set; }
    }
}

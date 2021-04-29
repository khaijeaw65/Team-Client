using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team.Models
{
    internal class TopicModel
    {  
        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("topicID")]
        public int teamID { get; set; }

        [JsonProperty("createAt")]
        public DateTime createAt { get; set; }
    }
}

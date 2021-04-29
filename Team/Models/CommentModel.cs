using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team.Models
{
    class CommentModel
    {
        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("detail")]
        public String Detail { get; set; }

        [JsonProperty("createAT")]
        public DateTime CreateAt { get; set; }

        [JsonProperty("topicID")]
        public int TopicID { get; set; }
    }
}

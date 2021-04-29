using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team.Helper
{
    class TopicComboBoxHelper
    {
        public String ID { get; set; }
        public String Name { get; set; }

        public TopicComboBoxHelper(String id, String name)
        {
            ID = id;
            Name = name;
        }
    }
}

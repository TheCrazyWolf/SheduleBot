using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShedulerBotSgk.ModelDB
{
    public class Setting
    {
        [Key]
        public int id { get; set; }

        public string TypeBot { get; set; }
        public int Timer { get; set; }
        public string Token { get; set; }

        public long? IdGroup { get; set; }
        public long? AdminID { get; set; }

        public List<Task>? Tasks { get; set; }

        public Setting()
        {
            Tasks = new List<Task>();
        }

    }
}

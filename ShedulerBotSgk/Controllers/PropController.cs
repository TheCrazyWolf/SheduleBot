using Microsoft.EntityFrameworkCore;
using ShedulerBotSgk.ModelDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShedulerBotSgk.Controllers
{
    internal class PropController
    {
        private static List<Setting> _settings;

        public PropController()
        {
            Refresh();
        }

        public void Refresh()
        {
            using(DB ef = new DB())
            {
                _settings = ef.Settings.Include(x => x.Tasks).ToList();
            }
        }
        public List<Setting> GetSettingsList()
        {
            Refresh();
            return _settings;
        }
    }
}

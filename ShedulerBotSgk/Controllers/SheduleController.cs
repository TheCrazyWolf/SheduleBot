using ShedulerBotSgk.ModelShedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static ShedulerBotSgk.CustomConsole;

namespace ShedulerBotSgk.Controllers
{
    internal class SheduleController
    {


        public ScheduleApi GetLessons(DateTime date, char type, int value)
        {
            var json = "";
            switch (type)
            {
                case 'S':
                    json = Response($"https://asu.samgk.ru/api/schedule/{value}/{date.ToString("yyyy-MM-dd")}");
                    break;
                case 'T':
                    json = Response($"https://asu.samgk.ru/api/schedule/teacher/{date.ToString("yyyy-MM-dd")}/{value}");
                    break;
            }

            ScheduleApi lessons = JsonSerializer.Deserialize<ScheduleApi>(json);
            return lessons;
        }


        public List<GroupElementApi> GetGroups()
        {
            var json = Response("https://mfc.samgk.ru/api/groups");
            try
            {
                var groups = JsonSerializer.Deserialize<List<GroupElementApi>>(json);
                return groups;
            }
            catch (Exception ex)
            {
                WriteError(ex.ToString());
            }
            return null;
        }

        public List<TeacherElementApi> GetTeachers()
        {
            var json = Response("https://asu.samgk.ru/api/teachers");
            try
            {
                var teachers = JsonSerializer.Deserialize<List<TeacherElementApi>>(json);
                return teachers;
            }
            catch (Exception ex)
            {
                WriteError(ex.ToString());
            }
            return null;
        }

        public string Response(string url)
        {
            try
            {
                Write($"[HTTP/JSON] -> {url}");
                using (var wb = new WebClient())
                {
                    wb.Headers.Set("Accept", "application/json");
                    return wb.DownloadString(url);
                }
            }
            catch (Exception ex)
            {
                WriteError(ex.ToString());
            }

            return null;
        }
    }
}

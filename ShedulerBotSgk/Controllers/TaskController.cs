using ShedulerBotSgk.ModelDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = ShedulerBotSgk.ModelDB.Task;
using static ShedulerBotSgk.CustomConsole;
using Microsoft.EntityFrameworkCore;
using VkNet.Enums.Filters;

namespace ShedulerBotSgk.Controllers
{
    public class TaskController
    {

        private Setting _setting;

        public TaskController(Setting set)
        {
            _setting = set;
        }

        public string AddTask(int bot_id, long chatid, string value)
        {
			try
			{
				using(DB ef = new DB())
				{
					if (ef.Settings.Include(x => x.Tasks).FirstOrDefault(x => x.id == bot_id) == null)
                        return "[!] Чет пошло не так... ";

                    if (ef.Settings.Include(x => x.Tasks).FirstOrDefault(x => x.id == bot_id).Tasks.FirstOrDefault(x => x.PeerId == Convert.ToInt64(chatid)) != null)
                        return "[!] К этому диалогу уже есть подключенные задачи";

                    var teacher = ef.CacheTeachers.FirstOrDefault(x => x.name == value || x.id == value);
                    var group = ef.CacheGroups.FirstOrDefault(x => x.name == value);

                    if(teacher == null && group == null)
                        return "[!] Не смог найти по группу (или преподавателя). Попробуй ID";

                    char chartask = ' ';
                    string valuetask = "0";


                    if (teacher != null)
                    {
                        chartask = 'T';
                        valuetask = teacher.id;
                    }
                    

                    if (group != null)
                    {
                        chartask = 'G';
                        valuetask = group.id.ToString();
                    }


                    Task task = new Task()
                    {
                        PeerId = chatid,
                        TypeTask = chartask,
                        Value = valuetask
                    };

                    var temp = ef.Settings.Include(x => x.Tasks).FirstOrDefault(x => x.id == bot_id);
                    temp.Tasks.Add(task);
					ef.SaveChanges();
                    
					return "[!] Расписание привязано";
                }
			}
			catch (Exception ex)
			{
                WriteError($"[Bot #{_setting.id}] {ex.Message.ToString()}");
                return $"[Bot #{_setting.id}] Failed\n{ex.Message.ToString()}";
			}
        }

        public string DeleteTask(int bot_id, long chatid)
        {
            try
            {
                using (DB ef = new DB())
                {

                    var find = ef.Tasks.FirstOrDefault(x => x.PeerId == chatid);

                    if (find == null)
                        return "[!] Отвязывать нечего :(";

                    ef.Remove(find);
                    ef.SaveChanges();
                    return "[!] Расписание отвязано";
                }
            }
            catch (Exception ex)
            {
                WriteError($"[Bot #{_setting.id}] {ex.Message.ToString()}");
                return $"[Bot #{_setting.id}] Failed\n{ex.Message.ToString()}";
            }
        }
    }
}

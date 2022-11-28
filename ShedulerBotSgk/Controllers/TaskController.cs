using ShedulerBotSgk.ModelDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = ShedulerBotSgk.ModelDB.Task;

namespace ShedulerBotSgk.Controllers
{
    public class TaskController
    {

        public string AddTask(int bot_id, char type, long chatid, string value)
        {
			Task task = new Task()
			{
				PeerId = chatid,
				TypeTask = type,
				Value = value
			};

			try
			{
				using(DB ef = new DB() )
				{
					var temp = ef.Settings.FirstOrDefault(x => x.id == bot_id);

					if (temp == null)
						return "Ошибка при добавление задач";

					temp.Tasks.Add(task);
					ef.SaveChanges();
                    
					return "Расписание привязано";
                }
			}
			catch (Exception ex )
			{
				
				return ex.ToString();
			}
        }
    }
}

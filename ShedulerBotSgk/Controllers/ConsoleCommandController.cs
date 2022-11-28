using ShedulerBotSgk.ModelDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ShedulerBotSgk.CustomConsole;

namespace ShedulerBotSgk.Controllers
{
    public class ConsoleCommandController
    {
        public static string AddNewBot(string[] args)
        {
            try
            {
                if (args.Length <= 3)
                    return "Использование: new <тип> <таймер> <token> (Далее для вк) <id группы> <IdАдмина>";

                Setting set = new Setting()
                {
                    TypeBot = args[1],
                    Timer = Convert.ToInt16(args[2]),
                    Token = args[3]
                };

                using (DB ef = new DB())
                {
                    ef.Add(set);
                    ef.SaveChanges();
                }

                return "Бот добавлен";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public static string GetBots()
        {
            try
            {
                WriteWaring("Список активных ботов");
                string text = "";
                using (DB ef = new DB())
                {
                    foreach (var item in ef.Settings)
                    {
                        text += $"=> #{item.id} {item.TypeBot} with Timer: {item.Timer}ms. Admin: {item.AdminID}\n";
                    }
                }

                return text;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public static string DelBot(int id)
        {
            try
            {
                using (DB ef = new DB())
                {
                    var temp = ef.Settings.FirstOrDefault(x => x.id == id);

                    if (temp != null)
                        ef.Remove(temp);

                    ef.SaveChanges();
                }

                return "Бот удален";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public static void OnReload()
        {
            WriteWaring($"Перезапуск ботов");

            System.Diagnostics.Process.Start("ShedulerBotSgk.exe");
            Environment.Exit(-1);
        }
    }
}

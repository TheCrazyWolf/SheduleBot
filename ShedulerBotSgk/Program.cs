using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static ShedulerBotSgk.CustomConsole;
using ShedulerBotSgk;
using ShedulerBotSgk.Controllers;
using ShedulerBotSgk.ModelDB;
using VkNet;
using VkNet.Model;
using VkNet.Enums.Filters;
using System.IO;
using Task = System.Threading.Tasks.Task;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

internal class Program
{
    private static void Main(string[] args)
    {
        Welcome();

        if (!IsLoadedCache())
            return;
        ServiceTask();

        while (true)
        {
            Command(Console.ReadLine());
        }
    }

    private static void Command(string command)
    {
        var split_command = command.Split(' ');

        switch (split_command[0].ToLower())
        {
            case "new":
                Write(ConsoleCommandController.AddNewBot(split_command));
                break;
            case "del":
                if(split_command.Length <=1)
                    Write(ConsoleCommandController.DelBot(Convert.ToInt32(split_command[1])));
                break;
            case "bots":
            case "list":
                Write(ConsoleCommandController.GetBots());
                break;
            case "reload":
                System.Diagnostics.Process.Start("ShedulerBotSgk.exe");
                Environment.Exit(-1);
                break;

        }
    }


    private static void ServiceTask()
    {
        using (DB ef = new DB())
        {
            foreach (var item in ef.Settings.Include(x => x.Tasks).ToList())
            {
                switch (item.TypeBot)
                {
                    case "T":
                        Thread thread = new Thread(() => new TelegController(item.Token, item));
                        thread.Start();
                        break;
                }
            }
        }
    }

    private static bool IsLoadedCache()
    {
        try
        {
            SheduleController s = new SheduleController();

            var groups_actual = s.GetGroups();
            var teachers_actual = s.GetTeachers();
            //Удаление кеша И добавление

            using (DB ef = new DB())
            {
                if (ef.CacheGroups.ToList().Count != groups_actual.Count)
                {
                    foreach (var item in ef.CacheGroups)
                    {
                        ef.Remove(item);
                        WriteError($"[Cache] Элемент {item.name} удален");
                    }

                    foreach (var item in groups_actual)
                    {
                        ef.Add(item);
                        WriteWaring($"[Cache] Элемент {item.name} закеширован");
                    }
                }
                else
                {
                    WriteWaring($"[Cache] Группы не требуют повторного кеширования");
                }


                if (ef.CacheTeachers.ToList().Count != teachers_actual.Count)
                {
                    foreach (var item in ef.CacheTeachers)
                    {
                        ef.Remove(item);
                        WriteError($"[Cache] Элемент {item.name} удален");
                    }


                    foreach (var item in teachers_actual)
                    {
                        ef.Add(item);
                        WriteWaring($"[Cache] Элемент {item.name} закеширован");
                    }
                }
                else
                {
                    WriteWaring($"[Cache] Преподаватели не требуют повторного кеширования");
                }


                ef.SaveChanges();
                return true;
            }
        }
        catch (Exception ex )
        {
            WriteError("Ошибка в кеширование");
            WriteWaring(ex.ToString());
            return false;
        }
    }
}
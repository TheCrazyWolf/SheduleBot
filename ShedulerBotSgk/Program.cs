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

internal class Program
{
    private static void Main(string[] args)
    {
        Welcome();


        if (!IsLoadedCache())
            return;

        ServiceTask();




        //ShedulerBotSgk.ModelDB.Task task = new()
        //{
        //    TypeTask = 'T',
        //    PeerId = 133156422,
        //    Value = 1468.ToString()
        //};
        //using(DB ef = new DB())
        //{
        //    var search = ef.Settings.FirstOrDefault(x => x.id == 2);
        //    if (search.Tasks == null)
        //        search.Tasks = new List<ShedulerBotSgk.ModelDB.Task>();
            
        //    search.Tasks.Add(task);
        //    ef.SaveChanges();
        //}

        Console.ReadLine();
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
                        TelegController tg = new TelegController(item.Token, item);
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
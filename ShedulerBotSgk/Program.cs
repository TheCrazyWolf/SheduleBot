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

internal class Program
{
    private static void Main(string[] args)
    {
        Welcome();
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
                VkApi api = new VkApi();
                api.Authorize(new ApiAuthParams()
                {
                    AccessToken = item.Token
                });

                //VkController v = new VkController(api, item);
                //Thread thread = new Thread(() => v.ConnectLongPollServer());
                //thread.Start();
            }
        }
    }
}
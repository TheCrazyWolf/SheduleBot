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

internal class Program
{
    private static void Main(string[] args)
    {
        Welcome();
        ServiceTask();

        Console.ReadLine();
    }

    private static void ServiceTask()
    {
        PropController s = new PropController();
        foreach (var item in s.GetSettingsList())
        {
            VkApi api = new VkApi();
            api.Authorize(new ApiAuthParams()
            {
                AccessToken = item.Token
            });

            VkController v = new VkController(api,item);
            Thread thread = new Thread(() => v.ConnectLongPollServer());
            thread.Start();
        }
    }
}
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static ShedulerBotSgk.CustomConsole;
using ShedulerBotSgk;
using ShedulerBotSgk.Controllers;
using ShedulerBotSgk.ModelDB;
using VkNet;
using VkNet.Model;
using VkNet.Enums.Filters;

internal class Program
{
    private static void Main(string[] args)
    {
        Welcome();


        AuthOnLoad();

        PropController s = new PropController();

        Console.ReadLine();
    }

    private static void AuthOnLoad()
    {
        PropController s = new PropController();
        foreach (var item in s.GetSettingsList())
        {
            try
            {
                VkApi api = new VkApi();
                api.Authorize(new ApiAuthParams()
                {
                    AccessToken = item.Token
                });
                Write($"[Bot #{item.id}] Auth success");
            }
            catch (Exception ex)
            {
                WriteError($"[Bot #{item.id}] Auth failed, see more in console");
                WriteWaring(ex.Message);
                WriteWaring(ex.StackTrace);
            }
        }
    }
}
using ShedulerBotSgk.ModelDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.GettingUpdates;
using static ShedulerBotSgk.CustomConsole;

namespace ShedulerBotSgk.Controllers
{
    public class TelegController
    {
        private BotClient _api;
        private int _counterr;
        private Setting _setting;

        public TelegController(string token, Setting setting)
        {
            _setting = setting;
            _api = new BotClient(token);
            Thread poll = new Thread(() => StartPolling());
            poll.Start();

            Thread shh = new Thread(() => Sheduler());
            shh.Start();
        }

        private void Sheduler()
        {
            if (WatchDog())
                return;


        } 
        private void StartPolling()
        {
            var updates = _api.GetUpdates();
            while (true)
            {
                if (WatchDog())
                    return;

                try
                {
                    if (updates.Any())
                    {
                        foreach (var update in updates)
                        {
                            CheckEvent(update);
                        }
                        var offset = updates.Last().UpdateId + 1;
                        updates = _api.GetUpdates(offset);
                    }
                    else
                    {
                        updates = _api.GetUpdates();
                    }
                }
                catch (Exception ex)
                {
                    _counterr++;
                }
            }
        }

        private void CheckEvent(Update update)
        {
            if (update.Message.Text == null)
                return;

            string[] msg = update.Message.Text.Split(' ');

            switch (msg[0].ToLower())
            {
                case "привет":
                    _api.SendMessage(update.Message.Chat.Id, "Хай");
                    break;
                default:
                    break;
            }
        }


        // WatchDog
        private bool WatchDog()
        {
            if (_counterr > 5)
            {
                WriteError($"[Bot #{_setting.id}] WatchDog: Отключен (превышено количество ошибок)");
                return false;
            }
            return true;
        }

    }
}

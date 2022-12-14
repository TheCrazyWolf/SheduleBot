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

            try
            {
                Thread poll = new Thread(() => StartPolling());
                poll.Start();

                Thread shh = new Thread(() => Sheduler());
                shh.Start();

                Write($"[Bot #{_setting.id}] Запущен");
            }
            catch (Exception ex)
            {
                _counterr = 99999;
                WriteError($"[Bot #{_setting.id}] Не смог запустится");
                WriteError(ex.ToString());
            }
        }

        private void Sheduler()
        {
            if (IsBotDead())
                return;
        } 

        private void StartPolling()
        {
            var updates = _api.GetUpdates();
            while (true)
            {
                if (IsBotDead())
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

            TaskController s = new TaskController(_setting);
            switch (msg[0].ToLower())
            {
                case "привет":
                    _api.SendMessage(update.Message.Chat.Id, "Хай");
                    break;
                case "/setup":
                    _api.SendMessage(update.Message.Chat.Id, s.AddTask(_setting.id, update.Message.Chat.Id, msg[1]));
                    break;
                case "/delete":
                    _api.SendMessage(update.Message.Chat.Id, s.DeleteTask(_setting.id, update.Message.Chat.Id));
                    break;
                default:
                    break;
            }
        }

        // WatchDog
        private bool IsBotDead()
        {
            if (_counterr >= 5)
            {
                WriteError($"[Bot #{_setting.id}] WatchDog: Отключен (превышено количество ошибок)");
                return true;
            }
            return false;
        }

    }
}

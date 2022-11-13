using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model.RequestParams;
using VkNet.Model;
using static ShedulerBotSgk.CustomConsole;
using VkNet;
using ShedulerBotSgk.ModelDB;

namespace ShedulerBotSgk.Controllers
{
    internal class VkController
    {

        private int _counterr = 0;

        public void StartLongPoll(VkApi api, Setting d)
        {
            while (true)
            {
                Write($"[Bot #{d.id}] Longpoll! ok");
                try
                {
                    var s = api.Groups.GetLongPollServer((ulong)d.IdGroup);
                    var poll = api.Groups.GetBotsLongPollHistory(new BotsLongPollHistoryParams()
                    {
                        Server = s.Server,
                        Key = s.Key,
                        Ts = s.Ts,
                        Wait = 25
                    });
                    Write($"[Bot #{d.id}] Longpoll! ok");
                    if (poll?.Updates == null) continue;

                }
                catch (Exception ex)
                {
                    WriteError($"[Bot #{d.id}] FAILED TO START UP SERVICE");
                    WriteError(ex.Message);
                    _counterr++;

                    if(_counterr > 5)
                    {
                        WriteError($"[Bot #{d.id}] Отключен (превышено количество ошибок)");
                        return;
                    }
                }

            }
        }




    }
}

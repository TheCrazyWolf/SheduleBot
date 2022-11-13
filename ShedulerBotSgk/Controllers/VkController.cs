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
        private Setting _settings;
        private VkApi _api;


        public VkController(VkApi api, Setting settings)
        {
            _api = api;
            _settings = settings;
        }

        public void ConnectLongPollServer()
        {
            while (true)
            {
                Write($"[Bot #{_settings.id}] Longpoll! ok");
                try
                {
                    var s = _api.Groups.GetLongPollServer((ulong)_settings.IdGroup);
                    var poll = _api.Groups.GetBotsLongPollHistory(new BotsLongPollHistoryParams()
                    {
                        Server = s.Server,
                        Key = s.Key,
                        Ts = s.Ts,
                        Wait = 25
                    });
                    Write($"[Bot #{_settings.id}] Longpoll! ok");
                    if (poll?.Updates == null) continue;

                }
                catch (Exception ex)
                {
                    WriteError($"[Bot #{_settings.id}] FAILED TO START UP SERVICE");
                    WriteError(ex.Message);
                    _counterr++;

                    if(_counterr > 5)
                    {
                        WriteError($"[Bot #{_settings.id}] Отключен (превышено количество ошибок)");
                        return;
                    }
                }

            }
        }

        public void Send(string text, long? peerid)
        {
            Write($"[Message.Send] -> Беседа #{peerid}. Содержимое {text.Replace("\n", " ")}");
            try
            {
                _api.Messages.Send(new MessagesSendParams()
                {

                    Message = text,
                    PeerId = peerid,
                    RandomId = new Random().Next()
                });
            }
            catch (Exception ex)
            {
                WriteError(ex.ToString());
            }
        }


    }
}

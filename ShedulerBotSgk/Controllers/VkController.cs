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
using VkNet.Enums.Filters;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using VkNet.Enums.SafetyEnums;

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
            Thread thread = new Thread(() => ServiceSchedule());
            thread.Start();
        }

        private void ServiceSchedule()
        {
            while (true)
            {
                if (!WatchDog())
                    break;

                Thread.Sleep(_settings.Timer);

                //if (DateTime.Now.Hour >= 22 || DateTime.Now.Hour <= 10)
                //    continue;

                if (_settings.Tasks == null)
                    continue;
                
                foreach (var item in _settings.Tasks)
                {
                    Thread.Sleep(500);
                    Write($"[Bot #{_settings.id}] Task #{item.IdTask} запущен по расписанию");


                    SheduleController controller = new();
                    var s = controller.GetLessons(DateTime.Now.AddDays(1), (char)item.TypeTask, Convert.ToInt32(item.Value));
                    string rasp = controller.GetLessonsString(s, _settings, _api, item);

                    rasp = Debug(rasp, item);

                    if (item.ResultText == rasp)
                        continue;

                    if (s.lessons.Count == 0)
                        continue;

                    using (DB ef = new DB())
                    {
                        var temp = ef.Tasks.FirstOrDefault(x => x.IdTask == item.IdTask);
                        if (temp != null)
                            temp.ResultText = rasp;
                        ef.SaveChanges();

                        var temp_settings = ef.Settings.Include(x=> x.Tasks).FirstOrDefault(x=> x.id == _settings.id);
                        if (temp_settings != null)
                            _settings = temp_settings;
                    }

                    Send(rasp, Convert.ToInt64(item.PeerId));

                }
            }
        }

        public void ConnectLongPollServer()
        {
            while (true)
            {
                if (!WatchDog())
                    break;

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


                    CheckEvent(poll);

                }
                catch (Exception ex)
                {
                    WriteError($"[Bot #{_settings.id}] {ex.Message}");
                    _counterr++;
                    WatchDog();
                }

            }
        }

        public void CheckEvent(BotsLongPollHistoryResponse response)
        {
            //Write($"{response.Updates.Count()} событие.");
            foreach (var item in response.Updates)
            {

                if (item.Type == GroupUpdateType.MessageNew)
                {
                    switch (App.Application.Debug)
                    {
                        case true:
                            Write($"[Bot #{_settings.id}] [Message.New] <- Беседа #{item.Message.PeerId}. Отправитель #{item.Message.FromId}. Содержимое: {item.Message.Text}");
                            break;
                    }

                    var user_msg = new Regex("\\[.*\\][\\s,]*").Replace(item.Message.Text.ToLower(), "").Split(" ");

                    CommandController controller = new ();

                    switch (user_msg[0])
                    {

                        case "начать":
                        case "старт":
                            Send("Здравствуйте! Этот бот будет отправлять вам расписание в лс. О том как его настроить или подключить к беседе - !справка", item.Message.PeerId);
                            break;
                        case "!справка":
                        case "справка":
                        case "помощь":
                        case "!помощь":
                            Send("Информация по командам:\n\n" +
                                "!привязать <ИС-22-01/Фио препода> - привязать беседу для расписания\n" +
                                "!отвязать - отвязать беседу\n" +
                                "!расписание <вчера/сегодня/завтра>- расписание на вчера, сегодня и на завтра\n" +
                                "<=Словарь=>\n" +
                                "!словарь <слово!ответ;ответ> - добавление слов в словарь> \n(Например !словарь как дела!отлично;класс;успешно)\n" +
                                "!редсловарь <слово!ответ;ответ> - редактирование словаря\n" +
                                "!слово <слово> - словарь ответов\n" +
                                "\n<=Администрирование бота=>\n" +
                                "!админ <значение ID vk> - назначить нового админа\n" +
                                "!рассылка <значение> - рассылка текста по группам\n" +
                                "!задачи - текущие задачи\n" +
                                "!удалзадачи <значение>\n" +
                                "!конфиг - перезагрузить конфигурацию", item.Message.PeerId);
                            break;

                        // Раписание
                        case "!расписание":
                            Send(controller.GetLessonsNow(item, user_msg), item.Message.PeerId);
                            break;
                        case "!привязать":
                            Send(controller.FindAddNewTask(item, user_msg), item.Message.PeerId);
                            break;
                        case "!отвязать":
                            Send(controller.DeleteTask(item, user_msg), item.Message.PeerId);
                            break;

                        //Админстрирование
                        case "!админ":
                            Send(controller.AddNewAdmin(item, user_msg), item.Message.PeerId);
                            break;
                        case "!рассылка":
                            Send(controller.SendAllResponse(item, user_msg), item.Message.PeerId);
                            break;
                        case "!задачи":
                            Send(controller.GetTasks(item, user_msg), item.Message.PeerId);
                            break;
                        case "!удалзадачи":
                            Send(controller.DeleteTaskAdmin(item, user_msg), item.Message.PeerId);
                            break;
                        case "!конфиг":
                            Send(controller.ReloadConfig(item, user_msg), item.Message.PeerId);
                            break;

                        //Развлекаловка
                        case "!словарь":
                            Send(controller.AddNewBook(item, user_msg), item.Message.PeerId);
                            break;
                        case "!редсловарь":
                            Send(controller.EditBook(item, user_msg), item.Message.PeerId);
                            break;
                        case "!слово":
                            Send(controller.CheckBook(item, user_msg), item.Message.PeerId);
                            break;
                        case "!весьсловарь":
                            Send(controller.GetAllBook(item, user_msg), item.Message.PeerId);
                            break;
                        default:
                            Send(controller.GetAnswer(item, user_msg), item.Message.PeerId);
                            break;
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

        public string Debug(string text, ModelDB.Task task)
        {
            switch (App.Application.Debug)
            {
                case true:
                    string sub = _settings.Token.Substring(6);
                    text += $"\nDebug status: [App.Application.Debug]\nSheduleBot v{App.Application.Version}" +
                        $"\nBot #{_settings.id}\nТoken: {_settings.Token.Substring(190)}. " +
                        $"\nTask #{task.IdTask}. \nID SHEDULE: {task.Value}" +
                        $"\nError Count: {_counterr}";
                    break;
            }

            return text;
        }

        private bool WatchDog()
        {
            if (_counterr > 5)
            {
                WriteError($"[Bot #{_settings.id}] WatchDog: Отключен (превышено количество ошибок)");
                return false;
            }


            return true;
        }

    }
}

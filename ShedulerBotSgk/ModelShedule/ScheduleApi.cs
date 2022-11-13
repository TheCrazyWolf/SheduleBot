﻿namespace ShedulerBotSgk.ModelShedule
{
    internal class ScheduleApi
    {
        public string? date { get; set; }
        public List<Lessons>? lessons { get; set; }
    }

    public class Lessons
    {
        public string? title { get; set; }
        public string? num { get; set; }
        public string? teachername { get; set; }
        public object? nameGroup { get; set; }
        public string? cab { get; set; }
        public string? resource { get; set; }
    }
}

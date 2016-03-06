using System;

namespace Consolas
{
    public static class LoggerExtention
    {
        public static void AddApplicationLog(this Events events, EventTypes type, string description = null)
        {
            events.Event.AddEventRow(-1, (int)type, description ?? type.ToString(), DateTime.Now);
        }

        public static void AddTaskLog(this Events events, int taskId, EventTypes type, string description)
        {
            events.Event.AddEventRow(taskId, (int)type, description ?? type.ToString(), DateTime.Now);
        }
    }
}

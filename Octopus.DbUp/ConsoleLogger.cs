using System;

namespace Octopus.DbUp
{
    public interface ILogger
    {
        void Exception(Exception ex);
        void Info(string message);
        void Success(string message);
    }

    public class ConsoleLogger : ILogger
    {
        public void Exception(Exception ex)
        {
            WriteLog(ex.ToString(), ConsoleColor.Red);
        }

        public void Info(string message)
        {
            WriteLog(message, ConsoleColor.Yellow);
        }

        public void Success(string message)
        {
            WriteLog(message, ConsoleColor.Green);
        }

        private void WriteLog(string m, ConsoleColor c)
        {
            Console.ForegroundColor = c;
            Console.WriteLine(m);
            Console.ResetColor();
        }
    }
}

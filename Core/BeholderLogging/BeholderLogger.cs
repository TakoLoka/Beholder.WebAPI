using System;
using System.Collections.Generic;
using System.Text;

namespace Core.BeholderLogging
{
    public class BeholderLogger
    {
        public static void Log(Exception exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}

﻿namespace MagicVilla_VillaAPI.Logging;

public class Logging : ILogging
{
    public void Log(string message, string type)
    {
        if (type == "error")
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR - " + message);
            Console.ForegroundColor = ConsoleColor.Black;
        }
        else
        {
            Console.WriteLine(message);
        }
    }
}

using IniParser;
using System;

namespace SoVitsBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Bot starter (Main) running...");
            try
            {
                var bot = new Bot();
                // initialize a ini config reader, get the config path and read it 
                bot.botConfig = new FileIniDataParser().ReadFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
                await bot.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong... Exception following: {ex.Message}");
                Console.ReadLine();
            }
        }
    }
}

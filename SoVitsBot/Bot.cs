using DSharpPlus;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.SlashCommands;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SoVitsBot
{
    public class Bot : SingletonBase<Bot>
    {
        public DiscordClient discordClient;
        public IniData botConfig;
        public async Task Run()
        {
            discordClient = new DiscordClient(new DiscordConfiguration()
            {
                Token = botConfig["Bot"]["Token"],
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            });

            var slash = discordClient.UseSlashCommands();
            slash.RegisterCommands<VoiceCommands>(1098322278913167360);

            Console.WriteLine("Bot running!");

            await discordClient.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}

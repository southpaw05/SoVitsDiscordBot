using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using NReco;
using NReco.VideoConverter;

namespace SoVitsBot
{
    public class VoiceCommands : ApplicationCommandModule
    {
        private readonly string modelPath = "C:\\Users\\riley\\source\\repos\\SoVitsBot\\AiImpl\\Models\\";
        private readonly string dataPath = "C:\\Users\\riley\\source\\repos\\SoVitsBot\\AiImpl\\OutputData\\";

        [SlashCommand("Generate", "Generate/convert an audio file (in acapella form) to the AI Voice Model of your choosing.")]
        public async Task GenerateCommand(InteractionContext ctx, [Option("file", "Original audio file you want to convert. MP3 or WAV only")] DiscordAttachment audioFile,
            [Choice("Juice WRLD", 1)]
            [Choice("Kanye West", 2)]
            [Choice("Billie Ellish", 3)]
            [Choice("Melanie Martinez", 4)]
            [Choice("Lil Uzi Vert", 5)]
            [Choice("Eminem", 6)]
            [Choice("Kurt Cobain", 7)]
            [Choice("Drake", 8)]
            [Choice("The Weeknd", 9)]
            [Choice("Notti Osama", 10)]
            [Option("Model", "Which voice model do you wish the audio file be converted to mimic?")] long modelChoice, [Option("transpose", "Transpose pitch up/down")] long transpose = 0, [Option("pitchprediction", "Enable or disable pitch prediction. Disable if you have issues with pitch in the song")] bool pitchPrediction = true)
        {
            string fileName = $"{Random.Shared.Next()}.mp3";
            string filePath = Path.Combine(dataPath, fileName);

            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Generating audio.."));
            await DownloadFileAsync(audioFile.Url, filePath);
            RunConversion(filePath, modelChoice, transpose, pitchPrediction);

            using FileStream fileOpen = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Generation finished! Here's your audio file: "));
            await ctx.Channel.SendMessageAsync(new DiscordMessageBuilder().AddFile(fileOpen).AddMention(new UserMention(ctx.Member)));
            await Task.Delay(5000);
            File.Delete(filePath);
        }

        private void RunConversion(string filePath, long modelNum, long transpose, bool pitchPrediction)
        {
            string model = GetModelPath(modelNum);
            string config = GetConfigPath(modelNum);
            string pitchPred = "";
            if(pitchPrediction)
            {
                pitchPred = "--auto-predict-f0";
            } else
            {
                pitchPred = "--no-auto-predict-f0";
            }
            Process proc = new()
            {
                StartInfo =
                {
                    FileName = "C:\\Users\\riley\\AppData\\Local\\Programs\\Python\\Python310\\Scripts\\svc.exe",
                    Arguments = $"infer {filePath} -m {model} -c {config} -o {filePath} -ch 0.3 -t {transpose} {pitchPred} -fm parselmouth -n 0.5 -p 0.75"
                    // Arguments = $"infer {filePath} -m {model} -c {config} -o {filePath} -ch 0.4 -t {transpose} --no-auto-predict-f0 -fm parselmouth -n 0.5 -p 0.75 -ab"
                }
            };
            proc.Start();
            proc.WaitForExit();
        }

        public string GetModelPath(long modelNum)
        {
            if (modelNum == 1)
            {
                return $"{modelPath}Juice\\G_163200.pth";
            }
            if (modelNum == 2)
            {
                return $"{modelPath}Kanye\\G_199200.pth";
            }
            if (modelNum == 3)
            {
                return $"{modelPath}Billie\\G_8000.pth";
            }
            if (modelNum == 4)
            {
                return $"{modelPath}Melanie\\G_40000.pth";
            }
            if (modelNum == 5)
            {
                return $"{modelPath}Uzi\\G_237600.pth";
            }
            if (modelNum == 6)
            {
                return $"{modelPath}Eminem\\G_86400.pth";
            }
            if (modelNum == 7)
            {
                return $"{modelPath}Kurt\\G_138600.pth";
            }
            if (modelNum == 8)
            {
                return $"{modelPath}Drake\\G_106000.pth";
            }
            if (modelNum == 9)
            {
                return $"{modelPath}Weeknd\\G_100000.pth";
            }
            if (modelNum == 10)
            {
                return $"{modelPath}Notti\\G_32000.pth";
            }
            return "";
        }

        public string GetConfigPath(long modelNum)
        {
            if (modelNum == 1)
            {
                return $"{modelPath}Juice\\config.json";
            }
            if (modelNum == 2)
            {
                return $"{modelPath}Kanye\\config.json";
            }
            if (modelNum == 3)
            {
                return $"{modelPath}Billie\\config.json";
            }
            if (modelNum == 4)
            {
                return $"{modelPath}Melanie\\config.json";
            }
            if (modelNum == 5)
            {
                return $"{modelPath}Uzi\\config.json";
            }
            if (modelNum == 6)
            {
                return $"{modelPath}Eminem\\config.json";
            }
            if (modelNum == 7)
            {
                return $"{modelPath}Kurt\\config.json";
            }
            if (modelNum == 8)
            {
                return $"{modelPath}Drake\\config.json";
            }
            if (modelNum == 9)
            {
                return $"{modelPath}Weeknd\\config.json";
            }
            if (modelNum == 10)
            {
                return $"{modelPath}Notti\\config.json";
            }
            return "";
        }

        private static async Task DownloadFileAsync(string url, string destinationFilePath)
        {
            using HttpClient httpClient = new();
            using HttpResponseMessage response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            using Stream contentStream = await response.Content.ReadAsStreamAsync();
            using FileStream fileStream = new(destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);

            await contentStream.CopyToAsync(fileStream);
        }
    }
}

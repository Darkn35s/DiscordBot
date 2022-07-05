using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;

namespace dsboty
{
    public class MusicModule
    {
        public MusicModule()
        {

        }

        public async Task<IAudioClient> ConnectAudio(SocketCommandContext context)
        {
            SocketGuildUser master=context.User as SocketGuildUser;
            IVoiceChannel channel = master.VoiceChannel;
            if (channel == null)
            {
                await context.Message.Channel.SendMessageAsync("Нужно быть подключенным к голосовому каналу");
                return null;

            }
            return await channel.ConnectAsync();
        }

        public async Task YoutubeStreaming(IAudioClient client,string url)
        {

            using (var stream = CreateYoutubeStream(url))
            using (var output = stream.StandardOutput.BaseStream)
            using (var discord = client.CreatePCMStream(AudioApplication.Mixed, 96000))
                try { await output.CopyToAsync(discord); }
                finally { await discord.FlushAsync(); }

        }

        private Process CreateYoutubeStream(string url)
        {
            //ProcessStartInfo stream = new ProcessStartInfo
            //{

            //    FileName = "CMD.exe",
            //    Arguments = $@"youtube-dl --no-check-certificate -f bestaudio -o - {url} | ffmpeg -i pipe:0 -f s16le -ar 48000 -ac 2 pipe:1",
            //    UseShellExecute = false,
            //    RedirectStandardOutput = true,
            //    CreateNoWindow = false
            //};
            var path = @"C:\Users\Home\Downloads\1.mp3";
            ProcessStartInfo stream = new ProcessStartInfo
            {

                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false
            };
            System.Console.WriteLine("bilded");
            return Process.Start(stream);
        }
    }
}

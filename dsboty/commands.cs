using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace dsboty
{
    public class commands : ModuleBase<SocketCommandContext>
    {
        private Random random = new Random();
        public MusicModule music { get; set; }
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("Pong");
        }
        [Command("play", RunMode = RunMode.Async)]
        [Summary("YouTube")]
        [RequireContext(ContextType.Guild)]
        public async Task Play(string url)
        {
            var audio = await music.ConnectAudio(Context);
            if (audio == null)
            {
                return;
            }
            await music.YoutubeStreaming(audio, url.Split('&')[0]);
        }
        [Command("roll")]
        public async Task Roll()
        {
            await ReplyAsync(random.Next(0, 100).ToString());
        }

        [Command("flip")]
        public async Task Flip()
        {
            String[] coin = new String[2] { "орел", "решка" };
            await ReplyAsync(coin[random.Next(0,2)]);
        }
        
    }
}

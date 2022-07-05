using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using dsboty;

namespace Dsbot
{
    class Program
    {
        Random Random = new Random();
        CommandService commandService;
        IServiceProvider serviceProvider;
        DiscordSocketClient client;
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();


        private async Task MainAsync()
        {
            commandService=new CommandService();
            client = new DiscordSocketClient();
            client.MessageReceived += CommandsHandler;
            client.Log += Log;

            serviceProvider = new ServiceCollection()
                .AddSingleton(this)
                .AddSingleton(client)
                .AddSingleton(commandService)
                .AddSingleton<MusicModule>()
                .BuildServiceProvider();
            var token = ""; //поместить в JSON
            await commandService.AddModulesAsync(Assembly.GetEntryAssembly(), serviceProvider);
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await Task.Delay(-1);
        }
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg);
            return Task.CompletedTask;
        }
        private async Task CommandsHandler(SocketMessage msg)
        {
            var message = msg as SocketUserMessage;
            var context = new SocketCommandContext(client, message);
            if (message.Author.IsBot) return;
            int argPos = 0;
            if(message.HasStringPrefix("-",ref argPos)){
                var result = await commandService.ExecuteAsync(context, argPos, serviceProvider);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);

            }
        }
      

    }
}
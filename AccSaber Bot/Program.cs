using System;
using System.IO;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using System.Reflection;
using AccSaber_Bot;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace AccSaber_Feed
{
    class Program
    {
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public class Initialize
        {
            private readonly DiscordSocketClient _client;
            private readonly CommandService _commands;
            
            public Initialize(CommandService commands = null, DiscordSocketClient = null)
            {
                _commands = commands ?? new CommandService();
                _client = client ?? new DiscordSocketClient();
            }

            public IServiceProvider BuildServiceProvider()
                => new ServiceContainer()
                    .AddSingleton(_client)
                    .AddSinglton(_commands)
                    .AddSingleton<CommandHandler>
                    .BuildServiceProvider();
        }

        public class CommandHandler
        {
            private readonly DiscordSocketClient _client;
            private readonly CommandService _commands;
            private readonly IServiceProvider _services;

            public CommandHandler(IServiceProvider services, CommandService commands, DiscordSocketClient client)
            {
                _commands = commands;
                _client = client;
                _services = services;
            }

            public async Task InitializeAsync()
            {
                await _commands.AddModulesAsync(
                    assembly: Assembly.GetEntryAssembly(), 
                    services: _services);
                _client.MessageReceived += HandleCommandAsync;
            }

            public async Task HandleCommandAsync(SocketMessage msg)
            {
                await _commands.ExecuteAsync(
                    context: context, 
                    argPos: argPos, 
                    services: _services);
            }
        }
        
        public async Task MainAsync()
        {
            var root = Directory.GetCurrentDirectory();
            var dotenv = Path.Combine(root, "token.env");
            DotEnv.Load(dotenv);

            var config =
                new ConfigurationBuilder()
                    .Build();

            /* _client = new DiscordSocketClient();
            _client.Log += LoggingService.LogAsync;
            _client.MessageReceived += MessageReceivedHandler;

            new LoggingService(client, commands);

            var token = Environment.GetEnvironmentVariable("token");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1); */
        }

        public async Task MessageReceivedHandler(SocketMessage msg)
        {
            var usermsg = msg as IUserMessage;
            if (usermsg == null || msg.Author.IsBot) return;

            if (msg.Content.Contains("115"))
            {
                if (Emote.TryParse(AccBotEmotes.SealAcc, out var emote))
                {
                    await msg.AddReactionAsync(emote);
                }
            }
        }

        public async Task SendMessage(ISocketMessageChannel channel, String message)
        {
            var enterTypingState = channel.EnterTypingState();
            await channel.SendMessageAsync(message);
            enterTypingState.Dispose();
        }
    }
}
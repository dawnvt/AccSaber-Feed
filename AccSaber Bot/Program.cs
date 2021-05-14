using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading.Channels;
using AccSaber_Bot;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Logging;

namespace AccSaber_Feed
{
    class Program
    {
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public DiscordSocketClient _client;

        public async Task MainAsync()
        {
            var root = Directory.GetCurrentDirectory();
            var dotenv = Path.Combine(root, "token.env");
            DotEnv.Load(dotenv);

            var config =
                new ConfigurationBuilder()
                    .Build();

            _client = new DiscordSocketClient();
            _client.Log += LoggingService.LogAsync;
            _client.MessageReceived += MessageReceivedHandler;

            // new LoggingService(_client, _commands);

            var token = Environment.GetEnvironmentVariable("token");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
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
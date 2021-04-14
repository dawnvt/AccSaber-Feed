using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
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

        private DiscordSocketClient _client;

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

           // new LoggingService(_client, null);

            var token = Environment.GetEnvironmentVariable("token");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }
    }
}
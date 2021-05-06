using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;



namespace Logging
{
    public class LoggingService
    {

        // DEPENDENCY INJECTION IN CONSTRUCTOR
        public LoggingService(DiscordSocketClient _client, CommandService _command)
        {
            _client.Log += LogAsync;
            _command.Log += LogAsync;
        }

        // CLASS METHODS
        public static Task LogAsync(LogMessage message)
        {
            if (message.Exception is CommandException cmdException)
            {
                Console.WriteLine($"[Command/{message.Severity}] {cmdException.Command.Aliases.First()}"
                    + $" failed to execute in {cmdException.Context.Channel}.");
                Console.WriteLine(cmdException);
            }
            else
                Console.WriteLine($"[General/{message.Severity}] {message}");
            return Task.CompletedTask;
        }
    }
}

using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;

public class LoggingService
{
    ServerQueryBot bot;
    public LoggingService(DiscordSocketClient client, CommandService command,ServerQueryBot bot)
    {
        client.Log += LogAsync;
        command.Log += LogAsync;
        this.bot = bot;
    }
    private Task LogAsync(LogMessage message)
    {
        if (message.Exception is HttpException httpException)
        {
            if(httpException.HttpCode == System.Net.HttpStatusCode.Unauthorized)
            {
                bot.InvalidToken();
            }
            else
            {
                Console.WriteLine($"[General/{message.Severity}] {message}");
            }
        }
        else if( message.Exception is CommandException cmdException)
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

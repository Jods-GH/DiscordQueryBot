using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Discord;
using Google.Protobuf;
using System.Collections.Concurrent;

public class PingService
{
    private ConcurrentDictionary<IUserMessage, ServerEmbed> PingerList;

    public PingService()
    {
        PingerList = new ConcurrentDictionary<IUserMessage, ServerEmbed>();
        Task.Run(() => doWork());
    }

    private async Task doWork()
    {
        bool moretodo = true;
        while (moretodo)
        {
            var rand = new Random();
            for (int i = 0; i <PingerList.Count;i++)
            {
                if (i < PingerList.Count) { 
                    KeyValuePair<IUserMessage, ServerEmbed> keyValuePair = PingerList.ElementAt(i);
                    Utility.PingChecker(keyValuePair.Key, keyValuePair.Value);
                }
                Thread.Sleep(rand.Next(10, 60));
            }
            Thread.Sleep(rand.Next(10, 60) * 1000);
        }
    }

    public void AddPinger(IUserMessage message, ServerEmbed serverEmbed)
    {
        String success = PingerList.TryAdd(message, serverEmbed) ? "Successfull" : "Failed";
        Console.WriteLine($"[INFO] {DateTime.Now.ToString("HH:mm:ss")} Setting up Pinger for {serverEmbed.ServerDomain}:{serverEmbed.ServerPort} {success}");
    }
    public void RemovePinger(ServerEmbed serverEmbed)
    {
        foreach (KeyValuePair<IUserMessage, ServerEmbed> keyValuePair in PingerList)
        {
            if (keyValuePair.Value.Equals(serverEmbed))
            {
                String success = PingerList.Remove(keyValuePair.Key, out serverEmbed) ? "Successfull" : "Failed";
                Console.WriteLine($"[INFO] {DateTime.Now.ToString("HH:mm:ss")} Canceling task for {serverEmbed.ServerDomain}:{serverEmbed.ServerPort} for message {serverEmbed.MessageID} {success}");       
            }
        }
    }

}

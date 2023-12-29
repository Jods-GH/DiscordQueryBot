using A2S.Structs;
using A2S;
using Discord;
using SteamStorefrontAPI.Classes;
using SteamStorefrontAPI;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text.Json;
using Jering.Javascript.NodeJS;
using LightDB;
using static JsonFileReader;

/// <summary>
/// The utility class for getting info
/// </summary>
public class Utility
{
    private static readonly object _lock = new object();

    public static async Task<SteamApp> getGameInfo(int GameId)
    {
        SteamApp steamApp = await AppDetails.GetAsync(GameId);
        return steamApp;
    }


    public static async Task PingChecker(IUserMessage message,ServerEmbed serverEmbed, CancellationTokenSource ct)
    {
        Console.WriteLine($"[INFO] {DateTime.Now.ToString("HH:mm:ss")} setting up embed updater for: " + serverEmbed.ServerDomain + ":" + serverEmbed.ServerPort);
        bool moretodo = true;
        while (moretodo)
        {
            try
            {
                ct.Token.ThrowIfCancellationRequested();
                ServerGameDigInfo? serverInfo = GetServerInfo(serverEmbed.ServerDomain, serverEmbed.ServerPort,serverEmbed.gamedig);
                if(serverEmbed.PlayeOnlineList == null)
                {
                    serverEmbed.PlayeOnlineList = new Dictionary<DateTime, int>();
                }
                if(serverEmbed.PlayeOnlineList.Count > 0)
                {
                    foreach (KeyValuePair<DateTime, int> timeAmount in serverEmbed.PlayeOnlineList)
                    {
                        TimeSpan difference = DateTime.Now - timeAmount.Key;
                        if (difference.TotalDays > 2)
                        {
                            serverEmbed.PlayeOnlineList.Remove(timeAmount.Key);
                        }
                    }
                }
                Embed embed;
                if (serverInfo.HasValue)
                {
                    if(serverInfo.Value.players.Length > 0)
                    {
                        serverEmbed.LastActivity = DateTime.Now;
                    }
                    serverEmbed.LastOnline = DateTime.Now;
                    embed = DiscordEmbedCreator.CreateEmbedOnline(serverInfo.Value, Color.Green, serverEmbed).Result;
                }
                else
                {
                    embed = DiscordEmbedCreator.CreateEmbedOffline(serverEmbed).Result;
                }
                DateTime nearestFullHour = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);

                if(serverEmbed.PlayeOnlineList.TryGetValue(nearestFullHour,out int value)){
                    if (value < serverInfo.Value.players.Length)
                    {
                        serverEmbed.PlayeOnlineList[nearestFullHour] = serverInfo.Value.players.Length;
                    }
                }
                else
                {
                    serverEmbed.PlayeOnlineList.Add(nearestFullHour, serverInfo.Value.players.Length);
                }
                
                ComponentBuilder builder = new();
                ButtonBuilder button = new();
                button.WithLabel("join");
                button.WithStyle(ButtonStyle.Link);
                button.WithUrl($"https://Irrenhaus.tech/Servers/{serverEmbed.ServerDomain}:{serverEmbed.ServerPort}");
                builder.WithButton(button);
                try
                {
                    await message.ModifyAsync(msg => { msg.Embed = embed; msg.Components = builder.Build(); });
                }
                catch
                {
                    Console.WriteLine($"[Watning] {DateTime.Now.ToString("HH:mm:ss")} Modifying message for {serverEmbed.ServerDomain}:{serverEmbed.ServerPort} unsuccesfull");
                }                
                Console.WriteLine($"[INFO] {DateTime.Now.ToString("HH:mm:ss")} Updated Server Info for {serverEmbed.ServerDomain}:{serverEmbed.ServerPort}");
                var rand = new Random();
                Thread.Sleep(rand.Next(10,60)*1000); 
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine($"[INFO] {DateTime.Now.ToString("HH:mm:ss")} Canceling task for {serverEmbed.ServerDomain}:{serverEmbed.ServerPort}");
                moretodo = false;
            }
        }
    }

    public static IPAddress GetIPAddress(string address)
    {
        IPAddress ip;
        if (!IPAddress.TryParse(address, out ip))
        {
            ip = Dns.GetHostEntry(address).AddressList.First(addr => addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
        }
        return ip;
    }


   public static ServerGameDigInfo? GetServerInfo(string address, int port, String? game)
    {
        try
        {
            lock (_lock)
            {

                ServerGameDigInfo? gamedigInfo = GetGameDigInfo(address, port, game);
                ServerGameDigInfo info = new();
                if (gamedigInfo.HasValue)
                {
                    info = gamedigInfo.Value;
                }
                else
                {
                    IPAddress ip = GetIPAddress(address);
                    ServerInfo result = Server.Query(ip.ToString(), port, 10);
                    info.name = result.Name;
                    info.ping = (int)PingHost(address);
                    info.maxplayers = result.MaxPlayers;
                    info.map = result.Map;
                    info.gameID = result.GameId;
                    info.game = result.Game;
                    info.port = result.Port;
                    ServerPlayer[] players = new ServerPlayer[result.Players];
                    for (int i = 0; i <result.Players; i++)
                    {
                        ServerPlayer player = new();
                    }
                    info.players = players;
                  
                }
               
                return info;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[INFO] {DateTime.Now.ToString("HH:mm:ss")} Server detected as offline: {address}:{port}");
            return null;
        }
    }


    public static ServerGameDigInfo? GetGameDigInfo(string address, int port, String game)
    {
        JsonDocument gamedig;
        ServerGameDigInfo info;
        GameOption GameOption = JsonFileReader.Read<GameOption>("GameOption.json");
        try
        {
            
            var attempts = 3;
            gamedig = JsonDocument.Parse(StaticNodeJSService.InvokeFromFileAsync<String>("query.js", args: new string[]
                {
                        /* in case you're running the bot in the same system or internal NAT
                         * ex: i run the bot in a public facing VPS that acts as router with NAT,
                         * the game servers use VPN to connect to the VPS and get public ip with port forwarding
                         * 
                         * ex architecture:
                         * ~~~~~~~~~~~~~~~~~INTERNET~~~~~~~~~~~~~~~~
                         * ----------------Public IP----------------
                         * --------------------|--------------------
                         * -------------------VPS-------------------
                         * ------------NAT & Port Forward-----------
                         * -------------------=|=-------------------
                         * -------v=======Internal VPN======v-------
                         * ------=|=----------=|=----------=|=------
                         * --GameServer1--GameServer2--GameServer3--
                         */
                        NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(
                            intf => intf.GetIPProperties().UnicastAddresses.FirstOrDefault(
                                ipinfo => ipinfo.Address.Equals(address)) != null) == null ? address : "127.0.0.1",
                        game,
                        port.ToString(),
                        attempts.ToString(),
                        (20000 / attempts).ToString("#")
                }).Result);
            if (gamedig == null)
            {
                return null;
            }
            else
            {
                info = gamedig.Deserialize<ServerGameDigInfo>();
                if (info.name == null)
                {
                    return null;
                }
                else
                {
                    info.port = port;
                    GameOption.Games.TryGetValue(game,out GameOptionGame gamevalue);
                    if(gamevalue!= null)
                    {
                        info.gameID = (ulong)gamevalue.id;
                        info.game = gamevalue.name;
                    }
                    
                }
            }
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.ToString(), $"Failed updating game server with name {{ {address}:{port}:{game} }}" +
                $"Exception : {exc}");
            return null;
        }
        return info;
    }



    public static long? PingHost(string nameOrAddress)
    {
        Ping pinger = null;
        long? ping = null;
        try
        {
            pinger = new Ping();
            PingReply reply = pinger.Send(nameOrAddress);
            ping = reply.RoundtripTime;
            PingReply reply2 = pinger.Send("steam.de");
            ping = ping + reply2.RoundtripTime;
        }
        catch (PingException)
        {
            // Discard PingExceptions and return false;
        }
        finally
        {
            if (pinger != null)
            {
                pinger.Dispose();
            }
        }
        return ping;
    }
}

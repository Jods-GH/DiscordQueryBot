using A2S;
using A2S.Structs;
using Discord;
using Google.Protobuf.WellKnownTypes;
using SteamStorefrontAPI;
using SteamStorefrontAPI.Classes;
using System;

public class DiscordEmbedCreator
{
    public static async Task<Embed> SetupEmbed(ServerGameDigInfo serverInfo, Color color, string domain, int port)
    {
        EmbedFooterBuilder embedFooterBuilder = new EmbedFooterBuilder();
        SteamApp steamApp = Utility.getGameInfo((int)serverInfo.gameID).Result;
        embedFooterBuilder.Text = serverInfo.game;

        var embed = new EmbedBuilder
        {
            // Embed property can be set within object initializer
            Title = serverInfo.name,
            Description = serverInfo.map,
            Footer = embedFooterBuilder,
            Color = color,
            Url = "http://Irrenhaus.tech",
            Timestamp = DateTime.UtcNow,
            ThumbnailUrl = steamApp.HeaderImage
        };
        //Your embed needs to be built before it is able to be sent
        return embed.Build();
    }
    public static async Task<Embed> CreateEmbedOnline(ServerGameDigInfo serverInfo, Color color, ServerEmbed serverEmbed)
    {
        EmbedFooterBuilder embedFooterBuilder = new EmbedFooterBuilder();
        SteamApp steamApp = Utility.getGameInfo((int)serverInfo.gameID).Result;
        embedFooterBuilder.Text = serverInfo.game;
        embedFooterBuilder.IconUrl = steamApp.HeaderImage;
        List<EmbedFieldBuilder> fields = new();
        EmbedFieldBuilder field = new EmbedFieldBuilder();
        field.WithName("Players <:Gamer:1162738369151901796>");
        field.WithValue(serverInfo.players.Length + "/" + serverInfo.maxplayers);
        fields.Add(field);

        EmbedFieldBuilder linkField = new EmbedFieldBuilder();
        linkField.WithName("Connection Details");
        String IpText;
        if(serverInfo.port!= serverEmbed.ServerPort)
        {
            IpText = $"```{serverEmbed.ServerDomain}:{serverInfo.port}```Queryport: ({serverEmbed.ServerPort})";
        }
        else
        {
            IpText = $"```{serverEmbed.ServerDomain}:{serverInfo.port}```";
        }
        linkField.WithValue(IpText);
        linkField.IsInline = true;
        fields.Add(linkField);

        EmbedFieldBuilder map = new();
        map.WithName("Map :map:");
        map.WithValue($"```{serverInfo.map}```");
        map.IsInline = true; 
        fields.Add(map);

        EmbedFieldBuilder activity = new EmbedFieldBuilder();
        DateTimeOffset time = serverEmbed.LastActivity;
        activity.Name = "Server Stats <a:ServerStats:1162735659384049684>";
        long? ping = Utility.PingHost(serverEmbed.ServerDomain);
        if (ping.HasValue)
        {
            activity.Value = $"Last Activity: {new TimestampTag(time, TimestampTagStyles.Relative)} Ping :ping_pong: : {ping.Value} ms";
        }
        else
        {
            activity.Value = $"Last Activity: {new TimestampTag(time, TimestampTagStyles.Relative)}";

        }
        fields.Add(activity);
        var embed = new EmbedBuilder
        {
            // Embed property can be set within object initializer
            Title = serverInfo.name + " :green_circle:",
            Description = serverEmbed.AdditionalDescription,
            Fields = fields,
            Footer = embedFooterBuilder,
            Color = color,
            Url = "http://Irrenhaus.tech",
            Timestamp = DateTime.UtcNow,
            ThumbnailUrl = steamApp.HeaderImage
        };

        //Your embed needs to be built before it is able to be sent
        return embed.Build();
    }
    public static async Task<Embed> CreateEmbedOffline(ServerEmbed ServerEmbedData)
    {
        EmbedFooterBuilder embedFooterBuilder = new EmbedFooterBuilder();

        SteamApp steamApp = await AppDetails.GetAsync((int)ServerEmbedData.GameID);
        embedFooterBuilder.Text = steamApp.Name;

        List<EmbedFieldBuilder> fields = new();
        EmbedFieldBuilder field = new EmbedFieldBuilder();
        field.WithName("Offline");
        field.WithValue("Offline");
        fields.Add(field);

        EmbedFieldBuilder linkField = new EmbedFieldBuilder();
        linkField.WithName("Connection Details");
        String IpText = $"```{ServerEmbedData.ServerDomain}:{ServerEmbedData.ServerPort}```";
        linkField.WithValue(IpText);
        linkField.IsInline = true;
        fields.Add(linkField);

        EmbedFieldBuilder map = new();
        map.WithName("Map :map:");
        map.WithValue($"```{ServerEmbedData.Map}```");
        map.IsInline = true;
        fields.Add(map);

        EmbedFieldBuilder activity = new EmbedFieldBuilder();
        DateTimeOffset time = ServerEmbedData.LastOnline;
        activity.Name = "Server Stats <a:ServerStats:1162735659384049684>";
        activity.Value = $"Last Online: {new TimestampTag(time, TimestampTagStyles.Relative)}";
        fields.Add(activity);
        var embed = new EmbedBuilder
        {
            // Embed property can be set within object initializer
            Title = ServerEmbedData.Name+":red_circle:",
            Description = ServerEmbedData.AdditionalDescription,
            Fields = fields,
            Footer = embedFooterBuilder,
            Color = Color.Red,
            Url = "http://Irrenhaus.tech",
            Timestamp = DateTime.UtcNow,
            ThumbnailUrl = steamApp.HeaderImage
        };
        //Your embed needs to be built before it is able to be sent
        return embed.Build();
    }

}

using LiteDB;
using System;
using System.Security.Cryptography.X509Certificates;

/// <summary>
/// Summary description for Class1
/// </summary>
public class ServerEmbed
{
    [BsonField("MessageID")]
    public ulong MessageID { get; set; }
    [BsonField("ServerID")]
    public ulong ServerID { get; set; }
    [BsonField("ChannelID")]
    public ulong ChannelID { get; set; }
    [BsonField("ServerDomain ")]
    public string ServerDomain { get; set; }
    [BsonField("ServerPort")]
    public int ServerPort { get; set; }
    [BsonField("GameID")]
    public ulong GameID { get; set; }
    [BsonField("Name")]
    public string Name { get; set; }
    [BsonField("Map")]

    public string? gamedig { get; set; }
    [BsonField("gamedig")]
    public string Map { get; set; }
    [BsonField("MaxPlayers")]
    public int MaxPlayers { get; set; }

    [BsonField("LastActivity")]
    public DateTime LastActivity { get; set; }
    [BsonField("PlayerOnlineList")]

    public Dictionary<DateTime,int> PlayeOnlineList { get; set; }
    [BsonField("LastOnline")]
    public DateTime LastOnline { get; set; }
    [BsonField("AdditionalDescription")]
    public string AdditionalDescription { get; set; }

    public override string ToString()
    {
        return "MessageID: "+MessageID+", ServerID: "+ServerID+",ChannelID: "+ChannelID+", ServerDomain: "+ServerDomain+", ServerPort: "+ServerPort;
    }
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        else
        {
            ServerEmbed serverEmbed = (ServerEmbed)obj;
            if (serverEmbed.MessageID != MessageID)
            {
                return false;
            }
            if (serverEmbed.ServerDomain != ServerDomain)
            {
                return false;
            }
            if (serverEmbed.ServerPort != ServerPort)
            {
                return false;
            }
            if (serverEmbed.ServerID != ServerID)
            {
                return false;
            }
            if (serverEmbed.ChannelID != ChannelID)
            {
                return false;
            }
            return true;
        }
    }

}

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
    public string Map { get; set; }
    [BsonField("MaxPlayers")]
    public int MaxPlayers { get; set; }

    [BsonField("LastActivity")]
    public DateTime LastActivity { get; set; }
    [BsonField("LastOnline")]
    public DateTime LastOnline { get; set; }
    [BsonField("AdditionalDescription")]
    public string AdditionalDescription { get; set; }

    public override string ToString()
    {
        return "MessageID: "+MessageID+", ServerID: "+ServerID+",ChannelID: "+ChannelID+", ServerDomain: "+ServerDomain+", ServerPort: "+ServerPort;
    }

}

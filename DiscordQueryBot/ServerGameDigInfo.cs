using System;

public struct ServerGameDigInfo
{
    public String name { get; set; }
    public String map { get; set; }
    public String game { get; set; }
    public ulong gameID { get; set; }
    public bool password { get; set; }
    public int maxplayers { get; set; }
    public ServerPlayer[] players { get; set; }
    public ServerPlayer[] bots { get; set; }
    public int ping { get; set; }
    public int port { get; set; }
    public object raw { get; set; }

    public ServerGameDigInfo()
	{
	}
}

public class ServerPlayer
{
    public String name { get; set; }
    public object raw {  get; set; }
}
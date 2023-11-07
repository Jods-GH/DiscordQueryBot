using LiteDB;
using System;

/// <summary>
/// Summary description for Class1
/// </summary>
public class DataBaseHelper
{
    private LiteDatabase db;
    private ILiteCollection<ServerEmbed> collection;
	public DataBaseHelper()
	{
        db = new LiteDatabase(@"Embeds.db");
        // Get collection
        collection = db.GetCollection<ServerEmbed>("Embeds");
        // Create unique index in MessageID field
        collection.EnsureIndex(x => x.MessageID, true);
    }
    public void exit()
    {
        db.Dispose();
    }
    public void dropDatabase()
    {
        collection.DeleteAll();
    }
    public void addToDatabase(ServerEmbed embed)
    {
        collection.Insert(embed);
    }
    /// <summary>
    /// deletes entry from database.
    /// </summary>
    /// <param name="ID">Index is MessageID</param>
    public void removeFromDatabase(ulong ID)
    {
        collection.DeleteMany(x => x.MessageID == ID);
    }
    public bool deleteServer(string domain, int port)
    {
        return collection.DeleteMany(x => x.ServerDomain==domain&&x.ServerPort==port)>0;
    }

    public ServerEmbed getDatabaseEntry(ulong ID)
    {
        return collection.FindOne(x => x.MessageID == ID);
    }
    public List<ServerEmbed> getDatabaseEntry(string domain, int port)
    {
        List<ServerEmbed> result = new();
        foreach (ServerEmbed embed in collection.FindAll())
        {
            if(embed.ServerDomain==domain && embed.ServerPort == port)
            {
                result.Add(embed);
            }
        }
        return result;
    }

    public void editDatabaseEntry(ServerEmbed embed)
    {
        collection.Update(embed);
    }

    public List<ServerEmbed> getAllEntrys()
    {
        List<ServerEmbed> list = collection.FindAll().ToList();
        return list;
    }

}

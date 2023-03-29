using System.Xml;
using System.Xml.Serialization;

namespace RssReaders;

internal class Program
{
    private static HttpClient client = new HttpClient();
    
    static void Main(string[] args)
    {
        //Console.WriteLine(Numbers()[1]);
        //var ietms = Numbers();
        foreach(int nr in Numbers())
        {
            Console.WriteLine(nr);
        }

        client.BaseAddress = new Uri("https://nu.nl/rss/");
        var stream = client.GetStreamAsync("").Result;
        foreach (var item in GetFeed(stream))
        {
            Console.WriteLine(item.Title);
            Console.WriteLine(item.Description);
            Console.WriteLine("==========================================");
        }

        Console.ReadLine();
    }

    static IEnumerable<int> Numbers()
    {
        Console.WriteLine("First");
        yield return 1;
        Console.WriteLine("Second");
        yield return 2;
        Console.WriteLine("Third");
        yield return 3;
    }
    private static IEnumerable<Item?> GetFeed(Stream data)
    {
        XmlSerializer ser = new XmlSerializer(typeof(Item));
        XmlReader rdr = XmlReader.Create(data);
       // List<Item> items = new List<Item>();
        while (rdr.ReadToFollowing("item"))
       {
            yield return ser.Deserialize(rdr) as Item;
            //items.Add(ser.Deserialize(rdr) as Item);
        }
        //return items;
    }
}
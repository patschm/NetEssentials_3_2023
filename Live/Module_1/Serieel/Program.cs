using System.Xml;
using System.Xml.Serialization;

namespace Serieel;

internal class Program
{
    static void Main(string[] args)
    {
        var p1 = new Person
        {
            Id = 1,
            FirstName = "Test",
            LastName = "Last Test",
            Age = 27,
        };

        Serialize(p1);
        Person p2 = Deserialize();
        Console.WriteLine(p2.LastName) ;
    }

    private static Person Deserialize()
    {
        var fs = File.OpenRead(@"D:\Files\custom.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(Person));
        var p =serializer.Deserialize(fs) as Person;
        return p;
    }

    private static void Serialize(Person p1)
    {
        var fs = File.Create(@"D:\Files\custom.xml");
        var writer=XmlWriter.Create(fs);

        XmlSerializer serializer = new XmlSerializer(typeof(Person));
        serializer.Serialize(writer, p1);
        writer.Flush();
        writer.Close();
    }
}
using System.IO.Compression;
using System.Text;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;

namespace Stroming;

internal class Program
{
    static void Main(string[] args)
    {
        //ThoughWriting();
        //ThoughReading();

        //EasyWriter();
        //EasyReader();

        //EasyWriterZip();
        //EasyReaderZip();

        WriteXml();

        Console.WriteLine("Done");
    }

    private static void WriteXml()
    {
        //Stream fs = File.Create(@"D:\Files\person.xml");
        //XmlWriter writer = XmlWriter.Create(fs);

        //writer.WriteStartElement("people");
        //writer.WriteAttributeString("id", "1");
        //    writer.WriteStartElement("first-name");
        //        writer.WriteString("Kees");
        //    writer.WriteEndElement();
        //writer.WriteEndElement();

        //writer.Flush();
        // writer.Close();

        var fs = File.OpenRead(@"D:\Files\person.xml");
        XmlReader rdr = XmlReader.Create(fs);
        if (rdr.ReadToFollowing("first-name"))
        {
            //rdr.MoveToContent();
            //var rdr2 = rdr.ReadSubtree();
            //Console.WriteLine(rdr2.ReadInnerXml()) ;
            var data = rdr.ReadElementContentAsString();
            Console.WriteLine(data);
        }


        
        
        
        
       


    }

    private static void EasyReaderZip()
    {
        Stream fs = File.OpenRead(@"D:\Files\zipperdy.zip");
        GZipStream zipper = new GZipStream(fs, CompressionMode.Decompress);
        StreamReader reader = new StreamReader(zipper);
        string? line = "";

        while ((line = reader.ReadLine()) != null)
        {
            Console.WriteLine(line);
        }

        reader.Close();
    }

    private static void EasyWriterZip()
    {
        Stream fs = File.Create(@"D:\Files\zipperdy.zip");
        GZipStream zipper = new GZipStream(fs, CompressionMode.Compress);
        StreamWriter writer = new StreamWriter(zipper);
        for (int i = 0; i < 1000; i++)
        {
            writer.WriteLine($"Hello World {i}");
        }
        writer.Flush();
        writer.Close();

    }
    private static void EasyReader()
    {
        Stream fs = File.OpenRead(@"D:\Files\easy.txt");
        StreamReader reader = new StreamReader(fs);
        string? line = "";

        while ((line = reader.ReadLine()) != null)
        {
            Console.WriteLine(line);
        }

        reader.Close();
    }

    private static void EasyWriter()
    {
        Stream fs = File.Create(@"D:\Files\easy.txt");
        StreamWriter writer = new StreamWriter(fs);
        for (int i = 0; i < 1000; i++)
        {
            writer.WriteLine($"Hello World {i}");
        }
        writer.Flush();
        writer.Close();
        //fs.Flush();
       // fs.Close();
    }

    private static void ThoughWriting()
    {
        string text = "Hello World ";
        Stream fs = File.Create(@"D:\Files\hard.txt");

        for (int i = 0; i < 1000; i++)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text + i + "\r\n");
            fs.Write(buffer, 0, buffer.Length);
        }

        fs.Flush();
        fs.Close();
    }
    private static void ThoughReading()
    {
        Stream fs = File.OpenRead(@"D:\Files\hard.txt");
        byte[] bucket = new byte[13];

        int nrRead = 0;
        while ((nrRead = fs.Read(bucket, 0, bucket.Length)) > 0)
        {
            string data = Encoding.UTF8.GetString(bucket);
            Console.Write(data);
            //Console.Write($"({nrRead})");
            Array.Clear(bucket, 0, bucket.Length);
        }

        fs.Close();
        Console.WriteLine();
    }
}
using System.IO;


namespace FileSystem;

internal class Program
{
    static void Main(string[] args)
    {
        //StaticGroup();
        //InstanceGroup();
        WatchFolder();
    }

    private static void WatchFolder()
    {
        Directory.CreateDirectory(@"D:\Files");
        var watcher = new FileSystemWatcher();
        watcher.Path = @"D:\Files";
        watcher.Created += (s, e) =>
        {
            Console.WriteLine($"{e.ChangeType}: {e.FullPath}");
        };
        watcher.Changed += (s, e) =>
        {
            Console.WriteLine($"{e.ChangeType}: {e.FullPath}");
        };
        watcher.Deleted += (s, e) =>
        {
            Console.WriteLine($"{e.ChangeType}: {e.FullPath}");
        };

        watcher.EnableRaisingEvents = true;

        Console.WriteLine("Waiting....");
        Console.ReadLine();
    }

  
    private static void InstanceGroup()
    {
        var file = new FileInfo(@"D:\testfile.txt");
        if (file.Exists ) {
            var attribs = file.Attributes;
            Console.WriteLine(attribs);
            file.Delete();
        }
        else
        {
            file.Create().Close();
        }
    }

    private static void StaticGroup()
    {
        var fileName = @"D:\testfile.txt";

        if (File.Exists(fileName))
        {
            var attribs = File.GetAttributes(fileName);
            Console.WriteLine(attribs);
            File.Delete(fileName);
        }
        else
        {
            File.Create(fileName).Close();
        }
    }
}
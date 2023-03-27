namespace Garbage;

internal class Unmanaged : IDisposable
{
    private static bool _isOpen = false;
    private FileStream? _file = null;

    public void Open()
    {
        if (!_isOpen)
        {
            _isOpen = true;
            _file = File.Create(@"D:\Files\dump.txt");
            Console.WriteLine("File is open");
        }
        else
        {
            Console.WriteLine("Ooops! Already open.");
        }
    }
    public void Close()
    {
        _isOpen = false;
        Console.WriteLine("File is closed");
    }

    protected void CleanUp(bool fromDispose)
    {
        Close();
        if (fromDispose)
        {
            _file?.Dispose();
        }
    }
    public void Dispose()
    {
     CleanUp(true);
        GC.SuppressFinalize(this);
    }

    ~Unmanaged()
    {
        CleanUp(false);
    }
}

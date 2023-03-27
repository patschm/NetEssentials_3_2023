namespace Garbage;

internal class Program
{
    static Unmanaged m1 = new Unmanaged();
    static Unmanaged m2 = new Unmanaged();  

    static void Main(string[] args)
    {
        m1.Open();
        //m1.Close();
        m1.Dispose();
        m1 = null;

        //GC.Collect();
        //GC.WaitForPendingFinalizers();

        m2.Open();
        //m2.Close();
        m2.Dispose();
        m2 = null;

        GC.Collect();
        Console.ReadLine();
    }
}
namespace Garbage;

internal class Program
{
    static Unmanaged m1 = new Unmanaged();
    static Unmanaged m2 = new Unmanaged();  

    static void Main(string[] args)
    {
        try
        {
            m1.Open();
        }
        finally
        {
            m1.Dispose();
        }
        
        ////m1.Close();
        //m1.Dispose();
        m1 = null;

        //GC.Collect();
        //GC.WaitForPendingFinalizers();

        using (m2)
        {
            m2.Open();
        }
        //m2.Close();
        //m2.Dispose();
        m2 = null;

        using (var m3  = new Unmanaged())
        {
            m3.Open();
        }

        using (var m4 = new Unmanaged())
            m4.Open();


        GC.Collect();
        Console.ReadLine();
    }
}
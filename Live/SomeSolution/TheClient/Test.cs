

namespace TheClient;

[Obsolete("This class will be removed in future", false)]
[MyThing(Text = "Hello", Age = 5)]
internal class Test
{
    public void Work()
    {
        Console.WriteLine(  "Works");
    }
}

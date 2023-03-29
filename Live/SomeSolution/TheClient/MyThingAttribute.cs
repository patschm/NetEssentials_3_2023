
namespace TheClient;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
internal class MyThingAttribute : Attribute
{
    public string Text { get; set; }
    public int Age { get; set; }
}

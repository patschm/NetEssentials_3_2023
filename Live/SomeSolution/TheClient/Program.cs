//using SomeLibrary;
using System.Reflection;

namespace TheClient;

internal class Program
{
    static void Main(string[] args)
    {
        Test t1 = new Test();
        DoSomething(t1);
        Assembly asm = Assembly.LoadFrom("D:\\.NET Essentials\\NetEssentials_3_2023\\Live\\SomeLibrary.dll");
        Console.WriteLine(asm.FullName);

        foreach(Type t in asm.GetTypes())
        {
            Console.WriteLine(t.FullName);
            Console.WriteLine(t.BaseType.FullName);
        }

        Type? tp = asm.GetType("SomeLibrary.Person");
        Console.WriteLine(tp.FullName);

        foreach(MemberInfo m in tp.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic))
        {
            Console.WriteLine(m.Name);
        }

        dynamic p2 = Activator.CreateInstance(tp);

        p2.FirstName = "Pieter";
        p2.LastName = "Janssen";
        p2.Age = 56;

        p2.Introduce();


        object? p1 = Activator.CreateInstance(tp);

        PropertyInfo pFirst =  tp.GetProperty("FirstName");
        pFirst.SetValue(p1, "Kees");

        MethodInfo mIntro = tp.GetMethod("Introduce");
        mIntro.Invoke(p1, new object[] { });

        FieldInfo mAge = tp.GetField("age", BindingFlags.Instance | BindingFlags.NonPublic);
        mAge.SetValue(p1, -42);

        mIntro.Invoke(p1, new object[] { });

        //Person p = new Person
        //{
        //    FirstName = "Jan",
        //    LastName = "de Vries",
        //    Age = 43
        //};

        //p.Introduce();
    }

    private static void DoSomething(Test t1)
    {
        var attr = t1.GetType().GetCustomAttribute<MyThingAttribute>();
        if (attr.Age >= 18 && attr.Age < 65)
            t1.Work();
        else
            Console.WriteLine("Invalid Age");
    }
}
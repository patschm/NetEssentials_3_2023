namespace SomeLibrary;

public class Person
{
	private int age;

	public int Age
	{
		get { return age; }
		set 
		{ 
			if (value >= 0 && value <=123)
				age = value; 
		}
	}

	public string? FirstName { get; set; }
    public string? LastName { get; set; }

	public void Introduce()
	{
        Console.WriteLine($"Hello, I'm {FirstName} {LastName} and I'm {Age} year(s) old");
    }
}
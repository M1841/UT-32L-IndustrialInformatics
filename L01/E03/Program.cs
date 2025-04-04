namespace L01.E03;

public class Program
{
  public static void Main()
  {
    Console.Write("Enter a temperature and measurement unit (ex: 24C, 98F): ");
    string input = Console.ReadLine()!;

    char unit = input.ToUpper().Last();
    int value = int.Parse(input[0..^1]);

    switch (unit)
    {
      case 'C':
        Console.WriteLine($"{input} = {CelsiusToFahrenheit(value)}F");
        break;
      case 'F':
        Console.WriteLine($"{input} = {FahrenheitToCelsius(value)}C");
        break;
      default:
        Console.WriteLine("Unit must be 'C' for Celsius or 'F' for Fahrenheit");
        break;
    }
  }

  private static int CelsiusToFahrenheit(int degrees) => degrees * 9 / 5 + 32;
  private static int FahrenheitToCelsius(int degrees) => (degrees - 32) * 5 / 9;
}

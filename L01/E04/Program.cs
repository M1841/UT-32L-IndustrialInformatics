namespace L01.E04;

public class Program
{
  public static void Main()
  {
    Console.Write("Enter your height (cm): ");
    int height = int.Parse(Console.ReadLine()!);

    Console.Write("Enter your age (years): ");
    int age = int.Parse(Console.ReadLine()!);

    Console.Write("Enter your sex (F or M): ");
    char sex = Console.ReadLine()![0];

    new WeightCalculator(height, age, sex).PrintWeight();
  }
}

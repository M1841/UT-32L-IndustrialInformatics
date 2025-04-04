namespace L01.E02
{
  public class Program
  {
    public static void Main()
    {
      try
      {
        Console.WriteLine("Type two numbers:");

        Console.Write("a = ");
        double a = double.Parse(Console.ReadLine()!);

        Console.Write("b = ");
        double b = double.Parse(Console.ReadLine()!);

        Console.WriteLine("\nResults:");
        BinaryCalculator.PerformAll(a, b);
      }
      catch (Exception ex)
      {
        Console.Error.WriteLine(ex.Message);
      }
    }
  }
}

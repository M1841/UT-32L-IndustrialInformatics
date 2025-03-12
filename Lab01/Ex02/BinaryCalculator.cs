namespace Ex02
{
  public class BinaryCalculator
  {
    public static double Add(double a, double b) => a + b;
    public static double Subtract(double a, double b) => a - b;
    public static double Multiply(double a, double b) => a * b;
    public static double Divide(double a, double b) => a / b;

    public static void PerformAll(double a, double b)
    {
      Console.WriteLine($"{a} + {b} = {Add(a, b)}");
      Console.WriteLine($"{a} - {b} = {Subtract(a, b)}");
      Console.WriteLine($"{a} * {b} = {Multiply(a, b)}");
      Console.WriteLine($"{a} / {b} = {Divide(a, b)}");
    }
  }
}

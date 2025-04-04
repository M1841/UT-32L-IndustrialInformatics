namespace L01.E01
{
  public class Program
  {
    public static void Main()
    {
      int[] sequence = Fibonacci(5);

      foreach (int item in sequence)
      {
        Console.Write(item + " ");
      }
      Console.WriteLine();
    }

    private static int[] Fibonacci(int n)
    {
      if (n <= 0) { return [0]; }
      if (n == 1) { return [0, 1]; }

      int[] previous = Fibonacci(n - 1);
      return [.. previous, previous[^1] + previous[^2]];
    }
  }
}

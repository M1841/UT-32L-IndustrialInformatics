namespace L01.E05;

public class Program
{
  public static void Main()
  {
    Console.WriteLine("Enter a sequence of numbers, separated by spaces:");
    string input = Console.ReadLine()!;
    int[] nums = input.Split(' ').Select(int.Parse).ToArray();

    double arithmeticAvg = 0;
    double geometricAvg = 1;

    foreach (int num in nums)
    {
      arithmeticAvg += num;
      geometricAvg *= num;
    }

    arithmeticAvg /= nums.Length;
    geometricAvg = double.RootN(geometricAvg, nums.Length);

    Console.WriteLine($"Arithmetic average: {arithmeticAvg:0.##}");
    Console.WriteLine($"Geometric average: {geometricAvg:0.##}");
  }
}

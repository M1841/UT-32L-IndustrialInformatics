namespace L01.E04;

public class WeightCalculator(int height, int age, char sex)
{
  public int ComputeMale()
  {
    return (int)Math.Round(
      Height - 100 - ((Height - 150) / 4.0) + ((Age - 20) / 4.0));
  }

  public int ComputeFemale()
  {
    return (int)Math.Round(
      Height - 100 - ((Height - 150) / 2.5) + ((Age - 20) / 6.0));
  }

  public void PrintWeight()
  {
    int idealWeight = Sex == 'F' ? ComputeFemale() : ComputeMale();
    Console.WriteLine($"Ideal weight = {idealWeight}");
  }

  public int Height { get; set; } = height;
  public int Age { get; set; } = age;
  public char Sex { get; set; } = sex;
}

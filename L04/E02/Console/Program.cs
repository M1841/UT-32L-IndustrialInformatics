HttpClient http = new()
{
  BaseAddress = new Uri("http://localhost:5007")
};

Console.Write("Temperature in Fahrenheit: ");
double F = double.Parse(Console.ReadLine()!);
using (HttpResponseMessage response =
  await http.GetAsync($"ftoc?degrees={F}"))
{
  string _C = await response.Content.ReadAsStringAsync();
  Console.WriteLine($"{F}°F = {_C}°C");
}

Console.Write("\nTemperature in Celsius: ");
double C = double.Parse(Console.ReadLine()!);
using (HttpResponseMessage response =
  await http.GetAsync($"ctof?degrees={C}"))
{
  string _F = await response.Content.ReadAsStringAsync();
  Console.WriteLine($"{C}°C = {_F}°F");
}

using (HttpResponseMessage response =
  await http.GetAsync("now"))
{
  string _now = await response.Content.ReadAsStringAsync();
  Console.WriteLine($"\nCurrent time: {_now}");
}

using (HttpResponseMessage response =
  await http.GetAsync("numbers"))
{
  string[] _nums = (await response.Content
    .ReadAsStringAsync())[1..^1]
    .Split(',');
  Console.WriteLine("\nToday's lucky numbers:");
  foreach (string _num in _nums)
  {
    Console.WriteLine(_num);
  }
}

Console.Write("\nBalance in RON: ");
double RON = double.Parse(Console.ReadLine()!);
using (HttpResponseMessage response =
  await http.GetAsync($"rontoeur?amount={RON}"))
{
  string _EUR = await response.Content.ReadAsStringAsync();
  Console.WriteLine($"{RON} RON = {_EUR} EUR");
}

Console.Write("\nBalance in EUR: ");
double EUR = double.Parse(Console.ReadLine()!);
using (HttpResponseMessage response =
  await http.GetAsync($"eurtoron?amount={EUR}"))
{
  string _RON = await response.Content.ReadAsStringAsync();
  Console.WriteLine($"{EUR} EUR = {_RON} RON");
}

namespace L04.E02.WinForms;

public partial class Form1 : Form
{
  public Form1()
  {
    Controls.Add(leftGroup);

    Task.Run(async () =>
    {
      while (true)
      {
        using HttpResponseMessage response = await http.GetAsync("now");
        string now = await response.Content.ReadAsStringAsync();
        timeLabel.Text = now;
        Thread.Sleep(1000);
      }
    });
    leftGroup.Controls.Add(timeLabel);

    Task.Run(async () =>
    {
      using HttpResponseMessage response = await http.GetAsync("numbers");
      string[] nums = (await response.Content
        .ReadAsStringAsync())[1..^1]
        .Split(',');
      foreach (string num in nums)
      {
        numsList.Items.Add(num);
      }
    });
    leftGroup.Controls.Add(numsList);
    leftGroup.Controls.Add(new Label()
    {
      Text = "Today's lucky numbers",
      Location = new(15, 20),
      Width = 360
    });

    Controls.Add(rightGroup);

    rightGroup.Controls.Add(new Label()
    {
      Text = "Temperature Converter",
      Location = new(15, 20),
      Width = 360
    });
    rightGroup.Controls.Add(new Label()
    {
      Text = "°C",
      Location = new(170, 47),
      Width = 40,
      Height = 20,
    });
    rightGroup.Controls.Add(celsiusTextBox);
    celsiusTextBox.KeyUp += async (sender, args) =>
    {
      if (celsiusTextBox.Text == "")
      {
        fahrenheitTextBox.Text = "";
      }
      else
      {
        if (double.TryParse(celsiusTextBox.Text, out double C))
        {
          using HttpResponseMessage response =
            await http.GetAsync($"ctof?degrees={C}");
          string F = await response.Content.ReadAsStringAsync();
          fahrenheitTextBox.Text = F;
        }
        else
        {
          fahrenheitTextBox.Text = "NaN";
        }
      }
    };
    rightGroup.Controls.Add(new Label()
    {
      Text = "°F",
      Location = new(360, 47),
      Width = 40,
      Height = 20,
    });
    rightGroup.Controls.Add(fahrenheitTextBox);
    fahrenheitTextBox.KeyUp += async (sender, args) =>
    {
      if (fahrenheitTextBox.Text == "")
      {
        celsiusTextBox.Text = "";
      }
      else
      {
        if (double.TryParse(fahrenheitTextBox.Text, out double F))
        {
          using HttpResponseMessage response =
            await http.GetAsync($"ftoc?degrees={F}");
          string C = await response.Content.ReadAsStringAsync();
          celsiusTextBox.Text = C;
        }
        else
        {
          celsiusTextBox.Text = "NaN";
        }
      }
    };


    rightGroup.Controls.Add(new Label()
    {
      Text = "Currency Converter",
      Location = new(15, 90),
      Width = 360
    });
    rightGroup.Controls.Add(new Label()
    {
      Text = "RON",
      Location = new(170, 117),
      Width = 40,
      Height = 20,
    });
    rightGroup.Controls.Add(ronTextBox);
    ronTextBox.KeyUp += async (sender, args) =>
    {
      if (ronTextBox.Text == "")
      {
        eurTextBox.Text = "";
      }
      else
      {
        if (double.TryParse(ronTextBox.Text, out double RON))
        {
          using HttpResponseMessage response =
            await http.GetAsync($"rontoeur?amount={RON}");
          string EUR = await response.Content.ReadAsStringAsync();
          eurTextBox.Text = EUR;
        }
        else
        {
          eurTextBox.Text = "NaN";
        }
      }
    };
    rightGroup.Controls.Add(new Label()
    {
      Text = "EUR",
      Location = new(360, 117),
      Width = 40,
      Height = 20,
    });
    rightGroup.Controls.Add(eurTextBox);
    eurTextBox.KeyUp += async (sender, args) =>
    {
      if (eurTextBox.Text == "")
      {
        ronTextBox.Text = "";
      }
      else
      {
        if (double.TryParse(eurTextBox.Text, out double EUR))
        {
          using HttpResponseMessage response =
            await http.GetAsync($"eurtoron?amount={EUR}");
          string RON = await response.Content.ReadAsStringAsync();
          ronTextBox.Text = RON;
        }
        else
        {
          ronTextBox.Text = "NaN";
        }
      }
    };

    InitializeComponent();
  }

  private readonly GroupBox leftGroup = new()
  {
    Location = new(0, 0),
    Width = 400,
    Height = 450
  };
  private readonly Label timeLabel = new()
  {
    Location = new(20, 170),
    Width = 360
  };
  private readonly ListBox numsList = new()
  {
    Location = new(20, 45),
    Height = 120,
    Width = 360
  };

  private readonly GroupBox rightGroup = new()
  {
    Location = new(400, 0),
    Width = 400,
    Height = 450
  };
  private readonly TextBox celsiusTextBox = new()
  {
    Location = new(20, 45),
    Width = 150,
    Height = 20
  };
  private readonly TextBox fahrenheitTextBox = new()
  {
    Location = new(210, 45),
    Width = 150,
    Height = 20
  };
  private readonly TextBox ronTextBox = new()
  {
    Location = new(20, 115),
    Width = 150,
    Height = 20
  };
  private readonly TextBox eurTextBox = new()
  {
    Location = new(210, 115),
    Width = 150,
    Height = 20
  };

  private readonly HttpClient http = new()
  {
    BaseAddress = new Uri("http://localhost:5007")
  };
}

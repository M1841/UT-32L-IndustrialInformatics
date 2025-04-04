namespace L02.E04
{
  public partial class Booking : Form
  {
    public Booking()
    {
      Controls.Add(tabControl);
      tabControl.Controls.Add(elementsPage);
      tabControl.Controls.Add(radiosPage);
      elementsPage.Controls.Add(elementsList);
      radiosPage.Controls.Add(radioGroup1);
      radiosPage.Controls.Add(radioGroup2);
      radiosPage.Controls.Add(submitButton);

      foreach (string city in new string[] { "Cluj-Napoca", "Amsterdam", "New York City", "Tokyo" })
      {
        elementsList.Items.Add(city);
      }
      elementsList.Click += DisplayPicture;
      submitButton.Click += HandleSubmit;

      for (int i = 4; i > 0; i--)
      {
        radioGroup1.Controls.Add(new RadioButton()
        {
          Text = $"{i} Seat{(i > 1 ? "s" : "")}",
          Dock = DockStyle.Top,
        });
      }

      foreach (string item in new string[] { "First", "Business", "Premium", "Economy" })
      {
        radioGroup2.Controls.Add(new RadioButton()
        {
          Text = $"{item} Class",
          Dock = DockStyle.Top,
        });
      }

      InitializeComponent();
    }

    private readonly TabControl tabControl = new()
    {
      Dock = DockStyle.Fill,
    };
    private readonly TabPage elementsPage = new()
    {
      Text = "Destination",
    };
    private readonly TabPage radiosPage = new()
    {
      Text = "Ticket",
    };
    private readonly ListBox elementsList = new()
    {
      Width = 160,
      Height = 430,
    };
    private readonly GroupBox radioGroup1 = new()
    {
      Width = 400,
      Height = 320,
    };
    private readonly GroupBox radioGroup2 = new()
    {
      Width = 400,
      Height = 320,
      Location = new Point(400, 0)
    };
    private readonly Button submitButton = new()
    {
      Text = "Book",
      Location = new Point(360, 350),
      AutoSize = true,
      FlatStyle = FlatStyle.Popup,
    };

    private void DisplayPicture(object? sender, EventArgs e)
    {
      string? city = elementsList.SelectedItem?.ToString();
      if (city != null)
      {
        if (elementsPage.Controls.Count > 1)
        {
          elementsPage.Controls.RemoveAt(1);
        }
        Dictionary<string, string> paths = [];
        paths.Add("Cluj-Napoca", "cluj_napoca");
        paths.Add("Amsterdam", "amsterdam");
        paths.Add("New York City", "nyc");
        paths.Add("Tokyo", "tokyo");
        elementsPage.Controls.Add(new PictureBox()
        {
          Image = Image.FromFile($"img/{paths[city]}.jpg"),
          Location = new Point(160, 0),
          Width = 640,
          Height = 427,
        });

      }
    }

    private void HandleSubmit(object? sender, EventArgs e)
    {
      string? destination = elementsList.SelectedItem?.ToString();
      string? seats = null, flightClass = null;
      foreach (RadioButton item in radioGroup1.Controls)
      {
        if (item.Checked)
        {
          seats = item.Text;
          break;
        }
      }
      foreach (RadioButton item in radioGroup2.Controls)
      {
        if (item.Checked)
        {
          flightClass = item.Text;
          break;
        }
      }
      if (destination == null || seats == null || flightClass == null)
      {
        MessageBox.Show("You haven't selected options for all fields!", "Error", MessageBoxButtons.OK);
      }
      else
      {
        MessageBox.Show($"You are about to book {seats} on a {flightClass} Class flight to {destination}.", "Confirmation", MessageBoxButtons.OK);

      }
    }
  }
}

namespace T01
{
  public partial class Store : Form
  {
    public Store()
    {
      BackColor = Color.FromArgb(42, 42, 42);

      foreach (Album album in Albums)
      {
        albumsPage.Controls.Add(StoreItem(album));
      }

      tabControl.Controls.Add(albumsPage);
      tabControl.Controls.Add(cartPage);

      Controls.Add(tabControl);

      InitializeComponent();
    }

    private readonly TabControl tabControl = new()
    {
      Dock = DockStyle.Fill
    };
    private readonly TabPage albumsPage = new()
    {
      Text = "Albums",
      BackColor = Color.FromArgb(26, 26, 26),
      Dock = DockStyle.Fill
    };
    private readonly TabPage cartPage = new()
    {
      Text = "Cart",
      BackColor = Color.FromArgb(26, 26, 26),
      Dock = DockStyle.Fill
    };

    private GroupBox StoreItem(Album album)
    {
      GroupBox item = new()
      {
        Dock = DockStyle.Top,
        Height = 160,
      };

      Button buyButton = new()
      {
        AutoSize = true,
        Text = $"Add to Cart\n${album.Price:0.##}",
        ForeColor = Color.FromArgb(239, 239, 239),
        BackColor = Color.FromArgb(42, 42, 42),
        FlatStyle = FlatStyle.Popup,
        Dock = DockStyle.Right,
      };
      buyButton.Click += (sender, e) =>
      {
        if (!Cart.TryAdd(album.Id, 1))
        {
          Cart[album.Id]++;
        }
        cartPage.Controls.Clear();
        foreach (KeyValuePair<string, int> item in Cart.Reverse())
        {
          cartPage.Controls.Add(new Label()
          {
            Text = $"{item.Key} - {item.Value}",
            ForeColor = Color.FromArgb(239, 239, 239),
            Dock = DockStyle.Top
          });
        }
      };
      item.Controls.Add(buyButton);

      item.Controls.Add(new Label()
      {
        AutoSize = true,
        Text = $"{album.Title}\n{album.Year}",
        ForeColor = Color.FromArgb(239, 239, 239),
        Dock = DockStyle.Left
      });

      item.Controls.Add(new PictureBox()
      {
        Image = Image.FromFile($"img/{album.Id}.png"),
        Dock = DockStyle.Left,
        BackColor = Color.FromArgb(42, 42, 42),
        Width = 130,
        SizeMode = PictureBoxSizeMode.StretchImage
      });

      return item;
    }

    private readonly Album[] Albums = [
      new("All We Know Is Falling", 2005, 1, "awkif_2005"),
      new("Riot!",2007, 1,"r_2007"),
      new("Brand New Eyes", 2009, 1, "bne_2009"),
      new("Paramore", 2013, 1, "p_2013"),
      new("After Laughter", 2017, 1, "al_2017"),
      new("This Is Why", 2023, 1, "tiw_2023"),
    ];
    private readonly Dictionary<string, int> Cart = [];

    private record Album(
      string Title,
      int Year,
      double Price,
      string Id)
    { }
  }
}

/*
  Controls:
    1. GroupBox
    2. Button
    3. Label
    4. PictureBox
    5. TabControl
    6. TabPage
*/

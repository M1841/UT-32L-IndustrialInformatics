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

      checkoutButton.Click += (sender, e) =>
      {
        Albums = Albums.Select(a =>
        {
          a.Count = 0;
          return a;
        }).ToArray();
        ClearCart();
      };
      cartPage.Controls.Add(checkoutButton);
      ClearCart();

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
    private readonly Button checkoutButton = new()
    {
      AutoSize = true,
      Text = $"Checkout\n€{0:0.##}",
      ForeColor = Color.FromArgb(239, 239, 239),
      BackColor = Color.FromArgb(42, 42, 42),
      FlatStyle = FlatStyle.Popup,
      Dock = DockStyle.Bottom,
    };

    private void ClearCart()
    {
      cartPage.Controls.Clear();
      cartPage.Controls.Add(new Label()
      {
        Text = "Cart is empty",
        ForeColor = Color.FromArgb(239, 239, 239),
        Dock = DockStyle.Fill
      });
    }

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
        Text = $"Add to Cart\n€{album.Price:0.##}",
        ForeColor = Color.FromArgb(239, 239, 239),
        BackColor = Color.FromArgb(42, 42, 42),
        FlatStyle = FlatStyle.Popup,
        Dock = DockStyle.Right,
      };
      buyButton.Click += (sender, e) =>
      {
        Albums.Single(a => a.Id == album.Id).Count++;
        cartPage.Controls.Clear();
        foreach (Album item in Albums.Where(a => a.Count > 0))
        {
          cartPage.Controls.Add(new Label()
          {
            Text = $"{item.Title}: {item.Count} * €{item.Price} = €{item.Count * item.Price}",
            ForeColor = Color.FromArgb(239, 239, 239),
            Dock = DockStyle.Top
          });
        }
        checkoutButton.Text = $"Checkout\n€{Albums.Sum(a => a.Count * a.Price):0.##}";
        cartPage.Controls.Add(checkoutButton);
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

    private Album[] Albums = [
      new("All We Know Is Falling", 2005, 13.99, "awkif_2005", 0),
      new("Riot!",2007, 41.99,"r_2007", 0),
      new("Brand New Eyes", 2009, 33.99, "bne_2009", 0),
      new("Paramore", 2013, 13.99, "p_2013", 0),
      new("After Laughter", 2017, 41.99, "al_2017", 0),
      new("This Is Why", 2023, 35.99, "tiw_2023", 0),
    ];

    private class Album(
      string title,
      int year,
      double price,
      string id,
      int count)
    {
      public string Title { get; init; } = title;
      public int Year { get; init; } = year;
      public double Price { get; init; } = price;
      public string Id { get; init; } = id;
      public int Count { get; set; } = count;
    }
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

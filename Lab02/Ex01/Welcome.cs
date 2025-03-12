namespace Ex01
{
  public partial class Welcome : Form
  {
    public Welcome(string username)
    {
      InitializeComponent();
      label1.Text = $"Welcome {username}!";
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
      base.OnFormClosing(e);
      Application.Exit();
    }
  }
}

using System.Text.Json;

namespace Ex01
{
  public partial class Login : Form
  {
    public Login()
    {
      string content = File.ReadAllText("users.json");
      users = JsonSerializer.Deserialize<User[]>(content) ?? [];

      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      label3.Text = "";
      label4.Text = "";

      string username = textBox1.Text;
      string password = textBox2.Text;

      var user = users
        .Where(u => u.Username == username)
        .FirstOrDefault();

      if (user == null)
      {
        label3.Text = $"User {username} does not exist";
        return;
      }

      if (user.Password != password)
      {
        label4.Text = "Password is incorrect";
        return;
      }

      new Welcome(username).Show();
      Hide();
    }

    private readonly User[] users;

    private record User(
      string Username,
      string Password
    )
    { }
  }
}

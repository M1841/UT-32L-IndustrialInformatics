namespace L04.T00.Client;

public class UserForm : Form
{
  public UserForm()
  {
    InitializeComponent();
    Controls.Add(userLabel);
    Controls.Add(userTextbox);
    userTextbox.KeyUp += (sender, args) =>
    {
      continueBtn.Enabled = userTextbox.Text != "";
    };
    Controls.Add(continueBtn);
    continueBtn.Click += (sender, args) =>
    {
      if (userTextbox.Text != "")
      {
        Program.User = userTextbox.Text;
        new Threads().Show();
        Hide();
      }
    };
  }

  readonly Label userLabel = new()
  {
    Text = "Nickname",
    Location = new(10, 10)
  };
  readonly TextBox userTextbox = new()
  {
    Width = 200 - 30,
    Location = new(15, 35)
  };
  readonly Button continueBtn = new()
  {
    Text = "Continue",
    Enabled = false,
    Height = 30,
    Width = 200 - 30,
    Location = new(15, 80),
  };

  private System.ComponentModel.Container? components = null;
  protected override void Dispose(bool disposing)
  {
    if (disposing && (components != null))
    {
      components.Dispose();
    }
    base.Dispose(disposing);
  }
  private void InitializeComponent()
  {
    components = new System.ComponentModel.Container();
    AutoScaleMode = AutoScaleMode.Font;
    ClientSize = new Size(200, 130);
    Text = "User Form";
  }
}

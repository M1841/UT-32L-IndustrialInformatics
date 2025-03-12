namespace Ex04
{
  partial class Form1
  {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      BackColor = Color.FromArgb(42, 42, 42);
      this.Text = "Form1";
      Controls.Add(tabControl);
      tabControl.Controls.Add(elementsPage);
      tabControl.Controls.Add(radiosPage);
      elementsPage.Controls.Add(elementsList);
      radiosPage.Controls.Add(radioGroup1);
      radiosPage.Controls.Add(radioGroup2);
      radiosPage.Controls.Add(submitButton);

      foreach (string element in new string[] { "Cluj-Napoca", "Amsterdam", "New York City", "Tokyo" })
      {
        elementsList.Items.Add(element);
      }

      foreach (string item in new string[] { "C", "B", "A" })
      {
        radioGroup1.Controls.Add(new RadioButton()
        {
          Text = item,
          Dock = DockStyle.Top,
          ForeColor = Color.FromArgb(239, 239, 239)
        });
      }

      foreach (string item in new string[] { "3", "2", "1" })
      {
        radioGroup2.Controls.Add(new RadioButton()
        {
          Text = item,
          Dock = DockStyle.Top,
          ForeColor = Color.FromArgb(239, 239, 239)
        });
      }
    }

    private TabControl tabControl = new()
    {
      Dock = DockStyle.Fill,
    };
    private TabPage elementsPage = new()
    {
      BackColor = Color.FromArgb(26, 26, 26),
      Text = "Elements",
    };
    private TabPage radiosPage = new()
    {
      BackColor = Color.FromArgb(26, 26, 26),
      Text = "Radios",
    };
    private ListBox elementsList = new()
    {
      BackColor = Color.FromArgb(26, 26, 26),
      ForeColor = Color.FromArgb(239, 239, 239),
      Width = 400,
      Height = 430
    };
    private GroupBox radioGroup1 = new()
    {
      Width = 400,
      Height = 320,
    };
    private GroupBox radioGroup2 = new()
    {
      Width = 400,
      Height = 320,
      Location = new Point(400, 0)
    };
    private Button submitButton = new()
    {
      Text = "Submit",
      Location = new Point(370, 350),
      AutoSize = true,
      BackColor = Color.FromArgb(42, 42, 42),
      ForeColor = Color.FromArgb(239, 239, 239),
      FlatStyle = FlatStyle.Popup,
      UseVisualStyleBackColor = false
    };
  }
}

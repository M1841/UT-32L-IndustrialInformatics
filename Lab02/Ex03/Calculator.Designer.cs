namespace Ex03
{
  partial class Calculator
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
      BackColor = Color.FromArgb(26, 26, 26);
      this.ClientSize = new System.Drawing.Size(530, 70);
      Controls.Add(numBox1);
      Controls.Add(numBox2);
      Controls.Add(operatorMenu);
      Controls.Add(equalLabel);
      Controls.Add(resultBox);
      this.Text = "Calculator";
    }

    private TextBox numBox1 = new()
    {
      BackColor = Color.FromArgb(42, 42, 42),
      ForeColor = Color.FromArgb(239, 239, 239),
      BorderStyle = BorderStyle.FixedSingle,
      Location = new Point(20, 20)
    };
    private TextBox numBox2 = new()
    {
      BackColor = Color.FromArgb(42, 42, 42),
      ForeColor = Color.FromArgb(239, 239, 239),
      BorderStyle = BorderStyle.FixedSingle,
      Location = new Point(270, 20)
    };

    private MenuStrip operatorMenu = new()
    {
      BackColor = Color.FromArgb(42, 42, 42),
      ForeColor = Color.FromArgb(239, 239, 239),
      Location = new Point(130, 20),
      Dock = DockStyle.None
    };

    private Label equalLabel = new()
    {
      Text = "=",
      AutoSize = true,
      ForeColor = Color.FromArgb(239, 239, 239),
      Location = new Point(380, 20)
    };

    private TextBox resultBox = new()
    {
      BackColor = Color.FromArgb(42, 42, 42),
      ForeColor = Color.FromArgb(239, 239, 239),
      BorderStyle = BorderStyle.FixedSingle,
      Location = new Point(410, 20),
      ReadOnly = true
    };
  }
}

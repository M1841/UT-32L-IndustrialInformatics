namespace Ex02;

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
    SuspendLayout();

    exitButton.Click += HandleExit;
    copyButton.Click += HandleCopy;
    deleteButton.Click += HandleDelete;

    this.components = new System.ComponentModel.Container();
    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
    BackColor = Color.FromArgb(26, 26, 26);
    this.ClientSize = new System.Drawing.Size(600, 230);
    this.Text = "Form1";
    Controls.Add(listBox1);
    Controls.Add(listBox2);
    Controls.Add(copyButton);
    Controls.Add(deleteButton);
    Controls.Add(exitButton);
    ResumeLayout(false);
    PerformLayout();
  }

  private ListBox listBox1 = new()
  {
    BackColor = Color.FromArgb(42, 42, 42),
    ForeColor = Color.FromArgb(239, 239, 239),
    Location = new Point(50, y: 50),
    Width = 160,
    Height = 140,
    SelectionMode = SelectionMode.MultiExtended
  };
  private ListBox listBox2 = new()
  {
    BackColor = Color.FromArgb(42, 42, 42),
    ForeColor = Color.FromArgb(239, 239, 239),
    Location = new Point(390, y: 50),
    Width = 160,
    Height = 140,
    SelectionMode = SelectionMode.MultiExtended
  };
  private Button copyButton = new()
  {
    Text = "Copy",
    Location = new Point(260, 50),
    AutoSize = true,
    BackColor = Color.FromArgb(42, 42, 42),
    ForeColor = Color.FromArgb(239, 239, 239),
    FlatStyle = FlatStyle.Popup,
    UseVisualStyleBackColor = false
  };
  private Button deleteButton = new()
  {
    Text = "Delete",
    Location = new Point(260, 100),
    AutoSize = true,
    BackColor = Color.FromArgb(42, 42, 42),
    ForeColor = Color.FromArgb(239, 239, 239),
    FlatStyle = FlatStyle.Popup,
    UseVisualStyleBackColor = false
  };
  private Button exitButton = new()
  {
    Text = "Exit",
    Location = new Point(260, 150),
    AutoSize = true,
    BackColor = Color.FromArgb(42, 42, 42),
    ForeColor = Color.FromArgb(239, 239, 239),
    FlatStyle = FlatStyle.Popup,
    UseVisualStyleBackColor = false
  };

}

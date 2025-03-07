namespace Ex01 {
  partial class Login {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      label1 = new Label();
      textBox1 = new TextBox();
      label2 = new Label();
      textBox2 = new TextBox();
      button1 = new Button();
      label3 = new Label();
      label4 = new Label();
      SuspendLayout();
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(12, 9);
      label1.Name = "label1";
      label1.Size = new Size(75, 20);
      label1.TabIndex = 0;
      label1.Text = "Username";
      // 
      // textBox1
      // 
      textBox1.BackColor = Color.FromArgb(42, 42, 42);
      textBox1.BorderStyle = BorderStyle.FixedSingle;
      textBox1.Cursor = Cursors.IBeam;
      textBox1.ForeColor = Color.FromArgb(239, 239, 239);
      textBox1.Location = new Point(12, 32);
      textBox1.Name = "textBox1";
      textBox1.Size = new Size(254, 27);
      textBox1.TabIndex = 1;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(12, 92);
      label2.Name = "label2";
      label2.Size = new Size(70, 20);
      label2.TabIndex = 2;
      label2.Text = "Password";
      // 
      // textBox2
      // 
      textBox2.BackColor = Color.FromArgb(42, 42, 42);
      textBox2.BorderStyle = BorderStyle.FixedSingle;
      textBox2.Cursor = Cursors.IBeam;
      textBox2.ForeColor = Color.FromArgb(239, 239, 239);
      textBox2.Location = new Point(12, 115);
      textBox2.Name = "textBox2";
      textBox2.Size = new Size(254, 27);
      textBox2.TabIndex = 5;
      textBox2.UseSystemPasswordChar = true;
      // 
      // button1
      // 
      button1.AutoSize = true;
      button1.BackColor = Color.FromArgb(42, 42, 42);
      button1.Location = new Point(12, 182);
      button1.Name = "button1";
      button1.Size = new Size(254, 30);
      button1.TabIndex = 6;
      button1.Text = "Login";
      button1.UseVisualStyleBackColor = false;
      button1.Click += button1_Click;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.ForeColor = Color.FromArgb(200, 90, 100);
      label3.Location = new Point(12, 62);
      label3.Name = "label3";
      label3.Size = new Size(0, 20);
      label3.TabIndex = 7;
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.ForeColor = Color.FromArgb(200, 90, 100);
      label4.Location = new Point(12, 145);
      label4.Name = "label4";
      label4.Size = new Size(0, 20);
      label4.TabIndex = 8;
      // 
      // Login
      // 
      AutoScaleDimensions = new SizeF(8F, 20F);
      AutoScaleMode = AutoScaleMode.Font;
      BackColor = Color.FromArgb(26, 26, 26);
      ClientSize = new Size(278, 226);
      Controls.Add(label4);
      Controls.Add(label3);
      Controls.Add(button1);
      Controls.Add(textBox2);
      Controls.Add(label2);
      Controls.Add(textBox1);
      Controls.Add(label1);
      ForeColor = Color.FromArgb(239, 239, 239);
      Name = "Login";
      Text = "Login";
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Label label1;
    private TextBox textBox1;
    private Label label2;
    private TextBox textBox2;
    private Button button1;
    private Label label3;
    private Label label4;
  }
}

namespace L02.E03
{
  public partial class Calculator : Form
  {
    public Calculator()
    {
      Controls.Add(numBox1);
      Controls.Add(numBox2);
      Controls.Add(operatorMenu);
      Controls.Add(equalLabel);
      Controls.Add(resultBox);

      foreach (string op in new[] { "+", "-", "*", "/" })
      {
        ToolStripMenuItem item = new()
        {
          Text = op
        };
        item.Click += (sender, e) => Compute(op);
        operatorMenu.Items.Add(item);
      }
      InitializeComponent();
    }

    private readonly TextBox numBox1 = new()
    {
      BorderStyle = BorderStyle.FixedSingle,
      Location = new Point(20, 20)
    };
    private readonly TextBox numBox2 = new()
    {
      BorderStyle = BorderStyle.FixedSingle,
      Location = new Point(270, 20)
    };

    private readonly MenuStrip operatorMenu = new()
    {
      Location = new Point(130, 20),
      Dock = DockStyle.None
    };

    private readonly Label equalLabel = new()
    {
      Text = "=",
      AutoSize = true,
      Location = new Point(380, 20)
    };

    private readonly TextBox resultBox = new()
    {
      BorderStyle = BorderStyle.FixedSingle,
      Location = new Point(410, 20),
      ReadOnly = true
    };

    private void Compute(string op)
    {
      try
      {
        double num1 = double.Parse(numBox1.Text);
        double num2 = double.Parse(numBox2.Text);
        switch (op)
        {
          case "+":
            resultBox.Text = $"{num1 + num2:0.##}";
            break;
          case "-":
            resultBox.Text = $"{num1 - num2:0.##}";
            break;
          case "*":
            resultBox.Text = $"{num1 * num2:0.##}";
            break;
          case "/":
            resultBox.Text = $"{num1 / num2:0.##}";
            break;
        }
      }
      catch
      {
        resultBox.Text = "NaN";
      }
    }
  }
}

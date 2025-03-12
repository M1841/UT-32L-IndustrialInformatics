namespace Ex03
{
  public partial class Calculator : Form
  {
    public Calculator()
    {
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

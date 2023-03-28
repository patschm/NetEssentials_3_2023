namespace Calculator;

public partial class CalculatorApp : Form
{
    public CalculatorApp()
    {
        InitializeComponent();
    }

    private async  void button1_Click(object sender, EventArgs e)
    {
        //var threadHandle = SynchronizationContext.Current;
        if (int.TryParse(txtA.Text, out int a) && int.TryParse(txtB.Text, out int b))
        {
            // Synchronous
            //int result = LongAdd(a, b);
            //UpdateAnswer(result);

            // Using Tasks
            //Task.Run(()=>LongAdd(a, b))
            //    .ContinueWith(pt=> {
            //        threadHandle?.Post(UpdateAnswer, pt.Result);
            //    });

            //int result=await LongAddAsync(a,b);
            //UpdateAnswer(result);

            DoJob(a, b).Wait(); // Dead Lock
        }
    }

    private async Task DoJob(int a, int b)
    {
        int result = await LongAddAsync(a, b);
        UpdateAnswer(result);
    }

    private void UpdateAnswer(object result)
    {
        lblAnswer.Text = result.ToString();
    }

    private int LongAdd(int a, int b)
    {
        Task.Delay(10000).Wait();
        return a + b;
    }
    private Task<int> LongAddAsync(int a, int b)
    {
        return Task.Run(()=>LongAdd(a,b));
    }
}
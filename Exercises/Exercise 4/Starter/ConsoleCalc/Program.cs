using System.Threading.Channels;

namespace ConsoleCalc
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //SynchonousAdd();
            //AsynchonousAdd();
            AsynchonousAddLean();


            Console.WriteLine("The End");
            Console.ReadLine();
        }

        private static void AsynchonousAddLean()
        {
            var t1 = Task.Run(() => LongAdd(1, 2))
                .ContinueWith(pt=>Console.WriteLine(pt.Result)); 
        }

        private static void AsynchonousAdd()
        {
            //Func<int, int, int> del = LongAdd;
            //IAsyncResult ar = del.BeginInvoke(1, 2, null, null);
            //int result = del.EndInvoke(ar);

            var t1 = new Task<int>(() => {
                var result = LongAdd(1,2);
                return result;
            });

            t1.ContinueWith(previousTask =>
            {
                int result = previousTask.Result;
                Console.WriteLine(result);
                return 42;
            }).ContinueWith(pt => Console.WriteLine($"Hi! {pt.Result}"));

            t1.Start();

           
        }

        private static void SynchonousAdd()
        {
            var result = LongAdd(1,2);
            Console.WriteLine(result);
        }

        static int LongAdd(int a, int b)
        {
            Task.Delay(10000).Wait();
            return a + b;
        }
    }
}
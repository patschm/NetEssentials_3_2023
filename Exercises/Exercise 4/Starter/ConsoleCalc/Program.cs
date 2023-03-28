using System.Threading.Channels;

namespace ConsoleCalc
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //SynchonousAdd();
            //AsynchonousAdd();
            //AsynchonousAddLean();
            //AsynchronousWrongDoing();
            //AsyncTooLong();
            AsyncNice(42);

            Console.WriteLine("The End");
            Console.ReadLine();
        }

        private static async void AsyncNice(int x)
        {
            var t1 = Task.Run(() => LongAdd(1, 2));
            int result = await t1; // soft return;


            result = await LongAddAsync(5,6);
            Console.WriteLine(result);
            result += await Task.Run(() => LongAdd(11, 22));
            Console.WriteLine(result);
            Console.WriteLine($"The argument is {x}");
        }

        private static void AsyncTooLong()
        {

            var nikko = new CancellationTokenSource();
            CancellationToken bomb = nikko.Token;

            var x = Task.Run(() => { 
                for(; ; )
                {
                    Console.WriteLine("Again and ");
                    Task.Delay(500).Wait();
                    //if(bomb.IsCancellationRequested)
                    //{
                    //    Console.WriteLine("Kabooom");
                    //    return;
                    //}
                    bomb.ThrowIfCancellationRequested();
                }
            });

            Task.Delay(5000).Wait();
            //nikko.CancelAfter(10000);
            nikko.Cancel();
            Task.Delay(500).Wait();
            Console.WriteLine(x.Status);
        }

        private static void AsynchronousWrongDoing()
        {
            Task.Run(() =>
            {
                Console.WriteLine("Start");
                Task.Delay(1000).Wait();
                throw new Exception("Ooops");
            }).ContinueWith(pt => {
                Console.WriteLine(pt.Status);
                if (pt.Exception != null)
                    Console.WriteLine(pt.Exception?.InnerException?.Message);
            });


            try
            {
                Task.Run(() =>
                {
                    Console.WriteLine("Start");
                    Task.Delay(1000).Wait();
                    throw new Exception("Ooops");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
        static Task<int> LongAddAsync(int a, int b)
        {
            return Task.Run(() => LongAdd(a, b));
        }
    }
}
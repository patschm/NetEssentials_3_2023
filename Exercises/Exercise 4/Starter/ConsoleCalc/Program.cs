using System.Collections;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
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
            //AsyncNice(42);
            //TwoTasks();
            //ManyEvilDoers();
            //TheGarage();
            //Collections();

            MartinsFavorite();

            Console.WriteLine("The End");
            Console.ReadLine();
        }

        static Random rnd = new Random();
        private static void MartinsFavorite()
        {
            int[] nrs = new int[10];
            Task[] tasks = new Task[10];

            for(int i =0; i < tasks.Length; i++)
            {
                tasks[i] = new Task(() =>
                {
                    Console.WriteLine($"Number {i}");
                    nrs[i] = i + 1000;
                });
            }

            
            foreach (var ti in tasks)
            {
                ti.Start();
            }

            Task.Delay(1000).Wait();
            foreach(int nr in nrs)
            {
                Console.WriteLine(nr);
            }
            foreach (var ti in tasks)
            {
                Console.WriteLine(ti.Status);
                Console.WriteLine(ti.Exception.InnerException.Message);
            }
        }

        private static void Collections()
        {
            //List<int> list = new List<int>();
            ConcurrentBag<int> bag = new ConcurrentBag<int>();
            ConcurrentDictionary<string, int> dict;
          
            
            Parallel.For(0, 25, ind =>
            {
                //list.Add(ind);
                bag.Add(ind);
            });

            //foreach(var item in list)
            foreach (int item in bag)
            {
                Console.WriteLine(item);
            }
        }

        private static void TheGarage()
        {
            Random rnd = new Random();
            SemaphoreSlim garage = new SemaphoreSlim(10, 10);

            Parallel.For(0, 30, idx =>
            {
                Console.WriteLine($"Car {idx} arrives at the garage");
                garage.Wait();
                Console.WriteLine($"Car {idx} drives into the garage");
                Task.Delay(5000 + rnd.Next(5, 10) * 1000).Wait();
                Console.WriteLine($"Car {idx} leaves the garage");
                garage.Release();
                Console.WriteLine($"Car {idx} is out");
            });
        }

        static object stick = new object();
        private static void ManyEvilDoers()
        {
            int counter = 0;

            Parallel.For(0, 10, idx =>
            {
                Console.WriteLine($"Thread Index {idx}");
                lock (stick)
                {
                    int tmp = counter;
                    Task.Delay(100).Wait();
                    tmp++;
                    counter = tmp;
                    Console.WriteLine(counter);
                }
            });
        }

        private static async void TwoTasks()
        {
            int a = 0;
            int b = 0;

            var t1 = Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                a = 25;
            });
            var t2 = Task.Run(() =>
            {
                Task.Delay(2000).Wait();
                b = 17;
            });

            await Task.WhenAll(t1, t2);
            int resullt = a + b;
            Console.WriteLine(resullt);
        }

        private static async void AsyncNice(int x)
        {
            var t1 = Task.Run(() => LongAdd(1, 2));
            int result = await t1; // soft return;


            result = await LongAddAsync(5, 6);
            Console.WriteLine(result);
            result += await Task.Run(() => LongAdd(11, 22));
            Console.WriteLine(result);
            Console.WriteLine($"The argument is {x}");
        }

        private static void AsyncTooLong()
        {

            var nikko = new CancellationTokenSource();
            CancellationToken bomb = nikko.Token;

            var x = Task.Run(() =>
            {
                for (; ; )
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
            }).ContinueWith(pt =>
            {
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
                .ContinueWith(pt => Console.WriteLine(pt.Result));
        }

        private static void AsynchonousAdd()
        {
            //Func<int, int, int> del = LongAdd;
            //IAsyncResult ar = del.BeginInvoke(1, 2, null, null);
            //int result = del.EndInvoke(ar);

            var t1 = new Task<int>(() =>
            {
                var result = LongAdd(1, 2);
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
            var result = LongAdd(1, 2);
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
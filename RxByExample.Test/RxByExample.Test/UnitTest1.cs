using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RxByExample.Test
{
    using System.Reactive.Linq;
    using System.Threading;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Reactive.Disposables;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var xs = Observable.Interval(TimeSpan.FromSeconds(1));
            var ob = new MyConsole<long>();
            xs.Subscribe(
                i =>
                {
                    ob.OnNext(i);
                }
            );

        }

        /// <summary>
        /// Start - Run Code Asynchronously
        /// </summary>
        public static void StartBackgroundWork()
        {
            Console.WriteLine("Shows use of Start to start on a background thread:");
            var o = Observable.Start(() =>
            {
                //This starts on a background thread.
                Console.WriteLine("From background thread. Does not block main thread.");
                Console.WriteLine("Calculating...");
                Thread.Sleep(3000);
                Console.WriteLine("Background work completed.");
            }).Finally(() => Console.WriteLine("Main thread completed."));
            Console.WriteLine("\r\n\t In Main Thread...\r\n");
            o.Wait();   // Wait for completion of background operation.
        }

    }

    public class MyConsole<T> : IObserver<T>
    {

        public void OnCompleted()
        {
            Console.WriteLine("OnCompleted");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("OnError - " + error);
        }

        public void OnNext(T value)
        {
            Console.WriteLine("OnNext - " + value);
        }
    }

    public class Foo
    {
        // Synchronous operation
        public int DoLongRunningOperation(string param)
        {
            Console.WriteLine("Calculating...");
            Thread.Sleep(3000);
            Console.WriteLine("CalculatE completed");
            return 0;
        }

        public IObservable<int> LongRunningOperationAsync(string param)
        {
            return Observable.Create<int>(
                o => Observable.ToAsync<string, int>(DoLongRunningOperation)(param).Subscribe(o)
            );
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RxApp
{
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Threading;
    using System.Timers;
    using System.Reactive.Threading.Tasks;
    using NLog;
    using System.Reactive.Concurrency;

    static class Program
    {
        static IEnumerable<string> EndlessBarrageOfEmail()
        {
            var random = new Random();
            var emails = new List<String> { 
                "Here is an email!", 
                "Another email!", 
                "Yet another email!" };

            for (; ; )
            {
                // Return some random emails at random intervals.
                yield return emails[random.Next(emails.Count)];
                Thread.Sleep(random.Next(1000));
            }
        }

        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            Observable.Start(() =>
            {
                return 5;
            })
            .Subscribe(Console.WriteLine);

            Console.ReadKey();
        }

        static void Exmaple01()
        {
            var windowIdx = 0;
            var source = Observable.Interval(TimeSpan.FromSeconds(1)).Take(10);
            source.Window(3)
            .Subscribe(window =>
            {
                var id = windowIdx++;
                Console.WriteLine("--Starting new window");
                var windowName = "Window" + windowIdx;
                window.Subscribe(
                value => Console.WriteLine("{0} : {1}", windowName, value),
                ex => Console.WriteLine("{0} : {1}", windowName, ex),
                () => Console.WriteLine("{0} Completed", windowName));
            },
            () => Console.WriteLine("Completed"));
        }

        public static void Dump<T>(this IObservable<T> source, string name)
        {
            source
                //.SubscribeOn(Scheduler.NewThread)
                .Subscribe(
            i => Console.WriteLine("{0}-->{1}", name, i),
            ex => Console.WriteLine("{0} failed-->{1}", name, ex.Message),
            () => Console.WriteLine("{0} completed", name));
        }

        public class SumObserver : IObserver<int>
        {
            int sum = 0;
            public void OnCompleted()
            {
                //throw new NotImplementedException();
                Console.WriteLine(sum);
            }

            public void OnError(Exception error)
            {
                //throw new NotImplementedException();
            }

            public void OnNext(int value)
            {
                //throw new NotImplementedException();
                sum += value;

            }
        }
        public class SumMObserver : IObserver<int>
        {
            int sum = 0;
            public void OnCompleted()
            {
                //throw new NotImplementedException();
                Console.WriteLine(sum);
            }

            public void OnError(Exception error)
            {
                //throw new NotImplementedException();
            }

            public void OnNext(int value)
            {
                //throw new NotImplementedException();
                sum -= value;

            }
        }

        private static IEnumerable<T> Unfold<T>(T seed, Func<T, T> accumulator)
        {
            var nextValue = seed;
            while (true)
            {
                yield return nextValue;
                nextValue = accumulator(nextValue);
            }
        }

        private static void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine(e.SignalTime);
        }
    }
}

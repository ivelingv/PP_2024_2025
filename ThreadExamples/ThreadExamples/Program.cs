using System.Threading;

internal class Program
{
    private static object _locker = new object();

    private static int _globalVariable = 0;
    
    private static int[] _globalList = new int[10]
    {
        0,0,0,0,0,0,0,0,0,0
    };

    private static async Task Main(string[] args)
    {
        Console.WriteLine($"Start Global Variable " +
            $"{_globalVariable}");

        Console.WriteLine("Prepring for work...");

        var thread = new Thread(() => { DoWorkWithException(); });
        

        try
        {
            thread.Start();
            thread.Join();
        }
        catch (Exception ex) 
        { 
            Console.WriteLine(ex.Message);
        }


        //var task = new Task(() => DoWorkWithException());

        //try
        //{
        //    task.Start();
        //    await task;
        //}
        //catch (Exception ex) 
        //{ 
        //    Console.WriteLine(ex.Message);
        //}

        //var thread1 = Task.Run(DoWorkWithResult);
        //var thread2 = Task.Run(DoWorkWithResult);
        //
        //Console.WriteLine(await DoWorkWithResultAndTask());
        //Console.WriteLine(await DoWorkWithResultAndTask());
        //Console.WriteLine(await DoWorkWithResultAndTask());
        //Console.WriteLine(await DoWorkWithResultAndTask());

        //Console.ReadKey();

        //var task1 = DoWorkWithResultAndTask();
        //var task2 = DoWorkWithResultAndTask();
        //var task3 = DoWorkWithResultAndTask();
        //var task4 = DoWorkWithResultAndTask();
        //
        //await Task.WhenAll(task1, task2, task3, task4);
        //
        //Console.WriteLine(task1.Result);
        //Console.WriteLine(task2.Result);
        //Console.WriteLine(task3.Result);
        //Console.WriteLine(task4.Result);


        //thread2.Start();
        //thread1.Start();

        //Console.WriteLine("Waiting for task 1...");
        //var resultFrom1 = await thread1;
        //Console.WriteLine($"Result for task 1 - {resultFrom1}");
        //
        //Console.WriteLine("Waiting for task 2...");
        //var resultFrom2 = await thread2;
        //Console.WriteLine($"Result for task 2 - {resultFrom2}");

        //var thread1 = new Thread(DoWork);
        //var thread2 = new Thread(DoWork);
        //
        //thread2.Start();
        //thread1.Start();
        //
        //foreach(var thread in new[] { thread1, thread2 })
        //{
        //    thread.Join();
        //}

        Console.WriteLine(
            "Terminating program (Press Any Key)...");

        Console.WriteLine($"End Global Variable " +
            $"{_globalVariable}");

        Console.ReadKey();

        Console.WriteLine($"Actual Global Variable " +
            $"{_globalVariable}");
    }

    private static void DoWorkWithException()
    {
        try
        {
            Console.WriteLine("Doing work...");
            throw new Exception("Can't do work anymore");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static Task<string> DoWorkWithResultAndTask()
    {
        return Task.Run(() =>
        {
            var index = 0;

            Console.WriteLine(
                $"[{Thread.CurrentThread.ManagedThreadId}] " +
                    $"Started Work...");

            for (int i = 0; i < 10; i++)
            {
                index += Thread.CurrentThread.ManagedThreadId;
            }

            Console.WriteLine(
                $"[{Thread.CurrentThread.ManagedThreadId}] " +
                $"Finish Work...");

            return $"[{Thread.CurrentThread.ManagedThreadId}] " +
                $"Returning value {index}";
        });
    }

    private static string DoWorkWithResult()
    {
        var index = 0;

        Console.WriteLine(
            $"[{Thread.CurrentThread.ManagedThreadId}] " +
                $"Started Work...");

        for (int i = 0; i < 10; i++)
        {
            index += Thread.CurrentThread.ManagedThreadId;
        }

        Console.WriteLine(
            $"[{Thread.CurrentThread.ManagedThreadId}] " +
            $"Finish Work...");

        return $"[{Thread.CurrentThread.ManagedThreadId}] " +
            $"Returning value {index}";
    }

    private static void DoWork()
    {
        Console.WriteLine(
            $"[{Thread.CurrentThread.ManagedThreadId}] " +
                $"Started Work...");
        
        for (int i = 0; i < 10; i++)
        {
            //Console.WriteLine(
            //    $"[{Thread.CurrentThread.ManagedThreadId}] " +
            //    $"Doing Work...");
            //lock (_locker)
            //{
                var number = _globalList[i];
                Thread.Sleep(new Random().Next(40));

                if (number == 0)
                {
                    _globalList[i] = Thread
                        .CurrentThread
                        .ManagedThreadId;

                    Console.WriteLine(
                        $"Setting array element [{i}] " +
                        $"to {Thread.CurrentThread.ManagedThreadId}");
                }
            //}

            //_globalVariable = 
            //    Thread.CurrentThread.ManagedThreadId;

            //Thread.Sleep(100 * (Thread.CurrentThread.ManagedThreadId
            //        % 2 == 0 ? 1 : 2));
        }
        
        Console.WriteLine(
            $"[{Thread.CurrentThread.ManagedThreadId}] " +
            $"Finish Work...");
    }
}
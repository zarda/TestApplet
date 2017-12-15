using System;
using System.Threading;
using System.Collections.Generic;

class ThreadingExample
{
    public AutoResetEvent autoEvent = new AutoResetEvent(false);

    private static int N = 5;

    public void DoWork()
    {
        Console.WriteLine("   worker thread started, now waiting on event...");
        lock (this)
        {
            this.autoEvent.WaitOne(); 
        }

        Console.WriteLine("   worker thread reactivated, now exiting...");
    }
    static WaitHandle[] waitHandleList = new WaitHandle[N];
    
    static void Run(object autoEvent)
    {
        AutoResetEvent aEvent = (AutoResetEvent)autoEvent;
      
        Thread.Sleep(200);
        
        Console.WriteLine("   worker thread reactivated, now exiting...");
        aEvent.Set();
    }

    static void Main()
    {

        List<ThreadingExample> ExampleArray = new List<ThreadingExample>();

        List<Thread> ThreadArray = new List<Thread>();

        for (int i = 0; i < N; i++)
        {
            ThreadingExample Example = new ThreadingExample();
            ExampleArray.Add(Example);
            ThreadArray.Add( new Thread(Example.DoWork) );
        }

        foreach (var item in ThreadArray)
        {
            item.Start();
        }

        Console.WriteLine("main thread sleeping for 1 second...");
        Thread.Sleep(1000);

        foreach (var item in ExampleArray)
        {
            Console.WriteLine("main thread sleeping for 0.1 second...");
            Thread.Sleep(100);
            item.autoEvent.Set();
        }

        for (int i = 0; i < N; i++)
        {
            waitHandleList[i] = new AutoResetEvent(false);
        }

        Console.WriteLine("main thread starting worker thread...");

        foreach (var item in waitHandleList)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(Run), item); 
        }
        
        foreach (var item in waitHandleList)
        {
            Console.WriteLine($" the {WaitHandle.WaitAny(waitHandleList)}th thread done..."); 
        }
 
        Console.WriteLine("main thread starting worker thread...");

        foreach (var item in waitHandleList)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(Run), item);
        }

        WaitHandle.WaitAll(waitHandleList);
        Console.WriteLine("all threads done...");
    }
}
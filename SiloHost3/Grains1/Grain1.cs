using System.Threading.Tasks;
using System.Collections.Generic;
using System;

using Orleans;
using Orleans.Concurrency;

using GrainInterfaces1;

namespace Grains1
{
    /// <summary>
    /// Grain Employee
    /// </summary>
    [Reentrant]
    public class Employee : Grain, IEmployee
    {
        public Task<int> GetLevel()
        {
            return Task.FromResult(_level);
        }

        public Task Promote(int newLevel)
        {
            _level = newLevel;
            return Task.CompletedTask;
        }

        public Task<IManager> GetManager()
        {
            return Task.FromResult(_manager);
        }

        public Task SetManager(IManager manager)
        {
            _manager = manager;
            return Task.CompletedTask;
        }

        public virtual Task Greeting(IEmployee from, string message)
        {
            Console.WriteLine("{0} said: {1}", from.GetPrimaryKeyString().ToString(), message);
            Console.WriteLine(DateTime.Now.Millisecond);
            return Task.CompletedTask;
        }

        public Task<int> EceptionTest(int number)
        {
            for (int i = 0; i < 100; i++)
            {
                int temp = 1 / (i - 50);
            }

            return Task.FromResult(number);
        }

        public async Task Greeting(GreetingData data)
        {
            Console.WriteLine("{0} said: {1}", data.From, data.Message);

            // stop this from repeating endlessly
            if (data.Count >= 3) return;

            // send a message back to the sender
            var fromGrain = GrainFactory.GetGrain<IEmployee>(data.From);
            await fromGrain.Greeting(new GreetingData
            {
                From = this.GetPrimaryKeyString(),
                Message = "Thanks!",
                Count = data.Count + 1
            });
            Console.WriteLine(DateTime.Now.Millisecond);
        }

        private int _level;
        private IManager _manager;
    }

    /// <summary>
    /// Grain Manager
    /// </summary>
    public class Manager : Grain, IManager
    {
        public override Task OnActivateAsync()
        {
            _me = GrainFactory.GetGrain<IEmployee>(this.GetPrimaryKeyString());
            return base.OnActivateAsync();
        }

        public Task<List<IEmployee>> GetDirectReports()
        {
            return Task.FromResult(_reports);
        }

        public async Task AddDirectReport(IEmployee employee)
        {
            if (_reports.FindIndex((c) => c.GetPrimaryKeyString() == employee.GetPrimaryKeyString()) < 0)
            {
                _reports.Add(employee);
            }
            await employee.SetManager(this);
            //await employee.Greeting(_me, $"Welcome {employee.GetPrimaryKeyString().ToString()} to my team!");
            Console.WriteLine(DateTime.Now.Millisecond);
            await employee.Greeting(new GreetingData
            {
                From = this.GetPrimaryKeyString(),
                Message = "Welcome to my team!"
            });
            //return Task.CompletedTask;
        }

        public Task<IEmployee> AsEmployee()
        {
            return Task.FromResult(_me);
        }

        private IEmployee _me;
        private List<IEmployee> _reports = new List<IEmployee>();
    }
}

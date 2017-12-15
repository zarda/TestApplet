using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Orleans;
using Orleans.Concurrency;

namespace GrainInterfaces1
{
    /// <summary>
    /// Grain interface IEmployee
    /// </summary>
    public interface IEmployee : IGrainWithStringKey
    {
        Task<int> GetLevel();
        Task Promote(int newLevel);

        Task<IManager> GetManager();
        Task SetManager(IManager manager);
        Task Greeting(IEmployee from, string message);
        Task Greeting(GreetingData data);
        Task<int> EceptionTest(int number);
    }
    /// <summary>
    /// Grain interface IManager
    /// </summary>
    public interface IManager : IGrainWithStringKey
    {
        Task<IEmployee> AsEmployee();
        Task<List<IEmployee>> GetDirectReports();
        Task AddDirectReport(IEmployee employee);        
    }
    /// <summary>
    /// 
    /// </summary>
    [Immutable]
    public class GreetingData
    {
        public string From { get; set; }
        public string Message { get; set; }
        public int Count { get; set; }
    }
}

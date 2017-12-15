using System;
using System.Threading.Tasks;
using GrainInterfaces2;
using Orleans;

namespace Grains2
{
    /// <summary>
    /// Grain implementation class Grain1.
    /// </summary>
    public class Grain2 : Grain, IGrain2
    {
        Task<string> IGrain2.SayHello(string msg)
        {
            return Task.FromResult(string.Format("You said {0}, I say: Hello!", msg));
        }
        
        Task<string> IGrain2.SayHello(string msg, int num)
        {
            int intTemp = 1 / num;
            return Task.FromResult(string.Format($"You said {msg}, I say: Hello! Num:{num}"));
        }
    }
}

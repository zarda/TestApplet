using System;
using System.Threading.Tasks;
using GrainInterfaces1;
using Orleans;

namespace Grains1
{
    /// <summary>
    /// Grain implementation class Grain1.
    /// </summary>
    public class Grain1 : Grain, IGrain1
    {
        Task<string> IGrain1.SayHello(string msg)
        {
            return Task.FromResult(string.Format("You said {0}, I say: Hello!", msg));
        }

        Task<string> IGrain1.SayHello(string msg, int num)
        {
            int intTemp = 1 / num;
            return Task.FromResult(string.Format($"You said {msg}, I say: Hello! Num:{num}"));
        }

    
        private string text = "Hello World!";

        public Task<string> SayHelloOld(string greeting)
        {
            var oldText = text;
            text = greeting;
            return Task.FromResult(oldText);
        }
    }
}

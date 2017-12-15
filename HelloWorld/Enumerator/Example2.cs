using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Enumerator
{
    class Example2
    {
        public Task<(string ID, int Amount, string[] Data)> User(int i)
        {
            return Task.FromResult((ID: "", Amount: 1, Data: new string[]{"",""}));
        }
        public void DoTest()
        {
            var tempID = User(1).Result.ID;
            var tempAmount = User(1).Result.Amount;
            var tempData = User(1).Result.Data;
        }
    }
}

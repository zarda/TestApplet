using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading;


namespace UnitTestProject1_SpinWait
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var tim = Stopwatch.StartNew();
            
            Thread.Sleep(1000);
            
            var result = tim.ElapsedMilliseconds;

            Assert.AreEqual(1000, result);
        }
    
        [TestMethod]
        public void TestMethod2()
        {
            var tim = Stopwatch.StartNew();

            SpinWait.SpinUntil(()=> true, 1000);

            var result = tim.ElapsedMilliseconds;

            Assert.AreNotEqual(1000, result);
        }
    }
}

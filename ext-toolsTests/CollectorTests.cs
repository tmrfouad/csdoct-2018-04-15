using Microsoft.VisualStudio.TestTools.UnitTesting;
using doWhile.Doct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doWhile.Doct.Tests
{
    [TestClass()]
    public class CollectorTests
    {
        [TestMethod()]
        public void DelaysTest()
        {
            var collector = new Collector("");

            var time = TimeSpan.FromSeconds(1);

            var result = collector.Collect(CollectorMode.Delays, time);

            // No assert, should not crash
        }

        [TestMethod()]
        public void PerformanceTest()
        {
            var collector = new Collector("");

            var time = TimeSpan.FromSeconds(1);

            var tweaker = new Tweaker("");
            tweaker.Tweak(TweakerMode.ConsumeCores, 4);

            var result = collector.Collect(CollectorMode.Performance, time);

            System.Diagnostics.Debugger.Break();

            // No assert, should not crash
        }

        [TestMethod()]
        public void HitrateTest()
        {
            var collector = new Collector("");

            var time = TimeSpan.FromSeconds(1);

            var result = collector.Collect(CollectorMode.HitRate, time);

            // No assert, should not crash
        }
    }
}
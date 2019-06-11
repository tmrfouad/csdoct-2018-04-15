using Microsoft.VisualStudio.TestTools.UnitTesting;
using doWhile.Doct.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace doWhile.Doct.Tests
{
    [TestClass()]
    public class CollectorTaskRunnerTests
    {
        private TaskResult RunPerformanceTask(int threshold)
        {
            var collector = new Mock<Collector>(null);
            collector.Setup(x => x.Collect(CollectorMode.Performance, It.IsAny<TimeSpan>())).Returns(new List<string>() { "90", "95", "99", "94", "96" });

            var task = new Mock<CollectorTask>();
            task.Setup(x => x.Description).Returns("Task description");
            task.Setup(x => x.MillisecondDuration).Returns(1);
            task.Setup(x => x.Mode).Returns(CollectorMode.Performance);
            task.Setup(x => x.Threshold).Returns(90);

            return CollectorTaskRunner.RunTask(task.Object, collector.Object);
        }

        [TestMethod()]
        public void RunPerformanceTaskTest_Sucess()
        {
            var result = RunPerformanceTask(90);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Messages.Count == 1);
            Assert.AreEqual("All measurements above or at threshold", result.Messages[0]);
        }

        [TestMethod()]
        public void RunPerformanceTaskTest_Failure()
        {
            var result = RunPerformanceTask(95);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Messages.Count == 1);
            Assert.AreEqual("All measurements above or at threshold", result.Messages[0]);
        }

        private TaskResult PrepareDelaysTest(int threshold)
        {
            var collector = new Mock<Collector>(null);
            collector.Setup(x => x.Collect(CollectorMode.Delays, It.IsAny<TimeSpan>())).Returns(new List<string>() { "4, 5, 7, 6", "20, 8, 4, 7", "9, 6, 5, 11", "7, 14, 14, 8", "10, 6, 12, 6" });

            var task = new Mock<CollectorTask>();
            task.Setup(x => x.Description).Returns("Task description");
            task.Setup(x => x.MillisecondDuration).Returns(1);
            task.Setup(x => x.Mode).Returns(CollectorMode.Delays);
            task.Setup(x => x.Threshold).Returns(threshold);

            return CollectorTaskRunner.RunTask(task.Object, collector.Object);
        }

        [TestMethod()]
        public void RunDelaysTaskTest_Success()
        {
            var result = PrepareDelaysTest(20);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Messages.Count == 1);
            Assert.AreEqual("All delays below or at threshold", result.Messages[0]);
        }

        [TestMethod()]
        public void RunDelaysTaskTest_Failure()
        {
            var result = PrepareDelaysTest(10);

            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.Messages.Count == 5);
            Assert.AreEqual("Delay above threshold: 20", result.Messages[0]);
        }

    }
}
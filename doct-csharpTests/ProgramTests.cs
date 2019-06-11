using Microsoft.VisualStudio.TestTools.UnitTesting;
using doWhile.Doct.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using System.IO;
using System.Xml.Serialization;

namespace doWhile.Doct.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void SystemTest()
        {
            GenerateFlowFile();

            string basePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string flowPath = basePath + Path.DirectorySeparatorChar + "SystemTest.xml";
            Assert.IsTrue(File.Exists(flowPath));

            Program.Main(new[] { flowPath });
        }

        public void GenerateFlowFile()
        {
            var flow = new Flow() { Hostname = "host.name" };

            flow.Tasks.Add(new CollectorTask()
            {
                Description = "First task, performance",
                Mode = CollectorMode.Performance,
                MillisecondDuration = 1000,
                Threshold = 90
            });

            flow.Tasks.Add(new CollectorTask()
            {
                Description = "First task, delays",
                Mode = CollectorMode.Delays,
                MillisecondDuration = 1000,
                Threshold = 10
            });

            var serializer = new XmlSerializer(typeof(Flow));
            using (var writer = new StreamWriter("SystemTest.xml"))
                serializer.Serialize(writer, flow);
        }


    }
}
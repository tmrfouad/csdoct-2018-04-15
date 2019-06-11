using doWhile.Doct.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace doWhile.Doct
{
    /// <summary>
    /// Main flow-running engine
    /// </summary>
    internal class FlowRunner
    {
        /// <summary>
        /// Loads the flow at the given path and runs ut
        /// </summary>
        /// <param name="flowPath">Path of flow to run</param>
        /// <returns>A list of results, one for each task</returns>
        internal static List<TaskResult> RunFlow(string flowPath)
        {
            var flow = LoadFlow(flowPath);

            return RunFlow(flow);
        }

        /// <summary>
        /// Runs the given flow
        /// </summary>
        /// <param name="flow">Flow to run</param>
        /// <returns>A list of results, one for each task</returns>
        internal static List<TaskResult> RunFlow(Flow flow)
        {
            var results = new List<TaskResult>();

            foreach (var task in flow.Tasks)
            {
                var result = RunTask(task, flow.Hostname);
                results.Add(result);
            }

            return results;
        }

        private static TaskResult RunTask(Data.Task task, string hostname)
        {
            if (task is CollectorTask)
                return CollectorTaskRunner.RunTask(task as CollectorTask, new Collector(hostname));
            else if (task is TweakerTask)
                return TweakerTaskRunner.RunTask(task as TweakerTask, new Tweaker(hostname), hostname);

            throw new ArgumentException("Unknown task type " + task.GetType().ToString());
        }

        private static Flow LoadFlow(string flowPath)
        {
            var serializer = new XmlSerializer(typeof(Flow));
            using (var reader = new StreamReader(flowPath))
                return (Flow) serializer.Deserialize(reader);
        }
    }
}

using System;
using System.Linq;
using doWhile.Doct.Data;
using System.Collections.Generic;

namespace doWhile.Doct
{
    internal class CollectorTaskRunner
    {
        /// <summary>
        /// Runs a collector task on the provided collector
        /// </summary>
        /// <param name="task">A CollectorTask to run</param>
        /// <param name="collector">The Collector to run the task on</param>
        /// <returns>Results of running the task, format depends on task type</returns>
        internal static TaskResult RunTask(CollectorTask task, Collector collector)
        {
            var taskResult = new TaskResult() { Task = task };

            var duration = TimeSpan.FromMilliseconds(task.MillisecondDuration);

            var results = collector.Collect(task.Mode, duration);

            switch (task.Mode)
            {
                case CollectorMode.Performance:
                    CheckPerformanceResults(results, taskResult, task.Threshold);
                    break;
                case CollectorMode.Delays:
                    CheckDelaysResults(results, taskResult, task.Threshold);
                    break;
                case CollectorMode.HitRate:
                    CheckHitRateResults(results, taskResult, task.Threshold);
                    break;
                default:
                    throw new ArgumentException("Unknown CollectorMode " + task.Mode.ToString());
            }
            return taskResult;

        }

        /// <summary>
        /// Parse provided results and check if any values are ABOVE the threshold. Any value above this value will be logged and cause a failure.
        /// </summary>
        /// <param name="results">Results</param>
        /// <param name="taskResult">Instance to report check results to</param>
        /// <param name="threshold">maximum value allowed, any value above this will cause a fail</param>
        private static void CheckPerformanceResults(List<String> results, TaskResult taskResult, int threshold)
        {
            // Each line in the result is a single number
            var parsedResults = results.Select(x => int.Parse(x));

            if (parsedResults.Any(x => x < threshold))
            {
                taskResult.Success = false;

                foreach (int result in parsedResults.Where(x => x < threshold))
                {
                    taskResult.Messages.Add("Measurement below threshold: " + result);
                }
            }
            else
            {
                taskResult.Success = true;
                taskResult.Messages.Add("All measurements above or at threshold");
            }
        }

        /// <summary>
        /// Parse provided results and check if any values are ABOVE the threshold. Any value above this value will be logged and cause a failure.
        /// </summary>
        /// <param name="results">Results</param>
        /// <param name="taskResult">Instance to report check results to</param>
        /// <param name="threshold">maximum value allowed, any value above this will cause a fail</param>
        private static void CheckDelaysResults(List<String> results, TaskResult taskResult, int threshold)
        {
            // Each line in the result is one or more numbers, split by ", "
            var parsedResults = new List<int>();
            foreach (String result in results)
            {
                //Parse and flatten results
                var splitResults = result.Split(new[] { ", " }, StringSplitOptions.None);
                var parsedSplitResults = splitResults.Select(x => int.Parse(x));
                parsedResults.AddRange(parsedSplitResults);

            }

            if (parsedResults.Any(x => x > threshold))
            {
                taskResult.Success = false;

                foreach (int result in parsedResults.Where(x => x > threshold))
                {
                    taskResult.Messages.Add("Delay above threshold: " + result);
                }
            }
            else
            {
                taskResult.Success = true;
                taskResult.Messages.Add("All delays below or at threshold");
            }
        }

        /// <summary>
        /// Parse provided results and check if any values are ABOV the threshold. Any value above this value will be logged and cause a failure.
        /// </summary>
        /// <param name="results">Results</param>
        /// <param name="taskResult">Instance to report check results to</param>
        /// <param name="threshold">minimum value allowed, any value above this will cause a fail</param>
        private static void CheckHitRateResults(List<String> results, TaskResult taskResult, int threshold)
        {
            // Each line in the result is a single number
            var parsedResults = results.Select(x => new { enu = decimal.Parse(x.Split('/')[0]), den = decimal.Parse(x.Split('/')[1]) });
            var failureResults = parsedResults.Where(x => x.enu / x.den * 100 < threshold);

            if (failureResults.Any())
            {
                taskResult.Success = false;

                foreach (var result in failureResults)
                {
                    taskResult.Messages.Add("Measurement below threshold: " + (result.enu/result.den * 100).ToString("0.##"));
                }
            }
            else
            {
                taskResult.Success = true;
                taskResult.Messages.Add("All measurements above or at threshold");
            }
        }

    }
}
using System;
using System.Collections.Generic;

namespace doWhile.Doct
{
    /// <summary>
    /// Logs results of tasks
    /// </summary>
    internal class ResultLogger
    {
        /// <summary>
        /// Prints the success, task summary and list of messages for each task
        /// </summary>
        /// <param name="results">List of tasks to print results for</param>
        internal static void LogResults(List<TaskResult> results)
        {
            foreach(var result in results)
            {
                Console.Out.WriteLine((result.Success ? "Success" : "FAILURE") + ": " + result.Task.ToString());
                foreach(string message in result.Messages)
                {
                    Console.Out.WriteLine("    " + message);
                }
            }
        }
    }
}
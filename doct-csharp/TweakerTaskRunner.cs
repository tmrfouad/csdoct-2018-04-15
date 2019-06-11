using System;
using System.Linq;
using doWhile.Doct.Data;
using System.Collections.Generic;

namespace doWhile.Doct
{
    internal class TweakerTaskRunner
    {
        private static Dictionary<String, SystemInfo> SystemInfos { get; } = new Dictionary<string, SystemInfo>();

        /// <summary>
        /// Runs a tweaker task on the given host name using the given Tweaker
        /// </summary>
        /// <param name="task">A TweakerTask to run</param>
        /// <param name="tweaker">The Tweaker to run the task on</param>
        /// <param name="hostname">The host name that will be subjected to the tweak</param>
        /// <returns>Result of task, format depends on task type</returns>
        internal static TaskResult RunTask(TweakerTask task, Tweaker tweaker, string hostname)
        {
            var taskResult = new TaskResult() { Task = task };

            var tweak = ValidateAndCorrectTask(task, hostname);

            tweaker.Tweak(task.Mode, tweak);

            string resultMessage = "Tweaker mode " + task.Mode + " applied with value " + tweak;

            return new TaskResult()
            {
                Messages = new List<String>() { resultMessage },
                Task = task
            };

        }

        /// <summary>
        /// Validates and corrects tweak values before passing them to the Tweaker
        /// <para />
        /// This method may establish a connection to the server to fetch data required for validation
        /// </summary>
        /// <param name="task">Task to validate</param>
        /// <returns>Validated and/or corrected tweak value for task</returns>
        private static int ValidateAndCorrectTask(TweakerTask task, string hostname)
        {
            // No tweaker mode allows negative values
            if (task.Tweak < 0) return 0;

            switch (task.Mode)
            {
                case TweakerMode.ConsumeBandwidth:
                    var totalBandwidth = GetSystemInfo(hostname).BandwidthTotal;
                    if (task.Tweak > totalBandwidth)
                        return totalBandwidth;
                    break;

                case TweakerMode.ConsumeCores:
                    var info = GetSystemInfo(hostname);
                    int numCores = info.Cpus.Select(x => x.Core).Max();
                    if (task.Tweak > numCores)
                        return numCores;
                    break;

                case TweakerMode.ConsumeMemory:
                    var totalMemory = GetSystemInfo(hostname).MemoryTotal;
                    if (task.Tweak > totalMemory)
                        return totalMemory;
                    break;
            }

            return task.Tweak;
        }

        /// <summary>
        /// Gets SystemInfo for the given hostname from the cache. Fetches it if it is not already cached
        /// </summary>
        /// <param name="hostname">Hostname to fetch info for</param>
        /// <returns>System info</returns>
        private static SystemInfo GetSystemInfo(string hostname)
        {
            if (!SystemInfos.ContainsKey(hostname))
            {
                SystemInfos[hostname] = SystemInfo.Get(hostname);
            }

            return SystemInfos[hostname];
        }
    }
}
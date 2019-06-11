using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doWhile.Doct
{
    /// <summary>
    /// Represents the results of a task invocation
    /// </summary>
    internal class TaskResult
    {
        /// <summary>
        /// The task that was invoked
        /// </summary>
        internal Data.Task Task { get; set; }

        /// <summary>
        /// True if the task ran with a successful verdict
        /// </summary>
        internal bool Success { get; set; }

        /// <summary>
        /// Messages that were created as part of the task invocation
        /// </summary>
        internal List<String> Messages { get; set; } = new List<string>();
    }
}

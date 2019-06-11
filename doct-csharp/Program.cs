using doWhile.Doct.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace doWhile.Doct
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            string flowPath = args[0];

            List<TaskResult> results = FlowRunner.RunFlow(flowPath);

            ResultLogger.LogResults(results);

            //TODO: Remove before release
            #region Break on debug
            if (System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Break();
            #endregion
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace doWhile.Doct.Data
{
    public class CollectorTask : Task
    {
        public virtual CollectorMode Mode { get; set; }
        public virtual int Threshold { get; set; }
        public virtual int MillisecondDuration { get; set; }

        public override string ToString()
        {
            return "CollectorTask, mode: " + Mode.ToString() + ", threshold: " + Threshold + ", " + Description;
        }
    }
}

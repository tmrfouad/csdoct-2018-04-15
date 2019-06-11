using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace doWhile.Doct.Data
{
    public class TweakerTask : Task
    {
        public virtual TweakerMode Mode { get; set; }
        public virtual int Tweak { get; set; }

        public override string ToString()
        {
            return "TweakerTask, mode: " + Mode.ToString() + ", tweak: " + Tweak;
        }
    }
}

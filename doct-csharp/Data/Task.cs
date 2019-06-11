using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace doWhile.Doct.Data
{
    [XmlInclude(typeof(CollectorTask))]
    [XmlInclude(typeof(TweakerTask))]
    public abstract class Task
    {
        public virtual string Description { get; set; }
    }
}

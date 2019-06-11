using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doWhile.Doct.Data
{
    public class Flow
    {
        public virtual string Hostname { get; set; }
        public virtual List<Task> Tasks { get; set; } = new List<Task>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doWhile.Doct
{
    public class SystemInfo
    {
        private String hostname;

        private Random Random { get; } = new Random();

        /// <summary>
        /// Information about CPU cores and utilization
        /// </summary>
        public List<CpuCoreInfo> Cpus
        {
            get
            {
                return new List<CpuCoreInfo>()
                {
                    new CpuCoreInfo()
                    {
                        Core = 0,
                        CpuUtilization = Random.NextDouble(),
                        Hostname = hostname
                    },
                    new CpuCoreInfo()
                    {
                        Core = 1,
                        CpuUtilization = Random.NextDouble(),
                        Hostname = hostname
                    },
                    new CpuCoreInfo()
                    {
                        Core = 2,
                        CpuUtilization = Random.NextDouble(),
                        Hostname = hostname
                    },
                    new CpuCoreInfo()
                    {
                        Core = 3,
                        CpuUtilization = Random.NextDouble(),
                        Hostname = hostname
                    },
                };
            }
        }

        private int targetMemoryUsage;
        /// <summary>
        /// Memory used in megabytes
        /// </summary>
        public int MemoryUsage
        {
            get
            {
                int consumeMemory = Tweaker.Lookup(hostname).ConsumeMemory;
                int candidate = targetMemoryUsage + consumeMemory;
                candidate = (candidate - 512) + Random.Next(1024);
                if (candidate < consumeMemory) return consumeMemory;
                return candidate;
            }

            set { targetMemoryUsage = value; }
        }

        /// <summary>
        /// Total installed memory in megabytes
        /// </summary>
        public int MemoryTotal { get { return (int)(Constants.MEMORY_SIZE_BYTES / 1024 / 1024); } }

        /// <summary>
        /// Total available bandwidth in megabits per second
        /// </summary>
        public int BandwidthTotal { get { return (int)(Constants.MAX_BANDWIDTH_BPS / 1024 / 1024); } }

        private SystemInfo(String hostname)
        {
            this.hostname = hostname;
        }

        public static SystemInfo Get(string hostname)
        {
            return new SystemInfo(hostname);
        }
    }

    public class CpuCoreInfo
    {
        private static Random Random { get; } = new Random();

        internal String Hostname { get; set; }

        /// <summary>
        /// Core identifier (index, zero-based)
        /// </summary>
        public int Core { get; set; }

        private double targetCpuUtilization;

        /// <summary>
        /// CPU utilization as a fraction of 1
        /// </summary>
        public double CpuUtilization
        {
            get
            {
                if (Core < Tweaker.Lookup(Hostname).ConsumeCores)
                {
                    return 1.0;
                }
                else
                {
                    double candidate = (targetCpuUtilization - 0.1) + Random.NextDouble();
                    if (candidate < 0) return 0;
                    if (candidate > 1) return 1;
                    return candidate;
                }
            }

            set { targetCpuUtilization = value; }
        }



    }
}

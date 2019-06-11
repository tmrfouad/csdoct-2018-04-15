using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doWhile.Doct
{
    public class Collector
    {
        private String hostname;

        private Random Random { get; } = new Random();

        public Collector(String hostname)
        {
            this.hostname = hostname;
        }

        /// <summary>
        /// Colllects measurements from the given host every 50 milliseconds until the given time has expired <para/>
        /// If <c>mode</c> is <c>Performance</c>,  each line in the result will have a single value representing the performance as percent of the total (e.g. "0" to "100") <para/>
        /// If <c>mode</c> is <c>Delays</c>,  each line in the result will have one or more values, separated by ", ", representing the millisecond delays encountered (e.g. "2, 4, 3") <para/>
        /// If <c>mode</c> is <c>HitRate</c>,  each line in the result will have number of hits and total attempts, separated by "/" (e.g. "86/99")
        /// </summary>
        /// <param name="mode">Collector mode to use</param>
        /// <param name="duration">Duration during which measurements will be collected</param>
        /// <returns></returns>
        public virtual List<String> Collect(CollectorMode mode, TimeSpan duration)
        {
            var result = new List<String>();

            DateTime end = DateTime.Now.Add(duration);

            while (DateTime.Now < end)
            {
                switch (mode)
                {
                    case CollectorMode.Performance:
                        result.Add(CollectPerformance());
                        break;
                    case CollectorMode.Delays:
                        result.Add(CollectDelays());
                        break;
                    case CollectorMode.HitRate:
                        result.Add(CollectHitRate());
                        break;
                    default:
                        throw new ArgumentException("Unknown collector mode: " + mode.ToString());
                }
                System.Threading.Thread.Sleep(50);
            }
            return result;

        }

        private String CollectPerformance()
        {
            int consumeCores = Tweaker.Lookup(hostname).ConsumeCores;
            if (consumeCores == Constants.NUM_CORES /*All cores */) return (1 + Random.Next(4)).ToString();

            int candidate = 89 + Random.Next(12);
            if (consumeCores == 0) return candidate.ToString();

            double modifier = 1 - ((double)consumeCores / Constants.NUM_CORES);

            return ((int)(candidate * modifier)).ToString();

        }

        private String CollectDelays()
        {
            var result = "";
            int count = Random.Next(4) + 1;
            int delay = 0;

            if (Tweaker.Lookup(hostname).ConsumeBandwith > 0)
            {
                int consumeBandwidthBps = Tweaker.Lookup(hostname).ConsumeBandwith;

                if (consumeBandwidthBps < Constants.MAX_BANDWIDTH_BPS * 0.7)
                {
                    delay = 0;
                }
                else if (consumeBandwidthBps >= Constants.MAX_BANDWIDTH_BPS)
                {
                    delay = 400 + Random.Next(300);
                }
                else
                {
                    double modifier = (consumeBandwidthBps - Constants.MAX_BANDWIDTH_BPS * 0.7) / (Constants.MAX_BANDWIDTH_BPS * 0.3);
                    delay = (int) (400 * modifier);
                }
            }

            int addLatency = Tweaker.Lookup(hostname).AddLatency;

            for (int i = 0; i <= count; i++)
            {
                result += (delay + Random.Next(70) + addLatency) + (i == count ? "" : ", ");
            }
            return result;
        }

        private String CollectHitRate()
        {
            var roof = Random.Next(1000) + 1;
            var hits = Math.Ceiling(roof * 0.8 + (roof * 0.2 * Random.NextDouble()));

            return hits.ToString() + "/" + roof.ToString();
        }
    }

    public enum CollectorMode
    {
        Performance,
        Delays,
        HitRate
    }

}

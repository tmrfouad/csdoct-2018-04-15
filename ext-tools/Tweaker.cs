using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doWhile.Doct
{
    public class Tweaker
    {
        private String hostname;

        private static Dictionary<String, Tweaker> Instances { get; } = new Dictionary<String, Tweaker>();

        internal int AddLatency { get; private set; } = 0;
        internal int FailRandomAttempts { get; private set; } = 0;
        internal int ConsumeCores { get; private set; } = 0;
        internal int ConsumeMemory { get; private set; } = 0;
        internal int ConsumeBandwith { get; private set; } = 0;

        public Tweaker(String hostname)
        {
            this.hostname = hostname;
            Instances[hostname] = this;
        }

        internal static Tweaker Lookup(string hostname)
        {
            if(!Instances.ContainsKey(hostname))
            {
                Instances[hostname] = new Tweaker(hostname);
            }
            return Instances[hostname];
        }

        /// <summary>
        /// Performs the given tweak on the environment surrounding (and including) the given host <para/>
        /// If <c>mode</c> is <c>AddLatency</c>, introduces a network latency of the number of milliseconds defined by the latency <para/>
        /// If <c>mode</c> is <c>FailRandomAttempts</c>, forcibly fails up to the defined number of attempts <para/>
        /// If <c>mode</c> is <c>ConsumeCores</c>, fully utilizes the number of CPU cores defined by the tweak value <para/>
        /// If <c>mode</c> is <c>ConsumeMemory</c>, consume a number of megabytes of RAM, defined by the tweak value <para/>
        /// If <c>mode</c> is <c>ConsumeBandwidth</c>, consume a number of megabits per second in network bandwidth, defined by the tweak value
        /// </summary>
        /// <param name="mode">Tweaker mode to use</param>
        /// <param name="tweak">Value indicating the size of the tweak, see method description details</param>
        /// <returns></returns>
        public virtual void Tweak(TweakerMode mode, int tweak)
        {
            switch (mode)
            {
                case TweakerMode.AddLatency:
                    AddLatency = tweak;
                    break;
                case TweakerMode.ConsumeBandwidth:
                    ConsumeBandwith = tweak;
                    break;
                case TweakerMode.ConsumeCores:
                    ConsumeCores = tweak;
                    break;
                case TweakerMode.ConsumeMemory:
                    ConsumeMemory = tweak;
                    break;
                case TweakerMode.FailRandomAttempts:
                    FailRandomAttempts = tweak;
                    break;
            }
        }
    }

    public enum TweakerMode
    {
        AddLatency,
        FailRandomAttempts,
        ConsumeCores,
        ConsumeMemory,
        ConsumeBandwidth
    }
}

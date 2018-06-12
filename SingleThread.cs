using System;

namespace channelbench
{
    using System.Collections.Concurrent;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Jobs;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Attributes.Jobs;
    using BenchmarkDotNet.Running;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Threading.Channels;
    [Config(typeof(MultipleRuntime))]
    public class SingleThread
    {
        [Params(10000)]
        public int LoopNum;
        [Params(1, 100)]
        public int TaskNum;
        [Params(1, 100)]
        public int WTaskNum;

        public void ThreadingChannel()
        {
            var opt2 = new UnboundedChannelOptions();
            opt2.AllowSynchronousContinuations = false;
            opt2.SingleReader = true;
            var channel = Channel.CreateUnbounded<int>(opt2);
            for (int i = 0; i < LoopNum; i++)
            {
                channel.Writer.TryWrite(i);
            }
            for (int i = 0; i < LoopNum; i++)
            {
                channel.Reader.TryRead(out var x);
            }
        }
        public void Queue()
        {
            var queue = new ConcurrentQueue<int>();
            for (int i = 0; i < LoopNum; i++)
            {
                queue.Enqueue(i);
            }
            for (int i = 0; i < LoopNum; i++)
            {
                queue.TryDequeue(out var item);
            }
        }
    }
}
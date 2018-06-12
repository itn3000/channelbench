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
    public class QueueVsChannel
    {
        [Params(10000)]
        public int LoopNum;
        [Params(1, 100)]
        public int TaskNum;
        [Params(1, 100)]
        public int WTaskNum;

        // [Benchmark]
        public void ThreadingChannel()
        {
            var opt2 = new UnboundedChannelOptions();
            opt2.AllowSynchronousContinuations = false;
            var channel = Channel.CreateUnbounded<int>(opt2);
            Task.WaitAll(
                Task.WhenAll(Enumerable.Range(0, WTaskNum).Select(async (idx) =>
                {
                    await Task.Yield();
                    for (int i = 0; i < LoopNum / WTaskNum; i++)
                    {
                        channel.Writer.TryWrite(i);
                    }
                })).ContinueWith(t => channel.Writer.TryComplete())
                ,
                Task.WhenAll(Enumerable.Range(0, TaskNum).Select(async (idx) =>
                {
                    await Task.Yield();
                    while (await channel.Reader.WaitToReadAsync().ConfigureAwait(false))
                    {
                        await Task.Yield();
                        while (channel.Reader.TryRead(out var item))
                        {

                        }
                    }
                }))
            );
        }
        [Benchmark]
        public void ThreadingChannelWriteOnly()
        {
            var opt2 = new UnboundedChannelOptions();
            opt2.AllowSynchronousContinuations = false;
            var channel = Channel.CreateUnbounded<int>(opt2);
            Task.WhenAll(Enumerable.Range(0, WTaskNum).Select(async (idx) =>
            {
                await Task.Yield();
                for (int i = 0; i < LoopNum / WTaskNum; i++)
                {
                    channel.Writer.TryWrite(i);
                }
            })).ContinueWith(t => channel.Writer.TryComplete()).Wait();
        }
        [Benchmark]
        public void ThreadingChannelReadMulti()
        {
            var opt2 = new UnboundedChannelOptions();
            opt2.AllowSynchronousContinuations = false;
            var channel = Channel.CreateUnbounded<int>(opt2);
            for (int i = 0; i < LoopNum; i++)
            {
                channel.Writer.TryWrite(i);
            }
            channel.Writer.TryComplete();
            Task.WhenAll(Enumerable.Range(0, TaskNum).Select(async idx =>
            {
                await Task.Yield();
                while (await channel.Reader.WaitToReadAsync().ConfigureAwait(false))
                {
                    while (channel.Reader.TryRead(out var item))
                    {

                    }
                }
            })).Wait();
        }
        // [Benchmark]
        public void Queue()
        {
            var queue = new ConcurrentQueue<int>();
            using (var wend = new System.Threading.ManualResetEventSlim(false))
            {
                Task.WaitAll(
                    Task.WhenAll(Enumerable.Range(0, WTaskNum).Select(async (idx) =>
                    {
                        await Task.Yield();
                        for (int i = 0; i < LoopNum / WTaskNum; i++)
                        {
                            queue.Enqueue(i);
                        }
                    })).ContinueWith(t => wend.Set())
                    ,
                    Task.WhenAll(Enumerable.Range(0, TaskNum).Select(async (idx) =>
                    {
                        await Task.Yield();
                        while (!wend.IsSet)
                        {
                            await Task.Yield();
                            while (queue.TryDequeue(out var item))
                            {

                            }
                        }
                    }))
                );
            }
        }
    }
}
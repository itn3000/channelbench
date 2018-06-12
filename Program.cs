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
    public class ConcurrentBench
    {
        [Params(10000)]
        public int LoopNum;
        [Params(1, 100)]
        public int TaskNum;
        [Benchmark]
        public async Task Queue()
        {
            var queue = new ConcurrentQueue<int>();
            await Task.WhenAll(Enumerable.Range(0, TaskNum).Select(async (idx) =>
            {
                await Task.Yield();
                for (int i = 0; i < LoopNum / TaskNum; i++)
                {
                    queue.Enqueue(i + (idx * LoopNum));
                }
                // return Task.CompletedTask;
            })).ConfigureAwait(false);
            // for (int i = 0; i < LoopNum; i++)
            // {
            //     queue.TryDequeue(out var tmp);
            // }
        }
        // [Benchmark]
        public void Dic()
        {
            var queue = new ConcurrentDictionary<int, int>();
            for (int i = 0; i < LoopNum; i++)
            {
                queue.TryAdd(i, i);
            }
            for (int i = 0; i < LoopNum; i++)
            {
                queue.TryGetValue(i, out var tmp);
            }
        }
        [Benchmark]
        public void BlockingCollection()
        {
            using (var bc = new BlockingCollection<int>())
            {
                Task.WaitAll(
                    Task.WhenAll(Enumerable.Range(0, TaskNum).Select(async (idx) =>
                    {
                        await Task.Yield();
                        for (int i = 0; i < LoopNum / TaskNum; i++)
                        {
                            bc.Add(idx * TaskNum + i);
                        }
                    })).ContinueWith(t => bc.CompleteAdding())
                    ,
                    Task.Run(() =>
                    {
                        foreach (var item in bc.GetConsumingEnumerable())
                        {

                        }
                    })
                );

            }
        }
    }
    [Config(typeof(MultipleRuntime))]
    public class ChannelQueueBench
    {
        [Params(1, 1000)]
        public int TaskNum;
        [Params(10000)]
        public int LoopNum;
        [Params(false, true)]
        public bool AllowSync;
        [Params(false, true)]
        public bool IsSingleReader;
        // [Benchmark]
        public void ConcurrentQueueBench()
        {
            var queue = new ConcurrentQueue<int>();
            Task.WaitAll(
                Task.WhenAll(Enumerable.Range(0, TaskNum).Select(async idx =>
                {
                    await Task.Yield();
                    for (int i = 0; i < LoopNum / TaskNum; i++)
                    {
                        queue.Enqueue(idx);
                    }
                }))
                ,
                Task.Run(() =>
                {
                    for (int i = 0; i < LoopNum; i++)
                    {
                        while (!queue.TryDequeue(out var item))
                        {
                        }
                    }
                })
            );
        }
        [Benchmark]
        public void ChannelBenchAsync()
        {
            var opt2 = new UnboundedChannelOptions();
            opt2.AllowSynchronousContinuations = AllowSync;
            opt2.SingleReader = IsSingleReader;
            var channel = Channel.CreateUnbounded<int>(opt2);
            Task.WaitAll(
                Task.WhenAll(Enumerable.Range(0, TaskNum).Select(async idx =>
                {
                    await Task.Yield();
                    for (int i = 0; i < LoopNum / TaskNum; i++)
                    {
                        channel.Writer.TryWrite(1);
                    }
                })).ContinueWith(t => channel.Writer.Complete())
                ,
                Task.Run(async () =>
                {
                    while (await channel.Reader.WaitToReadAsync().ConfigureAwait(false))
                    {
                        while (channel.Reader.TryRead(out var item))
                        {

                        }
                    }
                })
            );
        }
        [Benchmark]
        public void ChannelBench()
        {
            var opt2 = new UnboundedChannelOptions();
            opt2.AllowSynchronousContinuations = false;
            opt2.SingleReader = IsSingleReader;
            var channel = Channel.CreateUnbounded<int>(opt2);
            Task.WaitAll(
                Task.WhenAll(Enumerable.Range(0, TaskNum).Select(async idx =>
                {
                    await channel.Writer.WaitToWriteAsync();
                    for (int i = 0; i < LoopNum / TaskNum; i++)
                    {
                        channel.Writer.TryWrite(1);
                    }
                })).ContinueWith(t => channel.Writer.Complete())
                ,
                Task.Run(() =>
                {
                    for (int i = 0; i < LoopNum; i++)
                    {
                        while (!channel.Reader.TryRead(out var item))
                        {
                        }
                    }
                })
            );
        }
    }
    [Config(typeof(MultipleRuntime))]
    public class ComparisonByCollectionMT
    {
        [Params(10000)]
        public int LoopNum;
        [Params(1, 100)]
        public int WriteTaskNum;
        [Params(1, 100)]
        public int ReadTaskNum;
        [Benchmark]
        public void BlockingCollectionReadWrite()
        {
            using(var bc = new BlockingCollection<int>())
            {
                Task.WaitAll(
                    Task.WhenAll(Enumerable.Range(0, WriteTaskNum).Select(async (idx) =>
                    {
                        for(int i = 0;i<LoopNum/WriteTaskNum;i++)
                        {
                            await Task.Yield();
                            bc.Add(i);
                        }
                        return 0;
                    })).ContinueWith(t => bc.CompleteAdding())
                    ,
                    Task.WhenAll(Enumerable.Range(0, ReadTaskNum).Select(async (idx) =>
                    {
                        foreach(var item in bc.GetConsumingEnumerable())
                        {
                            await Task.Yield();
                        }
                    }))
                );
            }

        }
        [Benchmark]
        public void ThreadingChannelReadWrite()
        {
            var channel = Channel.CreateUnbounded<int>();
            {
                Task.WaitAll(
                    Task.WhenAll(Enumerable.Range(0, WriteTaskNum).Select(async (idx) =>
                    {
                        for(int i = 0;i<LoopNum/WriteTaskNum;i++)
                        {
                            await Task.Yield();
                            channel.Writer.TryWrite(i);
                        }
                        return 0;
                    })).ContinueWith(t => channel.Writer.TryComplete())
                    ,
                    Task.WhenAll(Enumerable.Range(0, ReadTaskNum).Select(async (idx) =>
                    {
                        while(await channel.Reader.WaitToReadAsync().ConfigureAwait(false))
                        {
                            while(channel.Reader.TryRead(out var item))
                            {
                                await Task.Yield();
                            }
                        }
                    }))
                );
            }

        }
    }
    [MemoryDiagnoser]
    [Config(typeof(MultipleRuntime))]
    public class ComparisonByCollectionST
    {
        [Params(100000)]
        public int LoopNum;
        [Benchmark]
        public void ConcurrentQueueBenchWrite()
        {
            var queue = new ConcurrentQueue<int>();
            for (int i = 0; i < LoopNum; i++)
            {
                queue.Enqueue(i);
            }
        }
        [Benchmark]
        public void BlockingCollectionWrite()
        {
            using (var bc = new BlockingCollection<int>())
            {
                for (int i = 0; i < LoopNum; i++)
                {
                    bc.Add(i);
                }
            }
        }
        [Benchmark]
        public void ThreadingChannelsWrite()
        {
            var channel = Channel.CreateUnbounded<int>();
            for (int i = 0; i < LoopNum; i++)
            {
                channel.Writer.TryWrite(i);
            }
        }
        [Benchmark]
        public void ConcurrentQueueBenchWriteRead()
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
        [Benchmark]
        public void BlockingCollectionWriteRead()
        {
            using (var bc = new BlockingCollection<int>())
            {
                for (int i = 0; i < LoopNum; i++)
                {
                    bc.Add(i);
                }
                for (int i = 0; i < LoopNum; i++)
                {
                    bc.TryTake(out var item);
                }
            }
        }
        [Benchmark]
        public void ThreadingChannelsWriteRead()
        {
            var channel = Channel.CreateUnbounded<int>();
            for (int i = 0; i < LoopNum; i++)
            {
                channel.Writer.TryWrite(i);
            }
            for (int i = 0; i < LoopNum; i++)
            {
                channel.Reader.TryRead(out var item);
            }
        }
    }
    [MemoryDiagnoser]
    [Config(typeof(MultipleRuntime))]
    public class ComparisonByAllowAsync
    {
        [Params(10000)]
        public int LoopNum;
        [Params(1, 100)]
        public int WriteTaskNum;
        [Params(1, 100)]
        public int ReadTaskNum;
        [Params(false, true)]
        public bool AllowAsync;
        [Benchmark]
        public void AllowAsyncBench()
        {
            var opts = new UnboundedChannelOptions();
            opts.AllowSynchronousContinuations = AllowAsync;
            var channel = Channel.CreateUnbounded<int>(opts);
            {
                Task.WaitAll(
                    Task.WhenAll(Enumerable.Range(0, WriteTaskNum).Select(async (idx) =>
                    {
                        for(int i = 0;i<LoopNum/WriteTaskNum;i++)
                        {
                            await Task.Yield();
                            channel.Writer.TryWrite(i);
                        }
                        return 0;
                    })).ContinueWith(t => channel.Writer.TryComplete())
                    ,
                    Task.WhenAll(Enumerable.Range(0, ReadTaskNum).Select(async (idx) =>
                    {
                        while(await channel.Reader.WaitToReadAsync().ConfigureAwait(false))
                        {
                            while(channel.Reader.TryRead(out var item))
                            {
                                await Task.Yield();
                            }
                        }
                    }))
                );
            }
        }

    }
    [MemoryDiagnoser]
    [Config(typeof(MultipleRuntime))]
    public class ComparisonBySingleReaderWriter
    {
        [Params(10000)]
        public int LoopNum;
        [Params(true, false)]
        public bool SingleReader;
        [Params(true, false)]
        public bool SingleWriter;
        [Benchmark]
        public void SettingBench()
        {
            var opts = new UnboundedChannelOptions();
            opts.SingleReader = SingleReader;
            opts.SingleWriter = SingleWriter;
            var channel = Channel.CreateUnbounded<int>(opts);
            Task.WaitAll(
                Task.Run(async () =>
                {
                    await Task.Yield();
                    for(int i = 0;i<LoopNum;i++)
                    {
                        channel.Writer.TryWrite(i);
                    }
                }).ContinueWith(t => channel.Writer.TryComplete()),
                Task.Run(async () =>
                {
                    await Task.Yield();
                    while(await channel.Reader.WaitToReadAsync().ConfigureAwait(false))
                    {
                        while(channel.Reader.TryRead(out var item));
                    }
                })
            );
        }
    }
    class MultipleRuntime : ManualConfig
    {
        public MultipleRuntime()
        {
            Add(Job.Default.With(BenchmarkDotNet.Toolchains.CsProj.CsProjCoreToolchain.NetCoreApp21)
                .WithTargetCount(3)
                .WithWarmupCount(3));
            Add(Job.Default.With(BenchmarkDotNet.Toolchains.CsProj.CsProjCoreToolchain.NetCoreApp20)
                .WithTargetCount(3)
                .WithWarmupCount(3));
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            var switcher = new BenchmarkSwitcher(new Type[]
            {
                typeof(QueueVsChannel),
                typeof(ComparisonByCollectionST),
                typeof(ComparisonByCollectionMT),
                typeof(ComparisonBySingleReaderWriter),
                typeof(ComparisonByAllowAsync)
            });
            switcher.Run();
        }
    }
}

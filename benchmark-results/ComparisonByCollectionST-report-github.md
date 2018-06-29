``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-4712MQ CPU 2.30GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
Frequency=2240913 Hz, Resolution=446.2467 ns, Timer=TSC
.NET Core SDK=2.1.300-rc1-008673
  [Host]     : .NET Core 2.1.0-rc1 (CoreCLR 4.6.26426.02, CoreFX 4.6.26426.04), 64bit RyuJIT
  Job-YJTPQZ : .NET Core 2.0.6 (CoreCLR 4.6.26212.01, CoreFX 4.6.26212.01), 64bit RyuJIT
  Job-UBCRTU : .NET Core 2.1.0-rc1 (CoreCLR 4.6.26426.02, CoreFX 4.6.26426.04), 64bit RyuJIT

TargetCount=3  WarmupCount=3  

```
|                        Method |     Toolchain | LoopNum |      Mean |     Error |    StdDev |    Gen 0 |    Gen 1 |    Gen 2 | Allocated |
|------------------------------ |-------------- |-------- |----------:|----------:|----------:|---------:|---------:|---------:|----------:|
|     ConcurrentQueueBenchWrite | .NET Core 2.0 |  100000 |  1.816 ms | 0.3366 ms | 0.0190 ms | 285.1563 | 285.1563 | 285.1563 |      1 MB |
|       BlockingCollectionWrite | .NET Core 2.0 |  100000 |  6.002 ms | 1.6192 ms | 0.0915 ms | 281.2500 | 281.2500 | 281.2500 |      1 MB |
|        ThreadingChannelsWrite | .NET Core 2.0 |  100000 |  4.256 ms | 0.5604 ms | 0.0317 ms | 281.2500 | 281.2500 | 281.2500 |      1 MB |
| ConcurrentQueueBenchWriteRead | .NET Core 2.0 |  100000 |  3.243 ms | 0.3243 ms | 0.0183 ms | 285.1563 | 285.1563 | 285.1563 |      1 MB |
|   BlockingCollectionWriteRead | .NET Core 2.0 |  100000 | 12.885 ms | 2.9885 ms | 0.1689 ms | 281.2500 | 281.2500 | 281.2500 |      1 MB |
|    ThreadingChannelsWriteRead | .NET Core 2.0 |  100000 |  6.144 ms | 2.7683 ms | 0.1564 ms | 281.2500 | 281.2500 | 281.2500 |      1 MB |
|     ConcurrentQueueBenchWrite | .NET Core 2.1 |  100000 |  1.872 ms | 0.2313 ms | 0.0131 ms | 285.1563 | 285.1563 | 285.1563 |      1 MB |
|       BlockingCollectionWrite | .NET Core 2.1 |  100000 |  6.289 ms | 1.4640 ms | 0.0827 ms | 281.2500 | 281.2500 | 281.2500 |   1.01 MB |
|        ThreadingChannelsWrite | .NET Core 2.1 |  100000 |  4.618 ms | 1.6158 ms | 0.0913 ms | 281.2500 | 281.2500 | 281.2500 |   1.01 MB |
| ConcurrentQueueBenchWriteRead | .NET Core 2.1 |  100000 |  3.593 ms | 0.8562 ms | 0.0484 ms | 285.1563 | 285.1563 | 285.1563 |      1 MB |
|   BlockingCollectionWriteRead | .NET Core 2.1 |  100000 | 13.606 ms | 2.1582 ms | 0.1219 ms | 281.2500 | 281.2500 | 281.2500 |   1.01 MB |
|    ThreadingChannelsWriteRead | .NET Core 2.1 |  100000 |  6.412 ms | 0.6740 ms | 0.0381 ms | 281.2500 | 281.2500 | 281.2500 |   1.01 MB |

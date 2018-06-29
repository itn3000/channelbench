``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-4712MQ CPU 2.30GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
Frequency=2240913 Hz, Resolution=446.2467 ns, Timer=TSC
.NET Core SDK=2.1.300-rc1-008673
  [Host]     : .NET Core 2.1.0-rc1 (CoreCLR 4.6.26426.02, CoreFX 4.6.26426.04), 64bit RyuJIT
  Job-GETNSF : .NET Core 2.0.6 (CoreCLR 4.6.26212.01, CoreFX 4.6.26212.01), 64bit RyuJIT
  Job-WJKVXZ : .NET Core 2.1.0-rc1 (CoreCLR 4.6.26426.02, CoreFX 4.6.26426.04), 64bit RyuJIT

TargetCount=3  WarmupCount=3  

```
|                      Method |     Toolchain | LoopNum | WriteTaskNum | ReadTaskNum |       Mean |       Error |     StdDev |
|---------------------------- |-------------- |-------- |------------- |------------ |-----------:|------------:|-----------:|
| **BlockingCollectionReadWrite** | **.NET Core 2.0** |   **10000** |            **1** |           **1** |  **13.320 ms** |   **3.9529 ms** |  **0.2233 ms** |
|   ThreadingChannelReadWrite | .NET Core 2.0 |   10000 |            1 |           1 |  11.181 ms |   6.4940 ms |  0.3669 ms |
| BlockingCollectionReadWrite | .NET Core 2.1 |   10000 |            1 |           1 |  12.506 ms |   2.1169 ms |  0.1196 ms |
|   ThreadingChannelReadWrite | .NET Core 2.1 |   10000 |            1 |           1 |  11.865 ms |   6.1815 ms |  0.3493 ms |
| **BlockingCollectionReadWrite** | **.NET Core 2.0** |   **10000** |            **1** |         **100** | **257.627 ms** | **143.1464 ms** |  **8.0880 ms** |
|   ThreadingChannelReadWrite | .NET Core 2.0 |   10000 |            1 |         100 | 173.708 ms |  36.2240 ms |  2.0467 ms |
| BlockingCollectionReadWrite | .NET Core 2.1 |   10000 |            1 |         100 |  28.791 ms | 280.0257 ms | 15.8220 ms |
|   ThreadingChannelReadWrite | .NET Core 2.1 |   10000 |            1 |         100 | 107.643 ms |  27.7679 ms |  1.5689 ms |
| **BlockingCollectionReadWrite** | **.NET Core 2.0** |   **10000** |          **100** |           **1** |  **13.668 ms** |   **1.5261 ms** |  **0.0862 ms** |
|   ThreadingChannelReadWrite | .NET Core 2.0 |   10000 |          100 |           1 |  13.040 ms |   0.5886 ms |  0.0333 ms |
| BlockingCollectionReadWrite | .NET Core 2.1 |   10000 |          100 |           1 |  11.976 ms |   0.3496 ms |  0.0198 ms |
|   ThreadingChannelReadWrite | .NET Core 2.1 |   10000 |          100 |           1 |  11.635 ms |   1.3184 ms |  0.0745 ms |
| **BlockingCollectionReadWrite** | **.NET Core 2.0** |   **10000** |          **100** |         **100** |  **86.314 ms** |  **32.3458 ms** |  **1.8276 ms** |
|   ThreadingChannelReadWrite | .NET Core 2.0 |   10000 |          100 |         100 |   6.034 ms |   1.1547 ms |  0.0652 ms |
| BlockingCollectionReadWrite | .NET Core 2.1 |   10000 |          100 |         100 |  16.961 ms |  63.2741 ms |  3.5751 ms |
|   ThreadingChannelReadWrite | .NET Core 2.1 |   10000 |          100 |         100 |   2.876 ms |   2.6431 ms |  0.1493 ms |

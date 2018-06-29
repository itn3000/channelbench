``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-4712MQ CPU 2.30GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
Frequency=2240913 Hz, Resolution=446.2467 ns, Timer=TSC
.NET Core SDK=2.1.300-rc1-008673
  [Host]     : .NET Core 2.1.0-rc1 (CoreCLR 4.6.26426.02, CoreFX 4.6.26426.04), 64bit RyuJIT
  Job-QLZWOS : .NET Core 2.0.6 (CoreCLR 4.6.26212.01, CoreFX 4.6.26212.01), 64bit RyuJIT
  Job-SEZKKX : .NET Core 2.1.0-rc1 (CoreCLR 4.6.26426.02, CoreFX 4.6.26426.04), 64bit RyuJIT

TargetCount=3  WarmupCount=3  

```
|          Method |     Toolchain | LoopNum | WriteTaskNum | ReadTaskNum | AllowAsync |       Mean |       Error |    StdDev |      Gen 0 |   Gen 1 | Allocated |
|---------------- |-------------- |-------- |------------- |------------ |----------- |-----------:|------------:|----------:|-----------:|--------:|----------:|
| **AllowAsyncBench** | **.NET Core 2.0** |   **10000** |            **1** |           **1** |      **False** |  **11.653 ms** |   **0.7141 ms** | **0.0403 ms** |   **250.0000** |       **-** |   **2.73 KB** |
| AllowAsyncBench | .NET Core 2.1 |   10000 |            1 |           1 |      False |  11.013 ms |   1.7051 ms | 0.0963 ms |   250.0000 |       - |   2.69 KB |
| **AllowAsyncBench** | **.NET Core 2.0** |   **10000** |            **1** |           **1** |       **True** |  **12.351 ms** |   **3.2105 ms** | **0.1814 ms** |   **250.0000** |       **-** |   **2.73 KB** |
| AllowAsyncBench | .NET Core 2.1 |   10000 |            1 |           1 |       True |  10.855 ms |   2.2629 ms | 0.1279 ms |   250.0000 |       - |   2.69 KB |
| **AllowAsyncBench** | **.NET Core 2.0** |   **10000** |            **1** |         **100** |      **False** | **169.190 ms** |  **65.8978 ms** | **3.7233 ms** | **17562.5000** |       **-** |  **39.01 KB** |
| AllowAsyncBench | .NET Core 2.1 |   10000 |            1 |         100 |      False | 105.575 ms |  64.6819 ms | 3.6546 ms | 15937.5000 |       - |  28.03 KB |
| **AllowAsyncBench** | **.NET Core 2.0** |   **10000** |            **1** |         **100** |       **True** | **293.862 ms** |  **13.3134 ms** | **0.7522 ms** | **35875.0000** |       **-** |  **39.09 KB** |
| AllowAsyncBench | .NET Core 2.1 |   10000 |            1 |         100 |       True | 301.211 ms | 146.6420 ms | 8.2855 ms | 33125.0000 |       - |  28.24 KB |
| **AllowAsyncBench** | **.NET Core 2.0** |   **10000** |          **100** |           **1** |      **False** |  **12.921 ms** |   **1.6427 ms** | **0.0928 ms** |   **265.6250** | **46.8750** |  **31.09 KB** |
| AllowAsyncBench | .NET Core 2.1 |   10000 |          100 |           1 |      False |  11.487 ms |   0.8529 ms | 0.0482 ms |   281.2500 | 46.8750 |  20.99 KB |
| **AllowAsyncBench** | **.NET Core 2.0** |   **10000** |          **100** |           **1** |       **True** |  **12.872 ms** |   **0.2811 ms** | **0.0159 ms** |   **265.6250** | **46.8750** |  **31.09 KB** |
| AllowAsyncBench | .NET Core 2.1 |   10000 |          100 |           1 |       True |  11.378 ms |   1.1391 ms | 0.0644 ms |   296.8750 | 15.6250 |  20.99 KB |
| **AllowAsyncBench** | **.NET Core 2.0** |   **10000** |          **100** |         **100** |      **False** |   **5.755 ms** |   **1.0442 ms** | **0.0590 ms** |   **492.1875** |       **-** |  **60.98 KB** |
| AllowAsyncBench | .NET Core 2.1 |   10000 |          100 |         100 |      False |   2.810 ms |   0.4191 ms | 0.0237 ms |   285.1563 |       - |  40.82 KB |
| **AllowAsyncBench** | **.NET Core 2.0** |   **10000** |          **100** |         **100** |       **True** |   **7.418 ms** |   **3.4187 ms** | **0.1932 ms** |   **562.5000** |       **-** |  **60.98 KB** |
| AllowAsyncBench | .NET Core 2.1 |   10000 |          100 |         100 |       True |   4.918 ms |   9.8277 ms | 0.5553 ms |   281.2500 |       - |  40.83 KB |

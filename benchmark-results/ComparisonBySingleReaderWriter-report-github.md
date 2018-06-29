``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-4712MQ CPU 2.30GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
Frequency=2240913 Hz, Resolution=446.2467 ns, Timer=TSC
.NET Core SDK=2.1.300-rc1-008673
  [Host]     : .NET Core 2.1.0-rc1 (CoreCLR 4.6.26426.02, CoreFX 4.6.26426.04), 64bit RyuJIT
  Job-UBGGMQ : .NET Core 2.0.6 (CoreCLR 4.6.26212.01, CoreFX 4.6.26212.01), 64bit RyuJIT
  Job-WVLGZE : .NET Core 2.1.0-rc1 (CoreCLR 4.6.26426.02, CoreFX 4.6.26426.04), 64bit RyuJIT

TargetCount=3  WarmupCount=3  

```
|       Method |     Toolchain | LoopNum | SingleReader | SingleWriter |       Mean |     Error |    StdDev |   Gen 0 | Allocated |
|------------- |-------------- |-------- |------------- |------------- |-----------:|----------:|----------:|--------:|----------:|
| **SettingBench** | **.NET Core 2.0** |   **10000** |        **False** |        **False** | **1,091.2 us** | **352.63 us** | **19.924 us** | **15.6250** |   **1.88 KB** |
| SettingBench | .NET Core 2.1 |   10000 |        False |        False | 1,145.9 us | 917.52 us | 51.841 us |  1.9531 |   2.05 KB |
| **SettingBench** | **.NET Core 2.0** |   **10000** |        **False** |         **True** | **1,093.2 us** | **159.06 us** |  **8.987 us** | **19.5313** |   **1.88 KB** |
| SettingBench | .NET Core 2.1 |   10000 |        False |         True | 1,054.6 us | 303.04 us | 17.122 us |  1.9531 |   2.05 KB |
| **SettingBench** | **.NET Core 2.0** |   **10000** |         **True** |        **False** |   **882.1 us** |  **84.44 us** |  **4.771 us** | **16.6016** |   **1.87 KB** |
| SettingBench | .NET Core 2.1 |   10000 |         True |        False |   644.6 us | 426.33 us | 24.088 us |  1.9531 |   1.85 KB |
| **SettingBench** | **.NET Core 2.0** |   **10000** |         **True** |         **True** |   **875.6 us** |  **54.41 us** |  **3.075 us** | **15.6250** |   **1.87 KB** |
| SettingBench | .NET Core 2.1 |   10000 |         True |         True |   629.9 us | 228.31 us | 12.900 us |  1.9531 |   1.85 KB |

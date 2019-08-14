# Benchmark-Diacritics
This is a benchmark of 3 different methods of removing Diacritics from a string.

```// * Summary *

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.18362
Intel Core i9-9900K CPU 3.60GHz (Coffee Lake), 1 CPU, 16 logical and 8 physical cores
.NET Core SDK=3.0.100-preview6-012264
  [Host]     : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT


|            Method |      N |       Mean |     Error |    StdDev | Ratio |    Gen 0 |    Gen 1 |    Gen 2 |  Allocated |
|------------------ |------- |-----------:|----------:|----------:|------:|---------:|---------:|---------:|-----------:|
|     WithNormalize | 100000 | 7,277.3 us | 58.068 us | 54.317 us |  1.00 | 398.4375 | 398.4375 | 398.4375 |    1490 KB |
| FromStackOverflow | 100000 | 6,212.1 us | 18.210 us | 17.033 us |  0.85 | 398.4375 | 398.4375 | 398.4375 | 1489.23 KB |
|      WithEncoding | 100000 |   615.8 us |  5.764 us |  5.392 us |  0.08 |  90.8203 |  90.8203 |  90.8203 |  293.02 KB |


|            Method |      N |     Mean |    Error |   StdDev | Ratio |      Gen 0 |     Gen 1 |     Gen 2 | Allocated |
|------------------ |------- |---------:|---------:|---------:|------:|-----------:|----------:|----------:|----------:|
|     WithNormalize | 100000 | 251.6 ms | 2.518 ms | 2.232 ms |  1.00 |  6000.0000 | 2000.0000 |         - |   38.3 MB |
| FromStackOverflow | 100000 | 126.3 ms | 2.418 ms | 2.879 ms |  0.50 | 15000.0000 | 4000.0000 |         - |  96.23 MB |
|      WithEncoding | 100000 | 118.7 ms | 2.093 ms | 1.958 ms |  0.47 |  8600.0000 | 3600.0000 | 1000.0000 |  46.11 MB |


|                   Method |      N |      Mean |     Error |    StdDev | Ratio | RatioSD |
|------------------------- |------- |----------:|----------:|----------:|------:|--------:|
|          Encoding_Serial | 100000 | 109.19 ms | 0.1414 ms | 0.1322 ms |  1.00 |    0.00 |
| Encoding_ParallelForEach | 100000 |  79.13 ms | 1.5414 ms | 2.0042 ms |  0.72 |    0.02 |```

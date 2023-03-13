``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 11 (10.0.22621.1265/22H2/2022Update/SunValley2)
11th Gen Intel Core i9-11900H 2.50GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.200
  [Host]     : .NET 7.0.3 (7.0.323.6910), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.3 (7.0.323.6910), X64 RyuJIT AVX2


```
|                       Method |          Mean |      Error |     StdDev |   Gen0 | Allocated |
|----------------------------- |--------------:|-----------:|-----------:|-------:|----------:|
|                   DotNetFail | 6,109.4701 ns | 41.4692 ns | 36.7614 ns | 0.0229 |     344 B |
|                 OxidizedFail |    16.5280 ns |  0.3502 ns |  0.4675 ns | 0.0102 |     128 B |
|   SuccessIntrinsicTypeDotNet |     0.0335 ns |  0.0093 ns |  0.0073 ns |      - |         - |
| SuccessIntrinsicTypeOxidized |     2.8728 ns |  0.0373 ns |  0.0349 ns | 0.0019 |      24 B |
|       SuccessValueTypeDotNet |     0.0341 ns |  0.0068 ns |  0.0061 ns |      - |         - |
|     SuccessValueTypeOxidized |     2.9401 ns |  0.0717 ns |  0.0671 ns | 0.0019 |      24 B |
|   SuccessReferenceTypeDotNet |     3.1022 ns |  0.0361 ns |  0.0337 ns | 0.0019 |      24 B |
| SuccessReferenceTypeOxidized |     5.7636 ns |  0.0742 ns |  0.0619 ns | 0.0038 |      48 B |

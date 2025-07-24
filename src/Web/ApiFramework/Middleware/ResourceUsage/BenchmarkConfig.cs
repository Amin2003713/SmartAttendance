// using BenchmarkDotNet.Columns;
// using BenchmarkDotNet.Configs;
// using BenchmarkDotNet.Diagnosers;
// using BenchmarkDotNet.Environments;
// using BenchmarkDotNet.Exporters;
// using BenchmarkDotNet.Exporters.Csv;
// using BenchmarkDotNet.Jobs;
//
// namespace PRIMA.ApiFramework.Middleware.ResourceUsage;
//
// public class BenchmarkConfig : ManualConfig
// {
//     public BenchmarkConfig()
//     {
//         // 1. ستون‌های اضافی (BaselineRatio, Rank, ThreadCount, Etc.)
//         AddColumn(BaselineRatioColumn.RatioMean , BaselineRatioColumn.RatioStdDev);
//         AddColumn(StatisticColumn.Min , StatisticColumn.Max , StatisticColumn.Mean , StatisticColumn.StdDev , StatisticColumn.OperationsPerSecond);
//
//         // 2. Exporter (Markdown, CSV و غیره)
//         AddExporter(MarkdownExporter.GitHub);
//         AddExporter(CsvExporter.Default);
//
//         // 3. Diagnoserهای حافظه و GC
//         AddDiagnoser(MemoryDiagnoser.Default); // آمار حافظه: Allocated bytes, Gen0/1/2 Collections
//
//         // 6. Job با پیکربندی پیش‌فرضِ Release
//         AddJob(Job.Default.WithRuntime(CoreRuntime.Core60) // .NET 6.0 (می‌توانید آن را به net7/net8 تغییر دهید)
//                   .
//                   WithId("Net6.0_Release").
//                   WithPlatform(Platform.X64).
//                   WithGcServer(true).
//                   WithGcForce(true)
//         );
//
//         // 7. یک Job دیگر به عنوان Baseline (مثلاً .NET Framework یا نسخهٔ دیگری)
//         AddJob(Job.Default.WithRuntime(ClrRuntime.Net472) // .NET Framework 4.7.2 
//                   .
//                   WithId("NetFx472_Baseline").
//                   WithPlatform(Platform.X64).
//                   WithGcServer(true).
//                   WithGcForce(true).
//                   AsBaseline() // این Job در گزارش نهایی به عنوان Baseline علامت‌گذاری می‌شود
//         );
//
//
//     }
// }


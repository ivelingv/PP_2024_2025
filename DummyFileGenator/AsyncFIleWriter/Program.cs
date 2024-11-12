using System;
using System.Diagnostics;

namespace AsyncFIleWriter
{
    internal class Program
    {
        static Stopwatch Stopwatch = new Stopwatch();

        static string SourseFile = "C:\\Users\\iveli\\source" +
            "\\repos\\PP_2024_2025\\DummyFileGenator\\AsyncFileGenerator" +
            "\\bin\\Debug\\net6.0\\sourceFile.txt";

        static async Task Main(string[] args)
        {
            if (!int.TryParse(
              Console.ReadLine()?.ToString(),
              out var threadsCount)
              || threadsCount < 1
              || threadsCount > 100)
            {
                Console.WriteLine($"Invalid Thread Count");
                return;
            }

            Stopwatch.Start();

            await CopyFileInParallelAsync(threadsCount);
            //await CopyFileAsync(SourseFile, Path.GetFileName(SourseFile));

            Stopwatch.Stop();

            Console.WriteLine($"File copy completed " +
                $"for [{Stopwatch.Elapsed.TotalMilliseconds}ms]...");

            Console.ReadKey();
        }

        static async Task CopyFileAsync(
            string sourceFileName,
            string destinationFileName)
        {
            using (var sourceStream = new FileStream(
                path: sourceFileName,
                mode: FileMode.Open,
                access: FileAccess.Read,
                share: FileShare.None))
            {
                using (var destinationStream = new FileStream(
                    path: destinationFileName,
                    mode: FileMode.Create,
                    access: FileAccess.ReadWrite,
                    share: FileShare.ReadWrite))
                {
                    while (sourceStream.Position != sourceStream.Length)
                    {
                        var buffer = new byte[4096];

                        var readBytes = await sourceStream.ReadAsync(
                            buffer: buffer,
                            offset: 0,
                            count: buffer.Length);

                        await destinationStream.WriteAsync(
                            buffer: buffer,
                            offset: 0,
                            count: readBytes);
                    }
                }
            }
        }

        static async Task CopyFileInParallelAsync(
            int threadsCount)
        {
            var fileSize = await GetFileSizeAsync(SourseFile);
            var bytesPerThread = (int)Math.Ceiling((decimal)fileSize / threadsCount);
            var taskList = new List<Task>(threadsCount);

            for (int i = 0; i < threadsCount; i++)
            {
                var currentPosition = i * bytesPerThread;
                var endPosition = (int)Math.Min(currentPosition + bytesPerThread, fileSize);

                var task = CopyFileInParallelAsync(
                    currentPosition,
                    endPosition,
                    SourseFile,
                    Path.GetFileName(SourseFile));

                taskList.Add(task);
            }

            await Task.WhenAll(taskList.ToArray());

        }

        static async Task CopyFileInParallelAsync(
            int startPosition,
            int endPosition,
            string sourceFileName,
            string destinationFileName)
        {
            var buffer = new byte[4096];

            using (var sourceStream = new FileStream(
               path: sourceFileName,
               mode: FileMode.Open,
               access: FileAccess.Read,
               share: FileShare.Read))
            {
                using (var destinationStream = new FileStream(
                    path: destinationFileName,
                    mode: FileMode.Create,
                    access: FileAccess.Write,
                    share: FileShare.Write))
                {
                    while (sourceStream.Position != sourceStream.Length)
                    {
                        var readBytes = await sourceStream.ReadAsync(
                            buffer: buffer,
                            offset: 0,
                            count: buffer.Length);

                        await destinationStream.WriteAsync(
                            buffer: buffer,
                            offset: 0,
                            count: readBytes);
                    }
                }
            }
        }

        static Task<long> GetFileSizeAsync(
            string fileName)
        {
            var info = new FileInfo(fileName);
            return Task.FromResult(info.Length);
        }
    }
}

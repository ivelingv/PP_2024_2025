using System;
using System.Diagnostics;

namespace AsyncFIleWriter
{
    internal class Program
    {
        static Stopwatch Stopwatch = new Stopwatch();

        static string SourseFile = "C:\\Users\\iveli\\source" +
            "\\repos\\DummyFileGenator\\AsyncFileGenerator" +
            "\\bin\\Debug\\net6.0\\sourceFile.txt";

        static async Task Main(string[] args)
        {
            Stopwatch.Start();

            if (!int.TryParse(
                Console.ReadKey().KeyChar.ToString(), 
                out var threadsCount) 
                || threadsCount < 1 
                || threadsCount > 100)
            {
                Console.WriteLine($"Invalid Thread Count");
                return;
            }

            var fileSize = await GetFileSizeAsync(SourseFile);
            var bytesPerThread = (int)(fileSize / threadsCount);
            var currentPosition = 0;

            for (int i = 0; i < threadsCount; i++) 
            {
                CopyFileInParallelAsync(
                    currentPosition,
                    (int)(fileSize - currentPosition > bytesPerThread 
                        ? bytesPerThread
                        : fileSize - currentPosition),
                    SourseFile,
                    Path.GetFileName(SourseFile));
            }


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
            int startPosition,
            int numberOfBytes,
            string sourceFileName,
            string destinationFileName)
        {
            int totalNumberOfBytesRead = 0;
            var buffer = new byte[4096];

            using (var sourceStream = new FileStream(
                path: sourceFileName,
                mode: FileMode.Open,
                access: FileAccess.Read,
                share: FileShare.None))
            {
                sourceStream.Seek(startPosition, SeekOrigin.Begin);

                using (var destinationStream = new FileStream(
                    path: destinationFileName,
                    mode: FileMode.Create,
                    access: FileAccess.ReadWrite,
                    share: FileShare.ReadWrite))
                {
                    destinationStream.Seek(startPosition, SeekOrigin.Begin);

                    while (numberOfBytes != totalNumberOfBytesRead)
                    {
                        var bytesToRead = buffer.Length;

                        if (numberOfBytes - totalNumberOfBytesRead < bytesToRead)
                        {
                            bytesToRead = numberOfBytes - totalNumberOfBytesRead;
                        }

                        var readBytes = await sourceStream.ReadAsync(
                            buffer: buffer,
                            offset: 0,
                            count: bytesToRead);

                        await destinationStream.WriteAsync(
                            buffer: buffer,
                            offset: 0,
                            count: readBytes);
                    }
                }
            }
        }

        static Task<long> GetFileSizeAsync(string fileName)
        {
            var info = new FileInfo(fileName);
            return Task.FromResult(info.Length);
        }
    }
}

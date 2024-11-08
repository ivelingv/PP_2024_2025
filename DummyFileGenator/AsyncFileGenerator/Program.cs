using System.Text;

namespace AsyncFileGenerator
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var fileName = "sourceFile.txt";
            var content = "this is some random text";
            var buffer = Encoding.UTF8.GetBytes(content);

            using (var fileStream = new FileStream(
                path: fileName,
                mode: FileMode.Append,
                access: FileAccess.Write,
                share: FileShare.None)) 
            {
                while (true)
                {
                    for (int i = 0; i < 1_000_000; i++)
                    {
                        await fileStream.WriteAsync(
                            buffer: buffer,
                            offset: 0,
                            count: buffer.Length);
                    }

                    Console.WriteLine(
                        $"The current file size is: {fileStream.Length}");

                    var key = Console.ReadKey();

                    if (key.KeyChar == 'q')
                    {
                        break;
                    }
                }
            }
        }
    }
}

using archiver.processors;
using CommandLine;
using System;
using System.IO.Compression;

namespace archiver
{
    class Program
    {
        private static Processor processor;
        static void Main(string[] args)
        {
            Zip zip = new Zip();
            OutputConsole.StartMessage();
            Console.ReadKey();
            Console.WriteLine();

            Console.CancelKeyPress += Handler;
                     
            try
            {
                var options = Parser.Default.ParseArguments<DecompressionOption,CompressionOption>(args)
                    .WithParsed<CompressionOption>(opts => Run(CompressionMode.Compress,opts))
                     .WithParsed<DecompressionOption>(opts => Run(CompressionMode.Decompress,opts))
                      .WithNotParsed(errors => WorkError(errors));
            }
            catch (Exception ex)
            {
                OutputConsole.DisplayError(ex);
            }
            OutputConsole.ExitMessage();
            Console.ReadKey();
        }
        private static void WorkError(object errors)
        {
            OutputConsole.ShowMessage("Неверный синтаксис команды.");
        }
        private static void Run(CompressionMode operType,IZipOption options)
        {
            if (operType == CompressionMode.Compress)
            {
                processor = new CompressProcessor(options);
            }
            else
            {
                processor = new DecompressProcessor(options);
            }
                processor.Run();
        }
        /// <summary>
        /// Событие отмены работы программы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void Handler(object sender, ConsoleCancelEventArgs args)
        {
            if (processor != null)
            {
                processor.Abort();
            }
            args.Cancel = true;
        }
    }
}

using CommandLine;

namespace archiver
{
    [Verb("decompress", HelpText = "Разархивировать файл")]
    public class DecompressionOption:IZipOption
{
        public string InputFileName
        { get; set; }

        public string OutputFileName
        { get; set; }
    }
}
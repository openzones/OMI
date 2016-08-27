using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace OMInsurance.DBUtils
{
    public static class ZipHelper
    {
        public static byte[] ZipFiles(List<FileWrapper> files)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (ZipArchive archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in files)
                    {
                        var entry = archive.CreateEntry(file.Filename, CompressionLevel.NoCompression);
                        using (Stream writer = entry.Open())
                        {
                            writer.Write(file.Content, 0, file.Content.Length);
                        }
                    }
                }
                return memoryStream.ToArray();
            }
        }

        public static void UnZipFiles(string filename, string directoryName)
        {
            ZipFile.ExtractToDirectory(filename, directoryName);
        }
    }
}

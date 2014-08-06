using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace RawPageTools
{
    public static class Compression
	{
		public static async Task DecompressToFileAsync(string filePath, string decompressedFilePath)
		{
			var file = File.Open(filePath, FileMode.Open);

			using (var decompressedFileStream = File.Create(decompressedFilePath))
			{
				using (var decompressionStream = new DeflateStream(file, CompressionMode.Decompress))
				{
					await decompressionStream.CopyToAsync(decompressedFileStream);
				}
			}
		}

		public static void DecompressToFile(string filePath, string decompressedFilePath)
		{
			DecompressToFileAsync(filePath, decompressedFilePath).RunSynchronously();
		}

		public static async Task CompressToFileAsync(string filePath, string compressedFilePath)
		{
			var file = File.Open(filePath, FileMode.Open);

			using (var compressedFileStream = File.Create(compressedFilePath))
			{
				using (var compressionStream = new DeflateStream(file, CompressionMode.Compress))
				{
					await compressionStream.CopyToAsync(compressedFileStream);
				}
			}
		}

		public static void CompressToFile(string filePath, string compressedFilePath)
		{
			CompressToFileAsync(filePath, compressedFilePath).RunSynchronously();
		}
    }
}

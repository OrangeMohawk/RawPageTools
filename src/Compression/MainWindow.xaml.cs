using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows;

namespace Compression
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void MainWindow_PreviewDragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
				e.Effects = DragDropEffects.Move;
			e.Handled = true;
		}

		private async void MainWindow_PreviewDrop(object sender, DragEventArgs e)
		{
			var files = e.Data.GetData(DataFormats.FileDrop) as string[];
			if (files == null)
				return;
			var lastFile = files[files.Length - 1];

			await DecompressToFile(lastFile, lastFile + ".uncompressed");
		}

		private static async Task DecompressToFile(string filePath, string decompressedFilePath)
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

		// UNTESTED
		private static async Task CompressToFile(string filePath, string compressedFilePath)
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
	}
}

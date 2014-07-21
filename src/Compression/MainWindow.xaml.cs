using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
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
			// this actually works
			var files = e.Data.GetData(DataFormats.FileDrop) as string[];
			if (files == null)
				return;
			var lastFile = files[files.Length - 1];

			var file = File.Open(lastFile, FileMode.Open);

			using (var decompressedFileStream = File.Create(file.Name + ".uncompressed"))
			{
				using (var decompressionStream = new DeflateStream(file, CompressionMode.Decompress))
				{
					await decompressionStream.CopyToAsync(decompressedFileStream);
				}
			}

		}
	}
}

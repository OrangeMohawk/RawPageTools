using System;
using System.Globalization;
using System.IO;
using System.Windows;
using RawPageTools;

namespace RawPageHashGetter
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
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

			var hashData = await Hashing.GetHashesAsync(File.ReadAllBytes(lastFile));

			CrcTextBox.Text = hashData.CRC.ToString(CultureInfo.InvariantCulture);
			Sha1EntireBufferTextBox.Text = BitConverter.ToString(hashData.EntireBufferHash).Replace("-", "");
			Sha1FirstChunkTextBox.Text = BitConverter.ToString(hashData.FirstChunkHash).Replace("-", "");
			Sha1LastChunkTextBox.Text = BitConverter.ToString(hashData.LastChunkHash).Replace("-", "");
			e.Handled = true;
		}
	}
}

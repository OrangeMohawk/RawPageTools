using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace RawPageHashGetter
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private byte[] _entireBufferHash, _firstChunkHash, _lastChunkHash;
		private uint _crc;

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

			await Task.Run(async () =>
			{
				var bytes = File.ReadAllBytes(lastFile);

				// TODO: Have a dedicated class which has all these objects
				_crc = await RawPageTools.Hashing.GetCrcAsync(bytes);
				_entireBufferHash = await RawPageTools.Hashing.GetEntireBufferHashAsync(bytes);
				_firstChunkHash = await RawPageTools.Hashing.GetFirstChunkHashAsync(bytes);
				_lastChunkHash = await RawPageTools.Hashing.GetLastChunkHashAsync(bytes);

				CrcTextBox.Text = _crc.ToString(CultureInfo.InvariantCulture);
				Sha1EntireBufferTextBox.Text = BitConverter.ToString(_entireBufferHash).Replace("-", "");
				Sha1FirstChunkTextBox.Text = BitConverter.ToString(_firstChunkHash).Replace("-", "");
				Sha1LastChunkTextBox.Text = BitConverter.ToString(_lastChunkHash).Replace("-", "");
				e.Handled = true;
			});
		}
	}
}

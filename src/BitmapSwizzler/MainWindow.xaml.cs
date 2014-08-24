using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using BitmapSwizzler.Helpers;
using BitmapSwizzler.Models;
using Blamite.Blam;
using Blamite.Blam.Resources;
using Blamite.Injection;
using Blamite.IO;

namespace BitmapSwizzler
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
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

			var container = TagContainerHelper.OpenReader(lastFile);

			var bitmapInfo = TagContainerHelper.GetInfo(container);

			for (var i = 0; i < bitmapInfo.Count; i++)
			{
				byte[] data;

				ushort assetSalt = 0xFFFF;
				ushort assetIndex = 0xFFFF;
				var rawLength = 0;
				var offset = 0;

				// Get our raw id
				if (bitmapInfo[i].NormalRawInfo.Count != 0)
				{
					assetSalt = bitmapInfo[i].NormalRawInfo[0].AssetSalt;
					assetIndex = bitmapInfo[i].NormalRawInfo[0].AssetIndex;
					rawLength = bitmapInfo[i].SubMaps[0].RawLength;
				}

				else if (bitmapInfo[i].InterleavedRawInfo.Count != 0)
				{
					assetSalt = bitmapInfo[i].InterleavedRawInfo[bitmapInfo[i].SubMaps[0].Index1].AssetSalt;
					assetIndex = bitmapInfo[i].InterleavedRawInfo[bitmapInfo[i].SubMaps[0].Index1].AssetIndex;

					for (var x = 0; x < bitmapInfo[i].SubMaps.Count; x++)
					{
						if (bitmapInfo[i].SubMaps[x].Index1 == bitmapInfo[i].SubMaps[0].Index1)
						{
							rawLength += bitmapInfo[i].SubMaps[x].RawLength;

							if (x == 0)
								break;

							offset += bitmapInfo[i].SubMaps[x].RawLength;
						}
					}
				}

				else if (bitmapInfo[i].UnknownRawInfo.Count != 0)
				{
					assetSalt = bitmapInfo[i].UnknownRawInfo[bitmapInfo[i].SubMaps[0].Index1].AssetSalt;
					assetIndex = bitmapInfo[i].UnknownRawInfo[bitmapInfo[i].SubMaps[0].Index1].AssetIndex;

					for (var x = 0; x < bitmapInfo[i].SubMaps.Count; x++)
					{
						if (bitmapInfo[i].SubMaps[x].Index1 == bitmapInfo[i].SubMaps[0].Index1)
						{
							rawLength += bitmapInfo[i].SubMaps[x].RawLength;

							if (x == 0)
								break;

							offset += bitmapInfo[i].SubMaps[x].RawLength;
						}
					}
				}

				if (assetSalt == 0xFFFF && assetIndex == 0xFFFF)
					data = new byte[0];

				var resourceInfo = container.FindResource(new DatumIndex(assetSalt, assetIndex));

				ResourcePage page1, page2;

				try
				{

					page1 = container.FindResourcePage(resourceInfo.Location.OriginalPrimaryPageIndex);
					page2 = container.FindResourcePage(resourceInfo.Location.OriginalSecondaryPageIndex);
				}
				catch (Exception)
				{
					
				}


			}
		}
	}
}
using System;
using System.Windows;

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

		private enum TextureFormat
		{
			// ReSharper disable InconsistentNaming
			A8R8G8B8,
			AY8,
			CTX1,
			DXN,
			DXN_mono_alpha,
			DXT1,
			DXT3,
			DXT3a_alpha,
			DXT3a_mono,
			DXT5,
			DXT5a_alpha,
			DXT5a_mono,
			Unknown31,
			Y8
			// ReSharper restore InconsistentNaming
		}

		private static byte[] ModifyLinearTexture(byte[] data, int width, int height, TextureFormat texture, bool toLinear)
		{
			var destinationArray = new byte[data.Length];

			int num1, num2, num3;

			switch (texture)
			{
				case TextureFormat.DXT5a_mono:
				case TextureFormat.DXT5a_alpha:
				case TextureFormat.DXT1:
				case TextureFormat.CTX1:
				case TextureFormat.Unknown31:
				case TextureFormat.DXT3a_alpha:
				case TextureFormat.DXT3a_mono:
					num1 = 4;
					num2 = 4;
					num3 = 8;
					break;

				case TextureFormat.DXT3:
				case TextureFormat.DXT5:
				case TextureFormat.DXN:
				case TextureFormat.DXN_mono_alpha:
					num1 = 4;
					num2 = 4;
					num3 = 16;
					break;

				case TextureFormat.AY8:
				case TextureFormat.Y8:
					num1 = 1;
					num2 = 1;
					num3 = 1;
					break;

				case TextureFormat.A8R8G8B8:
					num1 = 1;
					num2 = 1;
					num3 = 4;
					break;

				default:
					num1 = 1;
					num2 = 1;
					num3 = 2;
					break;
			}

			var xChunks = width / num1;
			var yChunks = height / num2;
			try
			{
				for (var i = 0; i < yChunks; i++)
				{
					for (var j = 0; j < xChunks; j++)
					{
						var offset = (i * xChunks) + j;
						var num9 = XGAddress2DTiledX(offset, xChunks, num3);
						var num10 = XGAddress2DTiledY(offset, xChunks, num3);
						var sourceIndex = ((i * xChunks) * num3) + (j * num3);
						var destinationIndex = ((num10 * xChunks) * num3) + (num9 * num3);
						if (toLinear)
							Array.Copy(data, sourceIndex, destinationArray, destinationIndex, num3);
						else
							Array.Copy(data, destinationIndex, destinationArray, sourceIndex, num3);
					}
				}
			}
			catch { }
			return destinationArray;
		}

		// ReSharper disable InconsistentNaming
		private static int XGAddress2DTiledX(int offset, int width, int texelPitch)
		{
			var alignedWidth = (width + 31) & ~31;

			var logBPP = (texelPitch >> 2) + ((texelPitch >> 1) >> (texelPitch >> 2));
			// ReSharper restore InconsistentNaming
			var offsetB = offset << logBPP;
			var offsetT = (((offsetB & ~4095) >> 3) + ((offsetB & 1792) >> 2)) + (offsetB & 63);
			var offsetM = offsetT >> (7 + logBPP);

			var macroX = (offsetM % (alignedWidth >> 5)) << 2;
			var tile = (((offsetT >> (5 + logBPP)) & 2) + (offsetB >> 6)) & 3;
			var macro = (macroX + tile) << 3;
			var micro = ((((offsetT >> 1) & ~15) + (offsetT & 15)) & ((texelPitch << 3) - 1)) >> logBPP;

			return (macro + micro);
		}

		// ReSharper disable InconsistentNaming
		private static int XGAddress2DTiledY(int offset, int width, int texelPitch)
		{
			var alignedWidth = (width + 31) & ~31;

			var logBPP = (texelPitch >> 2) + ((texelPitch >> 1) >> (texelPitch >> 2));
			// ReSharper restore InconsistentNaming
			var offsetB = offset << logBPP;
			var offsetT = (((offsetB & ~4095) >> 3) + ((offsetB & 1792) >> 2)) + (offsetB & 63);
			var offsetM = offsetT >> (7 + logBPP);

			var macroY = (offsetM / (alignedWidth >> 5)) << 2;
			var tile = ((offsetT >> (6 + logBPP)) & 1) + ((offsetB & 2048) >> 10);
			var macro = (macroY + tile) << 3;
			var micro = (((offsetT & (((texelPitch << 6) - 1) & ~31)) + ((offsetT & 15) << 1)) >> (3 + logBPP)) & ~1;

			return ((macro + micro) + ((offsetT & 16) >> 4));
		}
	}
}
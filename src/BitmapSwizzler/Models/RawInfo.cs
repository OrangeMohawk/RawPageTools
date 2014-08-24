using System.Windows;
using BitmapSwizzler.Helpers;
using Blamite.IO;

namespace BitmapSwizzler.Models
{
	public class RawInfo : NotifyPropertyChangedBase
	{
		public RawInfo(IReader reader)
		{
			AssetSalt = reader.ReadUInt16();		// 0x0
			AssetIndex = reader.ReadUInt16();	// 0x2
			reader.ReadInt32();					// 0x4
		}

		public ushort AssetSalt
		{
			get { return _assetSalt; }
			set { SetField(ref _assetSalt, value); }
		}
		private ushort _assetSalt;

		public ushort AssetIndex
		{
			get { return _assetIndex; }
			set { SetField(ref _assetIndex, value); }
		}
		private ushort _assetIndex;
	}
}

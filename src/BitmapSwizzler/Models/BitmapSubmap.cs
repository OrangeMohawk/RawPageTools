using System.Windows;
using BitmapSwizzler.Helpers;
using Blamite.IO;

namespace BitmapSwizzler.Models
{
	public class BitmapSubmap : NotifyPropertyChangedBase
	{
		public BitmapSubmap(IReader reader, Game game)
		{
			if (game != Game.HaloReach && game != Game.Halo4)
				reader.ReadInt32();							// 0x0

			Width = reader.ReadInt16();						// 0x4 - 0x0
			Height = reader.ReadInt16();					// 0x6 - 0x2
			Depth = reader.ReadByte();						// 0x8 - 0x4
			reader.ReadByte();								// 0x9 - 0x5
			Type = (TextureType)reader.ReadInt16();			// 0xA - 0x6
			Format = (TextureFormat)reader.ReadInt16();		// 0xC - 0x8

			Flags = (BitmapFlags)reader.ReadInt16();		// 0xE - 0xA
			RegistrationPointX = reader.ReadInt16();		// 0x10 - 0xC
			RegistrationPointY = reader.ReadInt16();		// 0x12 - 0xE
			MipMapCount = reader.ReadByte();				// 0x14 - 0x10
			reader.ReadByte();								// 0x15 - 0x11
			Index1 = reader.ReadByte();						// 0x16 - 0x12
			Index2 = reader.ReadByte();						// 0x17 - 0x13
			PixelOffset = reader.ReadInt32();				// 0x18 - 0x14
			PixelCount = reader.ReadInt32();				// 0x1C - 0x18
			reader.ReadInt32();								// 0x20 - 0x1C
			RawLength = reader.ReadInt32();					// 0x24 - 0x20
		}

		public int Width
		{
			get { return _width; }
			set { SetField(ref _width, value); }
		}
		private int _width;

		public int Height
		{
			get { return _height; }
			set { SetField(ref _height, value); }
		}
		private int _height;

		public int Depth
		{
			get { return _depth; }
			set { SetField(ref _depth, value); }
		}
		private int _depth;

		public TextureType Type
		{
			get { return _type; }
			set { SetField(ref _type, value); }
		}
		private TextureType _type;

		public TextureFormat Format
		{
			get { return _format; }
			set { SetField(ref _format, value); }
		}
		private TextureFormat _format;

		public BitmapFlags Flags
		{
			get { return _flags; }
			set { SetField(ref _flags, value); }
		}
		private BitmapFlags _flags;

		public int RegistrationPointX
		{
			get { return _registrationPointX; }
			set { SetField(ref _registrationPointX, value); }
		}
		private int _registrationPointX;

		public int RegistrationPointY
		{
			get { return _registrationPointY; }
			set { SetField(ref _registrationPointY, value); }
		}
		private int _registrationPointY;

		public int MipMapCount
		{
			get { return _mipMapCount; }
			set { SetField(ref _mipMapCount, value); }
		}
		private int _mipMapCount;

		public int Index1
		{
			get { return _index1; }
			set { SetField(ref _index1, value); }
		}
		private int _index1;

		public int Index2
		{
			get { return _index2; }
			set { SetField(ref _index2, value); }
		}
		private int _index2;

		public int PixelOffset
		{
			get { return _pixelOffset; }
			set { SetField(ref _pixelOffset, value); }
		}
		private int _pixelOffset;

		public int PixelCount
		{
			get { return _pixelCount; }
			set { SetField(ref _pixelCount, value); }
		}
		private int _pixelCount;

		public int RawLength
		{
			get { return _rawLength; }
			set { SetField(ref _rawLength, value); }
		}
		private int _rawLength;

		public int VirtualWidth
		{
			get { return GetVirtualDimension(_width); }
		}

		public int VirtualHeight
		{
			get { return GetVirtualDimension(_height); }
		}

		private static int GetVirtualDimension(int size)
		{
			if (size % 128 != 0)
				size += (128 - size % 128);
			return size;
		}
	}
}

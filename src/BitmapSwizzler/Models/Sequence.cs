using System.Collections.ObjectModel;
using Blamite.IO;

namespace BitmapSwizzler.Models
{
	public class Sequence : NotifyPropertyChangedBase
	{
		public Sequence(IReader reader)
		{
			Name = reader.ReadAscii(0x20);			// 0x0
			FirstBitmapIndex = reader.ReadUInt16();	// 0x20
			BitmapCount = reader.ReadUInt16();		// 0x22
			reader.ReadInt32();						// 0x24
			reader.ReadInt32();						// 0x28
			reader.ReadInt32();						// 0x2C
			reader.ReadInt32();						// 0x30
			SpriteCount = reader.ReadUInt32();		// 0x34
			SpriteAddress = reader.ReadUInt32();	// 0x38
		}

		public string Name
		{
			get { return _name; }
			set { SetField(ref _name, value); }
		}
		private string _name;

		public ushort FirstBitmapIndex
		{
			get { return _firstBitmapIndex; }
			set { SetField(ref _firstBitmapIndex, value); }
		}
		private ushort _firstBitmapIndex;

		public ushort BitmapCount
		{
			get { return _bitmapCount; }
			set { SetField(ref _bitmapCount, value); }
		}
		private ushort _bitmapCount;

		public uint SpriteCount
		{
			get { return _spriteCount; }
			set { SetField(ref _spriteCount, value); }
		}
		private uint _spriteCount;

		public uint SpriteAddress
		{
			get { return _spriteAddress; }
			set { SetField(ref _spriteAddress, value); }
		}
		private uint _spriteAddress;

		public ObservableCollection<Sprite> Sprites
		{
			get { return _sprites; }
			set { SetField(ref _sprites, value); }
		}
		private ObservableCollection<Sprite> _sprites;
	}

	public class Sprite : NotifyPropertyChangedBase
	{
		public Sprite(IReader reader)
		{
			BitmapIndex = reader.ReadUInt32();			// 0x0
			reader.ReadUInt32();						// 0x4
			Left = reader.ReadFloat();					// 0x8
			Right = reader.ReadFloat();					// 0xC
			Top = reader.ReadFloat();					// 0x10
			Bottom = reader.ReadFloat();				// 0x14
			RegistrationPointX = reader.ReadFloat();	// 0x18
			RegistrationPointY = reader.ReadFloat();	// 0x1C
		}

		public uint BitmapIndex
		{
			get { return _bitmapIndex; }
			set { SetField(ref _bitmapIndex, value); }
		}
		private uint _bitmapIndex;

		public float Left
		{
			get { return _left; }
			set { SetField(ref _left, value); }
		}
		private float _left;

		public float Right
		{
			get { return _right; }
			set { SetField(ref _right, value); }
		}
		private float _right;

		public float Top
		{
			get { return _top; }
			set { SetField(ref _top, value); }
		}
		private float _top;

		public float Bottom
		{
			get { return _bottom; }
			set { SetField(ref _bottom, value); }
		}
		private float _bottom;

		public float RegistrationPointX
		{
			get { return _registrationPointX; }
			set { SetField(ref _registrationPointX, value); }
		}
		private float _registrationPointX;

		public float RegistrationPointY
		{
			get { return _registrationPointY; }
			set { SetField(ref _registrationPointY, value); }
		}
		private float _registrationPointY;
	}
}

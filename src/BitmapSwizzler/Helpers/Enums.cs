using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitmapSwizzler.Helpers
{
	[Flags]
	public enum BitmapFlags
	{
		None = 0,
		UpTwoDimensions = 1 << 0,
		Compressed = 1 << 1,
		Palettized = 1 << 2,
		Swizzled = 1 << 3,
		Linear = 1 << 4,
		V16U16 = 1 << 5,
		Unknown6 = 1 << 6,
		HUDBitmap = 1 << 7,
		Unknown8 = 1 << 8,
		AlwaysOn = 1 << 9,
		Unknown10 = 1 << 10,
		Unknown11 = 1 << 11,
		Interlaced = 1 << 12,
		Unknown13 = 1 << 13,
		Unknown14 = 1 << 14,
		Unknown15 = 1 << 15,
	}

	public enum Game
	{
		Other,
		Halo3,
		HaloReachBeta,
		HaloReach,
		Halo4
	}

	public enum TextureFormat
	{
		A8 = 0x0000,
		Y8 = 0x0001,
		AY8 = 0x0002,
		A8Y8 = 0x0003,
		R5G6B5 = 0x0006,
		A1R5G5B5 = 0x0008,
		A4R4G4B4 = 0x0009,
		X8R8G8B8 = 0x000A,
		A8R8G8B8 = 0x000B,
		DXT1 = 0x000E,
		DXT3 = 0x000F,
		DXT5 = 0x0010,
		P8 = 0x0011,
		Lightmap = 0x0012,
		U8V8 = 0x0016,
		DXN = 0x0021,
		CTX1 = 0x0022,
	}

	public enum TextureType
	{
		Texture2D = 0x0000,
		Texture3D = 0x0001,
		CubeMap = 0x0002,
		Sprite = 0x0003,
		UIBitmap = 0x0004
	}
}

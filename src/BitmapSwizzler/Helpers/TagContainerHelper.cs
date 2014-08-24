using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using BitmapSwizzler.Models;
using Blamite.Injection;
using Blamite.IO;

namespace BitmapSwizzler.Helpers
{
	public static class TagContainerHelper
	{
		public static TagContainer OpenReader(string path)
		{
			return TagContainerReader.ReadTagContainer(new EndianReader(File.Open(path, FileMode.Open), Endian.BigEndian));
		}

		public static ObservableCollection<BitmapInfo> GetInfo(TagContainer container)
		{
			var bitmapTags = container.Tags.Where(tag => tag.Class == 0x6269746D).ToList();

			var bitmapInfos = new ObservableCollection<BitmapInfo>();

			foreach (var bitmapTag in bitmapTags)
			{
				var sequences = new ObservableCollection<Sequence>();
				var submaps = new ObservableCollection<BitmapSubmap>();
				var normalRawInfo = new ObservableCollection<RawInfo>();
				var interleavedRawInfo = new ObservableCollection<RawInfo>();
				var unknownRawInfo = new ObservableCollection<RawInfo>();

				var dataBlock = container.FindDataBlock(bitmapTag.OriginalAddress);
				var stream = new MemoryStream(dataBlock.Data);
				var dataReader = new EndianReader(stream, Endian.BigEndian);
				Game game;

				switch (dataBlock.Data.Length)
				{
					case 0xA4:
						game = Game.Halo3;
						break;

					case 0xB0:
						game = Game.HaloReachBeta;
						break;

					case 0xC0:
						game = Game.HaloReach;
						break;

					case 0xCC:
						game = Game.Halo4;
						break;

					default:
						game = Game.Other;
						break;
				}

				// Sequences
				dataReader.SeekTo((game == Game.Halo3) ? 0x54 : (game == Game.HaloReachBeta) ? 0x60 : 0x70);			// 0x54 Halo 3 - 0x60 Reach Beta - 0x70 Reach/H4
				var sequencesCount = dataReader.ReadUInt32();
				var sequencesAddress = dataReader.ReadUInt32();

				// Bitmap Info
				dataReader.SeekTo((game == Game.Halo3) ? 0x60 : (game == Game.HaloReachBeta) ? 0x6C : 0x7C);			// 0x60 Halo 3 -  0x6C Reach Beta - 0x7C Reach/H4
				var bitmapsCount = dataReader.ReadUInt32();
				var bitmapsAddress = dataReader.ReadUInt32();

				// Raw Info
				dataReader.SeekTo((game == Game.Halo3) ? 0x8C : (game == Game.HaloReachBeta) ? 0x98 : 0xA8);			// 0x8C Halo 3 -  0x98 Reach Beta - 0xA8 Reach/H4
				var normalRawInfoCount = dataReader.ReadUInt32();
				var normalRawInfoAddress = dataReader.ReadUInt32();

				// Raw Info 2
				dataReader.SeekTo((game == Game.Halo3) ? 0x98 : (game == Game.HaloReachBeta) ? 0xA4 : 0xB4);			// 0x98 Halo 3 -  0xA4 Reach Beta - 0xB4 Reach/H4
				var interleavedRawInfoCount = dataReader.ReadUInt32();
				var interleavedRawInfoAddress = dataReader.ReadUInt32();

				// Raw Info 3
				dataReader.SeekTo((game == Game.Halo4) ? 0xC0 : 0x0);													// 0xC0 Halo 4 (Only H4)
				var unknownRawInfoCount = dataReader.ReadUInt32();
				var unknownRawInfoAddress = dataReader.ReadUInt32();

				if (sequencesAddress != 0 && sequencesCount != 0)
				{

					var sequenceBlock = container.FindDataBlock(sequencesAddress);
					var sequenceStream = new MemoryStream(sequenceBlock.Data);
					var sequenceReader = new EndianReader(sequenceStream, Endian.BigEndian);

					for (var i = 0; i < sequencesCount; i++)
					{
						sequenceReader.SeekTo(i * 0x40);
						var sequence = new Sequence(sequenceReader);

						var spriteBlock = container.FindDataBlock(sequence.SpriteAddress);
						var spriteStream = new MemoryStream(spriteBlock.Data);
						var spriteReader = new EndianReader(spriteStream, Endian.BigEndian);
						for (var ind = 0; ind < sequence.SpriteCount; ind++)
						{
							sequenceReader.SeekTo(i * 0x20);
							sequence.Sprites.Add(new Sprite(spriteReader));
						}
						spriteReader.Close();
						spriteStream.Close();

						sequences.Add(sequence);
					}

					sequenceReader.Close();
					sequenceStream.Close();
				}

				if (bitmapsAddress != 0 && bitmapsCount != 0)
				{
					var bitmapBlock = container.FindDataBlock(bitmapsAddress);
					var bitmapStream = new MemoryStream(bitmapBlock.Data);
					var bitmapReader = new EndianReader(bitmapStream, Endian.BigEndian);

					for (var i = 0; i < bitmapsCount; i++)
					{
						bitmapReader.SeekTo(i * ((game == Game.HaloReach || game == Game.Halo4) ? 0x2C : 0x30));
						submaps.Add(new BitmapSubmap(bitmapReader, game));
					}

					bitmapReader.Close();
					bitmapStream.Close();
				}

				if (normalRawInfoAddress != 0 && normalRawInfoCount != 0)
				{
					var rawBlock = container.FindDataBlock(normalRawInfoAddress);
					var rawStream = new MemoryStream(rawBlock.Data);
					var rawReader = new EndianReader(rawStream, Endian.BigEndian);

					for (var i = 0; i < normalRawInfoCount; i++)
					{
						rawReader.SeekTo(i * 8);
						normalRawInfo.Add(new RawInfo(rawReader));
					}

					rawReader.Close();
					rawStream.Close();
				}

				if (interleavedRawInfoAddress != 0 && interleavedRawInfoCount != 0)
				{
					var rawBlock = container.FindDataBlock(interleavedRawInfoAddress);
					var rawStream = new MemoryStream(rawBlock.Data);
					var rawReader = new EndianReader(rawStream, Endian.BigEndian);

					for (var i = 0; i < interleavedRawInfoCount; i++)
					{
						rawReader.SeekTo(i * 8);
						interleavedRawInfo.Add(new RawInfo(rawReader));
					}

					rawReader.Close();
					rawStream.Close();
				}

				if (game == Game.Halo4 && unknownRawInfoAddress != 0 && unknownRawInfoCount != 0)
				{
					var rawBlock = container.FindDataBlock(unknownRawInfoAddress);
					var rawStream = new MemoryStream(rawBlock.Data);
					var rawReader = new EndianReader(rawStream, Endian.BigEndian);

					for (var i = 0; i < unknownRawInfoCount; i++)
					{
						rawReader.SeekTo(i * 8);
						unknownRawInfo.Add(new RawInfo(rawReader));
					}

					rawReader.Close();
					rawStream.Close();
				}

				dataReader.Close();
				stream.Close();

				bitmapInfos.Add(new BitmapInfo
				{
					Sequences = sequences,
					SubMaps = submaps,
					NormalRawInfo = normalRawInfo,
					InterleavedRawInfo = interleavedRawInfo,
					UnknownRawInfo = unknownRawInfo
				});
			}

			return bitmapInfos;
		}
	}
}

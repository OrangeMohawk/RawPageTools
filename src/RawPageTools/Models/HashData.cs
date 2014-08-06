namespace RawPageTools.Models
{
	public class HashData : NotifyPropertyChangedBase
	{
		public byte[] EntireBufferHash
		{
			get { return _entireBufferHash; }
			set { SetField(ref _entireBufferHash, value); }
		}
		private byte[] _entireBufferHash;

		public byte[] FirstChunkHash
		{
			get { return _firstChunkHash; }
			set { SetField(ref _firstChunkHash, value); }
		}
		private byte[] _firstChunkHash;

		public byte[] LastChunkHash
		{
			get { return _lastChunkHash; }
			set { SetField(ref _lastChunkHash, value); }
		}
		private byte[] _lastChunkHash;

// ReSharper disable once InconsistentNaming
		public uint CRC
		{
			get { return _crc; }
			set { SetField(ref _crc, value); }
		}
		private uint _crc;
	}
}

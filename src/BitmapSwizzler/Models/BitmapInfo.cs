using System.Collections.ObjectModel;
using BitmapSwizzler.Helpers;

namespace BitmapSwizzler.Models
{
	public class BitmapInfo : NotifyPropertyChangedBase
	{
		public ObservableCollection<Sequence> Sequences
		{
			get { return _sequences; }
			set { SetField(ref _sequences, value); }
		}
		private ObservableCollection<Sequence> _sequences;

		public ObservableCollection<BitmapSubmap> SubMaps
		{
			get { return _subMaps; }
			set { SetField(ref _subMaps, value); }
		}
		private ObservableCollection<BitmapSubmap> _subMaps;

		public ObservableCollection<RawInfo> NormalRawInfo
		{
			get { return _normalRawInfo; }
			set { SetField(ref _normalRawInfo, value); }
		}
		private ObservableCollection<RawInfo> _normalRawInfo;

		public ObservableCollection<RawInfo> InterleavedRawInfo
		{
			get { return _interleavedRawInfo; }
			set { SetField(ref _interleavedRawInfo, value); }
		}
		private ObservableCollection<RawInfo> _interleavedRawInfo;

		public ObservableCollection<RawInfo> UnknownRawInfo // Halo 4 only
		{
			get { return _unknownRawInfo; }
			set { SetField(ref _unknownRawInfo, value); }
		}
		private ObservableCollection<RawInfo> _unknownRawInfo;
	}
}

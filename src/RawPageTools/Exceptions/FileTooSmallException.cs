using System;

namespace RawPageTools.Exceptions
{
	public class FileTooSmallException : Exception
	{
		public FileTooSmallException()
		{
		}

		public FileTooSmallException(string message) : base(message)
		{
		}
	}
}
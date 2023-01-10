using System.Runtime.Serialization;

namespace NiTiS.IO;

public class OperationNotSupportedException : Exception
{
	public OperationNotSupportedException()
	{
	}

	public OperationNotSupportedException(string? message) : base(message)
	{
	}

	public OperationNotSupportedException(string? message, Exception? innerException) : base(message, innerException)
	{
	}

	protected OperationNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}

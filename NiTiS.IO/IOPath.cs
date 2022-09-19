using System.Runtime.Serialization;

namespace NiTiS.IO;

public abstract class IOPath : ISerializable
{
	public abstract string Name { get; }
	public abstract string Path { get; }
	public abstract bool IsDirectory { get; }
	public abstract Directory? Parent { get; }
	public abstract bool Exists { get; }
	public abstract bool Readonly { get; }
	public abstract bool Hidden { get; }
	public override string ToString()
		=> Path;
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue("path", Path);
		info.AddValue("directory", IsDirectory);
	}
#if NET7_0_OR_GREATER
 	public static T Combine<T>(Directory dir, params string[] args) where T : IOPath
		=> new T()
#endif
	public const char UnixPathSeparator = ':';
	public const char WindowsPathSeparator = ';';
	public static readonly char PathSeparator = SPath.PathSeparator;
}
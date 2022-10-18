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
	public const char UnixPathSeparator = ':';
	public const char WindowsPathSeparator = ';';
	public const char UnixVolumeSeparator = '/';
	public const char WindowsVolumeSeparator = ':';
	public static readonly char PathSeparator = SPath.PathSeparator;
	public static readonly char VolumeSeparator = SPath.VolumeSeparatorChar;
	public static File CombinePathFile(params string[] paths)
		=> new(SPath.Combine(paths));
	public static Directory CombinePathDirectory(params string[] paths)
		=> new(SPath.Combine(paths));
}
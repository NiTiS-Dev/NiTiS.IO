using System.Runtime.Serialization;

namespace NiTiS.IO;

public static class IOPath
{
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
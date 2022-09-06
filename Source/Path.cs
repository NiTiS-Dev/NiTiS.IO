using System.Linq;

namespace NiTiS.IO;

public abstract class Path
{
	protected string path;
	public static string Combine(params string[]? args) => args is null ? String.Empty : SPath.Combine(args);
	public static char DirectorySeparator => SPath.DirectorySeparatorChar;
	public static char AltDirectorySeparator => SPath.AltDirectorySeparatorChar;
	public static char VolumeSeparator => SPath.VolumeSeparatorChar;
	public static char PathSeparator => SPath.PathSeparator;
	public static char[] InvalidPathChars => SPath.GetInvalidPathChars();
	public static char[] InvalidFileNameChars => SPath.GetInvalidFileNameChars();
	public abstract MemorySize Size { get; }
	public Path(string path)
	{
		this.path = path;
	}
	public abstract bool IsExists();
	public static bool IsPathValid(Path path) => IsPathValid(path.path);
	public static bool IsPathValid(string path) => path.All(x => !InvalidPathChars.Contains(x)) && path.Split(DirectorySeparator).Last().All(x => InvalidFileNameChars.Contains(x));
}

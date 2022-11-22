using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NiTiS.IO;

/// <summary>
/// Presentation of some directory
/// </summary>
[Serializable]
public class Directory : IDirectory, IFormattable
{
	protected internal readonly DirectoryInfo self;
	public Directory(string path)
	{
		self = new(path);
	}
	public Directory(DirectoryInfo path)
	{
		self = path ?? new DirectoryInfo(string.Empty);
	}

	public string Name => self.Name;
	public MemorySize Size => GetDirectorySize();
	public string Path => self.FullName;
	public bool IsRoot => SPath.IsPathRooted(self.FullName);
	public IDirectory? Parent => new Directory(self.Parent);
	public bool Exists => self.Exists;
	public MemorySize GetDirectorySize()
		=> new(unchecked((ulong)GetNestedFiles().Sum(x => unchecked((long)x.Size.Bytes))));
	public IEnumerable<IFile> GetNestedFiles(bool topLevelOnly = false)
		=> self.GetFiles("*", topLevelOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories).Select(x => new File(x));

	#region Created/Edited time
	public DateTime CreationTime { get => self.CreationTime; set => self.CreationTime = value; }
	public DateTime LastAccessTime { get => self.LastAccessTime; set => self.LastAccessTime = value; }
	public DateTime CreationTimeUTC { get => self.CreationTimeUtc; set => self.CreationTimeUtc = value; }
	public DateTime LastAccessTimeUTC { get => self.LastAccessTimeUtc; set => self.LastAccessTimeUtc = value; }
	#endregion
	public bool Create()
	{
		if (self.Exists)
			return false;
		try
		{
			self.Create();
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}
	public bool Delete()
	{
		if (!self.Exists)
			return false;
		try
		{
			self.Delete();
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}
	public IFile File(string name)
		=> new File(SPath.Combine(self.FullName, name));
	public IDirectory SubDirectory(string name)
		=> new Directory(SPath.Combine(self.FullName, name));

	public string ToString(string? format, IFormatProvider? formatProvider)
		=> ToString(format);
	public string ToString(string format)
	{
		if (format == nameof(Name))
		{
			return Name;
		}
		if (format == nameof(Path))
		{
			return Path;
		}
		throw new ArgumentException("Invalid format");
	}
	public override string ToString()
		=> Path;
	public static explicit operator File(Directory dir)
		=> new(dir.Path);
	public static explicit operator string(Directory dir)
		=> dir.Path;
	public static Directory GetTemp()
		=> new(SPath.GetTempPath());

	public const char UnuxSeparator = '/';
	public const char WindowsSeparator = '\\';
	public static readonly char Separator = SPath.DirectorySeparatorChar;
	public static readonly char[] InvalidChars = SPath.GetInvalidPathChars();
}
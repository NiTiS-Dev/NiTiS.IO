using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NiTiS.IO;

public sealed class RealFile : IRealFile, IFormattable
{
	private readonly FileInfo info;

	public RealFile(FileInfo fileInfo)
	{
		this.info = fileInfo;
	}
	public RealFile(string path)
	{
		this.info = new FileInfo(path);
	}

	public bool Exists
		=> info.Exists;
	public string Name
		=> info.Name;
	public string Path
		=> info.FullName;
	public string? Extension
		=> info.Extension;

	public IDirectory? Parent
		=> info.Directory is null ? null : new RealDirectory(info.Directory);

	public MemorySize Size
		=> new(info.Length);

	public Stream? Create()
		=> info.Create();
	public bool Delete()
	{
		if (info.Exists)
		{
			info.Delete();
			return true;
		}
		return false;
	}
	public Stream? Open(FileOpenMode openMode, FileOpenLocation location, bool clearData)
	{
		if (!info.Exists)
			return null;

		FileAccess access = openMode switch {
			FileOpenMode.Read => FileAccess.Read,
			FileOpenMode.Write => FileAccess.Write,
			FileOpenMode.ReadWrite => FileAccess.ReadWrite,
			_ => throw new NotSupportedException()
		};

		FileMode mode = location switch
		{
			FileOpenLocation.End when access is FileAccess.Read => throw new InvalidOperationException("Reading imposible when start location is end"),
			FileOpenLocation.End => FileMode.Append,
			_ when clearData => FileMode.Truncate,
			FileOpenLocation.Begin => FileMode.OpenOrCreate,
			_ => throw new NotSupportedException()
		};


#if NET6_0_OR_GREATER
		return SFile.Open(info.FullName, new FileStreamOptions()
		{
			Access = access,
			Mode = mode,
		});
#else
		return SFile.Open(info.FullName, mode, access);
#endif
	}

	public override string ToString()
		=> info.Name;
	public string ToString(string? format, IFormatProvider? formatProvider)
		=> ToString(format);
	public string ToString(string? format)
	{
		if (format is null)
			return ToString();

		if (format.StartsWith("Size"))
		{
			format = format.Substring(4);
			
			if (format.StartsWith(":"))
			{
				format = format.Substring(1);
				return Size.ToString(format);
			}
			else if (format.Length == 0)
				return Size.ToString();
			else
				throw new ArgumentException("Invalid file size format", nameof(format));
		}

		if (format == nameof(Name))
			return Name;

		if (format == nameof(Path))
			return Path;

		if (format == nameof(Extension))
			return Extension ?? Name;

		throw new ArgumentException("Invalid file format", nameof(format));
	}
}
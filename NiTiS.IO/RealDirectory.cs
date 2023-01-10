using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NiTiS.IO;

public sealed class RealDirectory : IRealDirectory
{
	private readonly DirectoryInfo info;

	public RealDirectory(DirectoryInfo directoryInfo)
	{
		this.info = directoryInfo;
	}
	public RealDirectory(string path)
	{
		this.info = new DirectoryInfo(path);
	}

	public bool IsRoot
		=> SDir.GetDirectoryRoot(info.FullName) == info.FullName;
	public bool Exists
		=> info.Exists;
	public string Name
		=> info.Name;
	public string Path
		=> info.FullName;
	public IDirectory? Parent
		=> info.Parent is null ? null : new RealDirectory(info.Parent);
	public bool Create()
	{
		if (info.Exists)
			return false;

		info.Create();
		return true;
	}
	public bool Delete()
	{
		if (!info.Exists)
			return false;

		info.Delete(true);
		return true;
	}
	public IEnumerable<IDirectory>? GetNestedDirectories(bool recursive)
		=> info.GetDirectories("", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Select(d => new RealDirectory(d));

	public IEnumerable<IFile>? GetNestedFiles(bool recursive)
		=> info.GetFiles("", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Select(d => new RealFile(d));
}
using System.Collections.Generic;
using System.Linq;

namespace NiTiS.IO;

/// <summary>
/// Presentation of some directory
/// </summary>
[System.Diagnostics.DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public sealed class Directory : NamedPath
{
	public Directory(params string[] path) : base(Combine(path)) { }
	public Directory(string path) : base(path) { }
	public Directory(Directory directory, string directoryName) : base(Combine(directory.Path, directoryName)) { }
	public Directory(File file) : base(file.Parent.path) { }
	public string Path => this.path;
	public override string Name => SPath.GetFileName(this.path);
	/// <summary>
	/// Directory where located this directory
	/// </summary>
	/// <exception cref="NiTiS.IO.RootFolderNotFoundException"></exception>
	public Directory Parent
	{
		get
		{
			System.IO.DirectoryInfo? info = null;
			try
			{
				info = SDir.GetParent(this.path);
			}
			catch (System.IO.DirectoryNotFoundException) { }
			return info is null ? throw new DirectoryNotExistsExeption(this) : (new(info.FullName));
		}
	}
	public DateTime CreationTime { get => SDir.GetCreationTime(this.path); set => SDir.SetCreationTime(this.path, value); }
	public DateTime LastAccessTime { get => SDir.GetLastAccessTime(this.path); set => SDir.SetLastAccessTime(this.path, value); }
	public DateTime CreationTimeUTC { get => SDir.GetLastWriteTimeUtc(this.path); set => SDir.SetCreationTimeUtc(this.path, value); }
	public DateTime LastAccessTimeUTC { get => SDir.GetLastAccessTimeUtc(this.path); set => SDir.SetLastAccessTime(this.path, value); }
	public override bool IsExists() => SDir.Exists(path);

	public override MemorySize Size
	{
		get
		{
			MemorySize size = new(GetDirectories().Select(s => s.Size).Sum(x => x.bytes));
			MemorySize size2 = new(GetDirectories().Select(s => s.Size).Sum(x => x.bytes));

			return size + size2;
		}
	}

	/// <summary>
	/// Create <see cref="IO.File"/> instance (without creating a real file)
	/// </summary>
	/// <returns>File with name "<paramref name="fileName"/>" alocated in this directory</returns>
	public File File(string fileName)
		=> new(path, fileName);
	/// <summary>
	/// Renaming the directory
	/// </summary>
	/// <param name="newName">New name</param>
	/// <param name="overwrite">Should I delete the directory with the same name</param>
	public void Rename(string newName, bool overwrite = false)
	{
		ThrowIfNotExists();
		Directory newLocation = new(Parent.Path, newName);
		if (overwrite)
		{
			newLocation.Delete();
		}
		this.path = newLocation.path;
	}
	/// <summary>
	/// Returns path separated by folders
	/// </summary>
	public String[] Separate()
	{
#if NET48
		return path.Split(new char[] { SPath.DirectorySeparatorChar }, StringSplitOptions.None);
#else
		return this.path.Split(SPath.DirectorySeparatorChar, StringSplitOptions.None);
#endif
	}
	/// <summary>
	/// Get other folders from this directory
	/// </summary>
	/// <param name="selfInclude">Include this directory</param>
	/// <returns></returns>
	public Directory[] GetNearbyDirectories(bool selfInclude = false)
	{
		return SDir.GetDirectories(SDir.GetDirectoryRoot(this.path)).Where(s => selfInclude || s != this.path).Select(s => new Directory(s)).ToArray();
	}
	/// <summary>
	/// Get internal files
	/// </summary>
	public File[] GetFiles()
	{
		return SDir.GetFiles(this.path).Select(s => new File(s)).ToArray();
	}
	/// <summary>
	/// Create directory if not exists
	/// </summary>
	public void Create()
	{
		if (IsExists()) return;
		SDir.CreateDirectory(this.path);
	}
	/// <summary>
	/// Get internal directories
	/// </summary>
	public Directory[] GetDirectories()
	{
		return SDir.GetDirectories(this.path).Select(s => new Directory(s)).ToArray();
	}
	public void ThrowIfNotExists()
	{
		if (!IsExists()) throw new DirectoryNotExistsExeption(this);
	}
	/// <summary>
	/// Delete current file from storage
	/// </summary>
	public void Delete(bool recursive) => SDir.Delete(path, recursive);
	/// <summary>
	/// Delete current file from storage
	/// </summary>
	public void Delete() => SDir.Delete(path);

	public static Directory GetCurrentDirectory() => new Directory(SDir.GetCurrentDirectory());
	public static Directory GetEnviromentDirectory(Environment.SpecialFolder folder) => new Directory(Environment.GetFolderPath(folder));

	public override string ToString()
	{
		return $"\"{this.path}\" [{(IsExists() ? "E" : "NE")}]";
	}
	private string GetDebuggerDisplay() => ToString();
	/// <summary>
	/// Returns <see langword="true"/> when all directories exists and not null
	/// </summary>
	public static bool AllExists(params Directory[] directories)
	{
		foreach (Directory? directory in directories)
		{
			if (!(directory?.IsExists() ?? false)) return false;
		}
		return true;
	}
}
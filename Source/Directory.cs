using NiTiS.IO.Windows;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Versioning;

namespace NiTiS.IO;

/// <summary>
/// Presentation of some directory
/// </summary>
[Serializable]
public class Directory : IOPath
{
	private DirectoryInfo self;
	public Directory(string path) : base()
	{
		self = new(path);
	}
	public Directory(DirectoryInfo path) : base()
	{
		self = path;
	}

	public override string Name => self.Name;
	public override string Path => self.FullName;
	public override bool IsDirectory => true;
	public override Directory? Parent
		=> self.Parent is null ? null :
		new(self.Parent);
	public override bool Exists => self.Exists;
	public override bool Readonly => self.Attributes.HasFlag(FileAttributes.ReadOnly);
	public override bool Hidden => self.Attributes.HasFlag(FileAttributes.Hidden);
	public virtual bool IsRoot => SPath.IsPathRooted(self.FullName);
	public MemorySize GetDirectorySize()
		=> new(self.GetFiles("*", SearchOption.AllDirectories).Sum(x => x.Length));

	#region Created/Edited time
	public DateTime CreationTime { get => self.CreationTime; set => self.CreationTime = value; }
	public DateTime LastAccessTime { get => self.LastAccessTime; set => self.LastAccessTime = value; }
	public DateTime CreationTimeUTC { get => self.CreationTimeUtc; set => self.CreationTimeUtc = value; }
	public DateTime LastAccessTimeUTC { get => self.LastAccessTimeUtc; set => self.LastAccessTimeUtc = value; }
	#endregion

	public File File(string name)
		=> new(SPath.Combine(self.FullName, name));
	public Directory SubDirectory(string name)
		=> new(SPath.Combine(self.FullName, name));

	/// <summary>
	/// Change the pointer directory name (does not change the name of the real directory)
	/// </summary>
	/// <param name="newName">New name of the directory</param>
	public void VirtualRename(string newName)
	{
		self = new DirectoryInfo(ItselfPathWithOtherName(newName));
	}
	/// <summary>
	/// Changes the name of a directory
	/// </summary>
	/// <param name="newName">New name of the directory</param>
	public void Rename(string newName)
	{
		string newPath = ItselfPathWithOtherName(newName);
		self.MoveTo(newPath);
	}
	protected string ItselfPathWithOtherName(string newName)
		=> (self.Parent is null ? Separator.ToString() : self.Parent.FullName + Separator) + newName;
	/// <summary>
	/// Move the pointer directory (does not change the name of the real directory)
	/// </summary>
	/// <param name="dest">New path of the directory</param>
	public void VirtualMove(string dest)
	{
		self = new DirectoryInfo(dest);
	}
	/// <summary>
	/// Move the directory
	/// </summary>
	/// <param name="dest">New path of the directory</param>
	public void Move(string dest)
	{
		self.MoveTo(dest);
	}
	public void Create()
		=> self.Create();
	public void Delete()
		=> self.Delete();
	public bool TryDelete()
	{
		try
		{
			Delete();
			return true;
		}
		catch (IOException)
		{
			return false;
		}
	}
	/// <summary>
	/// Creates a file symbolic link identified by <paramref name="linkFile"/> that points to <see langword="this"/>
	/// </summary>
	/// <param name="linkFile">File path to place link</param>
	/// <returns>Symbolic link</returns>
	/// <exception cref="IOException" />
#if NET5_0_OR_GREATER
	[SupportedOSPlatform("Windows")]
#endif
	[DebuggerStepThrough]
	public Directory CreateSymbolicLink(File linkFile)
	{
		if (1 != WindowsAPI.CreateSymbolicLink(linkFile.Path, self.FullName, SymbolicLinkOptions.ToDirectory))
		{
			int errCode = Marshal.GetLastWin32Error();
			if (errCode == 183)
				throw new IOException("File or Directory already exists");
			throw new IOException("Unable to create symbolic link, error code: " + errCode);
		}

		return new(linkFile.Path);
	}

	public static explicit operator File(Directory dir)
		=> new(dir.Path);
	public static explicit operator String(Directory dir)
		=> dir.Path;

	public const char UnuxSeparator = '/';
	public const char WindowsSeparator = '\\';
	public static readonly char Separator = SPath.DirectorySeparatorChar;
}
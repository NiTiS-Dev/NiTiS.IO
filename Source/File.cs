using NiTiS.IO.Windows;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Versioning;

namespace NiTiS.IO;

/// <summary>
/// Provides methods for action with existing file
/// </summary>
[Serializable]
public class File : IOPath, ISerializable
{
	private FileInfo self;
	public File(string path) : base()
	{
		self = new(path);
	}
	public File(FileInfo path) : base()
	{
		self = path;
	}
	public override string Name => self.Name;
	public override string Path => self.FullName;
	public override bool IsDirectory => false;
	public override Directory? Parent
		=> self.Directory is null ? null
		: new(self.Directory);
	public override bool Exists => self.Exists;
	public override bool Readonly => self.Attributes.HasFlag(FileAttributes.ReadOnly);
	public override bool Hidden => self.Attributes.HasFlag(FileAttributes.Hidden);
	#region File-only
	public string NameWithoutExtension => SPath.GetFileNameWithoutExtension(self.FullName);
	public string Extension => self.Extension;
	public MemorySize Size => new(self.Length);
	#endregion
	#region Created/Edited time
	public DateTime CreationTime { get => self.CreationTime; set => self.CreationTime = value; }
	public DateTime LastAccessTime { get => self.LastAccessTime; set => self.LastAccessTime = value; }
	public DateTime CreationTimeUTC { get => self.CreationTimeUtc; set => self.CreationTimeUtc = value; }
	public DateTime LastAccessTimeUTC { get => self.LastAccessTimeUtc; set => self.LastAccessTimeUtc = value; }
	#endregion
	/// <summary>
	/// Change the pointer file name (does not change the name of the real file)
	/// </summary>
	/// <param name="newName">New name of the file</param>
	public void VirtualRename(string newName)
	{
		self = new FileInfo(ItselfPathWithOtherName(newName));
	}
	/// <summary>
	/// Changes the name of the file
	/// </summary>
	/// <param name="newName">New name of the file</param>
	public void Rename(string newName)
	{
		string newPath = ItselfPathWithOtherName(newName);
		self.MoveTo(newPath);
	}
	protected string ItselfPathWithOtherName(string newName)
		=> (self.Directory is null ? Directory.Separator.ToString() : self.Directory.FullName + Directory.Separator) + newName;
	/// <summary>
	/// Move the pointer file (does not change the name of the real file)
	/// </summary>
	/// <param name="dest">New path of the file</param>
	public void VirtualMove(string dest)
	{
		self = new FileInfo(dest);
	}
	/// <summary>
	/// Move the file
	/// </summary>
	/// <param name="dest">New path of the file</param>
	public void Move(string dest)
	{
		self.MoveTo(dest);
	}
	[DebuggerStepThrough]
	public void Create()
		=> self.Create();
	[DebuggerStepThrough]
	public void Delete()
		=> self.Delete();
	[DebuggerStepThrough]
	public bool TryDelete()
	{
		try
		{
			Delete();
			return true;
		}
		catch(Exception)
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
	public File CreateSymbolicLink(File linkFile)
	{
		if (1 != WindowsAPI.CreateSymbolicLink(linkFile.Path, self.FullName, SymbolicLinkOptions.ToFile))
		{
			int errCode = Marshal.GetLastWin32Error();
			if (errCode == 183)
				throw new IOException("File already exists");
			throw new IOException("Unable to create symbolic link, error code: " + errCode);
		}

		return linkFile;
	}

	public static explicit operator Directory(File dir)
		=> new(dir.Path);
	public static explicit operator String(File dir)
		=> dir.Path;
}
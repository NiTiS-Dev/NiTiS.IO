using System.IO;

namespace NiTiS.IO;

/// <summary>
/// Provides methods for action with existing file
/// </summary>
public sealed class File : IOPath
{
	private readonly FileInfo self;
	public File(string path) : base(path)
	{
		self = new(path);
	}
	public File(FileInfo path) : base(path.FullName)
	{
		self = path;
	}
	public override string? Name => self.Name;
	public override string? Path => self.FullName;
	public override bool IsDirectory => false;
	public override Directory? Parent
		=> self.Directory is null ? null :
		new(self.Directory);
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
}
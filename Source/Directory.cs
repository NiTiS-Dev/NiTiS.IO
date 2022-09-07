using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NiTiS.IO;

/// <summary>
/// Presentation of some directory
/// </summary>
public sealed class Directory : IOPath
{
	private readonly DirectoryInfo self;
	public Directory(string path) : base(path)
	{
		self = new(path);
	}
	public Directory(DirectoryInfo path) : base(path.FullName)
	{
		self = path;
	}

	public override string? Name => self.Name;
	public override string? Path => self.FullName;
	public override bool IsDirectory => true;
	public override Directory? Parent
		=> self.Parent is null ? null :
		new(self.Parent);
	public override bool Exists => self.Exists;
	public override bool Readonly => self.Attributes.HasFlag(FileAttributes.ReadOnly);
	public override bool Hidden => self.Attributes.HasFlag(FileAttributes.Hidden);
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
	public void MoveTo(IOPath to)
		=> self.MoveTo(to.InitPath);
	public void MoveTo(string to)
		=> self.MoveTo(to);
}
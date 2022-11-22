using System.Collections.Generic;

namespace NiTiS.IO;

public interface IDirectory : IPath
{
	public string Name { get; }
	public MemorySize Size { get; }
	public bool IsRoot { get; }
	public bool Exists { get; }
	public IEnumerable<IFile> GetNestedFiles(bool topLevelOnly = false);
	public bool Delete();
	public bool Create();
	public IDirectory SubDirectory(string name);
	public IFile File(string name);
}
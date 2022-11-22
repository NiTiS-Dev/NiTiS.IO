namespace NiTiS.IO;

public interface IFile : IPath
{
	public string Name { get; }
	public string Extension { get; }
	public bool Exists { get; }
	public MemorySize Size { get; }
	public bool Create();
	public bool Delete();
}
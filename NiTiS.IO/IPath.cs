namespace NiTiS.IO;

public interface IPath
{
	public string Path { get; }
	public IDirectory? Parent { get; }
}
namespace NiTiS.IO;

public abstract class IOPath
{
	public readonly string InitPath;
	protected IOPath(string path)
	{
		InitPath = path;
	}
	public abstract string? Name { get; }
	public abstract string? Path { get; }
	public abstract bool IsDirectory { get; }
	public abstract Directory? Parent { get; }
	public abstract bool Exists { get; }
	public abstract bool Readonly { get; }
	public abstract bool Hidden { get; }
	public override string ToString()
		=> InitPath;
}
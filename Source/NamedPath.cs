namespace NiTiS.IO;

public abstract class NamedPath : Path
{
	protected NamedPath(string path) : base(path)
	{
	}
	public abstract string Name { get; }
}
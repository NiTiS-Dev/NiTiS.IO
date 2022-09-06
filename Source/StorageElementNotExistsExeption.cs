namespace NiTiS.IO;

public sealed class DirectoryNotExistsExeption : Exception
{
	public Directory Directory { get; }
	public DirectoryNotExistsExeption(Directory directory)
	{
		Directory = directory;
	}
	public override string Message => $"Directory not found by path {Directory.Path}";
}
public sealed class FileNotExistsExeption : Exception
{
	public File File { get; }
	public FileNotExistsExeption(File file)
	{
		File = file;
	}
	public override string Message => $"File not found by path {File.Path}";
}
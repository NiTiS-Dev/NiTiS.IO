namespace NiTiS.IO;

public sealed class DirectoryAlreadyExistsException : Exception
{
	public Directory Directory { get; }
	public DirectoryAlreadyExistsException(Directory directory)
	{
		Directory = directory;
	}
	public override string Message => $"Directory already exists by path {Directory.Path}";
}
public sealed class FileAlreadyExistsException : Exception
{
	public File File { get; }
	public FileAlreadyExistsException(File file)
	{
		File = file;
	}
	public override string Message => $"File already exists by path {File.Path}";
}
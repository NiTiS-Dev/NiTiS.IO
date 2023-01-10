namespace NiTiS.IO;

public interface IFileSystem
{
	IFile GetFile(bool @internal = false);
	IDirectory GetDirectory(bool @internal = false);
}
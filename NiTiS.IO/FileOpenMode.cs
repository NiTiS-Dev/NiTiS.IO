namespace NiTiS.IO;

public enum FileOpenMode : byte
{
	Read = 1,
	Write = 2,
	ReadWrite = Read | Write,
}
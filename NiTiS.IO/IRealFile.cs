using System.IO;

namespace NiTiS.IO;

public interface IRealFile : IFile
{
	/// <summary>
	/// File size
	/// </summary>
	MemorySize Size { get; }
	/// <summary>
	/// Exists file or not
	/// </summary>
	bool Exists { get; }
	/// <summary>
	/// Creates file if not exists
	/// </summary>
	/// <returns>Returns stream when creation is success, otherwise returns <see langword="null"/></returns>
	Stream? Create();
	/// <summary>
	/// Deletes file
	/// </summary>
	/// <returns>Return back <see langword="true"/> if deletion was successfully, otherwise <see langword="false"/></returns>
	bool Delete();
}

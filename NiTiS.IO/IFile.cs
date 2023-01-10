using System.IO;

namespace NiTiS.IO;

public interface IFile
{
	/// <summary>
	/// File name
	/// </summary>
	string Name { get; }
	/// <summary>
	/// Path to file
	/// </summary>
	string Path { get; }
	/// <summary>
	/// File path part after dot
	/// </summary>
	string? Extension { get; }

	IDirectory? Parent { get; }

	/// <summary>
	/// Creates file
	/// </summary>
	/// <param name="openMode">Write or Read</param>
	/// <param name="location">Location to begin write or read</param>
	/// <param name="clearData">Rewrite whole file when <see langword="true" /></param>
	/// <returns></returns>
	Stream? Open(FileOpenMode openMode, FileOpenLocation location, bool clearData);
}
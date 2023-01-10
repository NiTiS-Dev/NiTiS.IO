using System.Collections.Generic;

namespace NiTiS.IO;

public interface IDirectory
{
	/// <summary>
	/// Directory name
	/// </summary>
	string Name { get; }
	/// <summary>
	/// Directory path
	/// </summary>
	string Path { get; }
	/// <summary>
	/// Directory's father
	/// </summary>
	IDirectory? Parent { get; }
	IEnumerable<IFile>? GetNestedFiles(bool recursive);
	IEnumerable<IDirectory>? GetNestedDirectories(bool recursive);
}

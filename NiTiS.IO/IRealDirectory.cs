using System.IO;

namespace NiTiS.IO;

public interface IRealDirectory : IDirectory
{
	/// <summary>
	/// Directory is root or not
	/// </summary>
	bool IsRoot { get; }
	/// <summary>
	/// Exists directory or not
	/// </summary>
	bool Exists { get; }
	/// <summary>
	/// Creates directory if not exists
	/// </summary>
	/// <returns>Return back <see langword="true"/> if creation was successfully, otherwise <see langword="false"/></returns>
	bool Create();

	/// <summary>
	/// Deletes directory
	/// </summary>
	/// <returns>Return back <see langword="true"/> if deletion was successfully, otherwise <see langword="false"/></returns>
	bool Delete();
}
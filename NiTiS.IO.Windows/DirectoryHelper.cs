using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace NiTiS.IO.Windows;

public static class DirectoryHelper
{
	/// <summary>
	/// Creates a file symbolic link identified by <paramref name="linkFile"/> that points to <see langword="this"/>
	/// </summary>
	/// <param name="linkFile">File path to place link</param>
	/// <returns>Symbolic link</returns>
	/// <exception cref="IOException" />
#if NET5_0_OR_GREATER
	[SupportedOSPlatform("Windows")]
#endif
	[DebuggerStepThrough]
	public static Directory CreateSymbolicLink(this Directory directory, File linkFile)
	{
		if (1 != WindowsAPI.CreateSymbolicLink(linkFile.Path, directory.self.FullName, SymbolicLinkOptions.ToDirectory))
		{
			int errCode = Marshal.GetLastWin32Error();
			if (errCode == 183)
				throw new IOException("File or Directory already exists");
			throw new IOException("Unable to create symbolic link, error code: " + errCode);
		}

		return new(linkFile.Path);
	}
}

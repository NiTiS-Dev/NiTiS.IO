using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiTiS.IO.Windows;

public enum SymbolicLinkOptions : uint
{
	/// <summary>
	/// The link target is a file
	/// </summary>
	ToFile = 0,
	/// <summary>
	/// The link target is a directory
	/// </summary>
	ToDirectory = 1,
	/// <summary>
	/// Specify this flag to allow creation of symbolic links when the process is not elevated<br/>
	/// <a href="https://docs.microsoft.com/en-us/windows/apps/get-started/enable-your-device-for-development">Developer</a> Mode must first be enabled on the machine before this option will function
	/// </summary>
	AllowUnprivilegedCrate = 2,
}

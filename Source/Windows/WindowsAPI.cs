using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace NiTiS.IO.Windows;

#if NET5_0_OR_GREATER
[SupportedOSPlatform("Windows")]
#endif
internal static class WindowsAPI
{
	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
#if NET5_0_OR_GREATER
	[SupportedOSPlatform("Windows")]
#endif
	internal static extern DriveType GetDriveType(string path);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
#if NET5_0_OR_GREATER
	[SupportedOSPlatform("Windows")]
#endif
	internal static extern int CreateSymbolicLink(string path, string destPath, SymbolicLinkOptions flags);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
#if NET5_0_OR_GREATER
	[SupportedOSPlatform("Windows")]
#endif
	internal static extern uint GetLastError();
}
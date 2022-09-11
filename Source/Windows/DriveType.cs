namespace NiTiS.IO.Windows;

public enum DriveType : uint
{
	/// <summary>
	/// The drive type cannot be determined
	/// </summary>
	Unknown = 0,
	/// <summary>
	/// The root path is invalid; for example, there is no volume mounted at the specified path
	/// </summary>
	NoRootDirectory = 1,
	/// <summary>
	/// The drive has removable media; for example, a floppy drive, thumb drive, or flash card reader.
	/// </summary>
	Removable = 2,
	/// <summary>
	/// The drive has fixed media; for example, a hard disk drive or flash drive
	/// </summary>
	Fixed = 3,
	/// <summary>
	/// The drive is a remote (network) drive
	/// </summary>
	Remote = 4,
	/// <summary>
	/// The drive is a CD-ROM drive
	/// </summary>
	CDRoom = 5,
	/// <summary>
	/// The drive is a RAM disk
	/// </summary>
	RamDisk = 6,
}

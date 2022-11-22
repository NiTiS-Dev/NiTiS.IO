namespace NiTiS.IO;

///<summary>
///Specifies how the operating system should open a file
/// </summary> 
public enum FileMode : int
{
	CreateNew = 1,
	Create = 2,
	Open = 3,
	OpenOrCreate = 4,
	Truncate = 5,
	Append = 6
}
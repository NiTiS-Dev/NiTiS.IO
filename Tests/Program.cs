namespace NiTiS.IO.Tests;

internal class Program
{
	static void Main(string[] args)
	{
		//__FileTests();
		__DirectoryTests();
	}
	private static void __DirectoryTests()
	{

	}
	private static void __FileTests()
	{
		IRealFile file = new RealFile("__.txt");

		using Stream stream = file.Open(FileOpenMode.Write, FileOpenLocation.End, false)!;
		using BinaryWriter writer = new(stream);

		writer.Write(short.MaxValue);
		writer.Flush();

		const ulong _a1 = 2;
		Console.WriteLine(_a1 + " Bits = " + MemorySize.Convert(_a1, SizeFormat.Bit, SizeFormat.Byte) + " Bytes");
		const ulong _a2 = 200_20;

		foreach (SizeFormat format in typeof(SizeFormat).GetEnumValues())
		{
			Console.WriteLine("20020 Bytes = " + MemorySize.Convert(_a2, SizeFormat.Byte, format) + " " + format + "s");
		}
	}
}

namespace NiTiS.IO.Tests;

internal class Program
{
	static void Main(string[] args)
	{
		File file = new("NiTiS.IO.dll");

		Console.WriteLine($"{file:Size:KiB}");
		Console.WriteLine($"{file:Size:KiB!0.00 KiB}");
		Console.WriteLine($"{file:Size:MiB!0.0000 MiB}");
		Console.WriteLine($"{file:Size:B!0 Bytes}");

		const ulong _a1 = 2;
        Console.WriteLine(_a1 + " Bits = " + MemorySize.Convert(_a1, SizeFormat.Bit, SizeFormat.Byte) + " Bytes");
        const ulong _a2 = 200_20;

		foreach (SizeFormat format in typeof(SizeFormat).GetEnumValues())
		{
			Console.WriteLine("20020 Bytes = " + MemorySize.Convert(_a2, SizeFormat.Byte, format) + " " + format + "s");
		}
    }
}

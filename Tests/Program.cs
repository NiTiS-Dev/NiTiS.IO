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
	}
}

namespace NiTiS.IO.Tests;

internal class Program
{
	static void Main(string[] args)
	{
		File file = new("NiTiS.IO.dll");

		Console.WriteLine($"{file:size:KiB}");
		Console.WriteLine($"{file:size:KiB!0.00 KiB}");
		Console.WriteLine($"{file:size:MiB!0.0000 MiB}");
		Console.WriteLine($"{file:size:B!0 Bytes}");
	}
}

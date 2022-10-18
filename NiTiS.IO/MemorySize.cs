namespace NiTiS.IO;

public readonly struct MemorySize: IEquatable<MemorySize>, IEquatable<ulong>, IComparable<MemorySize>, IComparable<ulong>, IFormattable
{
	public readonly ulong Bytes;
	#region Consts
	public static MemorySize Zero => new(0);
	public static MemorySize Byte => new(1);

	public static MemorySize Kilobyte => new(1, SizeFormat.Kilobyte);
	public static MemorySize Megabyte => new(1, SizeFormat.Megabyte);
	public static MemorySize Gigabyte => new(1, SizeFormat.Gigabyte);
	public static MemorySize Terabyte => new(1, SizeFormat.Terabyte);

	public static MemorySize Kibibyte => new(1, SizeFormat.Kibibyte);
	public static MemorySize Mebibyte => new(1, SizeFormat.Mebibyte);
	public static MemorySize Gibibyte => new(1, SizeFormat.Gibibyte);
	public static MemorySize Tebibyte => new(1, SizeFormat.Tebibyte);
	#endregion Consts
	public MemorySize(ulong bytes)
	{
		this.Bytes = bytes;
	}
	public MemorySize(long bytes)
	{
		if (bytes < 0) throw new ArgumentOutOfRangeException(nameof(bytes));
		this.Bytes = (ulong)bytes;
	}
	public MemorySize(ulong size, SizeFormat format) : this((ulong)ToBytes(size, format)) { }

	/// <inheritdoc/>
	public override string ToString()
		=> ToBytes(Bytes, SizeFormat.Byte).ToString();
	public string ToString(SizeFormat format)
		=> Convert(Bytes, SizeFormat.Byte, format).ToString();
	/// <summary>
	/// Convert size to byte equivalent
	/// </summary>
	/// <param name="size">some size in <paramref name="format"/> units</param>
	/// <param name="format"><paramref name="size"/> format</param>
	/// <returns>Byte count</returns>
	public static decimal ToBytes(decimal size, SizeFormat format)
		=> format switch
		{
			SizeFormat.Bit => size / 8,
			SizeFormat.Kilobyte => size * 1_000m,
			SizeFormat.Megabyte => size * 1_000_000m,
			SizeFormat.Gigabyte => size * 1_000_000_000m,
			SizeFormat.Terabyte => size * 1_000_000_000_000m,

			SizeFormat.Kibibyte => size * 1024m,
			SizeFormat.Mebibyte => size * 1024m * 1024m,
			SizeFormat.Gibibyte => size * 1024m * 1024m * 1024m,
			SizeFormat.Tebibyte => size * 1024m * 1024m * 1024m * 1024m,
			_ => size
		};
	/// <summary>
	/// Convert size to <paramref name="requiredFormat"/> equivalent
	/// </summary>
	/// <param name="size">some size in <paramref name="format"/> units</param>
	/// <param name="format"><paramref name="size"/> format</param>
	/// <param name="requiredFormat"><see langword="return"/> size format</param>
	public static decimal Convert(ulong size, SizeFormat format, SizeFormat requiredFormat)
	{
		if (format == SizeFormat.Byte)
			return FromBytes(size, requiredFormat);
		return FromBytes(ToBytes(size, format), requiredFormat);
	}
	/// <summary>
	/// Convert bytes to <paramref name="requiredFormat"/>
	/// </summary>
	public static decimal FromBytes(decimal bytes, SizeFormat requiredFormat)
	=> requiredFormat switch
	{
		SizeFormat.Bit => bytes * 8,
		SizeFormat.Kilobyte => bytes / 1_000m,
		SizeFormat.Megabyte => bytes / 1_000_000m,
		SizeFormat.Gigabyte => bytes / 1_000_000_000m,
		SizeFormat.Terabyte => bytes / 1_000_000_000_000m,

		SizeFormat.Kibibyte => bytes / 1024m,
		SizeFormat.Mebibyte => bytes / 1024m * 1024m,
		SizeFormat.Gibibyte => bytes / 1024m * 1024m * 1024m,
		SizeFormat.Tebibyte => bytes / 1024m * 1024m * 1024m * 1024m,
		_ => bytes
	};
	public static MemorySize GetSizeByFormat(ulong size, SizeFormat format)
		=> new MemorySize(size, format);
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// 1 KiB => 2^10 (1024) <br/>
	/// 1 KB  => 10^3 (1000) <br/>
	/// <a href="https://en.wikipedia.org/wiki/Byte#Multiple-byte_units">Wiki page about KB size</a>
	/// </remarks>
	/// <param name="format"></param>
	/// <returns></returns>
	public string ToString(string? format)
	{
		if (String.IsNullOrEmpty(format))
			format = "B";

		string decFormat = String.Empty;

		// Improved formatting
		{
			int vosklSymbl = format.IndexOf('!');

			if (vosklSymbl != -1)
			{
				decFormat = format.Substring(vosklSymbl + 1);
				format = format.Substring(0, vosklSymbl);
			}
		}

		decimal dec = format switch
		{
			"b" => FromBytes(Bytes, SizeFormat.Bit),
			"B" => FromBytes(Bytes, SizeFormat.Byte),

			"kB" or "KB" => FromBytes(Bytes, SizeFormat.Kilobyte),
			"MB" => FromBytes(Bytes, SizeFormat.Megabyte),
			"GB" => FromBytes(Bytes, SizeFormat.Gigabyte),
			"TB" => FromBytes(Bytes, SizeFormat.Terabyte),

			"KiB" => FromBytes(Bytes, SizeFormat.Kibibyte),
			"MiB" => FromBytes(Bytes, SizeFormat.Mebibyte),
			"GiB" => FromBytes(Bytes, SizeFormat.Gibibyte),
			"TiB" => FromBytes(Bytes, SizeFormat.Tebibyte),
			_ => throw new ArgumentException("Unknown format"),
		};
		return dec.ToString(decFormat);
	}
	public string ToString(string? format, IFormatProvider? formatProvider)
		=> ToString(format);
	public override bool Equals(object? obj)
		=> obj is not null && (obj is MemorySize ms ? Equals(ms) : obj is long lg ? Equals(lg) : false);
	public bool Equals(ulong bytes)
		=> this.Bytes == bytes;
	public bool Equals(MemorySize size)
		=> this.Bytes == size.Bytes;
	public int CompareTo(ulong other)
		=> this.Bytes.CompareTo(other);
	public int CompareTo(MemorySize other) 
		=> this.Bytes.CompareTo(other.Bytes);

	public override int GetHashCode() 
		=> Bytes.GetHashCode();

	public static bool operator ==(MemorySize left, MemorySize right)
		=> left.Equals(right);

	public static bool operator !=(MemorySize left, MemorySize right)
		=> !(left == right);

	public static bool operator <(MemorySize left, MemorySize right)
		=> left.CompareTo(right) < 0;

	public static bool operator <=(MemorySize left, MemorySize right)
		=> left.CompareTo(right) <= 0;

	public static bool operator >(MemorySize left, MemorySize right)
		=> left.CompareTo(right) > 0;

	public static bool operator >=(MemorySize left, MemorySize right)
		=> left.CompareTo(right) >= 0;
	public static bool operator ==(MemorySize left, ulong right)
		=> left.Equals(right);

	public static bool operator !=(MemorySize left, ulong right)
		=> !(left == right);

	public static bool operator <(MemorySize left, ulong right)
		=> left.CompareTo(right) < 0;

	public static bool operator <=(MemorySize left, ulong right)
		=> left.CompareTo(right) <= 0;

	public static bool operator >(MemorySize left, ulong right)
		=> left.CompareTo(right) > 0;

	public static bool operator >=(MemorySize left, ulong right)
		=> left.CompareTo(right) >= 0;
	public static MemorySize operator *(MemorySize left, double right)
		=> new((ulong)(left.Bytes * right));
	public static MemorySize operator /(MemorySize left, double right)
		=> new((ulong)(left.Bytes / right));
	public static MemorySize operator ++(MemorySize left)
		=> new(left.Bytes + 1);
	public static MemorySize operator --(MemorySize left)
		=> new(left.Bytes - 1);
	public static MemorySize operator +(MemorySize left, ulong right)
		=> new(left.Bytes + right);
	public static MemorySize operator -(MemorySize left, ulong right)
		=> new(left.Bytes - right);
	public static MemorySize operator +(MemorySize left, MemorySize right)
		=> new(left.Bytes + right.Bytes);
	public static MemorySize operator -(MemorySize left, MemorySize right)
		=> new(left.Bytes - right.Bytes);
	public static implicit operator ulong(MemorySize left)
		=> left.Bytes;
	public static explicit operator decimal(MemorySize left)
		=> (decimal)left.Bytes;
}

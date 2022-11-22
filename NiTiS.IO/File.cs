﻿using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace NiTiS.IO;

/// <summary>
/// Provides methods for action with existing file
/// </summary>
[Serializable]
public class File : IFile, IFormattable
{
	protected internal readonly FileInfo self;
	public File(string path) : base()
	{
		self = new(path);
	}
	public File(FileInfo path) : base()
	{
		self = path;
	}
	public string Name => self.Name;
	public string Path => self.FullName;
	public IDirectory? Parent
		=> self.Directory is null ? null
		: new Directory(self.Directory);
	public bool Exists => self.Exists;
	public bool Readonly => self.Attributes.HasFlag(FileAttributes.ReadOnly);
	public bool Hidden => self.Attributes.HasFlag(FileAttributes.Hidden);
	#region File-only
	public string NameWithoutExtension => SPath.GetFileNameWithoutExtension(self.FullName);
	public string Extension => self.Extension;
	public MemorySize Size => new((ulong)self.Length);
	#endregion
	#region Created/Edited time
	public DateTime CreationTime { get => self.CreationTime; set => self.CreationTime = value; }
	public DateTime LastAccessTime { get => self.LastAccessTime; set => self.LastAccessTime = value; }
	public DateTime CreationTimeUTC { get => self.CreationTimeUtc; set => self.CreationTimeUtc = value; }
	public DateTime LastAccessTimeUTC { get => self.LastAccessTimeUtc; set => self.LastAccessTimeUtc = value; }
	#endregion
	/// <summary>
	/// Changes the name of the file
	/// </summary>
	/// <param name="newName">New name of the file</param>
	public void Rename(string newName)
	{
		string newPath = ItselfPathWithOtherName(newName);
		self.MoveTo(newPath);
	}
	protected string ItselfPathWithOtherName(string newName)
		=> (self.Directory is null ? Directory.Separator.ToString() : self.Directory.FullName + Directory.Separator) + newName;
	/// <summary>
	/// Move the file
	/// </summary>
	/// <param name="dest">New path of the file</param>
	public void Move(string dest)
	{
		self.MoveTo(dest);
	}
	[DebuggerStepThrough]
	public bool Create()
	{
		if (self.Exists)
			return false;
		try
		{
			self.Create()?.Dispose();
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}
	[DebuggerStepThrough]
	public bool Delete()
	{
		if (!self.Exists)
			return false;
		try
		{
			self.Delete();
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}
	[DebuggerStepThrough]
	public FileStream CreateOpen()
		=> self.Create();
	public SFileStream Open(FileMode mode)
		=> self.Open((System.IO.FileMode)mode);
	public SFileStream Read()
		=> self.OpenRead();
	public string ReadAllText()
	{
		using SFileStream stream = Read();
		using StreamReader reader = new(stream);

		return reader.ReadToEnd();
	}
	public async Task<string> ReadAllTextAsync()
	{
		using SFileStream stream = Read();
		using StreamReader reader = new(stream);

		return await reader.ReadToEndAsync();
	}
	public byte[] ReadAllBytes()
	{
		using SFileStream stream = Read();

		if (stream.Length > int.MaxValue)
		{
			using MemoryStream ms = new();

			stream.CopyTo(ms);

			return ms.ToArray();
		}
		else
		{
			using BinaryReader reader = new(stream);

			return reader.ReadBytes((int)stream.Length);
		}
	}
#if !NET48
	public async Task<byte[]> ReadAllBytesAsync()
	{
		using SFileStream stream = Read();

		if (stream.Length > int.MaxValue)
		{
			using MemoryStream ms = new();

			await stream.CopyToAsync(ms);

			return ms.ToArray();
		}
		else
		{
			byte[] arr = new byte[stream.Length];
			await stream.ReadAsync(arr);
			return arr;
		}
	}
#endif
	public SFileStream Write()
		=> self.OpenWrite();
	public void WriteAllText(string text)
	{
		using SFileStream stream = Write();
		using StreamWriter writer = new(stream);

		writer.Write(text);
	}
	public void WriteAllText(string text, Encoding encoding)
	{
		using SFileStream stream = Write();
		using StreamWriter writer = new(stream, encoding);

		writer.Write(text);
	}
	public async Task WriteAllTextAsync(string text, Encoding encoding)
	{
		using SFileStream stream = Write();
		using StreamWriter writer = new(stream, encoding);

		await writer.WriteAsync(text);
	}
	public void WriteAllBytes(byte[]  bytes)
	{
		using SFileStream stream = Write();

		stream.Write(bytes, 0 , bytes.Length);
	}
#if !NET48
	public void WriteAllBytes(ReadOnlySpan<byte> bytes)
	{
		using SFileStream stream = Write();

		stream.Write(bytes);
	}
#endif
	public async Task WriteAllBytesAsync(byte[] bytes)
	{
		using SFileStream stream = Write();

		await stream.WriteAsync(bytes, 0, bytes.Length);
	}
	public SFileStream Append()
		=> self.Open(System.IO.FileMode.Append);
	public SFileStream Truncate()
		=> self.Open(System.IO.FileMode.Truncate);
	public string ToString(string? format, IFormatProvider? formatProvider)
		=> ToString(format);
	public string ToString(string? format)
	{
		if (format == null)
			return ToString();

		if (format.StartsWith("Size"))
		{
			format = format.Substring(4);
			if (format.StartsWith(":"))
			{
				format = format.Substring(1);
				return Size.ToString(format);
			}
			return Size.ToString();
		}
		if (format == nameof(Name))
		{
			return Name;
		}
		if (format == nameof(Path))
		{
			return Path;
		}
		if (format == nameof(Extension))
		{
			return Extension;
		}
		throw new ArgumentException("Invalid format");
	}
	public static readonly char[] InvalidChars = SPath.GetInvalidPathChars();
	public static File GetRandom() => new(SPath.GetRandomFileName());
	public static File GetTemp() => new(SPath.GetTempFileName());
	public override string ToString()
		=> Path;

	public static explicit operator Directory(File dir)
		=> new(dir.Path);
	public static explicit operator String(File dir)
		=> dir.Path;
}
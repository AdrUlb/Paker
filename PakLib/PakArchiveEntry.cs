using System.Diagnostics;

namespace PakLib;

public class PakArchiveEntry
{
	public readonly PakArchive Archive;
	public readonly string Name;
	public readonly int Size;
	public readonly DateTime Timestamp;
	private readonly int _offset;

	internal PakArchiveEntry(PakArchive archive, string name, int size, DateTime timestamp, int offset)
	{
		Archive = archive;
		Name = name;
		Size = size;
		Timestamp = timestamp;
		_offset = offset;
	}

	public void ExtractToFile(string fileName)
	{
		var fileDirPath = Path.GetDirectoryName(fileName) ?? throw new ArgumentException("Invalid file name.", nameof(fileName));
		var buffer = new byte[Size];

		if (!Directory.Exists(fileDirPath))
			Directory.CreateDirectory(fileDirPath);

		using var fs = File.Create(fileName);
		fs.SetLength(Size);
		Archive.Stream.Seek(Archive.DataOffset + _offset, SeekOrigin.Begin);
		Archive.Stream.ReadExactly(buffer);
		fs.Write(buffer);
	}
}

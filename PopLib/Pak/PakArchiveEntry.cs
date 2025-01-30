using PopLib.Misc;

namespace PopLib.Pak;

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

	public Stream GetStream() => Archive.Stream.SubStream(Archive.DataOffset + _offset, Size, true);

	public void ExtractToFile(string fileName)
	{
		var fileDirPath = Path.GetDirectoryName(fileName) ?? throw new ArgumentException("Invalid file name.", nameof(fileName));

		if (!Directory.Exists(fileDirPath))
			Directory.CreateDirectory(fileDirPath);

		using var fs = File.Create(fileName);
		fs.SetLength(Size);
		GetStream().CopyTo(fs);
	}
}

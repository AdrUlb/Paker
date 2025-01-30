using System.Buffers.Binary;
using System.Globalization;
using System.Text;

namespace PakLib;

public class PakArchive : IDisposable, IAsyncDisposable
{
	public readonly PakStream Stream;

	public readonly IReadOnlyList<PakArchiveEntry> Entries;

	internal readonly int DataOffset;

	private PakArchive(string filePath)
	{
		Stream = new(File.OpenRead(filePath));

		Span<byte> buf = stackalloc byte[256];
		var buf4 = buf[..4];
		var buf8 = buf[..8];
		var buf9 = buf[..9];

		Stream.ReadExactly(buf9);
		if (!buf9.SequenceEqual((ReadOnlySpan<byte>) [0xC0, 0x4A, 0xC0, 0xBA, 0, 0, 0, 0, 0]))
			throw new("FIXME");

		var entries = new List<PakArchiveEntry>();

		var maxFileSize = 0;

		var offset = 0;

		do
		{
			var nameLength = Stream.ReadByte();
			Stream.ReadExactly(buf[..nameLength]);
			var name = Encoding.UTF8.GetString(buf[..nameLength]).Replace('\\', '/');

			Stream.ReadExactly(buf4);
			var size = BinaryPrimitives.ReadInt32LittleEndian(buf4);

			Stream.ReadExactly(buf8);
			var timestamp = DateTime.FromFileTime(BinaryPrimitives.ReadInt64LittleEndian(buf8));

			entries.Add(new(this, name, size, timestamp, offset));

			if (size > maxFileSize)
				maxFileSize = size;

			offset += size;
		} while (Stream.ReadByte() == 0);

		DataOffset = (int)Stream.Position;

		Entries = entries;
	}

	public static PakArchive OpenRead(string fileName) => new(fileName);

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		Stream.Dispose();
	}

	public async ValueTask DisposeAsync()
	{
		GC.SuppressFinalize(this);
		await Stream.DisposeAsync();
	}
}

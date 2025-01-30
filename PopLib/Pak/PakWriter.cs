using System.Buffers.Binary;
using System.Text;

namespace PopLib.Pak;

public class PakWriter : IDisposable, IAsyncDisposable
{
	private struct PakWriterFile(string filePath, string pakPath)
	{
		public readonly string FilePath = filePath;
		public readonly string PakPath = pakPath;
	}

	private readonly List<PakWriterFile> _files = [];

	private readonly PakStream _stream;

	private bool _written = false; 

	public PakWriter(string filePath)
	{
		_stream = new(File.OpenWrite(filePath));
		WriteHeader();
	}

	private void WriteHeader() => _stream.Write([0xC0, 0x4A, 0xC0, 0xBA, 0, 0, 0, 0, 0]);

	private void WriteDirectoryEntry(PakWriterFile writerFile, bool terminateDirectory)
	{
		Span<byte> buf = stackalloc byte[256];
		var buf4 = buf[..4];
		var buf8 = buf[..8];

		var name = writerFile.PakPath;
		var nameBuf = buf[..name.Length];
		Encoding.UTF8.GetBytes(name, nameBuf);
		_stream.WriteByte((byte)name.Length);
		_stream.Write(nameBuf);

		var fileInfo = new FileInfo(writerFile.FilePath);

		BinaryPrimitives.WriteInt32LittleEndian(buf4, (int)fileInfo.Length);
		_stream.Write(buf4);

		BinaryPrimitives.WriteInt64LittleEndian(buf8, fileInfo.LastWriteTime.ToFileTime());
		_stream.Write(buf8);

		_stream.WriteByte(terminateDirectory ? (byte)0x80 : (byte)0);
	}

	private void WriteFileData(PakWriterFile writerFile)
	{
		using var fs = File.OpenRead(writerFile.FilePath);
		fs.CopyTo(_stream);
	}

	public void SubmitFile(string filePath, string pakPath)
	{
		pakPath = pakPath.Replace("/", "\\");
		_files.Add(new(filePath, pakPath));
	}

	public void Write()
	{
		if (_written)
			return;
		
		for (var i = 0; i < _files.Count; i++)
		{
			var file = _files[i];
			var isLast = i == _files.Count - 1;
			WriteDirectoryEntry(file, isLast);
		}
		
		foreach (var file in _files)
			WriteFileData(file);

		_written = true;
	}

	public void Dispose()
	{
		Write();
		GC.SuppressFinalize(this);
		_stream.Dispose();
	}

	public async ValueTask DisposeAsync()
	{
		GC.SuppressFinalize(this);
		await _stream.DisposeAsync();
	}
}

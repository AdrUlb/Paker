namespace PopLib.Pak;

public sealed class PakStream(Stream baseStream) : Stream
{
	public override bool CanRead => baseStream.CanRead;

	public override bool CanSeek => baseStream.CanSeek;

	public override bool CanWrite => baseStream.CanWrite;

	public override long Length => baseStream.Length;

	public override long Position
	{
		get => baseStream.Position;
		set => baseStream.Position = value;
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		var bytesRead = baseStream.Read(buffer, offset, count);

		for (var i = 0; i < bytesRead; i++)
			buffer[offset + i] ^= 0xF7;

		return bytesRead;
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		Span<byte> buf = stackalloc byte[512];

		for (var i = 0; i < count; i += buf.Length)
		{
			var toRead = Math.Min(buf.Length, count - i);
			buffer[i..(i + toRead)].CopyTo(buf);
			for (var j = 0; j < toRead; j++)
				buf[j] ^= 0xF7;

			baseStream.Write(buf[..toRead]);
		}
	}

	public override long Seek(long offset, SeekOrigin origin) => baseStream.Seek(offset, origin);

	public override void SetLength(long value) => baseStream.SetLength(value);

	public override void Flush() => baseStream.Flush();

	protected override void Dispose(bool disposing)
	{
		if (disposing)
			baseStream.Dispose();

		base.Dispose(disposing);
	}
}

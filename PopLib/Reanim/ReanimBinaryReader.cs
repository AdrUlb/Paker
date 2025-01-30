using System.Buffers.Binary;
using System.IO.Compression;
using System.Text;

namespace PopLib.Reanim;

public static class ReanimBinaryReader
{
	public static ReanimAnimation ReadFromStream(Stream stream)
	{
		var magic = stream.ReadUint();
		if (magic != 0xDEADFED4)
			throw new("FIXME");

		// TODO
		stream.ReadUint();
		
		using var deflateStream = new ZLibStream(stream, CompressionMode.Decompress);

		if (deflateStream.ReadUint() != 0xB393B4C0)
			throw new("FIXME");

		// TODO
		deflateStream.ReadUint();

		var trackCount = deflateStream.ReadUint();

		var fps = deflateStream.ReadFloat();

		if (deflateStream.ReadUint() != 0)
			throw new("FIXME");

		if (deflateStream.ReadUint() != 12)
			throw new("FIXME");

		var tracks = new ReanimTrack[trackCount];

		for (var i = 0; i < trackCount; i++)
		{
			// TODO
			deflateStream.ReadUint();
			deflateStream.ReadUint();

			var transformCount = deflateStream.ReadUint();

			tracks[i].Transforms = new ReanimTransform[transformCount];
		}

		for (var trackIndex = 0; trackIndex < tracks.Length; trackIndex++)
			ParseTrack(ref tracks[trackIndex], deflateStream);

		return new ReanimAnimation(fps, tracks);
	}

	private static void ParseTrack(ref ReanimTrack track, Stream stream)
	{
		Span<byte> buf = stackalloc byte[512];

		var nameLength = stream.ReadInt();

		stream.ReadExactly(buf[..nameLength]);
		track.Name = Encoding.UTF8.GetString(buf[..nameLength]);

		if (stream.ReadInt() != 0x0000002C)
			throw new("FIXME");

		for (var transformIndex = 0; transformIndex < track.Transforms.Length; transformIndex++)
		{
			track.Transforms[transformIndex].X = stream.ReadFloat();
			track.Transforms[transformIndex].Y = stream.ReadFloat();
			track.Transforms[transformIndex].SkewX = stream.ReadFloat();
			track.Transforms[transformIndex].SkewY = stream.ReadFloat();
			track.Transforms[transformIndex].ScaleX = stream.ReadFloat();
			track.Transforms[transformIndex].ScaleY = stream.ReadFloat();
			track.Transforms[transformIndex].Frame = stream.ReadFloat();
			track.Transforms[transformIndex].Alpha = stream.ReadFloat();

			// TODO
			stream.ReadUint();
			stream.ReadUint();
			stream.ReadUint();
		}

		for (var transformIndex = 0; transformIndex < track.Transforms.Length; transformIndex++)
		{
			var imageNameLength = stream.ReadInt();
			stream.ReadExactly(buf[..imageNameLength]);
			track.Transforms[transformIndex].ImageName = Encoding.UTF8.GetString(buf[..imageNameLength]);

			var fontNameLength = stream.ReadInt();
			stream.ReadExactly(buf[..fontNameLength]);
			var fontName = Encoding.UTF8.GetString(buf[..fontNameLength]);

			var textLength = stream.ReadInt();
			stream.ReadExactly(buf[..textLength]);
			var text = Encoding.UTF8.GetString(buf[..textLength]);
		}
	}

	private static int ReadInt(this Stream stream)
	{
		Span<byte> buf = stackalloc byte[4];
		stream.ReadExactly(buf);
		return BinaryPrimitives.ReadInt32LittleEndian(buf);
	}

	private static uint ReadUint(this Stream stream)
	{
		Span<byte> buf = stackalloc byte[4];
		stream.ReadExactly(buf);
		return BinaryPrimitives.ReadUInt32LittleEndian(buf);
	}

	private static float ReadFloat(this Stream stream)
	{
		Span<byte> buf = stackalloc byte[4];
		stream.ReadExactly(buf);
		return BinaryPrimitives.ReadSingleLittleEndian(buf);
	}
}

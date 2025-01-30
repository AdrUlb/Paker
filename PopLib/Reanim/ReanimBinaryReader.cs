using System.Buffers.Binary;
using System.IO.Compression;
using System.Text;

namespace PopLib.Reanim;

public static class ReanimBinaryReader
{
	public static ReanimAnimation ReadFromStream(Stream stream)
	{
		if (stream.ReadUint() != 0xDEADFED4)
			throw new("FIXME");

		// FIXME
		stream.ReadUint();

		using var zlibStream = new ZLibStream(stream, CompressionMode.Decompress);

		if (zlibStream.ReadUint() != 0xB393B4C0)
			throw new("FIXME");

		// FIXME
		zlibStream.ReadUint();

		var trackCount = zlibStream.ReadInt();
		var fps = zlibStream.ReadFloat();

		if (zlibStream.ReadUint() != 0)
			throw new("FIXME");

		if (zlibStream.ReadUint() != 12)
			throw new("FIXME");

		var tracks = new ReanimTrack[trackCount];

		for (var i = 0; i < trackCount; i++)
		{
			// FIXME
			zlibStream.ReadUint();
			zlibStream.ReadUint();

			var transformCount = zlibStream.ReadInt();

			tracks[i].Transforms = new ReanimTransform[transformCount];
		}

		for (var trackIndex = 0; trackIndex < tracks.Length; trackIndex++)
			ReadTrack(ref tracks[trackIndex], zlibStream);

		return new ReanimAnimation(fps, tracks);
	}

	private static void ReadTrack(ref ReanimTrack track, Stream stream)
	{
		track.Name = stream.ReadString();

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

			// FIXME
			stream.ReadUint();
			stream.ReadUint();
			stream.ReadUint();
		}

		for (var transformIndex = 0; transformIndex < track.Transforms.Length; transformIndex++)
		{
			track.Transforms[transformIndex].ImageName = stream.ReadString();
			var fontName = stream.ReadString();
			var text = stream.ReadString();
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

	private static string ReadString(this Stream stream)
	{
		var length = stream.ReadInt();
		Span<byte> buf = stackalloc byte[length];
		stream.ReadExactly(buf);
		return Encoding.UTF8.GetString(buf);
	}
}

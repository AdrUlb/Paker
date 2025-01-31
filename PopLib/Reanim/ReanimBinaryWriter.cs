using System.Buffers.Binary;
using System.IO.Compression;
using System.Text;

namespace PopLib.Reanim;

public static class ReanimBinaryWriter
{

	public static void WriteToStream(in ReanimAnimation animation, Stream stream)
	{
		using var ms = new MemoryStream();

		//
		ms.WriteUint(0xB393B4C0);

		// FIXME
		ms.WriteInt(0);

		ms.WriteInt(animation.Tracks.Length);
		ms.WriteFloat(animation.Fps);

		ms.WriteInt(0);
		ms.WriteInt(12);

		for (var i = 0; i < animation.Tracks.Length; i++)
		{
			ref var track = ref animation.Tracks[i];

			// FIXME
			ms.WriteInt(0);
			ms.WriteInt(0);

			ms.WriteInt(track.Transforms.Length);
		}

		for (var i = 0; i < animation.Tracks.Length; i++)
			WriteTrack(animation.Tracks[i], ms);

		stream.WriteUint(0xDEADFED4);
		stream.WriteUint((uint)ms.Length);
		using var zlibStream = new ZLibStream(stream, CompressionMode.Compress);
		ms.WriteTo(zlibStream);
	}

	private static void WriteTrack(in ReanimTrack track, Stream stream)
	{
		stream.WriteString(track.Name);

		stream.WriteInt(0x0000002C);

		for (var transformIndex = 0; transformIndex < track.Transforms.Length; transformIndex++)
		{
			stream.WriteFloat(track.Transforms[transformIndex].X);
			stream.WriteFloat(track.Transforms[transformIndex].Y);
			stream.WriteFloat(track.Transforms[transformIndex].SkewX);
			stream.WriteFloat(track.Transforms[transformIndex].SkewY);
			stream.WriteFloat(track.Transforms[transformIndex].ScaleX);
			stream.WriteFloat(track.Transforms[transformIndex].ScaleY);
			stream.WriteFloat(track.Transforms[transformIndex].Frame);
			stream.WriteFloat(track.Transforms[transformIndex].Alpha);

			// FIXME
			stream.WriteInt(0);
			stream.WriteInt(0);
			stream.WriteInt(0);
		}

		for (var transformIndex = 0; transformIndex < track.Transforms.Length; transformIndex++)
		{
			stream.WriteString(track.Transforms[transformIndex].ImageName ?? "");
			stream.WriteString(track.Transforms[transformIndex].FontName ?? "");
			stream.WriteString(track.Transforms[transformIndex].Text ?? "");
		}
	}

	private static void WriteUint(this Stream stream, uint value)
	{
		Span<byte> buf = stackalloc byte[4];
		BinaryPrimitives.WriteUInt32LittleEndian(buf, value);
		stream.Write(buf);
	}

	private static void WriteInt(this Stream stream, int value)
	{
		Span<byte> buf = stackalloc byte[4];
		BinaryPrimitives.WriteInt32LittleEndian(buf, value);
		stream.Write(buf);
	}

	private static void WriteFloat(this Stream stream, float value)
	{
		Span<byte> buf = stackalloc byte[4];
		BinaryPrimitives.WriteSingleLittleEndian(buf, value);
		stream.Write(buf);
	}

	private static void WriteString(this Stream stream, string str)
	{
		stream.WriteInt(str.Length);
		Span<byte> buf = stackalloc byte[str.Length];
		Encoding.UTF8.GetBytes(str, buf);
		stream.Write(buf);
	}
}

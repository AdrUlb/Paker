using PopLib.Misc;
using System.IO.Compression;

namespace PopLib.Reanim;

public static class ReanimBinaryReader
{
	public static ReanimAnimation ReadFromStream(Stream stream)
	{
		using var ms = new MemoryStream();
		AssetCompression.Decompress(stream, ms);
		ms.Position = 0;

		if (ms.ReadUint() != 0xB393B4C0)
			throw new("FIXME");

		// FIXME
		ms.ReadUint();

		var trackCount = ms.ReadInt();
		var fps = ms.ReadFloat();

		if (ms.ReadUint() != 0)
			throw new("FIXME");

		if (ms.ReadUint() != 12)
			throw new("FIXME");

		var tracks = new ReanimTrack[trackCount];
		Span<int> transformCounts = stackalloc int[trackCount];

		for (var i = 0; i < trackCount; i++)
		{
			// FIXME
			ms.ReadUint();
			ms.ReadUint();

			transformCounts[i] = ms.ReadInt();
		}

		for (var i = 0; i < tracks.Length; i++)
			tracks[i] = ReadTrack(transformCounts[i], ms);

		return new(fps, tracks);
	}

	private static ReanimTrack ReadTrack(int transformCount, Stream stream)
	{
		var name = stream.ReadString();
		var transforms = new ReanimTransform[transformCount];

		if (stream.ReadInt() != 0x0000002C)
			throw new("FIXME");

		for (var i = 0; i < transforms.Length; i++)
		{
			transforms[i].X = stream.ReadFloat();
			transforms[i].Y = stream.ReadFloat();
			transforms[i].SkewX = stream.ReadFloat();
			transforms[i].SkewY = stream.ReadFloat();
			transforms[i].ScaleX = stream.ReadFloat();
			transforms[i].ScaleY = stream.ReadFloat();
			transforms[i].Frame = stream.ReadFloat();
			transforms[i].Alpha = stream.ReadFloat();

			// FIXME
			stream.ReadUint();
			stream.ReadUint();
			stream.ReadUint();
		}

		for (var i = 0; i < transforms.Length; i++)
		{
			transforms[i].ImageName = stream.ReadString();
			transforms[i].FontName = stream.ReadString();
			transforms[i].Text = stream.ReadString();
		}

		return new(name, transforms);
	}
}

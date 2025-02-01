using PopLib.Reanim.Definition;
using PopLib.Util;
using PopLib.Util.Extensions;

namespace PopLib.Reanim.Serialization;

public static class ReanimBinarySerializer
{
	public static void Serialize(ReanimAnimation animation, Stream stream)
	{
		using var ms = new MemoryStream();

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

		foreach (var track in animation.Tracks)
			WriteTrack(track, ms);

		ms.Position = 0;
		AssetCompression.Compress(ms, stream);
	}

	private static void WriteTrack(ReanimTrack track, Stream stream)
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

	public static ReanimAnimation Deserialize(Stream stream)
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

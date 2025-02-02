using PopLib.Reanim.Definition;
using PopLib.Util;
using PopLib.Util.Extensions;

namespace PopLib.Reanim.Serialization;

public static class ReanimBinarySerializer
{
	public static void Serialize(ReanimDefinition definition, Stream stream)
	{
		using var ms = new MemoryStream();

		ms.WriteUint(0xB393B4C0);

		// FIXME
		ms.WriteInt(0);

		ms.WriteInt(definition.Tracks.Length);
		ms.WriteFloat(definition.Fps);

		ms.WriteInt(0);
		ms.WriteInt(12);

		for (var i = 0; i < definition.Tracks.Length; i++)
		{
			ref var track = ref definition.Tracks[i];

			// FIXME
			ms.WriteInt(0);
			ms.WriteInt(0);

			ms.WriteInt(track.Transforms.Length);
		}

		foreach (var track in definition.Tracks)
			WriteTrack(track, ms);

		ms.Position = 0;
		AssetCompression.Compress(ms, stream);
	}

	private static void WriteTrack(ReanimTrackDefinition trackDefinition, Stream stream)
	{
		stream.WriteString(trackDefinition.Name);

		stream.WriteInt(0x2C);

		for (var transformIndex = 0; transformIndex < trackDefinition.Transforms.Length; transformIndex++)
		{
			stream.WriteFloat(trackDefinition.Transforms[transformIndex].X);
			stream.WriteFloat(trackDefinition.Transforms[transformIndex].Y);
			stream.WriteFloat(trackDefinition.Transforms[transformIndex].SkewX);
			stream.WriteFloat(trackDefinition.Transforms[transformIndex].SkewY);
			stream.WriteFloat(trackDefinition.Transforms[transformIndex].ScaleX);
			stream.WriteFloat(trackDefinition.Transforms[transformIndex].ScaleY);
			stream.WriteFloat(trackDefinition.Transforms[transformIndex].Frame);
			stream.WriteFloat(trackDefinition.Transforms[transformIndex].Alpha);

			// FIXME
			stream.WriteInt(0);
			stream.WriteInt(0);
			stream.WriteInt(0);
		}

		for (var transformIndex = 0; transformIndex < trackDefinition.Transforms.Length; transformIndex++)
		{
			stream.WriteString(trackDefinition.Transforms[transformIndex].ImageName ?? "");
			stream.WriteString(trackDefinition.Transforms[transformIndex].FontName ?? "");
			stream.WriteString(trackDefinition.Transforms[transformIndex].Text ?? "");
		}
	}

	public static ReanimDefinition Deserialize(Stream stream)
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

		var tracks = new ReanimTrackDefinition[trackCount];
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

	private static ReanimTrackDefinition ReadTrack(int transformCount, Stream stream)
	{
		var name = stream.ReadString();
		var transforms = new ReanimTransform[transformCount];

		if (stream.ReadInt() != 0x0000002C)
			throw new("FIXME");

		for (var i = 0; i < transforms.Length; i++)
		{
			var x = stream.ReadFloat();
			var y = stream.ReadFloat();
			var skewX = stream.ReadFloat();
			var skewY = stream.ReadFloat();
			var scaleX = stream.ReadFloat();
			var scaleY = stream.ReadFloat();
			var frame = stream.ReadFloat();
			var alpha = stream.ReadFloat();
			transforms[i] = new ReanimTransform(x, y, skewX, skewY, scaleX, scaleY, frame, alpha);
			
			// FIXME
			stream.ReadUint();
			stream.ReadUint();
			stream.ReadUint();
		}

		for (var i = 0; i < transforms.Length; i++)
		{
			ref var transform = ref transforms[i];
			transform.ImageName = stream.ReadString();
			transform.FontName = stream.ReadString();
			transform.Text = stream.ReadString();
		}

		var track = new ReanimTrackDefinition(name, transforms);
		track.ReplaceTransformsPlaceholders();
		return track;
	}
}

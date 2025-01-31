using PopLib.Misc;
using System.IO.Compression;

namespace PopLib.Reanim;

public static class ReanimBinaryWriter
{
	public static void WriteToStream(ReanimAnimation animation, Stream stream)
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
}

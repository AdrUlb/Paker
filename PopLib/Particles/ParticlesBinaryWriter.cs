using PopLib.Misc;

namespace PopLib.Particles;

public static class ParticlesBinaryWriter
{
	public static void WriteToStream(ParticlesDefinition animation, Stream stream)
	{
		using var ms = new MemoryStream();

		ms.WriteUint(0x411F994D);

		// FIXME
		ms.WriteInt(0);

		ms.WriteInt(animation.Emitters.Length);

		ms.WriteInt(0x164);

		foreach (var emitter in animation.Emitters)
		{
			// FIXME
			ms.WriteInt(0);

			ms.WriteInt(emitter.ImageCol);
			ms.WriteInt(emitter.ImageRow);
			ms.WriteInt(emitter.ImageFrames);
			ms.WriteInt(emitter.Animated);

			var particleFlags = 0u;
			if (emitter.RandomLaunchSpin) particleFlags |= 1u;
			if (emitter.AlignLaunchSpin) (particleFlags) |= 1u << 1;
			if (emitter.SystemLoops) (particleFlags) |= 1u << 3;
			if (emitter.ParticleLoops) (particleFlags) |= 1u << 4;
			if (emitter.ParticlesDontFollow) (particleFlags) |= 1u << 5;
			if (emitter.RandomStartTime) (particleFlags) |= 1u << 6;
			if (emitter.DieIfOverloaded) (particleFlags) |= 1u << 7;
			if (emitter.Additive) (particleFlags) |= 1u << 8;
			if (emitter.FullScreen) (particleFlags) |= 1u << 9;
			if (emitter.HardwareOnly) (particleFlags) |= 1u << 11;
			ms.WriteUint(particleFlags);

			ms.WriteInt((int)emitter.EmitterType);

			// FIXME
			for (var j = 0; j < 47; j++)
				ms.WriteInt(0);

			ms.WriteInt(emitter.Fields?.Length ?? 0);

			// FIXME
			ms.WriteInt(0);

			ms.WriteInt(emitter.SystemFields?.Length ?? 0);

			// FIXME
			for (var j = 0; j < 32; j++)
				ms.WriteInt(0);
		}

		foreach (var emitter in animation.Emitters)
		{
			ms.WriteString(emitter.Image ?? "");
			ms.WriteString(emitter.Name ?? "");

			ms.WriteFloatParameterTrack(emitter.SystemDuration);

			ms.WriteInt(0);

			ms.WriteFloatParameterTrack(emitter.CrossFadeDuration);
			ms.WriteFloatParameterTrack(emitter.SpawnRate);
			ms.WriteFloatParameterTrack(emitter.SpawnMinActive);
			ms.WriteFloatParameterTrack(emitter.SpawnMaxActive);
			ms.WriteFloatParameterTrack(emitter.SpawnMaxLaunched);
			ms.WriteFloatParameterTrack(emitter.EmitterRadius);
			ms.WriteFloatParameterTrack(emitter.EmitterOffsetX);
			ms.WriteFloatParameterTrack(emitter.EmitterOffsetY);
			ms.WriteFloatParameterTrack(emitter.EmitterBoxX);
			ms.WriteFloatParameterTrack(emitter.EmitterBoxY);

			ms.WriteInt(0);

			ms.WriteFloatParameterTrack(emitter.EmitterSkewX);
			ms.WriteFloatParameterTrack(emitter.EmitterSkewY);
			ms.WriteFloatParameterTrack(emitter.ParticleDuration);

			ms.WriteInt(0);
			ms.WriteInt(0);
			ms.WriteInt(0);

			ms.WriteFloatParameterTrack(emitter.SystemAlpha);

			ms.WriteInt(0);

			ms.WriteFloatParameterTrack(emitter.LaunchSpeed);
			ms.WriteFloatParameterTrack(emitter.LaunchAngle);

			ms.WriteFields(emitter.Fields);
			ms.WriteFields(emitter.SystemFields);

			ms.WriteFloatParameterTrack(emitter.ParticleRed);
			ms.WriteFloatParameterTrack(emitter.ParticleGreen);
			ms.WriteFloatParameterTrack(emitter.ParticleBlue);
			ms.WriteFloatParameterTrack(emitter.ParticleAlpha);
			ms.WriteFloatParameterTrack(emitter.ParticleBrightness);
			ms.WriteFloatParameterTrack(emitter.ParticleSpinAngle);
			ms.WriteFloatParameterTrack(emitter.ParticleSpinSpeed);
			ms.WriteFloatParameterTrack(emitter.ParticleScale);
			ms.WriteFloatParameterTrack(emitter.ParticleStretch);
			ms.WriteFloatParameterTrack(emitter.CollisionReflect);
			ms.WriteFloatParameterTrack(emitter.CollisionSpin);
			ms.WriteFloatParameterTrack(emitter.ClipTop);
			ms.WriteFloatParameterTrack(emitter.ClipBottom);
			ms.WriteFloatParameterTrack(emitter.ClipLeft);
			ms.WriteFloatParameterTrack(emitter.ClipRight);
			ms.WriteFloatParameterTrack(emitter.AnimationRate);
		}

		ms.Position = 0;
		AssetCompression.Compress(ms, stream);
	}

	private static void WriteFields(this Stream stream, ParticlesField[]? fields)
	{
		stream.WriteInt(20);

		if (fields == null)
			return;

		foreach (var field in fields)
		{
			stream.WriteInt((int)field.FieldType);

			// FIXME
			stream.WriteInt(0);
			stream.WriteInt(0);
			stream.WriteInt(0);
			stream.WriteInt(0);
		}

		foreach (var field in fields)
		{
			stream.WriteFloatParameterTrack(field.X);
			stream.WriteFloatParameterTrack(field.Y);
		}
	}

	private static void WriteFloatParameterTrack(this Stream stream, ParticlesFloatParameterTrack? track)
	{
		if (track == null)
		{
			stream.WriteInt(0);
			return;
		}

		stream.WriteInt(track.Nodes.Length);

		foreach (var node in track.Nodes)
		{
			stream.WriteFloat(node.Time);
			stream.WriteFloat(node.LowValue);
			stream.WriteFloat(node.HighValue);
			stream.WriteInt((int)node.CurveType);
			stream.WriteInt((int)node.Distribution);
		}
	}
}

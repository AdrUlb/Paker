using PopLib.Misc;

namespace PopLib.Particles;

public static class ParticlesBinaryReader
{
	public static ParticlesEffect ReadFromStream(Stream stream)
	{
		using var ms = new MemoryStream();
		AssetCompression.Decompress(stream, ms);
		ms.Position = 0;

		if (ms.ReadUint() != 0x411F994D)
			throw new("FIXME");

		// FIXME
		ms.ReadInt();

		var emitterCount = ms.ReadInt();

		if (ms.ReadUint() != 0x164)
			throw new("FIXME");

		var emitters = new ParticlesEmitter[emitterCount];

		for (var i = 0; i < emitterCount; i++)
		{
			ref var emitter = ref emitters[i];
			emitter = new();

			// FIXME
			ms.Position += 4;

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			emitter.ImageFrames = ms.ReadInt();

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			var particleFlags = ms.ReadUint();

			emitter.EmitterType = (ParticlesEmitterType)ms.ReadInt();

			// FIXME
			ms.Position += 47 * 4;

			var emitterFieldCount = ms.ReadInt();
			if (emitterFieldCount > 0)
				emitter.Fields = new ParticlesField[emitterFieldCount];

			ms.Position += 4;

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			ms.Position += 32 * 4;

			if ((particleFlags & ~0b100101111001) != 0)
				throw new($"FIXME: {Convert.ToString(particleFlags, 2)}");

			emitter.RandomLaunchSpin = (particleFlags & 1) != 0;
			emitter.SystemLoops = ((particleFlags >> 3) & 1) != 0;
			emitter.ParticleLoops = ((particleFlags >> 4) & 1) != 0;
			emitter.ParticlesDontFollow = ((particleFlags >> 5) & 1) != 0;
			emitter.RandomStartTime = ((particleFlags >> 6) & 1) != 0;
			emitter.Additive = ((particleFlags >> 8) & 1) != 0;
			emitter.HardwareOnly = ((particleFlags >> 11) & 1) != 0;
		}

		for (var i = 0; i < emitterCount; i++)
		{
			ref var emitter = ref emitters[i];
			emitter.Image = ms.ReadString();
			emitter.Name = ms.ReadString();

			emitter.SystemDuration = ms.ReadFloatParameterTrack();

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			emitter.CrossfadeDuration = ms.ReadFloatParameterTrack();
			emitter.SpawnRate = ms.ReadFloatParameterTrack();
			emitter.SpawnMinActive = ms.ReadFloatParameterTrack();

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			emitter.SpawnMaxLaunched = ms.ReadFloatParameterTrack();
			emitter.EmitterRadius = ms.ReadFloatParameterTrack();
			emitter.EmitterOffsetX = ms.ReadFloatParameterTrack();
			emitter.EmitterOffsetY = ms.ReadFloatParameterTrack();
			emitter.EmitterBoxX = ms.ReadFloatParameterTrack();
			emitter.EmitterBoxY = ms.ReadFloatParameterTrack();

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			emitter.ParticleDuration = ms.ReadFloatParameterTrack();

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			emitter.LaunchSpeed = ms.ReadFloatParameterTrack();
			emitter.LaunchAngle = ms.ReadFloatParameterTrack();

			ms.ReadFields(emitter.Fields);

			if (ms.ReadUint() != 20)
				throw new("FIXME");

			emitter.ParticleRed = ms.ReadFloatParameterTrack();
			emitter.ParticleGreen = ms.ReadFloatParameterTrack();
			emitter.ParticleBlue = ms.ReadFloatParameterTrack();

			emitter.ParticleAlpha = ms.ReadFloatParameterTrack();
			emitter.ParticleBrightness = ms.ReadFloatParameterTrack();
			emitter.ParticleSpinAngle = ms.ReadFloatParameterTrack();
			emitter.ParticleSpinSpeed = ms.ReadFloatParameterTrack();
			emitter.ParticleScale = ms.ReadFloatParameterTrack();

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			if (ms.ReadUint() != 0)
				throw new("FIXME");
		}

		return new(emitters);
	}

	private static void ReadFields(this Stream stream, ParticlesField[]? fields)
	{
		if (stream.ReadUint() != 20)
			throw new("FIXME");

		if (fields == null)
			return;

		for (var i = 0; i < fields.Length; i++)
		{
			ref var field = ref fields[i];
			field = new()
			{
				FieldType = (ParticlesFieldType)stream.ReadInt()
			};

			// FIXME
			stream.ReadInt();
			stream.ReadInt();
			stream.ReadInt();
			stream.ReadInt();
		}

		for (var i = 0; i < fields.Length; i++)
		{
			ref var field = ref fields[i];
			field.X = stream.ReadFloatParameterTrack();
			field.Y = stream.ReadFloatParameterTrack();
		}
	}

	private static ParticlesFloatParameterTrack ReadFloatParameterTrack(this Stream stream)
	{
		var count = stream.ReadInt();
		var nodes = new ParticlesFloatParameterTrackNode[count];

		for (var i = 0; i < count; i++)
		{
			ref var node = ref nodes[i];
			var time = stream.ReadFloat();
			var lowValue = stream.ReadFloat();
			var highValue = stream.ReadFloat();
			var curveType = (ParticlesCurveType)stream.ReadInt();
			var distribution = (ParticlesCurveType)stream.ReadInt();
			node = new(time, lowValue, highValue, curveType, distribution);
		}
		return new(nodes);
	}
}

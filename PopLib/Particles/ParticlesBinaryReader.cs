using PopLib.Misc;

namespace PopLib.Particles;

public static class ParticlesBinaryReader
{
	public static ParticlesDefinition ReadFromStream(Stream stream)
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
			var emitter = new ParticlesEmitter();
			emitters[i] = emitter;

			// FIXME
			ms.ReadInt();

			emitter.ImageCol = ms.ReadInt();
			emitter.ImageRow = ms.ReadInt();
			emitter.ImageFrames = ms.ReadInt();
			emitter.Animated = ms.ReadInt();

			var particleFlags = ms.ReadUint();
			emitter.RandomLaunchSpin = (particleFlags & 1) != 0;
			emitter.AlignLaunchSpin = ((particleFlags >> 1) & 1) != 0;
			emitter.SystemLoops = ((particleFlags >> 3) & 1) != 0;
			emitter.ParticleLoops = ((particleFlags >> 4) & 1) != 0;
			emitter.ParticlesDontFollow = ((particleFlags >> 5) & 1) != 0;
			emitter.RandomStartTime = ((particleFlags >> 6) & 1) != 0;
			emitter.DieIfOverloaded = ((particleFlags >> 7) & 1) != 0;
			emitter.Additive = ((particleFlags >> 8) & 1) != 0;
			emitter.FullScreen = ((particleFlags >> 9) & 1) != 0;
			emitter.HardwareOnly = ((particleFlags >> 11) & 1) != 0;

			emitter.EmitterType = (ParticlesEmitterType)ms.ReadInt();

			// FIXME
			ms.Position += 47 * 4;

			var fieldCount = ms.ReadInt();
			if (fieldCount > 0)
				emitter.Fields = new ParticlesField[fieldCount];

			// FIXME
			ms.ReadInt();

			var systemFieldCount = ms.ReadInt();
			if (systemFieldCount > 0)
				emitter.SystemFields = new ParticlesField[systemFieldCount];

			ms.Position += 32 * 4;

			if ((particleFlags & ~0b101111111011) != 0)
				throw new($"FIXME: {Convert.ToString(particleFlags, 2)}");
		}

		for (var i = 0; i < emitterCount; i++)
		{
			var emitter = emitters[i];
			emitter.Image = ms.ReadString();
			emitter.Name = ms.ReadString();

			emitter.SystemDuration = ms.ReadFloatParameterTrack();

			if (ms.ReadInt() != 0)
				throw new("FIXME");

			emitter.CrossFadeDuration = ms.ReadFloatParameterTrack();
			emitter.SpawnRate = ms.ReadFloatParameterTrack();
			emitter.SpawnMinActive = ms.ReadFloatParameterTrack();
			emitter.SpawnMaxActive = ms.ReadFloatParameterTrack();
			emitter.SpawnMaxLaunched = ms.ReadFloatParameterTrack();
			emitter.EmitterRadius = ms.ReadFloatParameterTrack();
			emitter.EmitterOffsetX = ms.ReadFloatParameterTrack();
			emitter.EmitterOffsetY = ms.ReadFloatParameterTrack();
			emitter.EmitterBoxX = ms.ReadFloatParameterTrack();
			emitter.EmitterBoxY = ms.ReadFloatParameterTrack();

			if (ms.ReadInt() != 0)
				throw new("FIXME");

			emitter.EmitterSkewX = ms.ReadFloatParameterTrack();
			emitter.EmitterSkewY = ms.ReadFloatParameterTrack();
			emitter.ParticleDuration = ms.ReadFloatParameterTrack();

			if (ms.ReadInt() != 0)
				throw new("FIXME");

			if (ms.ReadInt() != 0)
				throw new("FIXME");

			if (ms.ReadInt() != 0)
				throw new("FIXME");

			emitter.SystemAlpha = ms.ReadFloatParameterTrack();

			if (ms.ReadInt() != 0)
				throw new("FIXME");

			emitter.LaunchSpeed = ms.ReadFloatParameterTrack();
			emitter.LaunchAngle = ms.ReadFloatParameterTrack();

			ms.ReadFields(emitter.Fields);
			ms.ReadFields(emitter.SystemFields);

			emitter.ParticleRed = ms.ReadFloatParameterTrack();
			emitter.ParticleGreen = ms.ReadFloatParameterTrack();
			emitter.ParticleBlue = ms.ReadFloatParameterTrack();
			emitter.ParticleAlpha = ms.ReadFloatParameterTrack();
			emitter.ParticleBrightness = ms.ReadFloatParameterTrack();
			emitter.ParticleSpinAngle = ms.ReadFloatParameterTrack();
			emitter.ParticleSpinSpeed = ms.ReadFloatParameterTrack();
			emitter.ParticleScale = ms.ReadFloatParameterTrack();
			emitter.ParticleStretch = ms.ReadFloatParameterTrack();
			emitter.CollisionReflect = ms.ReadFloatParameterTrack();
			emitter.CollisionSpin = ms.ReadFloatParameterTrack();
			;
			emitter.ClipTop = ms.ReadFloatParameterTrack();
			emitter.ClipBottom = ms.ReadFloatParameterTrack();
			emitter.ClipLeft = ms.ReadFloatParameterTrack();
			emitter.ClipRight = ms.ReadFloatParameterTrack();
			emitter.AnimationRate = ms.ReadFloatParameterTrack();
		}

		return new(emitters);
	}

	private static void ReadFields(this Stream stream, ParticlesField[]? fields)
	{
		if (stream.ReadInt() != 20)
			throw new("FIXME");

		if (fields == null)
			return;

		for (var i = 0; i < fields.Length; i++)
		{
			var field = new ParticlesField
			{
				FieldType = (ParticlesFieldType)stream.ReadInt()
			};
			fields[i] = field;

			// FIXME
			stream.ReadInt();
			stream.ReadInt();
			stream.ReadInt();
			stream.ReadInt();
		}

		foreach (var field in fields)
		{
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
			var time = stream.ReadFloat();
			var lowValue = stream.ReadFloat();
			var highValue = stream.ReadFloat();
			var curveType = (ParticlesCurveType)stream.ReadInt();
			var distribution = (ParticlesCurveType)stream.ReadInt();
			nodes[i] = new(time, lowValue, highValue, curveType, distribution);
		}
		return new(nodes);
	}
}

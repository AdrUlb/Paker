using PopLib.Particles.Definition;
using PopLib.Util;
using PopLib.Util.Extensions;

namespace PopLib.Particles.Serialization;

public static class ParticlesBinarySerializer
{
	public static void Serialize(PopParticlesDefinition animation, Stream outputStream)
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

			ms.WriteTrack(emitter.SystemDuration);

			ms.WriteInt(0);

			ms.WriteTrack(emitter.CrossFadeDuration);
			ms.WriteTrack(emitter.SpawnRate);
			ms.WriteTrack(emitter.SpawnMinActive);
			ms.WriteTrack(emitter.SpawnMaxActive);
			ms.WriteTrack(emitter.SpawnMaxLaunched);
			ms.WriteTrack(emitter.EmitterRadius);
			ms.WriteTrack(emitter.EmitterOffsetX);
			ms.WriteTrack(emitter.EmitterOffsetY);
			ms.WriteTrack(emitter.EmitterBoxX);
			ms.WriteTrack(emitter.EmitterBoxY);

			ms.WriteInt(0);

			ms.WriteTrack(emitter.EmitterSkewX);
			ms.WriteTrack(emitter.EmitterSkewY);
			ms.WriteTrack(emitter.ParticleDuration);

			ms.WriteInt(0);
			ms.WriteInt(0);
			ms.WriteInt(0);

			ms.WriteTrack(emitter.SystemAlpha);

			ms.WriteInt(0);

			ms.WriteTrack(emitter.LaunchSpeed);
			ms.WriteTrack(emitter.LaunchAngle);

			ms.WriteFields(emitter.Fields);
			ms.WriteFields(emitter.SystemFields);

			ms.WriteTrack(emitter.ParticleRed);
			ms.WriteTrack(emitter.ParticleGreen);
			ms.WriteTrack(emitter.ParticleBlue);
			ms.WriteTrack(emitter.ParticleAlpha);
			ms.WriteTrack(emitter.ParticleBrightness);
			ms.WriteTrack(emitter.ParticleSpinAngle);
			ms.WriteTrack(emitter.ParticleSpinSpeed);
			ms.WriteTrack(emitter.ParticleScale);
			ms.WriteTrack(emitter.ParticleStretch);
			ms.WriteTrack(emitter.CollisionReflect);
			ms.WriteTrack(emitter.CollisionSpin);
			ms.WriteTrack(emitter.ClipTop);
			ms.WriteTrack(emitter.ClipBottom);
			ms.WriteTrack(emitter.ClipLeft);
			ms.WriteTrack(emitter.ClipRight);
			ms.WriteTrack(emitter.AnimationRate);
		}

		ms.Position = 0;
		AssetCompression.Compress(ms, outputStream);
	}

	private static void WriteFields(this Stream stream, PopParticlesField[]? fields)
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
			stream.WriteTrack(field.X);
			stream.WriteTrack(field.Y);
		}
	}

	private static void WriteTrack(this Stream stream, PopParticlesTrack? track)
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

	public static PopParticlesDefinition Deserialize(Stream stream)
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

		var emitters = new PopParticlesEmitter[emitterCount];

		for (var i = 0; i < emitterCount; i++)
		{
			var emitter = new PopParticlesEmitter();
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

			emitter.EmitterType = (PopParticlesEmitterType)ms.ReadInt();

			// FIXME
			ms.Position += 47 * 4;

			var fieldCount = ms.ReadInt();
			if (fieldCount > 0)
				emitter.Fields = new PopParticlesField[fieldCount];

			// FIXME
			ms.ReadInt();

			var systemFieldCount = ms.ReadInt();
			if (systemFieldCount > 0)
				emitter.SystemFields = new PopParticlesField[systemFieldCount];

			ms.Position += 32 * 4;

			if ((particleFlags & ~0b101111111011) != 0)
				throw new($"FIXME: {Convert.ToString(particleFlags, 2)}");
		}

		for (var i = 0; i < emitterCount; i++)
		{
			var emitter = emitters[i];
			emitter.Image = ms.ReadString();
			emitter.Name = ms.ReadString();

			emitter.SystemDuration = ms.ReadTrack();

			if (ms.ReadInt() != 0)
				throw new("FIXME");

			emitter.CrossFadeDuration = ms.ReadTrack();
			emitter.SpawnRate = ms.ReadTrack();
			emitter.SpawnMinActive = ms.ReadTrack();
			emitter.SpawnMaxActive = ms.ReadTrack();
			emitter.SpawnMaxLaunched = ms.ReadTrack();
			emitter.EmitterRadius = ms.ReadTrack();
			emitter.EmitterOffsetX = ms.ReadTrack();
			emitter.EmitterOffsetY = ms.ReadTrack();
			emitter.EmitterBoxX = ms.ReadTrack();
			emitter.EmitterBoxY = ms.ReadTrack();

			if (ms.ReadInt() != 0)
				throw new("FIXME");

			emitter.EmitterSkewX = ms.ReadTrack();
			emitter.EmitterSkewY = ms.ReadTrack();
			emitter.ParticleDuration = ms.ReadTrack();

			if (ms.ReadInt() != 0)
				throw new("FIXME");

			if (ms.ReadInt() != 0)
				throw new("FIXME");

			if (ms.ReadInt() != 0)
				throw new("FIXME");

			emitter.SystemAlpha = ms.ReadTrack();

			if (ms.ReadInt() != 0)
				throw new("FIXME");

			emitter.LaunchSpeed = ms.ReadTrack();
			emitter.LaunchAngle = ms.ReadTrack();

			ms.ReadFields(emitter.Fields);
			ms.ReadFields(emitter.SystemFields);

			emitter.ParticleRed = ms.ReadTrack();
			emitter.ParticleGreen = ms.ReadTrack();
			emitter.ParticleBlue = ms.ReadTrack();
			emitter.ParticleAlpha = ms.ReadTrack();
			emitter.ParticleBrightness = ms.ReadTrack();
			emitter.ParticleSpinAngle = ms.ReadTrack();
			emitter.ParticleSpinSpeed = ms.ReadTrack();
			emitter.ParticleScale = ms.ReadTrack();
			emitter.ParticleStretch = ms.ReadTrack();
			emitter.CollisionReflect = ms.ReadTrack();
			emitter.CollisionSpin = ms.ReadTrack();
			;
			emitter.ClipTop = ms.ReadTrack();
			emitter.ClipBottom = ms.ReadTrack();
			emitter.ClipLeft = ms.ReadTrack();
			emitter.ClipRight = ms.ReadTrack();
			emitter.AnimationRate = ms.ReadTrack();
		}

		return new(emitters);
	}

	private static void ReadFields(this Stream stream, PopParticlesField[]? fields)
	{
		if (stream.ReadInt() != 20)
			throw new("FIXME");

		if (fields == null)
			return;

		for (var i = 0; i < fields.Length; i++)
		{
			var field = new PopParticlesField
			{
				FieldType = (PopParticlesFieldType)stream.ReadInt()
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
			field.X = stream.ReadTrack();
			field.Y = stream.ReadTrack();
		}
	}

	private static PopParticlesTrack ReadTrack(this Stream stream)
	{
		var count = stream.ReadInt();
		var nodes = new PopParticlesTrackNode[count];

		for (var i = 0; i < count; i++)
		{
			var time = stream.ReadFloat();
			var lowValue = stream.ReadFloat();
			var highValue = stream.ReadFloat();
			var curveType = (PopParticlesCurveType)stream.ReadInt();
			var distribution = (PopParticlesCurveType)stream.ReadInt();
			nodes[i] = new(time, lowValue, highValue, curveType, distribution);
		}
		return new(nodes);
	}
}

using PopLib.Misc;
using System.Globalization;
using System.Text;

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
			// FIXME
			ms.Position += 4;

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			if (ms.ReadUint() != 1)
				throw new("FIXME");

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			var particleFlags = ms.ReadUint();

			if (ms.ReadUint() != 1)
				throw new("FIXME");

			// FIXME
			ms.Position += 47 * 4;

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			ms.Position += 4;

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			ms.Position += 32 * 4;

			ref var emitter = ref emitters[i];

			if ((particleFlags & ~0b100101011001) != 0)
				throw new($"FIXME: {Convert.ToString(particleFlags, 2)}");

			emitter.RandomLaunchSpin = (particleFlags & 1) != 0;
			emitter.SystemLoops = ((particleFlags >> 3) & 1) != 0;
			emitter.ParticleLoops = ((particleFlags >> 4) & 1) != 0;
			emitter.RandomStartTime = ((particleFlags >> 6) & 1) != 0;
			emitter.Additive = ((particleFlags >> 8) & 1) != 0;
			emitter.HardwareOnly = ((particleFlags >> 11) & 1) != 0;
		}

		for (var i = 0; i < emitterCount; i++)
		{
			ref var emitter = ref emitters[i];
			emitter.Image = ms.ReadString();
			emitter.Name = ms.ReadString();

			emitter.SystemDuration = ReadFloatParameterTrack(ms);

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			emitter.SpawnMinActive = ReadFloatParameterTrack(ms);

			if (ms.ReadUint() != 0)
				throw new("FIXME");

			emitter.SpawnMaxLaunched = ReadFloatParameterTrack(ms);

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

			emitter.ParticleDuration = ReadFloatParameterTrack(ms);

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

			if (ms.ReadUint() != 20)
				throw new("FIXME");

			if (ms.ReadUint() != 20)
				throw new("FIXME");

			emitter.ParticleRed = ReadFloatParameterTrack(ms);
			emitter.ParticleGreen = ReadFloatParameterTrack(ms);
			emitter.ParticleBlue = ReadFloatParameterTrack(ms);

			emitter.ParticleAlpha = ReadFloatParameterTrack(ms);
			emitter.ParticleBrightness = ReadFloatParameterTrack(ms);
			emitter.ParticleSpinAngle = ReadFloatParameterTrack(ms);
			emitter.ParticleSpinSpeed = ReadFloatParameterTrack(ms);
			emitter.ParticleScale = ReadFloatParameterTrack(ms);

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

	private static ParticlesFloatParameterTrack ReadFloatParameterTrack(Stream stream)
	{
		var count = stream.ReadInt();
		var nodes = new ParticlesFloatParameterTrackNode[count];

		for (var i = 0; i < count; i++)
		{
			ref var node = ref nodes[i];
			node.Time = stream.ReadFloat();
			node.LowValue = stream.ReadFloat();
			node.HighValue = stream.ReadFloat();
			node.CurveType = (ParticlesCurveType)stream.ReadInt();
			node.Distribution = (ParticlesCurveType)stream.ReadInt();
		}
		return new(nodes);
	}
}

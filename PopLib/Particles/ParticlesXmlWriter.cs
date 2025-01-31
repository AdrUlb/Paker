using System.Globalization;
using System.Text;

namespace PopLib.Particles;

public class ParticlesXmlWriter
{
	public static void WriteToStream(ParticlesEffect particles, Stream stream)
	{
		using var writer = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true);

		var builder = new StringBuilder();
		WriteToStringBuilder(particles, builder);
		writer.Write(builder.ToString());
	}

	public static void WriteToStringBuilder(ParticlesEffect particles, StringBuilder builder)
	{
		foreach (var emitter in particles.Emitters)
			WriteEmitter(emitter, builder);
	}

	private static string WriteEmitter(ParticlesEmitter emitter, StringBuilder builder)
	{
		var spawnMinActive = TrackToString(emitter.SpawnMinActive);
		var spawnMaxLaunched = TrackToString(emitter.SpawnMaxLaunched);
		var particleScale = TrackToString(emitter.ParticleScale);
		var particleDuration = TrackToString(emitter.ParticleDuration);
		var systemDuration = TrackToString(emitter.SystemDuration);
		var particleRed = TrackToString(emitter.ParticleRed);
		var particleGreen = TrackToString(emitter.ParticleGreen);
		var particleBlue = TrackToString(emitter.ParticleBlue);
		var particleAlpha = TrackToString(emitter.ParticleAlpha);
		var particleSpinSpeed = TrackToString(emitter.ParticleSpinSpeed);
		var particleBrightness = TrackToString(emitter.ParticleBrightness);
		var particleSpinAngle = TrackToString(emitter.ParticleSpinAngle);

		builder.AppendLine("<Emitter>");

		if (!string.IsNullOrEmpty(emitter.Name))
			builder.Append($"  <Name>").Append(emitter.Name).AppendLine("</Name>");

		if (!string.IsNullOrEmpty(spawnMinActive))
			builder.Append("  <SpawnMinActive>").Append(spawnMinActive).AppendLine("</SpawnMinActive>");

		if (!string.IsNullOrEmpty(spawnMaxLaunched))
			builder.Append("  <SpawnMaxLaunched>").Append(spawnMaxLaunched).AppendLine("</SpawnMaxLaunched>");

		if (!string.IsNullOrEmpty(particleScale))
			builder.Append("  <ParticleScale>").Append(particleScale).AppendLine("</ParticleScale>");

		if (!string.IsNullOrEmpty(particleDuration))
			builder.Append("  <ParticleDuration>").Append(particleDuration).AppendLine("</ParticleDuration>");

		if (!string.IsNullOrEmpty(systemDuration))
			builder.Append("  <SystemDuration>").Append(systemDuration).AppendLine("</SystemDuration>");

		if (!string.IsNullOrEmpty(emitter.Image))
			builder.Append("  <Image>").Append(emitter.Image).AppendLine("</Image>");

		if (!string.IsNullOrEmpty(particleAlpha))
			builder.Append("  <ParticleAlpha>").Append(particleAlpha).AppendLine("</ParticleAlpha>");

		if (!string.IsNullOrEmpty(particleRed))
			builder.Append("  <ParticleRed>").Append(particleRed).AppendLine("</ParticleRed>");

		if (!string.IsNullOrEmpty(particleGreen))
			builder.Append("  <ParticleGreen>").Append(particleGreen).AppendLine("</ParticleGreen>");

		if (!string.IsNullOrEmpty(particleBlue))
			builder.Append("  <ParticleBlue>").Append(particleBlue).AppendLine("</ParticleBlue>");

		if (!string.IsNullOrEmpty(particleSpinSpeed))
			builder.Append("  <ParticleSpinSpeed>").Append(particleSpinSpeed).AppendLine("</ParticleSpinSpeed>");

		if (emitter.RandomLaunchSpin)
			builder.AppendLine("  <RandomLaunchSpin>1</RandomLaunchSpin>");

		if (emitter.SystemLoops)
			builder.AppendLine("  <SystemLoops>1</SystemLoops>");

		if (emitter.ParticleLoops)
			builder.AppendLine("  <ParticleLoops>1</ParticleLoops>");

		if (emitter.RandomStartTime)
			builder.AppendLine("  <RandomStartTime>1</RandomStartTime>");

		if (emitter.Additive)
			builder.AppendLine("  <Additive>1</Additive>");

		if (emitter.HardwareOnly)
			builder.AppendLine("  <HardwareOnly>1</HardwareOnly>");

		if (!string.IsNullOrEmpty(particleBrightness))
			builder.Append("  <ParticleBrightness>").Append(particleBrightness).AppendLine("</ParticleBrightness>");

		if (!string.IsNullOrEmpty(particleSpinAngle))
			builder.Append("  <ParticleSpinAngle>").Append(particleSpinAngle).AppendLine("</ParticleSpinAngle>");

		builder.AppendLine("</Emitter>");
		return builder.ToString();
	}

	private static string TrackToString(ParticlesFloatParameterTrack track)
	{
		return string.Join(' ', track.Nodes.Select((node, index) => TrackNodeToString(node, index, track.Nodes.Length)));
	}

	private static string TrackNodeToString(ParticlesFloatParameterTrackNode node, int index, int count)
	{
		if (node.Distribution != ParticlesCurveType.Linear)
			throw new("FIXME");

		var time = node.Time;

		var timeStr = float.Round(node.Time * 100, 2).ToString(CultureInfo.InvariantCulture);

		var valueStr = $"{node.LowValue:#.#}";
		if (valueStr == "")
			valueStr = "0";

		if (node.LowValue != node.HighValue)
		{
			var valueStr2 = $"{node.HighValue:#.#}";
			if (valueStr2 == "")
				valueStr2 = "0";
			valueStr = $"[{valueStr} {valueStr2}]";
		}

		var sb = new StringBuilder();

		sb.Append(valueStr);

		if (count != 1 && time != ((index + 1) / count))
			sb.Append(',').Append(timeStr);

		if (node.CurveType != ParticlesCurveType.Linear)
		{
			sb.Append(' ').Append(node.CurveType);
		}

		return sb.ToString();
	}
}

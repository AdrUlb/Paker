using System.Diagnostics;
using System.Text;

namespace PopLib.Particles;

public static class ParticlesXmlWriter
{
	public static void WriteToStream(ParticlesDefinition particles, Stream stream)
	{
		using var writer = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true);

		var builder = new StringBuilder();
		WriteToStringBuilder(particles, builder);
		writer.Write(builder.ToString());
	}

	public static void WriteToStringBuilder(ParticlesDefinition particles, StringBuilder builder)
	{
		foreach (var emitter in particles.Emitters)
			WriteEmitter(emitter, builder);
	}

	private static string WriteEmitter(ParticlesEmitter emitter, StringBuilder builder)
	{
		var systemDuration = emitter.SystemDuration?.ToXmlString();
		var crossFadeDuration = emitter.CrossFadeDuration?.ToXmlString();
		var spawnRate = emitter.SpawnRate?.ToXmlString();
		var spawnMinActive = emitter.SpawnMinActive?.ToXmlString();
		var spawnMaxActive = emitter.SpawnMaxActive?.ToXmlString();
		var spawnMaxLaunched = emitter.SpawnMaxLaunched?.ToXmlString();
		var emitterRadius = emitter.EmitterRadius?.ToXmlString();
		var emitterOffsetX = emitter.EmitterOffsetX?.ToXmlString();
		var emitterOffsetY = emitter.EmitterOffsetY?.ToXmlString();
		var emitterBoxX = emitter.EmitterBoxX?.ToXmlString();
		var emitterBoxY = emitter.EmitterBoxY?.ToXmlString();
		var emitterSkewX = emitter.EmitterSkewX?.ToXmlString();
		var emitterSkewY = emitter.EmitterSkewY?.ToXmlString();
		var particleDuration = emitter.ParticleDuration?.ToXmlString();
		var systemAlpha = emitter.SystemAlpha?.ToXmlString();
		var launchSpeed = emitter.LaunchSpeed?.ToXmlString();
		var launchAngle = emitter.LaunchAngle?.ToXmlString();
		var particleRed = emitter.ParticleRed?.ToXmlString();
		var particleGreen = emitter.ParticleGreen?.ToXmlString();
		var particleBlue = emitter.ParticleBlue?.ToXmlString();
		var particleAlpha = emitter.ParticleAlpha?.ToXmlString();
		var particleBrightness = emitter.ParticleBrightness?.ToXmlString();
		var particleSpinAngle = emitter.ParticleSpinAngle?.ToXmlString();
		var particleSpinSpeed = emitter.ParticleSpinSpeed?.ToXmlString();
		var particleScale = emitter.ParticleScale?.ToXmlString();
		var particleStretch = emitter.ParticleStretch?.ToXmlString();
		var collisionReflect = emitter.CollisionReflect?.ToXmlString();
		var collisionSpin = emitter.CollisionSpin?.ToXmlString();
		var clipTop = emitter.ClipTop?.ToXmlString();
		var clipBottom = emitter.ClipBottom?.ToXmlString();
		var clipLeft = emitter.ClipLeft?.ToXmlString();
		var clipRight = emitter.ClipRight?.ToXmlString();
		var animationRate = emitter.AnimationRate?.ToXmlString();

		builder.AppendLine("<Emitter>");

		if (emitter.ImageCol != ParticlesEmitter.DefaultImageCol)
			builder.Append("  <ImageCol>").Append(emitter.ImageCol).AppendLine("</ImageCol>");

		if (emitter.ImageRow != ParticlesEmitter.DefaultImageRow)
			builder.Append("  <ImageRow>").Append(emitter.ImageRow).AppendLine("</ImageRow>");

		if (emitter.ImageFrames != ParticlesEmitter.DefaultImageFrames)
			builder.Append("  <ImageFrames>").Append(emitter.ImageFrames).AppendLine("</ImageFrames>");

		if (emitter.Animated != ParticlesEmitter.DefaultAnimated)
			builder.Append("  <Animated>").Append(emitter.Animated).AppendLine("</Animated>");

		if (emitter.RandomLaunchSpin)
			builder.AppendLine("  <RandomLaunchSpin>1</RandomLaunchSpin>");

		if (emitter.AlignLaunchSpin)
			builder.AppendLine("  <AlignLaunchSpin>1</AlignLaunchSpin>");

		if (emitter.SystemLoops)
			builder.AppendLine("  <SystemLoops>1</SystemLoops>");

		if (emitter.ParticleLoops)
			builder.AppendLine("  <ParticleLoops>1</ParticleLoops>");

		if (emitter.ParticlesDontFollow)
			builder.AppendLine("  <ParticlesDontFollow>1</ParticlesDontFollow>");

		if (emitter.RandomStartTime)
			builder.AppendLine("  <RandomStartTime>1</RandomStartTime>");

		if (emitter.DieIfOverloaded)
			builder.AppendLine("  <DieIfOverloaded>1</DieIfOverloaded>");

		if (emitter.Additive)
			builder.AppendLine("  <Additive>1</Additive>");

		if (emitter.FullScreen)
			builder.AppendLine("  <FullScreen>1</FullScreen>");

		if (emitter.HardwareOnly)
			builder.AppendLine("  <HardwareOnly>1</HardwareOnly>");

		if (emitter.EmitterType != ParticlesEmitter.DefaultEmitterType)
			builder.Append("  <EmitterType>").Append(emitter.EmitterType).AppendLine("</EmitterType>");

		if (emitter.Fields != null)
		{
			foreach (var field in emitter.Fields)
			{
				var x = field.X?.ToXmlString();
				var y = field.Y?.ToXmlString();
				builder.AppendLine("  <Field>");
				builder.Append("    <FieldType>").Append(field.FieldType).AppendLine("</FieldType>");

				if (!string.IsNullOrWhiteSpace(x))
					builder.Append("    <X>").Append(x).AppendLine("</X>");

				if (!string.IsNullOrWhiteSpace(y))
					builder.Append("    <Y>").Append(y).AppendLine("</Y>");
				builder.AppendLine("  </Field>");
			}
		}

		if (emitter.SystemFields != null)
		{
			foreach (var field in emitter.SystemFields)
			{
				var x = field.X?.ToXmlString();
				var y = field.Y?.ToXmlString();
				builder.AppendLine("  <SystemField>");
				builder.Append("    <FieldType>").Append(field.FieldType).AppendLine("</FieldType>");

				if (!string.IsNullOrWhiteSpace(x))
					builder.Append("    <X>").Append(x).AppendLine("</X>");

				if (!string.IsNullOrWhiteSpace(y))
					builder.Append("    <Y>").Append(y).AppendLine("</Y>");
				builder.AppendLine("  </SystemField>");
			}
		}

		if (!string.IsNullOrEmpty(emitter.Image))
			builder.Append("  <Image>").Append(emitter.Image).AppendLine("</Image>");

		if (!string.IsNullOrEmpty(emitter.Name))
			builder.Append($"  <Name>").Append(emitter.Name).AppendLine("</Name>");

		if (!string.IsNullOrEmpty(systemDuration))
			builder.Append("  <SystemDuration>").Append(systemDuration).AppendLine("</SystemDuration>");

		if (!string.IsNullOrEmpty(crossFadeDuration))
			builder.Append("  <CrossFadeDuration>").Append(crossFadeDuration).AppendLine("</CrossFadeDuration>");

		if (!string.IsNullOrEmpty(spawnRate))
			builder.Append("  <SpawnRate>").Append(spawnRate).AppendLine("</SpawnRate>");

		if (!string.IsNullOrEmpty(spawnMinActive))
			builder.Append("  <SpawnMinActive>").Append(spawnMinActive).AppendLine("</SpawnMinActive>");

		if (!string.IsNullOrEmpty(spawnMaxActive))
			builder.Append("  <SpawnMaxActive>").Append(spawnMaxActive).AppendLine("</SpawnMaxActive>");

		if (!string.IsNullOrEmpty(spawnMaxLaunched))
			builder.Append("  <SpawnMaxLaunched>").Append(spawnMaxLaunched).AppendLine("</SpawnMaxLaunched>");

		if (!string.IsNullOrEmpty(emitterRadius))
			builder.Append("  <EmitterRadius>").Append(emitterRadius).AppendLine("</EmitterRadius>");

		if (!string.IsNullOrEmpty(emitterOffsetX))
			builder.Append("  <EmitterOffsetX>").Append(emitterOffsetX).AppendLine("</EmitterOffsetX>");

		if (!string.IsNullOrEmpty(emitterOffsetY))
			builder.Append("  <EmitterOffsetY>").Append(emitterOffsetY).AppendLine("</EmitterOffsetY>");

		if (!string.IsNullOrEmpty(emitterBoxX))
			builder.Append("  <EmitterBoxX>").Append(emitterBoxX).AppendLine("</EmitterBoxX>");

		if (!string.IsNullOrEmpty(emitterBoxY))
			builder.Append("  <EmitterBoxY>").Append(emitterBoxY).AppendLine("</EmitterBoxY>");

		if (!string.IsNullOrEmpty(emitterSkewX))
			builder.Append("  <EmitterSkewX>").Append(emitterSkewX).AppendLine("</EmitterSkewX>");

		if (!string.IsNullOrEmpty(emitterSkewY))
			builder.Append("  <EmitterSkewY>").Append(emitterSkewY).AppendLine("</EmitterSkewY>");

		if (!string.IsNullOrEmpty(particleDuration))
			builder.Append("  <ParticleDuration>").Append(particleDuration).AppendLine("</ParticleDuration>");

		if (!string.IsNullOrEmpty(systemAlpha))
			builder.Append("  <SystemAlpha>").Append(systemAlpha).AppendLine("</SystemAlpha>");

		if (!string.IsNullOrEmpty(launchSpeed))
			builder.Append("  <LaunchSpeed>").Append(launchSpeed).AppendLine("</LaunchSpeed>");

		if (!string.IsNullOrEmpty(launchAngle))
			builder.Append("  <LaunchAngle>").Append(launchAngle).AppendLine("</LaunchAngle>");

		if (!string.IsNullOrEmpty(particleRed))
			builder.Append("  <ParticleRed>").Append(particleRed).AppendLine("</ParticleRed>");

		if (!string.IsNullOrEmpty(particleGreen))
			builder.Append("  <ParticleGreen>").Append(particleGreen).AppendLine("</ParticleGreen>");

		if (!string.IsNullOrEmpty(particleBlue))
			builder.Append("  <ParticleBlue>").Append(particleBlue).AppendLine("</ParticleBlue>");

		if (!string.IsNullOrEmpty(particleAlpha))
			builder.Append("  <ParticleAlpha>").Append(particleAlpha).AppendLine("</ParticleAlpha>");

		if (!string.IsNullOrEmpty(particleBrightness))
			builder.Append("  <ParticleBrightness>").Append(particleBrightness).AppendLine("</ParticleBrightness>");

		if (!string.IsNullOrEmpty(particleSpinAngle))
			builder.Append("  <ParticleSpinAngle>").Append(particleSpinAngle).AppendLine("</ParticleSpinAngle>");

		if (!string.IsNullOrEmpty(particleSpinSpeed))
			builder.Append("  <ParticleSpinSpeed>").Append(particleSpinSpeed).AppendLine("</ParticleSpinSpeed>");

		if (!string.IsNullOrEmpty(particleScale))
			builder.Append("  <ParticleScale>").Append(particleScale).AppendLine("</ParticleScale>");

		if (!string.IsNullOrEmpty(particleStretch))
			builder.Append("  <ParticleStretch>").Append(particleStretch).AppendLine("</ParticleStretch>");

		if (!string.IsNullOrEmpty(collisionReflect))
			builder.Append("  <CollisionReflect>").Append(collisionReflect).AppendLine("</CollisionReflect>");

		if (!string.IsNullOrEmpty(collisionSpin))
			builder.Append("  <CollisionSpin>").Append(collisionSpin).AppendLine("</CollisionSpin>");

		if (!string.IsNullOrEmpty(clipTop))
			builder.Append("  <ClipTop>").Append(clipTop).AppendLine("</ClipTop>");

		if (!string.IsNullOrEmpty(clipBottom))
			builder.Append("  <ClipBottom>").Append(clipBottom).AppendLine("</ClipBottom>");

		if (!string.IsNullOrEmpty(clipLeft))
			builder.Append("  <ClipLeft>").Append(clipLeft).AppendLine("</ClipLeft>");

		if (!string.IsNullOrEmpty(clipRight))
			builder.Append("  <ClipRight>").Append(clipRight).AppendLine("</ClipRight>");

		if (!string.IsNullOrEmpty(animationRate))
			builder.Append("  <AnimationRate>").Append(animationRate).AppendLine("</AnimationRate>");

		builder.AppendLine("</Emitter>");
		return builder.ToString();
	}

	private static string ToXmlString(this ParticlesFloatParameterTrack track)
	{
		return string.Join(' ', track.Nodes.Select((_, i) => ToXmlString(track, i)));
	}

	private static string ToXmlString(this ParticlesFloatParameterTrack track, int nodeIndex)
	{
		var node = track.Nodes[nodeIndex];
		var sb = new StringBuilder();

		// Constant distribution
		if (node.Distribution == ParticlesCurveType.Constant) // "Constant distribution"
		{
			if (node.CurveType != ParticlesCurveType.Linear)
				throw new("FIXME");

			if (node.LowValue != node.HighValue)
				throw new UnreachableException();

			var value = node.LowValue;
			sb.Append('[').Append(FormatValue(value)).Append(']');
		}
		else if (node.LowValue != node.HighValue) // Distribution
		{
			if (node.CurveType != ParticlesCurveType.Linear)
				throw new("FIXME");

			sb.Append('[').Append(FormatValue(node.LowValue));

			if (node.Distribution != ParticlesCurveType.Linear)
				sb.Append(' ').Append(node.Distribution);

			sb.Append(' ').Append(FormatValue(node.HighValue)).Append(']');
		}
		else // Scalar
		{
			if (node.Distribution != ParticlesCurveType.Linear)
				throw new("FIXME");

			var value = node.LowValue;
			sb.Append(FormatValue(value));
		}

		sb.Append(',').Append(FormatValue(node.Time * 100));

		if (node.CurveType != ParticlesCurveType.Linear)
			sb.Append(' ').Append(node.CurveType);

		return sb.ToString();

		string FormatValue(float value)
		{
			var ret = value.ToString("#.###");
			return ret == "" ? "0" : ret;
		}
	}
}

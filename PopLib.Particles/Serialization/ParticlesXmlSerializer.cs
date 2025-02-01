using PopLib.Particles.Definition;
using System.Diagnostics;
using System.Text;
using System.Xml;

namespace PopLib.Particles.Serialization;

public static class ParticlesXmlSerializer
{
	public static void Serialize(PopParticlesDefinition popParticles, Stream outputStream)
	{
		using var writer = new StreamWriter(outputStream, Encoding.UTF8, leaveOpen: true);

		var builder = new StringBuilder();
		Serialize(popParticles, builder);
		writer.Write(builder.ToString());
	}

	public static void Serialize(PopParticlesDefinition popParticles, StringBuilder outputStringBuilder)
	{
		foreach (var emitter in popParticles.Emitters)
			WriteEmitter(emitter, outputStringBuilder);
	}

	private static string WriteEmitter(PopParticlesEmitter emitter, StringBuilder builder)
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

		if (emitter.ImageCol != PopParticlesEmitter.DefaultImageCol)
			builder.Append("  <ImageCol>").Append(emitter.ImageCol).AppendLine("</ImageCol>");

		if (emitter.ImageRow != PopParticlesEmitter.DefaultImageRow)
			builder.Append("  <ImageRow>").Append(emitter.ImageRow).AppendLine("</ImageRow>");

		if (emitter.ImageFrames != PopParticlesEmitter.DefaultImageFrames)
			builder.Append("  <ImageFrames>").Append(emitter.ImageFrames).AppendLine("</ImageFrames>");

		if (emitter.Animated != PopParticlesEmitter.DefaultAnimated)
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

		if (emitter.EmitterType != PopParticlesEmitter.DefaultEmitterType)
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

	private static string ToXmlString(this PopParticlesTrack track)
	{
		return string.Join(' ', track.Nodes.Select((_, i) => ToXmlString(track, i)));
	}

	private static string ToXmlString(this PopParticlesTrack track, int nodeIndex)
	{
		var node = track.Nodes[nodeIndex];
		var sb = new StringBuilder();

		// Constant distribution
		if (node.Distribution == PopParticlesCurveType.Constant) // "Constant distribution"
		{
			if (node.CurveType != PopParticlesCurveType.Linear)
				throw new("FIXME");

			if (node.LowValue != node.HighValue)
				throw new UnreachableException();

			var value = node.LowValue;
			sb.Append('[').Append(FormatValue(value)).Append(']');
		}
		else if (node.LowValue != node.HighValue) // Distribution
		{
			if (node.CurveType != PopParticlesCurveType.Linear)
				throw new("FIXME");

			sb.Append('[').Append(FormatValue(node.LowValue));

			if (node.Distribution != PopParticlesCurveType.Linear)
				sb.Append(' ').Append(node.Distribution);

			sb.Append(' ').Append(FormatValue(node.HighValue)).Append(']');
		}
		else // Scalar
		{
			if (node.Distribution != PopParticlesCurveType.Linear)
				throw new("FIXME");

			var value = node.LowValue;
			sb.Append(FormatValue(value));
		}

		sb.Append(',').Append(FormatValue(node.Time * 100));

		if (node.CurveType != PopParticlesCurveType.Linear)
			sb.Append(' ').Append(node.CurveType);

		return sb.ToString();

		string FormatValue(float value)
		{
			var ret = value.ToString("#.###");
			return ret == "" ? "0" : ret;
		}
	}

	public static PopParticlesDefinition Deserialize(Stream stream)
	{
		using var streamReader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
		return Deserialize(streamReader.ReadToEnd());
	}

	public static PopParticlesDefinition Deserialize(string str)
	{
		using var stringReader = new StringReader("<root>" + str + "</root>");
		var settings = new XmlReaderSettings
		{
			IgnoreWhitespace = true
		};
		using var reader = XmlReader.Create(stringReader, settings);

		if (!reader.Read() || reader.NodeType != XmlNodeType.Element || reader.Name != "root")
			throw new("FIXME");

		var emitters = new List<PopParticlesEmitter>();

		while (reader.Read())
		{
			switch (reader.NodeType)
			{
				case XmlNodeType.EndElement:
					if (reader.Name != "root")
						throw new("FIXME");

					goto end;
				case XmlNodeType.Element:
					switch (reader.Name)
					{
						case "Emitter":
							emitters.Add(ReadEmitter(reader));
							break;
						default:
							throw new NotSupportedException($"Particles root tag {reader.Name} not supported.");
					}
					break;
				default:
					throw new InvalidDataException($"Encountered unexpected node of type {reader.NodeType} while parsing particles effect.");
			}
		}
		end:
		return new([..emitters]);
	}

	private static PopParticlesEmitter ReadEmitter(XmlReader reader)
	{
		var emitter = new PopParticlesEmitter();

		var fields = new List<PopParticlesField>();
		var systemFields = new List<PopParticlesField>();

		while (reader.Read())
		{
			switch (reader.NodeType)
			{
				case XmlNodeType.EndElement:
					if (reader.Name != "Emitter")
						throw new("FIXME");

					goto end;
				case XmlNodeType.Element:
					switch (reader.Name)
					{
						case "ImageCol": emitter.ImageCol = ReadIntParameter(reader, reader.Name); break;
						case "ImageRow": emitter.ImageRow = ReadIntParameter(reader, reader.Name); break;
						case "ImageFrames": emitter.ImageFrames = ReadIntParameter(reader, reader.Name); break;
						case "Animated": emitter.Animated = ReadIntParameter(reader, reader.Name); break;
						case "RandomLaunchSpin": emitter.RandomLaunchSpin = ReadBoolParameter(reader, reader.Name); break;
						case "AlignLaunchSpin": emitter.AlignLaunchSpin = ReadBoolParameter(reader, reader.Name); break;
						case "SystemLoops": emitter.SystemLoops = ReadBoolParameter(reader, reader.Name); break;
						case "ParticleLoops": emitter.ParticleLoops = ReadBoolParameter(reader, reader.Name); break;
						case "ParticlesDontFollow": emitter.ParticlesDontFollow = ReadBoolParameter(reader, reader.Name); break;
						case "RandomStartTime": emitter.RandomStartTime = ReadBoolParameter(reader, reader.Name); break;
						case "DieIfOverloaded": emitter.DieIfOverloaded = ReadBoolParameter(reader, reader.Name); break;
						case "Additive": emitter.Additive = ReadBoolParameter(reader, reader.Name); break;
						case "FullScreen": emitter.FullScreen = ReadBoolParameter(reader, reader.Name); break;
						case "HardwareOnly": emitter.HardwareOnly = ReadBoolParameter(reader, reader.Name); break;
						case "EmitterType": emitter.EmitterType = ReadEnumParameter<PopParticlesEmitterType>(reader, reader.Name); break;
						case "Field": fields.Add(ReadField(reader, reader.Name)); break;
						case "SystemField": systemFields.Add(ReadField(reader, reader.Name)); break;
						case "Image": emitter.Image = ReadStringParameter(reader, reader.Name).ToUpper(); break;
						case "Name": emitter.Name = ReadStringParameter(reader, reader.Name); break;
						case "SystemDuration": emitter.SystemDuration = ReadFloatParameterTrack(reader, reader.Name); break;
						case "CrossFadeDuration": emitter.CrossFadeDuration = ReadFloatParameterTrack(reader, reader.Name); break;
						case "SpawnRate": emitter.SpawnRate = ReadFloatParameterTrack(reader, reader.Name); break;
						case "SpawnMinActive": emitter.SpawnMinActive = ReadFloatParameterTrack(reader, reader.Name); break;
						case "SpawnMaxActive": emitter.SpawnMaxActive = ReadFloatParameterTrack(reader, reader.Name); break;
						case "SpawnMaxLaunched": emitter.SpawnMaxLaunched = ReadFloatParameterTrack(reader, reader.Name); break;
						case "EmitterRadius": emitter.EmitterRadius = ReadFloatParameterTrack(reader, reader.Name); break;
						case "EmitterOffsetX": emitter.EmitterOffsetX = ReadFloatParameterTrack(reader, reader.Name); break;
						case "EmitterOffsetY": emitter.EmitterOffsetY = ReadFloatParameterTrack(reader, reader.Name); break;
						case "EmitterBoxX": emitter.EmitterBoxX = ReadFloatParameterTrack(reader, reader.Name); break;
						case "EmitterBoxY": emitter.EmitterBoxY = ReadFloatParameterTrack(reader, reader.Name); break;
						case "EmitterSkewX": emitter.EmitterSkewX = ReadFloatParameterTrack(reader, reader.Name); break;
						case "EmitterSkewY": emitter.EmitterSkewY = ReadFloatParameterTrack(reader, reader.Name); break;
						case "ParticleDuration": emitter.ParticleDuration = ReadFloatParameterTrack(reader, reader.Name); break;
						case "SystemAlpha": emitter.SystemAlpha = ReadFloatParameterTrack(reader, reader.Name); break;
						case "LaunchSpeed": emitter.LaunchSpeed = ReadFloatParameterTrack(reader, reader.Name); break;
						case "LaunchAngle": emitter.LaunchAngle = ReadFloatParameterTrack(reader, reader.Name); break;
						case "ParticleRed": emitter.ParticleRed = ReadFloatParameterTrack(reader, reader.Name); break;
						case "ParticleGreen": emitter.ParticleGreen = ReadFloatParameterTrack(reader, reader.Name); break;
						case "ParticleBlue": emitter.ParticleBlue = ReadFloatParameterTrack(reader, reader.Name); break;
						case "ParticleAlpha": emitter.ParticleAlpha = ReadFloatParameterTrack(reader, reader.Name); break;
						case "ParticleBrightness": emitter.ParticleBrightness = ReadFloatParameterTrack(reader, reader.Name); break;
						case "ParticleSpinAngle": emitter.ParticleSpinAngle = ReadFloatParameterTrack(reader, reader.Name); break;
						case "ParticleSpinSpeed": emitter.ParticleSpinSpeed = ReadFloatParameterTrack(reader, reader.Name); break;
						case "ParticleScale": emitter.ParticleScale = ReadFloatParameterTrack(reader, reader.Name); break;
						case "ParticleStretch": emitter.ParticleStretch = ReadFloatParameterTrack(reader, reader.Name); break;
						case "CollisionReflect": emitter.CollisionReflect = ReadFloatParameterTrack(reader, reader.Name); break;
						case "CollisionSpin": emitter.CollisionSpin = ReadFloatParameterTrack(reader, reader.Name); break;
						case "ClipTop": emitter.ClipTop = ReadFloatParameterTrack(reader, reader.Name); break;
						case "ClipBottom": emitter.ClipBottom = ReadFloatParameterTrack(reader, reader.Name); break;
						case "ClipLeft": emitter.ClipLeft = ReadFloatParameterTrack(reader, reader.Name); break;
						case "ClipRight": emitter.ClipRight = ReadFloatParameterTrack(reader, reader.Name); break;
						case "AnimationRate": emitter.AnimationRate = ReadFloatParameterTrack(reader, reader.Name); break;
						default:
							throw new NotSupportedException($"Particles Emitter tag '{reader.Name}' not supported.");
					}
					break;
				default:
					throw new InvalidDataException($"Encountered unexpected node of type {reader.NodeType} while parsing particles Emitter.");
			}
		}
		end:

		if (fields.Count > 0)
			emitter.Fields = [..fields];

		if (systemFields.Count > 0)
			emitter.SystemFields = [..systemFields];

		return emitter;
	}

	private static string ReadStringParameter(XmlReader reader, string paramName)
	{
		if (!reader.Read() || reader.NodeType != XmlNodeType.Text)
			throw new("FIXME");

		var ret = reader.Value;

		if (!reader.Read() || reader.NodeType != XmlNodeType.EndElement || reader.Name != paramName)
			throw new($"FIXME");

		return ret;
	}

	private static int ReadIntParameter(XmlReader reader, string paramName) => int.Parse(ReadStringParameter(reader, paramName));

	private static T ReadEnumParameter<T>(XmlReader reader, string paramName) where T : struct
	{
		if (!Enum.TryParse<T>(ReadStringParameter(reader, paramName), out var value))
			throw new("FIXME");

		return value;
	}

	private static bool ReadBoolParameter(XmlReader reader, string paramName)
	{
		if (!reader.Read() || reader.NodeType != XmlNodeType.Text)
			throw new("FIXME");

		var ret = reader.Value switch
		{
			"0" => false,
			"1" => true,
			_ => throw new("FIXME")
		};

		if (!reader.Read() || reader.NodeType != XmlNodeType.EndElement || reader.Name != paramName)
			throw new($"FIXME");

		return ret;
	}

	private static PopParticlesField ReadField(XmlReader reader, string name)
	{
		var field = new PopParticlesField();

		while (reader.Read())
		{
			switch (reader.NodeType)
			{
				case XmlNodeType.EndElement:
					if (reader.Name != name)
						throw new("FIXME");

					goto end;
				case XmlNodeType.Element:
					switch (reader.Name)
					{
						case "FieldType": field.FieldType = ReadEnumParameter<PopParticlesFieldType>(reader, reader.Name); break;
						case "X": field.X = ReadFloatParameterTrack(reader, reader.Name); break;
						case "Y": field.Y = ReadFloatParameterTrack(reader, reader.Name); break;
						default:
							throw new NotSupportedException($"Particles Field tag '{reader.Name}' not supported.");
					}
					break;
				default:
					throw new InvalidDataException($"Encountered unexpected node of type {reader.NodeType} while parsing particles Emitter.");
			}
		}
		end:

		return field;
	}

	private static PopParticlesTrack ReadFloatParameterTrack(XmlReader reader, string paramName) => ParseFloatParameterTrack(ReadStringParameter(reader, paramName));

	private static PopParticlesTrack ParseFloatParameterTrack(string str)
	{
		str = str.Trim();

		if (str == "")
			return new([]);

		var nodes = new List<PopParticlesTrackNode>();
		{
			var i = 0;

			// format := value [ ',' time ] [ ' ' CURVETYPE ]
			// value := NUMBER | ( '[' NUMBER ( ']' | ( NUMBER ']'  [ DISTRIBUTION ] ) )

			while (i < str.Length)
			{
				float lowValue;
				float highValue;
				var time = float.NaN;
				var curveType = PopParticlesCurveType.Linear;
				var distribution = PopParticlesCurveType.Linear;

				SkipWhitespace();

				if (str[i] == '[') // Distribution
				{
					i++;
					SkipWhitespace();
					lowValue = ReadFloat();
					SkipWhitespace();

					if (str[i] == ']')
					{
						i++;
						highValue = lowValue;
						distribution = PopParticlesCurveType.Constant;
					}
					else
					{
						if (str[i] is (>= 'A' and <= 'Z') or (>= 'a' and <= 'z'))
						{
							if (!Enum.TryParse(ReadString(), out distribution))
								throw new("FIXME");

							SkipWhitespace();
						}

						highValue = ReadFloat();
						SkipWhitespace();

						if (str[i++] != ']')
							throw new("FIXME");
					}
				}
				else // Scalar
				{
					lowValue = highValue = ReadFloat();
				}

				if (i < str.Length && str[i] == ',')
				{
					i++;
					time = ReadFloat() / 100.0f;
				}

				SkipWhitespace();

				// Curve type
				if (i < str.Length && str[i] is (>= 'A' and <= 'Z') or (>= 'a' and <= 'z'))
				{
					if (!Enum.TryParse(ReadString(), out curveType))
						throw new("FIXME");
				}

				var node = new PopParticlesTrackNode(time, lowValue, highValue, curveType, distribution);
				nodes.Add(node);
			}

			void SkipWhitespace()
			{
				while (i < str.Length && char.IsWhiteSpace(str[i]))
					i++;
			}

			float ReadFloat()
			{
				var start = i;
				while (i < str.Length && str[i] is (>= '0' and <= '9') or '.' or '-')
					i++;

				var str2 = str[start..i];

				var sepIndex = str2.IndexOf('.');

				if (sepIndex != -1)
				{
					var sepIndex2 = str2[(sepIndex + 1)..].IndexOf('.');

					if (sepIndex2 != -1)
						str2 = str2[..(sepIndex + 1 + sepIndex2)];
				}

				return float.Parse(str2);
			}

			string ReadString()
			{
				var start = i;
				while (i < str.Length && str[i] is (>= 'A' and <= 'Z') or (>= 'a' and <= 'z'))
					i++;

				return str[start..i];
			}
		}
		if (nodes.Count == 0)
			throw new("FIXME");

		// If the first node does not have a defined time, set it to 0
		if (float.IsNaN(nodes[0].Time))
			nodes[0].Time = 0;

		// If the last node does not have a defined time, set it to 1
		if (nodes.Count > 1 && float.IsNaN(nodes[^1].Time))
			nodes[^1].Time = 1;

		// Interpolate between nodes
		var last = 0.0f;
		var delta = 0.0f;

		for (var i = 0; i < nodes.Count; i++)
		{
			// Figure out next delta
			if (!float.IsNaN(nodes[i].Time))
			{
				last = nodes[i].Time;

				if (i != nodes.Count - 1)
				{
					// Find next node with set value
					var j = i + 1;
					while (float.IsNaN(nodes[j].Time))
						j++;

					// Absolute difference between the next defined point in time and now
					var absoluteDelta = nodes[j].Time - nodes[i].Time;

					// The number of undefined steps
					delta = last + absoluteDelta / (j - i);
				}

				last = nodes[i].Time;
				continue;
			}

			nodes[i].Time = last + delta;
			last = nodes[i].Time;
		}

		return new([..nodes]);
	}
}

using PopLib.Reanim;
using System.Text;
using System.Xml;

namespace PopLib.Particles;

public static class ParticlesXmlReader
{
	public static ParticlesEffect ReadFromStream(Stream stream)
	{
		using var streamReader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
		return ReadFromString(streamReader.ReadToEnd());
	}

	public static ParticlesEffect ReadFromString(string str)
	{
		using var stringReader = new StringReader("<root>" + str + "</root>");
		var settings = new XmlReaderSettings
		{
			IgnoreWhitespace = true
		};
		using var reader = XmlReader.Create(stringReader, settings);

		if (!reader.Read() || reader.NodeType != XmlNodeType.Element || reader.Name != "root")
			throw new("FIXME");

		var emitters = new List<ParticlesEmitter>();

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

	private static ParticlesEmitter ReadEmitter(XmlReader reader)
	{
		var randomLaunchSpin = false;
		var systemLoops = false;
		var particleLoops = false;
		var randomStartTime = false;
		var additive = false;
		var hardwareOnly = false;
		var image = "";
		var name = "";
		var systemDuration = "";
		var spawnMinActive = "";
		var spawnMaxLaunched = "";
		var particleDuration = "";
		var particleRed = "";
		var particleGreen = "";
		var particleBlue = "";
		var particleAlpha = "";
		var particleBrightness = "";
		var particleSpinAngle = "";
		var particleSpinSpeed = "";
		var particleScale = "";

		var emitter = new ParticlesEmitter();

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
						case "RandomLaunchSpin": randomLaunchSpin = ReadBoolParameter(reader, reader.Name); break;
						case "SystemLoops": systemLoops = ReadBoolParameter(reader, reader.Name); break;
						case "ParticleLoops": particleLoops = ReadBoolParameter(reader, reader.Name); break;
						case "RandomStartTime": randomStartTime = ReadBoolParameter(reader, reader.Name); break;
						case "Additive": additive = ReadBoolParameter(reader, reader.Name); break;
						case "HardwareOnly": hardwareOnly = ReadBoolParameter(reader, reader.Name); break;
						case "Image": image = ReadStringParameter(reader, reader.Name); break;
						case "Name": name = ReadStringParameter(reader, reader.Name); break;
						case "SystemDuration": systemDuration = ReadStringParameter(reader, reader.Name); break;
						case "SpawnMinActive": spawnMinActive = ReadStringParameter(reader, reader.Name); break;
						case "SpawnMaxLaunched": spawnMaxLaunched = ReadStringParameter(reader, reader.Name); break;
						case "ParticleDuration": particleDuration = ReadStringParameter(reader, reader.Name); break;
						case "ParticleRed": particleRed = ReadStringParameter(reader, reader.Name); break;
						case "ParticleGreen": particleGreen = ReadStringParameter(reader, reader.Name); break;
						case "ParticleBlue": particleBlue = ReadStringParameter(reader, reader.Name); break;
						case "ParticleAlpha": particleAlpha = ReadStringParameter(reader, reader.Name); break;
						case "ParticleBrightness": particleBrightness = ReadStringParameter(reader, reader.Name); break;
						case "ParticleSpinAngle": particleSpinAngle = ReadStringParameter(reader, reader.Name); break;
						case "ParticleSpinSpeed": particleSpinSpeed = ReadStringParameter(reader, reader.Name); break;
						case "ParticleScale": particleScale = ReadStringParameter(reader, reader.Name); break;
						default:
							throw new NotSupportedException($"Particles Emitter tag '{reader.Name}' not supported.");
					}
					break;
				default:
					throw new InvalidDataException($"Encountered unexpected node of type {reader.NodeType} while parsing particles Emitter.");
			}
		}
		end:

		emitter.RandomLaunchSpin = randomLaunchSpin;
		emitter.SystemLoops = systemLoops;
		emitter.ParticleLoops = particleLoops;
		emitter.RandomStartTime = randomStartTime;
		emitter.Additive = additive;
		emitter.HardwareOnly = hardwareOnly;
		emitter.Image = image;
		emitter.Name = name;
		emitter.SystemDuration = ParseFloatParameterTrack(systemDuration);
		emitter.SpawnMinActive = ParseFloatParameterTrack(spawnMinActive);
		emitter.SpawnMaxLaunched = ParseFloatParameterTrack(spawnMaxLaunched);
		emitter.ParticleDuration = ParseFloatParameterTrack(particleDuration);
		emitter.ParticleRed = ParseFloatParameterTrack(particleRed);
		emitter.ParticleGreen = ParseFloatParameterTrack(particleGreen);
		emitter.ParticleBlue = ParseFloatParameterTrack(particleBlue);
		emitter.ParticleAlpha = ParseFloatParameterTrack(particleAlpha);
		emitter.ParticleBrightness = ParseFloatParameterTrack(particleBrightness);
		emitter.ParticleSpinAngle = ParseFloatParameterTrack(particleSpinAngle);
		emitter.ParticleSpinSpeed = ParseFloatParameterTrack(particleSpinSpeed);
		emitter.ParticleScale = ParseFloatParameterTrack(particleScale);

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

	private static ParticlesFloatParameterTrack ParseFloatParameterTrack(string str)
	{
		str = str.Trim();
		
		if (str == "")
			return new([]);

		var nodes = new List<ParticlesFloatParameterTrackNode>();
		var hasTimes = new List<bool>();

		var i = 0;

		var curveType = ParticlesCurveType.Linear;
		var lowValue = 0f;
		var highValue = 0f;
		float? time = null;
		var low = false;
		var inRange = false;
		var inTime = false;

		while (i < str.Length)
		{
			switch (str[i])
			{
				case '[' or ']':
					inRange = !inRange;
					break;
				case ' ' or '\t':
					if (inRange)
						break;

					while (i + 1 < str.Length && str[i + 1] is ' ' or '\t')
						i++;

					if (i + 1 < str.Length && str[i + 1] is not (>= 'A' and <= 'Z') and not (>= 'a' and <= 'z'))
					{
						Add();
						curveType = ParticlesCurveType.Linear;
					}

					low = false;
					inTime = false;
					time = null;
					break;
				case ',':
					inTime = true;
					break;
				case >= '0' and <= '9' or '.' or '-':
					{
						var start = i;
						while (i + 1 < str.Length && str[i + 1] is >= '0' and <= '9' or '.' or '-')
							i++;

						var value = float.Parse(str[start..(i + 1)]);

						if (inTime)
						{
							time = value / 100.0f;
						}
						else if (!low)
						{
							lowValue = value;
							highValue = lowValue;
							low = true;
						}
						else
							highValue = value;
					}
					break;
				case (>= 'A' and <= 'Z') or (>= 'a' and <= 'z'):
					{
						var start = i;
						i += str[i..].IndexOf(' ') - 1;
						var value = str[start..(i + 1)];
						if (!Enum.TryParse<ParticlesCurveType>(value, out curveType))
							throw new("FIXME");
					}
					break;
				default:
					throw new($"FIXME: {str[i]}");
			}

			i++;
		}

		Add();

		var nodesArr = nodes.ToArray();

		for (var j = 0; j < nodesArr.Length; j++)
		{
			if (hasTimes[j])
				continue;

			nodesArr[j].Time = (j + 1) / nodesArr.Length;
		}

		return new(nodesArr);

		void Add()
		{
			nodes.Add(new()
			{
				LowValue = lowValue,
				HighValue = highValue,
				Time = time.GetValueOrDefault(),
				Distribution = ParticlesCurveType.Linear,
				CurveType = curveType
			});
			hasTimes.Add(time.HasValue);
		}
	}
}

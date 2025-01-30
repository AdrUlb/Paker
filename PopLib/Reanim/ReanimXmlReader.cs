﻿using System.Text;
using System.Xml;

namespace PopLib.Reanim;

public static class ReanimXmlReader
{
	public static ReanimAnimation ReadFromStream(Stream stream)
	{
		using var streamReader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
		var xmlString = streamReader.ReadToEnd();
		return ReadFromString(xmlString);
	}

	private static ReanimAnimation ReadFromString(string str)
	{
		float? fps = null;

		using var stringReader = new StringReader("<root>" + str + "</root>");
		var settings = new XmlReaderSettings()
		{
			IgnoreWhitespace = true
		};
		using var reader = XmlReader.Create(stringReader, settings);

		if (!reader.Read() || reader.NodeType != XmlNodeType.Element || reader.Name != "root")
			throw new("FIXME");

		var tracks = new List<ReanimTrack>();

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
						case "fps":
							if (!reader.Read() || reader.NodeType != XmlNodeType.Text)
								throw new("FIXME");

							fps = float.Parse(reader.Value);

							if (!reader.Read() || reader.NodeType != XmlNodeType.EndElement || reader.Name != "fps")
								throw new($"FIXME");

							break;
						case "track":
							tracks.Add(ReadTrack(reader));
							break;
						default:
							throw new NotSupportedException($"Reanim root tag {reader.Name} not supported.");
					}
					break;
				default:
					throw new InvalidDataException($"Encountered unexpected node of type {reader.NodeType} while parsing reanim animation.");
			}
		}
	end:

		if (fps == null)
			throw new("FIXME");

		return new(fps.Value, [.. tracks]);
	}

	private static ReanimTrack ReadTrack(XmlReader reader)
	{
		string? name = null;

		var transforms = new List<ReanimTransform>();

		while (reader.Read())
		{
			switch (reader.NodeType)
			{
				case XmlNodeType.EndElement:
					if (reader.Name != "track")
						throw new("FIXME");

					goto end;
				case XmlNodeType.Element:
					switch (reader.Name)
					{
						case "name":
							if (!reader.Read() || reader.NodeType != XmlNodeType.Text)
								throw new("FIXME");

							name = reader.Value;

							if (!reader.Read() || reader.NodeType != XmlNodeType.EndElement || reader.Name != "name")
								throw new($"FIXME");

							break;
						case "t":
							transforms.Add(ReadTransform(reader));
							break;
						default:
							throw new NotSupportedException($"Reanim track tag '{reader.Name}' not supported.");
					}
					break;
				default:
					throw new InvalidDataException($"Encountered unexpected node of type {reader.NodeType} while parsing reanim track.");
			}
		}
	end:

		if (name == null)
			throw new("FIXME");

		return new()
		{
			Name = name,
			Transforms = [.. transforms]
		};
	}

	private static ReanimTransform ReadTransform(XmlReader reader)
	{
		var x = ReanimTransform.DefaultFieldPlaceholder;
		var y = ReanimTransform.DefaultFieldPlaceholder;
		var skewX = ReanimTransform.DefaultFieldPlaceholder;
		var skewY = ReanimTransform.DefaultFieldPlaceholder;
		var scaleX = ReanimTransform.DefaultFieldPlaceholder;
		var scaleY = ReanimTransform.DefaultFieldPlaceholder;
		var frame = ReanimTransform.DefaultFieldPlaceholder;
		var alpha = ReanimTransform.DefaultFieldPlaceholder;
		string? imageName = null;

		while (reader.Read())
		{
			switch (reader.NodeType)
			{
				case XmlNodeType.EndElement:
					if (reader.Name != "t")
						throw new("FIXME");

					goto end;
				case XmlNodeType.Element:
					{
						var propName = reader.Name;

						if (!reader.Read() || reader.NodeType != XmlNodeType.Text)
							throw new("FIXME");

						switch (propName)
						{
							case "x": x = float.Parse(reader.Value); break;
							case "y": y = float.Parse(reader.Value); break;
							case "kx": skewX = float.Parse(reader.Value); break;
							case "ky": skewY = float.Parse(reader.Value); break;
							case "sx": scaleX = float.Parse(reader.Value); break;
							case "sy": scaleY = float.Parse(reader.Value); break;
							case "f": frame = float.Parse(reader.Value); break;
							case "a": alpha = float.Parse(reader.Value); break;
							case "i": imageName = reader.Value; break;
							default:
								throw new NotSupportedException($"Reanim transform tag '{reader.Name}' not supported.");
						}

						if (!reader.Read() || reader.NodeType != XmlNodeType.EndElement || reader.Name != propName)
							throw new($"FIXME");
						break;
					}
				default:
					throw new InvalidDataException($"Encountered unexpected node of type {reader.NodeType} while parsing reanim transform.");
			}
		}
	end:

		return new ReanimTransform()
		{
			X = x,
			Y = y,
			SkewX = skewX,
			SkewY = skewY,
			ScaleX = scaleX,
			ScaleY = scaleY,
			Frame = frame,
			Alpha = alpha,
			ImageName = imageName
		};
	}
}

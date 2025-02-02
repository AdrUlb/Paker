using PopLib.Reanim.Definition;
using System.Text;
using System.Xml;

namespace PopLib.Reanim.Serialization;

public static class ReanimXmlSerializer
{
	public static void Serialize(ReanimDefinition definition, Stream outputStream)
	{
		using var writer = new StreamWriter(outputStream, Encoding.UTF8, leaveOpen: true);

		var builder = new StringBuilder();
		Serialize(definition, builder);
		writer.Write(builder.ToString());
	}

	public static void Serialize(ReanimDefinition definition, StringBuilder outputStringBuilder)
	{
		outputStringBuilder.Append("<fps>").Append(definition.Fps).AppendLine("</fps>");

		foreach (var t in definition.Tracks)
			WriteTrack(t, outputStringBuilder);
	}

	private static void WriteTrack(ReanimTrackDefinition trackDefinition, StringBuilder builder)
	{
		builder.AppendLine("<track>").Append("<name>").Append(trackDefinition.Name).AppendLine("</name>");

		for (var i = 0; i < trackDefinition.Transforms.Length; i++)
			WriteTransform(trackDefinition.Transforms[i], builder);

		builder.AppendLine("</track>");
	}

	private static void WriteTransform(ReanimTransform transform, StringBuilder builder)
	{
		builder.Append("<t>");

		if (transform.X != ReanimTransform.DefaultFieldPlaceholder)
			builder.Append("<x>").Append(transform.X).Append("</x>");

		if (transform.Y != ReanimTransform.DefaultFieldPlaceholder)
			builder.Append("<y>").Append(transform.Y).Append("</y>");

		if (transform.SkewX != ReanimTransform.DefaultFieldPlaceholder)
			builder.Append("<kx>").Append(transform.SkewX).Append("</kx>");

		if (transform.SkewY != ReanimTransform.DefaultFieldPlaceholder)
			builder.Append("<ky>").Append(transform.SkewY).Append("</ky>");

		if (transform.ScaleX != ReanimTransform.DefaultFieldPlaceholder)
			builder.Append("<sx>").Append(transform.ScaleX).Append("</sx>");

		if (transform.ScaleY != ReanimTransform.DefaultFieldPlaceholder)
			builder.Append("<sy>").Append(transform.ScaleY).Append("</sy>");

		if (transform.Frame != ReanimTransform.DefaultFieldPlaceholder)
			builder.Append("<f>").Append(transform.Frame).Append("</f>");

		if (transform.Alpha != ReanimTransform.DefaultFieldPlaceholder)
			builder.Append("<a>").Append(transform.Alpha).Append("</a>");

		if (!string.IsNullOrEmpty(transform.ImageName))
			builder.Append("<i>").Append(transform.ImageName).Append("</i>");

		if (!string.IsNullOrEmpty(transform.FontName))
			builder.Append("<font>").Append(transform.ImageName).Append("</font>");

		if (!string.IsNullOrEmpty(transform.Text))
			builder.Append("<text>").Append(transform.ImageName).Append("</text>");

		builder.AppendLine("</t>");
	}

	public static ReanimDefinition Deserialize(Stream stream)
	{
		using var streamReader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
		return Deserialize(streamReader.ReadToEnd());
	}

	public static ReanimDefinition Deserialize(string str)
	{
		using var stringReader = new StringReader("<root>" + str + "</root>");
		var settings = new XmlReaderSettings
		{
			IgnoreWhitespace = true
		};
		using var reader = XmlReader.Create(stringReader, settings);

		if (!reader.Read() || reader.NodeType != XmlNodeType.Element || reader.Name != "root")
			throw new("FIXME");

		float? fps = null;
		var tracks = new List<ReanimTrackDefinition>();

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

		return new(fps.Value, [..tracks]);
	}

	private static ReanimTrackDefinition ReadTrack(XmlReader reader)
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

		var track = new ReanimTrackDefinition(name, [..transforms]);
		track.ReplaceTransformsPlaceholders();
		return track;
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
		string? fontName = null;
		string? text = null;

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

						if (!reader.Read())
							throw new("FIXME");

						if (reader.NodeType == XmlNodeType.EndElement && reader.Name == propName)
						{
							imageName = null;
							break;
						}

						if (reader.NodeType != XmlNodeType.Text)
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
							case "font": fontName = reader.Value; break;
							case "text": text = reader.Value; break;
							default:
								throw new NotSupportedException($"Reanim transform property '{propName}' not supported.");
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

		return new ReanimTransform(x, y, skewX, skewY, scaleX, scaleY, frame, alpha)
		{
			ImageName = imageName,
			FontName = fontName,
			Text = text
		};
	}
}

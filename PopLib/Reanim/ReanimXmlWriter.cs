using System.Text;

namespace PopLib.Reanim;

public static class ReanimXmlWriter
{
	public static void WriteToStream(in ReanimAnimation animation, Stream stream)
	{
		using var writer = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true);

		var builder = new StringBuilder();
		WriteToStringBuilder(animation, builder);
		writer.Write(builder.ToString());
	}

	public static void WriteToStringBuilder(in ReanimAnimation animation, StringBuilder builder)
	{
		builder.Append("<fps>").Append(animation.Fps).AppendLine("</fps>");

		for (var i = 0; i < animation.Tracks.Length; i++)
			WriteTrack(animation.Tracks[i], builder);
	}

	private static void WriteTrack(in ReanimTrack track, StringBuilder builder)
	{
		builder.AppendLine("<track>").Append("<name>").Append(track.Name).AppendLine("</name>");

		for (var i = 0; i < track.Transforms.Length; i++)
			WriteTransform(track.Transforms[i], builder);

		builder.AppendLine("</track>");
	}

	private static void WriteTransform(in ReanimTransform transform, StringBuilder builder)
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
			throw new NotImplementedException();

		if (!string.IsNullOrEmpty(transform.Text))
			throw new NotImplementedException();

		builder.AppendLine("</t>");
	}
}

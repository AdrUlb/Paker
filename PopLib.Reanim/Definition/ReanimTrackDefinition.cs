namespace PopLib.Reanim.Definition;

public sealed class ReanimTrackDefinition(string name, ReanimTransform[] transforms)
{
	public readonly string Name = name;
	public readonly ReanimTransform[] Transforms = transforms;

	public void ReplaceTransformsPlaceholders()
	{
		var prevX = 0.0f;
		var prevY = 0.0f;
		var prevSkewX = 0.0f;
		var prevSkewY = 0.0f;
		var prevScaleX = 1.0f;
		var prevScaleY = 1.0f;
		var prevFrame = 0.0f;
		var prevAlpha = 1.0f;
		string? prevImageName = null;
		string? prevFontName = null;
		string? prevText = null;

		for (var i = 0; i < Transforms.Length; i++)
		{
			ref var transform = ref Transforms[i];
			ReplaceTransformsPlaceholderField(ref transform.X, ref prevX);
			ReplaceTransformsPlaceholderField(ref transform.Y, ref prevY);
			ReplaceTransformsPlaceholderField(ref transform.SkewX, ref prevSkewX);
			ReplaceTransformsPlaceholderField(ref transform.SkewY, ref prevSkewY);
			ReplaceTransformsPlaceholderField(ref transform.ScaleX, ref prevScaleX);
			ReplaceTransformsPlaceholderField(ref transform.ScaleY, ref prevScaleY);
			ReplaceTransformsPlaceholderField(ref transform.Frame, ref prevFrame);
			ReplaceTransformsPlaceholderField(ref transform.Alpha, ref prevAlpha);
			ReplaceTransformsPlaceholderField(ref transform.ImageName, ref prevImageName);
			ReplaceTransformsPlaceholderField(ref transform.FontName, ref prevFontName);
			ReplaceTransformsPlaceholderField(ref transform.Text, ref prevText);
		}
	}

	private void ReplaceTransformsPlaceholderField(ref float value, ref float other)
	{
		if (value == ReanimTransform.DefaultFieldPlaceholder)
			value = other;
		else
			other = value;
	}

	void ReplaceTransformsPlaceholderField(ref string? value, ref string? other)
	{
		if (string.IsNullOrEmpty(value))
			value = other;
		else
			other = value;
	}
}

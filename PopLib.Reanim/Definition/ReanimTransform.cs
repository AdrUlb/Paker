namespace PopLib.Reanim.Definition;

public struct ReanimTransform(float x, float y, float skewX, float skewY, float scaleX, float scaleY, float frame, float alpha)
{
	public const float DefaultFieldPlaceholder = -10000.0f;

	public float X = x;
	public float Y = y;
	public float SkewX = skewX;
	public float SkewY = skewY;
	public float ScaleX = scaleX;
	public float ScaleY = scaleY;
	public float Frame = frame;
	public float Alpha = alpha;

	public string? ImageName = null;
	public string? FontName = null;
	public string? Text = null;
}

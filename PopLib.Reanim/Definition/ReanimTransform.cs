namespace PopLib.Reanim.Definition;

public sealed class ReanimTransform
{
	public const float DefaultFieldPlaceholder = -10000.0f;

	public float X;
	public float Y;
	public float SkewX;
	public float SkewY;
	public float ScaleX;
	public float ScaleY;
	public float Frame;
	public float Alpha;

	public string? ImageName;
	public string? FontName;
	public string? Text;
}

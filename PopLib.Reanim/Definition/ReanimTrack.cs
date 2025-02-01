namespace PopLib.Reanim.Definition;

public sealed class ReanimTrack(string name, ReanimTransform[] transforms)
{
	public readonly string Name = name;
	public readonly ReanimTransform[] Transforms = transforms;
}

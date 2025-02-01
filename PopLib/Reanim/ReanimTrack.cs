namespace PopLib.Reanim;

public sealed class ReanimTrack(string name, ReanimTransform[] transforms)
{
	public readonly string Name = name;
	public readonly ReanimTransform[] Transforms = transforms;
}

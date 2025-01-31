namespace PopLib.Reanim;

public readonly struct ReanimTrack(string name, ReanimTransform[] transforms)
{
	public readonly string Name = name;
	public readonly ReanimTransform[] Transforms = transforms;
}

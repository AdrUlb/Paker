namespace PopLib.Reanim.Definition;

public sealed class ReanimDefinition(float fps, ReanimTrackDefinition[] tracks)
{
	public readonly float Fps = fps;
	public readonly ReanimTrackDefinition[] Tracks = tracks;
}

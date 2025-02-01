namespace PopLib.Reanim;

public sealed class ReanimAnimation(float fps, ReanimTrack[] tracks)
{
	public readonly float Fps = fps;
	public readonly ReanimTrack[] Tracks = tracks;
}

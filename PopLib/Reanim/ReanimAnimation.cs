namespace PopLib.Reanim;

public readonly struct ReanimAnimation(float fps, ReanimTrack[] tracks)
{
	public readonly float Fps = fps;
	public readonly ReanimTrack[] Tracks = tracks;
}

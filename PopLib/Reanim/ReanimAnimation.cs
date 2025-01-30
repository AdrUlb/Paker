namespace PopLib.Reanim;

public struct ReanimAnimation(float fps, ReanimTrack[] tracks)
{
	public float Fps = fps;
	public ReanimTrack[] Tracks = tracks;
}

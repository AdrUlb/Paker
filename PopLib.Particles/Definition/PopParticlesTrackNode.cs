namespace PopLib.Particles.Definition;

public sealed class PopParticlesTrackNode(float time, float lowValue, float highValue, PopParticlesCurveType curveType, PopParticlesCurveType distribution)
{
	public float Time = time;
	public readonly float LowValue = lowValue;
	public readonly float HighValue = highValue;
	public readonly PopParticlesCurveType CurveType = curveType;
	public readonly PopParticlesCurveType Distribution = distribution;
}

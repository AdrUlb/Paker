namespace PopLib.Particles;

public sealed class ParticlesFloatParameterTrackNode(float time, float lowValue, float highValue, ParticlesCurveType curveType, ParticlesCurveType distribution)
{
	public float Time = time;
	public readonly float LowValue = lowValue;
	public readonly float HighValue = highValue;
	public readonly ParticlesCurveType CurveType = curveType;
	public readonly ParticlesCurveType Distribution = distribution;
}

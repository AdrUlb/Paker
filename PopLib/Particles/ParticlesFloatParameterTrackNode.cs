namespace PopLib.Particles;

public class ParticlesFloatParameterTrackNode(float time, float lowValue, float highValue, ParticlesCurveType curveType, ParticlesCurveType distribution)
{
	public float Time = time;
	public float LowValue = lowValue;
	public float HighValue = highValue;
	public ParticlesCurveType CurveType = curveType;
	public ParticlesCurveType Distribution = distribution;
}

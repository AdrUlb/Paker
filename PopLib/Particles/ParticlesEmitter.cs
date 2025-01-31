namespace PopLib.Particles;

public struct ParticlesEmitter
{
	public bool RandomLaunchSpin;
	public bool SystemLoops;
	public bool ParticleLoops;
	public bool RandomStartTime;
	public bool Additive;
	public bool HardwareOnly;
	public string Image;
	public string Name;
	public ParticlesFloatParameterTrack SystemDuration;
	public ParticlesFloatParameterTrack SpawnMinActive;
	public ParticlesFloatParameterTrack SpawnMaxLaunched;
	public ParticlesFloatParameterTrack ParticleDuration;
	public ParticlesFloatParameterTrack ParticleRed;
	public ParticlesFloatParameterTrack ParticleGreen;
	public ParticlesFloatParameterTrack ParticleBlue;
	public ParticlesFloatParameterTrack ParticleAlpha;
	public ParticlesFloatParameterTrack ParticleBrightness;
	public ParticlesFloatParameterTrack ParticleSpinAngle;
	public ParticlesFloatParameterTrack ParticleSpinSpeed;
	public ParticlesFloatParameterTrack ParticleScale;
}

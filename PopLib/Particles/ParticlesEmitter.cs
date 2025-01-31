namespace PopLib.Particles;

public class ParticlesEmitter()
{
	public const int DefaultImageFrames = 1;
	public const ParticlesEmitterType DefaultEmitterType = ParticlesEmitterType.Box;
	
	public int ImageFrames = DefaultImageFrames;
	public bool RandomLaunchSpin;
	public bool SystemLoops;
	public bool ParticleLoops;
	public bool ParticlesDontFollow;
	public bool RandomStartTime;
	public bool Additive;
	public bool HardwareOnly;
	public ParticlesEmitterType EmitterType = DefaultEmitterType;
	public ParticlesField[]? Fields = null;
	public string? Image;
	public string? Name;
	public ParticlesFloatParameterTrack? SystemDuration = null;
	public ParticlesFloatParameterTrack? CrossfadeDuration = null;
	public ParticlesFloatParameterTrack? SpawnRate = null;
	public ParticlesFloatParameterTrack? SpawnMinActive = null;
	public ParticlesFloatParameterTrack? SpawnMaxLaunched = null;
	public ParticlesFloatParameterTrack? EmitterRadius = null;
	public ParticlesFloatParameterTrack? EmitterOffsetX = null;
	public ParticlesFloatParameterTrack? EmitterOffsetY = null;
	public ParticlesFloatParameterTrack? EmitterBoxX = null;
	public ParticlesFloatParameterTrack? EmitterBoxY = null;
	public ParticlesFloatParameterTrack? ParticleDuration = null;
	public ParticlesFloatParameterTrack? LaunchSpeed = null;
	public ParticlesFloatParameterTrack? LaunchAngle = null;
	public ParticlesFloatParameterTrack? ParticleRed = null;
	public ParticlesFloatParameterTrack? ParticleGreen = null;
	public ParticlesFloatParameterTrack? ParticleBlue = null;
	public ParticlesFloatParameterTrack? ParticleAlpha = null;
	public ParticlesFloatParameterTrack? ParticleBrightness = null;
	public ParticlesFloatParameterTrack? ParticleSpinAngle = null;
	public ParticlesFloatParameterTrack? ParticleSpinSpeed = null;
	public ParticlesFloatParameterTrack? ParticleScale = null;
}

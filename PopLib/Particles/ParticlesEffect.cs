namespace PopLib.Particles;

public struct ParticlesEffect(ParticlesEmitter[] emitters)
{
	public readonly ParticlesEmitter[] Emitters = emitters;
}

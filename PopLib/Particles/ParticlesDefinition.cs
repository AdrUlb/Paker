namespace PopLib.Particles;

public sealed class ParticlesDefinition(ParticlesEmitter[] emitters)
{
	public readonly ParticlesEmitter[] Emitters = emitters;
}

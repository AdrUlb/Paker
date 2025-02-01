namespace PopLib.Particles.Definition;

public sealed class PopParticlesDefinition(PopParticlesEmitter[] emitters)
{
	public readonly PopParticlesEmitter[] Emitters = emitters;
}

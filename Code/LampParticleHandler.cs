using Sandbox;
using static Sandbox.PhysicsContact;

public sealed class LampParticleHandler : Component
{
	[Property]
	private BoxCollider ColliderComponent { get; set; }

	[Property]
	private ParticleEmitter ParticleEmitter { get; set; }

	protected override void OnAwake()
	{
		ColliderComponent.OnTriggerEnter = ( Collider other ) =>
		{
			if ( other.Tags.Has( "car" ) && other.Tags.Has( "player" ) )
			{
				ParticleEmitter.Enabled = true;
				Log.Info( "RAWR" );
			}
		};
	}

}

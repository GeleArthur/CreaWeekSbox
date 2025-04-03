using Meteor.VehicleTool.Vehicle;
using Sandbox;
using static Sandbox.PhysicsContact;

public sealed class LampParticleHandler : Component
{
	[Property]
	private BoxCollider ColliderComponent { get; set; }

	[Property]
	private ParticleEmitter ParticleEmitter { get; set; }

	[Property] private PointLight Light { get; set; }
	private VehicleController _playerController;
	
	protected override void OnAwake()
	{
		_playerController = Game.ActiveScene.GetAllComponents<VehicleController>().First();
		ColliderComponent.OnTriggerEnter = ( Collider other ) =>
		{
			if ( other.Tags.Has( "car" ) && other.Tags.Has( "player" ) )
			{
				ParticleEmitter.Enabled = true;
			}
		};
	}

	protected override void OnUpdate()
	{
		if ( (_playerController.WorldPosition - WorldPosition).LengthSquared > 7000*7000 )
		{
			Light.Enabled = false;
		}
		else
		{
			Light.Enabled = true;
		}
	}
}

using Meteor.VehicleTool.Vehicle;
using Sandbox;
using System;

public sealed class OnCollideRagDoll : Component, Component.ICollisionListener
{
	private ModelPhysics _ragdollPhysics;
	private Rigidbody _rigidbody;
	private CapsuleCollider _modelCollider;
	private SkinnedModelRenderer _skinnedModelRenderer;
	
	

	private NavMeshAgent _agent;
	private PlayerController _player;
	private VehicleController _car;

	private TimeUntil _wanderTimer;
	private TimeUntil _damageTimer;
	private TimeUntil _ragdollTimer;

	private TimeUntil _zombiesKillEachOther;

	protected override void OnStart()
	{
		_car = Game.ActiveScene.GetAllComponents<VehicleController>().First();
		_skinnedModelRenderer = GetComponent<SkinnedModelRenderer>();
		_ragdollPhysics = GetComponent<ModelPhysics>();
		_rigidbody = GetComponent<Rigidbody>();
		_modelCollider = GetComponent<CapsuleCollider>();
		_agent = GetComponent<NavMeshAgent>();
		_ragdollPhysics.Enabled = false;
		_rigidbody.Enabled = true;
		_modelCollider.Enabled = true;
		_zombiesKillEachOther = 0.5f;

		
		_agent.MoveTo( WorldPosition );

		_player = Game.ActiveScene.GetAllComponents<PlayerController>().First();
	}
	public void OnCollisionStart( Collision collision )
	{
		if ( collision.Other.GameObject.Tags.Contains( "car" ) || 
		     (collision.Other.GameObject.Tags.Contains( "zombie" ) && _zombiesKillEachOther )
		     )
		{
			if ( collision.Contact.Speed.Length < 250 ) return;
			Ragdoll();
		}
		// _ragdollPhysics.PhysicsGroup.Velocity = collision.Contact.Speed * 1.1f;
	}
	public void Ragdoll()
	{
		_ragdollTimer = 10;
		_ragdollPhysics.Enabled = true;
		_rigidbody.Enabled = false;
		_modelCollider.Enabled = false;
	}
	public void UnRagdoll()
	{
		_ragdollPhysics.Enabled = false;
		_rigidbody.Enabled = true;
		_modelCollider.Enabled = true;
	}
	private void HitHealth( HealthComponent health )
	{
		if ( _damageTimer )
		{
			health.Damage( 10 );
			_damageTimer = 2;
		}
	}

	protected override void OnUpdate()
	{
		if ( !_agent.IsValid || !_rigidbody.IsValid ) return;

		bool success = false;
		
		if ( _player.GameObject.Active )
		{
			if ( Vector3.DistanceBetween( WorldPosition, _player.WorldPosition ) < 500 )
			{
				success = true;
				_wanderTimer = 0;
				_agent.MoveTo( _player.WorldPosition );
			}
		}
		else if( Vector3.DistanceBetween( WorldPosition, _car.WorldPosition ) < 500 )
		{
			success = true;
			_wanderTimer = 0;
			_agent.MoveTo( _car.WorldPosition );
		}

		if ( !success && _wanderTimer )
		{
			// Vector3 randomPos = Vector3.Random * Random.Shared.Float( 150, 300 ); ;
			// randomPos = Scene.NavMesh.GetClosestPoint( randomPos )!.Value;
			// _agent.MoveTo( LocalPosition + randomPos );
			// _wanderTimer = 5 + Random.Shared.Float( 0, 3 );
		}

		TurnTowards();

		if ( Vector3.DistanceBetween( WorldPosition, _player.WorldPosition ) < 100 )
		{
			HitHealth( _player.GetComponent<HealthComponent>() );
		}
		else if ( Vector3.DistanceBetween( WorldPosition, _car.WorldPosition ) < 100 )
		{
			HealthComponent health = _car.GetComponent<HealthComponent>();
			if ( health != null )
				HitHealth( _car.GetComponent<HealthComponent>() );
		}
		if ( _ragdollTimer )
		{
			UnRagdoll();
		}

	}
	private void TurnTowards()
	{
		if ( _agent.TargetPosition.HasValue )
		{
			var direction = _agent.TargetPosition.Value - WorldPosition;
			direction = direction / direction.Length;
			var angle = Math.Atan2( direction.y, direction.x );
			angle = angle / double.Pi * 180;

			Rotation rotation = _agent.WorldRotation;
			var angles = rotation.Angles();
			angles.yaw = (float)angle;
			var delta = Time.Delta;
			_rigidbody.LocalRotation = angles;
		}
		

	}

	protected override void OnFixedUpdate()
	{
		_agent.SetAgentPosition( WorldPosition );
		_rigidbody.Velocity = _agent.Velocity;
		// magic value 200f, can be replaced with a speed variable
		// if ( _agent.TargetPosition.HasValue )
		// {
		// 	if ( Vector3.DistanceBetweenSquared( _rigidbody.WorldPosition, _agent.TargetPosition.Value ) >= 2500 )
		// 	{
		// 		Vector3 direction = new Vector3( _agent.TargetPosition.Value.x, _agent.TargetPosition.Value.y, 0 );
		// 		Vector3 gravity = new Vector3( 0, 0, _rigidbody.Velocity.z );
		// 		Vector3 localPos = new Vector3( _rigidbody.LocalPosition.x, _rigidbody.LocalPosition.y, 0 );
		// 		_rigidbody.Velocity = gravity + (direction - localPos).Normal * 200f;
		// 	}
		// }
	}
}

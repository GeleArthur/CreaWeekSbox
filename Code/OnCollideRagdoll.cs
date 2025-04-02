using Sandbox;
using System;

public sealed class OnCollideRagDoll : Component, Component.ICollisionListener
{
	private ModelPhysics _ragdollPhysics;
	private Rigidbody _rigidbody;
	private ModelCollider _modelCollider;

	private NavMeshAgent _agent;
	private PlayerController _player;

	private TimeUntil _wanderTimer;
	private TimeUntil _damageTimer;
	private TimeUntil _ragdollTimer;

	protected override void OnStart()
	{
		_ragdollPhysics = GetComponent<ModelPhysics>();
		_rigidbody = GetComponent<Rigidbody>();
		_modelCollider = GetComponent<ModelCollider>();
		_agent = GetComponent<NavMeshAgent>();
		_ragdollPhysics.Enabled = false;
		_rigidbody.Enabled = true;
		_modelCollider.Enabled = true;

		_player = Game.ActiveScene.GetAllComponents<PlayerController>().First();
	}
	public void OnCollisionStart( Collision collision )
	{
		if ( collision.Other.GameObject.Tags.Contains( "car" ) || collision.Other.GameObject.Tags.Contains( "zombie" ) )
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
	private void GotHit()
	{
		if ( _damageTimer )
		{
			_player.GetComponent<HealthComponent>()?.Damage( 10 );
			_damageTimer = 2;
		}
	}

	protected override void OnUpdate()
	{
		if ( !_agent.IsValid || !_rigidbody.IsValid ) return;
		//_rigidbody.ApplyForce(_agent.WishVelocity);

		if ( Vector3.DistanceBetween( WorldPosition, _player.WorldPosition ) < 500 )
		{
			_agent.MoveTo( _player.WorldPosition );
			if ( _agent.TargetPosition.HasValue )
			{
				TurnTowards( _agent.TargetPosition.Value );
			}
		}
		else
		{
			if ( _wanderTimer )
			{
				Vector3 randomPos = Vector3.Random * Random.Shared.Float( 150, 300 ); ;
				randomPos = Scene.NavMesh.GetClosestPoint( randomPos )!.Value;
				_agent.MoveTo( LocalPosition + randomPos );
				if( _agent.TargetPosition.HasValue )
					TurnTowards( _agent.TargetPosition.Value );
				_wanderTimer = 5 + Random.Shared.Float( 0, 3 );
			}
		}
		if ( Vector3.DistanceBetween( WorldPosition, _player.WorldPosition ) < 100 )
		{
			GotHit();
		}
		if ( _ragdollTimer )
		{
			UnRagdoll();
		}
	}
	private void TurnTowards(Vector3 point)
	{
		var direction = point - WorldPosition;
		direction = direction / direction.Length;
		var angle = Math.Atan2( direction.y, direction.x );
		angle = angle / double.Pi * 180;

		Rotation rotation = _agent.WorldRotation;
		var angles = rotation.Angles();
		angles.yaw = (float)angle;
		var delta = Time.Delta;
		_rigidbody.LocalRotation = angles;
	}

	protected override void OnFixedUpdate()
	{
		if ( _agent.TargetPosition.HasValue )
			_rigidbody.Velocity = _agent.TargetPosition.Value - _rigidbody.LocalPosition;
	}
}

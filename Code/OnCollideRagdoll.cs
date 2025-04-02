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
			_player.GetComponent<HealthComponent>().Damage( 10 );
			_damageTimer = 2;
		}
	}

	protected override void OnUpdate()
	{
		if ( !_agent.IsValid || !_rigidbody.IsValid ) return;
		//_rigidbody.ApplyForce(_agent.WishVelocity);

		if ( Vector3.DistanceBetween( WorldPosition, _player.WorldPosition ) < 500 )
		{
			var direction = _player.WorldPosition - WorldPosition;
			direction = direction / direction.Length;
			var angle = Math.Atan2( direction.y, direction.x );
			angle = angle / double.Pi * 180;

			_agent.MoveTo( _player.WorldPosition );
			Rotation rotation = _agent.WorldRotation;
			var angles = rotation.Angles();
			angles.yaw = (float)angle;
			var delta = Time.Delta;
			//_rigidbody.ApplyTorque( new Vector3(0, 10, 0) );
			_rigidbody.LocalRotation = angles;
			//_rigidbody.SmoothRotate( in rotation, 0.5f, delta );
		}
		else
		{
			if ( _wanderTimer )
			{
				Vector3 randomPos = Vector3.Random * Random.Shared.Float( 1, 20 ); ;
				randomPos = Scene.NavMesh.GetClosestPoint( randomPos )!.Value;
				_agent.MoveTo( LocalPosition + randomPos );
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
	protected override void OnFixedUpdate()
	{
		_rigidbody.Velocity = (Vector3)_agent.TargetPosition - _rigidbody.LocalPosition;
	}
}

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

	protected override void OnStart()
	{
		_damageTimer = 2;

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
		if ( collision.Other.GameObject.Tags.Contains( "player" ) )
		{
			GotHit();
		}
		else if ( collision.Other.GameObject.Tags.Contains( "car" ) || collision.Other.GameObject.Tags.Contains( "zombie" ) )
		{
			if ( collision.Contact.Speed.Length < 250 ) return;
			Ragdoll();
		}
		// _ragdollPhysics.PhysicsGroup.Velocity = collision.Contact.Speed * 1.1f;
	}
	public void Ragdoll()
	{
		_ragdollPhysics.Enabled = true;
		_rigidbody.Enabled = false;
		_modelCollider.Enabled = false;
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
		_rigidbody.Velocity = _agent.Velocity;

		if ( Vector3.DistanceBetween( WorldPosition, _player.WorldPosition ) < 500 )
		{
			_agent.MoveTo( _player.WorldPosition );
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
	}
}

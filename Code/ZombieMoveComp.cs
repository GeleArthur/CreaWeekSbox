using Sandbox;
using System;

public sealed class ZombieMoveComp : Component, Component.ICollisionListener
{
	private GameObject _target;
	[Property]
	private float _chaseRadius = 0;
	[Property]
	private float _alertRadius = 0;
	[Property]
	private float _wanderSize = 100;
	
	private HealthComponent _health;
	private float _timeTillDelete = 7f;
	private SphereCollider _chaseRange;
	private SphereCollider _alertRange;
	private BoxCollider _wanderRange;
	private NavMeshAgent _agent;
	private bool _isChasing = false;
	private float _wanderTimer = 0;
	private readonly float _wanderMaxTime = 5;
	private readonly float _wanderTimeOffset = 2;
	private ModelPhysics _modelPhysics;

	public void OnCollisionStart( Collision collision )
	{
		if ( collision.Other.GameObject.Tags.Contains( "player" ) )
		{
			HealthComponent playerHealth = collision.Other.GameObject.GetComponent<HealthComponent>();
			playerHealth.Damage( 10 );
		}
		else if ( collision.Other.GameObject.Tags.Contains( "car" ) )
		{
			Ragdoll( collision.Contact.Speed );
		}
	}

	protected override void OnStart()
	{
		_modelPhysics = GameObject.GetComponent<ModelPhysics>();
		_health = GameObject.GetComponent<HealthComponent>();
		_health.Heal(100);
		_health.OnDeath += OnDeathRagdoll;
		_chaseRange = GameObject.AddComponent<SphereCollider>();
		_chaseRange.Radius = _chaseRadius;
		_chaseRange.IsTrigger = true;
		_chaseRange.OnTriggerExit += ( other ) =>
		{
			if ( other.Tags.Contains( "player" ) )
			{
				_target = null;
				_isChasing = false;
			}
		};
		_alertRange = GameObject.AddComponent<SphereCollider>();
		_alertRange.Radius = _alertRadius;
		_alertRange.IsTrigger = true;
		_alertRange.OnTriggerEnter += ( other ) =>
		{
			if ( other.Tags.Contains( "player" ) )
			{
				_target = other.GameObject;
				_isChasing = true;
			}
		};
		_wanderRange = GameObject.AddComponent<BoxCollider>();
		_wanderRange.Scale = new Vector3( _wanderSize, _wanderSize, 20 );
		_wanderRange.LocalTransform = LocalTransform;
		_wanderRange.IsTrigger = true;
		_agent = GameObject.GetComponent<NavMeshAgent>();

	}
	
	protected override void OnUpdate()
	{
		if( _health.Health <= 0 )
		{
			_timeTillDelete -= Time.Delta;
			if( _timeTillDelete <= 0 )
			{
				GameObject.Destroy();
			}
			return;
		}


		if ( _isChasing )
		{
			_agent.MoveTo( _target.WorldPosition );
		}
		else
		{
			Wander();
		}
	}
	private void Wander()
	{
		if ( _wanderTimer <= 0 )
		{
			Vector3 randomPos = _wanderRange.LocalBounds.RandomPointInside;
			randomPos = Scene.NavMesh.GetClosestPoint( randomPos )!.Value;
			_agent.MoveTo( LocalPosition + randomPos );
			_wanderTimer = _wanderMaxTime + Random.Shared.Float( -_wanderTimeOffset, _wanderTimeOffset );
		}
		else
		{
			_wanderTimer -= Time.Delta;
		}
	}

	private void Ragdoll( Vector3 direction )
	{
		_modelPhysics.MotionEnabled = true;
		_modelPhysics.PhysicsGroup.Velocity = direction * 1.1f;
	}
	private void OnDeathRagdoll()
	{
		//Ragdoll(new Vector3(0,0,0));
	}
}

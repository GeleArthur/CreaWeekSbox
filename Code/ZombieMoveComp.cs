using Sandbox;
using System;

public sealed class ZombieMoveComp : Component, Component.ICollisionListener
{
	private GameObject _target;
	
	[Property]
	private float ChaseRadius { get; set; }
	[Property]
	private float AlertRadius { get; set; }
	[Property]
	private float WanderSize { get; set; }
	
	// private HealthComponent _health;
	private TimeUntil _timeTillDelete;
	private SphereCollider _chaseRange;
	private SphereCollider _alertRange;
	private BoxCollider _wanderRange;
	private NavMeshAgent _agent;
	private bool _isChasing = false;
	private float _wanderTimer = 0;
	private readonly float _wanderMaxTime = 5;
	private readonly float _wanderTimeOffset = 2;

	private PlayerController _player;
	
	private ModelPhysics _ragdollPhysics;
	private Rigidbody _rigidbody;
	private ModelCollider _modelCollider;
	private bool _ragdolling;

	public void OnCollisionStart( Collision collision )
	{
		if ( collision.Contact.Speed.Length > 250 )
		{
			Ragdoll( collision.Contact.Speed );
		}
	}

	protected override void OnStart()
	{
		_ragdollPhysics =  GetComponent<ModelPhysics>();
		_rigidbody = GetComponent<Rigidbody>();
		_modelCollider = GetComponent<ModelCollider>();
		_ragdollPhysics.Enabled = false;
		_rigidbody.Enabled = true;
		_modelCollider.Enabled = true;
		_agent = GameObject.GetComponent<NavMeshAgent>();
		_player = Game.ActiveScene.GetAllComponents<PlayerController>().First();
		
		_chaseRange = GameObject.AddComponent<SphereCollider>();
		_chaseRange.Radius = ChaseRadius;
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
		_alertRange.Radius = AlertRadius;
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
		_wanderRange.Scale = new Vector3( WanderSize, WanderSize, 20 );
		_wanderRange.LocalTransform = LocalTransform;
		_wanderRange.IsTrigger = true;

	}
	
	protected override void OnUpdate()
	{
		if ( _ragdolling )
		{
			if ( _timeTillDelete )
				GameObject.Destroy();
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

	protected override void OnFixedUpdate()
	{
		if(_ragdolling) return;
		_rigidbody.Velocity = _agent.Velocity;
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
		_ragdollPhysics.Enabled = true;
		_rigidbody.Enabled = false;
		_modelCollider.Enabled = false;
		_agent.Enabled = false;
		_ragdollPhysics.PhysicsGroup.Velocity = direction * 1.1f;
		_timeTillDelete = 7f;
		_ragdolling = true;
	}
}

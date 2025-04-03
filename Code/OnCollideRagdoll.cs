using Meteor.VehicleTool.Vehicle;
using Sandbox;
using System;

public sealed class OnCollideRagDoll : Component, Component.ICollisionListener
{
	private ModelPhysics _ragdollPhysics;
	private Rigidbody _rigidbody;
	private CapsuleCollider _modelCollider;
	private HealthComponent _health;

	private NavMeshAgent _agent;
	private PlayerController _player;
	private VehicleController _car;

	private TimeUntil _wanderTimer;
	private TimeUntil _damageTimer;
	private TimeUntil _ragdollTimer;

	private TimeUntil _zombiesKillEachOther;

	[Property] private float _killDistance = 5000000f;

	protected override void OnStart()
	{
		_car = Game.ActiveScene.GetAllComponents<VehicleController>().First();
		_ragdollPhysics = GetComponent<ModelPhysics>();
		_rigidbody = GetComponent<Rigidbody>();
		_modelCollider = GetComponent<CapsuleCollider>();
		_agent = GetComponent<NavMeshAgent>();
		_health = GetComponent<HealthComponent>();
		_ragdollPhysics.Enabled = false;
		_rigidbody.Enabled = true;
		_modelCollider.Enabled = true;
		_zombiesKillEachOther = 0.5f;
		
		_agent.MoveTo( WorldPosition );
		_agent.MaxSpeed = 200;
		_agent.Acceleration = 2000;
		
		_player = Game.ActiveScene.GetAllComponents<ZombieManager>().First().Player;
		_health.OnDeath += OnDeath;
	}

	private void OnDeath()
	{
		Ragdoll();
	}

	public void OnCollisionStart( Collision collision )
	{
		if( collision.Other.GameObject.Tags.Contains( "car" ) || 
		     (collision.Other.GameObject.Tags.Contains( "zombie" ) && _zombiesKillEachOther )
		     )
		{
			if ( collision.Contact.Speed.Length < 250 ) return;
			_health.Damage( 300 );
		}
	}
	
	public void Ragdoll()
	{
		_ragdollTimer = 10;
		_ragdollPhysics.Enabled = true;
		_rigidbody.Enabled = false;
		_modelCollider.Enabled = false;
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
		if ( Vector3.DistanceBetween( _car.WorldPosition, this.WorldPosition ) > _killDistance )
		{
			this.GameObject.Destroy();
			return;
		}

		if ( _health.IsDeath && _ragdollTimer )
		{
			GameObject.Destroy();
			Game.ActiveScene.GetAllComponents<ZombieManager>().First().RemoveZombie( this );
			return;
		}
		
		if(_health.IsDeath) return;
		
		if ( _agent.IsValid && _rigidbody.IsValid )
		{
			bool success = false;

			if ( _player.GameObject.Active )
			{
				// if ( Vector3.DistanceBetween( WorldPosition, _player.WorldPosition ) < 5000 )
				{
					success = true;
					_wanderTimer = 0;
					_agent.MoveTo( _player.WorldPosition );
				}
			}
			else //if ( Vector3.DistanceBetween( WorldPosition, _car.WorldPosition ) < 5000 )
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
		}
	}
	private void TurnTowards()
	{
		if ( _agent.TargetPosition.HasValue )
		{
			Vector3 direction = _agent.TargetPosition.Value - WorldPosition;
			direction /= direction.Length;
			double angle = Math.Atan2( direction.y, direction.x );
			angle = angle / double.Pi * 180;

			Rotation rotation = _agent.WorldRotation;
			Angles angles = rotation.Angles();
			angles.yaw = (float)angle;
			_rigidbody.LocalRotation = angles;
		}
		

	}

	protected override void OnFixedUpdate()
	{
		_agent.SetAgentPosition( WorldPosition );
		_rigidbody.Velocity = _agent.Velocity;
	}
}

using Sandbox;
using System;

public sealed class ZombieMoveComp : Component
{
	[Property]
	public string Name { get; set; }

	private GameObject target;
	[Property]
	private float chaseRadius;
	[Property]
	private float alertRadius;
	[Property]
	private float wanderSize;
	[Property]
	private Model model;
	private HealthComponent health;
	private float timeTillDelete = 7f;
	private SphereCollider chaseRange;
	private SphereCollider alertRange;
	private BoxCollider wanderRange;
	private NavMeshAgent agent;
	private bool isChasing = false;
	private float wanderTimer = 0;
	private float wanderMaxTime = 5;
	private float wanderTimeOffset = 2;
	private Random random = new Random();
	protected override void OnStart()
	{
		health = GameObject.GetComponent<HealthComponent>();
		health.Heal(100);
		health.OnDeath += OnDeathRagdoll;
		chaseRange = GameObject.AddComponent<SphereCollider>();
		chaseRange.Radius = chaseRadius;
		chaseRange.IsTrigger = true;
		chaseRange.OnTriggerExit += ( other ) =>
		{
			if ( other.Tags.Contains( "player" ) )
			{
				target = null;
				isChasing = false;
			}
		};
		alertRange = GameObject.AddComponent<SphereCollider>();
		alertRange.Radius = alertRadius;
		alertRange.IsTrigger = true;
		alertRange.OnTriggerEnter += ( other ) =>
		{
			if ( other.Tags.Contains( "player" ) )
			{
				target = other.GameObject;
				isChasing = true;
			}
		};
		wanderRange = GameObject.AddComponent<BoxCollider>();
		wanderRange.Scale = new Vector3( wanderSize, wanderSize, 20 );
		wanderRange.LocalTransform = LocalTransform;
		wanderRange.IsTrigger = true;
		agent = GameObject.GetComponent<NavMeshAgent>();

	}
	
	protected override void OnUpdate()
	{
		if( health.Health <= 0 )
		{
			timeTillDelete -= Time.Delta;
			if( timeTillDelete <= 0 )
			{
				GameObject.Destroy();
			}
			return;
		}


		if ( isChasing )
		{
			agent.MoveTo( target.WorldPosition );
		}
		else
		{
			Wander();
		}
	}
	private void Wander()
	{
		if ( wanderTimer <= 0 )
		{
			Vector3 randomPos = wanderRange.LocalBounds.RandomPointInside;
			randomPos = (Vector3)Scene.NavMesh.GetClosestPoint( randomPos );
			agent.MoveTo( LocalPosition + randomPos );
			wanderTimer = wanderMaxTime + random.Float( -wanderTimeOffset, wanderTimeOffset );
		}
		else
		{
			wanderTimer -= Time.Delta;
		}
	}

	private void Ragdoll()
	{
		SkinnedModelRenderer renderer = GameObject.GetComponent<SkinnedModelRenderer>();
		if ( !renderer.IsValid ) return;
		var ragdoll = AddComponent<ModelPhysics>();
		ragdoll.Renderer = renderer;
		ragdoll.Model = model;
	}
	private void OnDeathRagdoll()
	{
		Ragdoll();
	}
}

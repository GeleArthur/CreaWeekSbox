using Sandbox;
using System;

public sealed class ZombieSpawnComponent : Component
{
	[Property]
	public PlayerController Player { get; set; }

	[Property]
	public float SpawnTimer { get; set; } = 3f;

	[Property]
	public List<GameObject> SpawnArea { get; set; }

	[Property]
	GameObject Zombie { get; set; }

	[Property]
	public int MaxAmountOfZombies { get; set; } = 10;

	[Property]
	public List<HullCollider> ColliderComp { get; set; }

	private List<bool> _canSpawn;

	private SceneTraceResult _sceneTrace;


	private int AmountOfZombies;

	protected override void OnAwake()
	{
		for ( int i = 0; i < ColliderComp.Count; ++i )

		{
			_canSpawn.Add( false );
			{
				ColliderComp[i].OnTriggerEnter = ( Collider other ) =>
				{
					if ( other.Tags.Has( "player" ) )
					{
						_canSpawn[i] = true;

					}
				};

				ColliderComp[i].OnTriggerExit = ( Collider other ) =>
				{
					_canSpawn[i] = false;
				};
			}

		}
	}


	protected override void OnUpdate()
	{

		if(MaxAmountOfZombies>AmountOfZombies)
		{
			var randomSpawner= Game.Random.FromList<GameObject>(SpawnArea );
			Zombie.Clone( randomSpawner.WorldPosition );

			++AmountOfZombies;

		}
		
	}



	
}

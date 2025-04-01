using Sandbox;
using System;

public sealed class ZombieSpawnComponent : Component
{

	[Property]
	public List<BoxCollider> SpawnArea { get; set; }
	private Queue<BoxCollider> _spawnAreas = new Queue<BoxCollider>();

	[Property]
    private PrefabFile Zombie { get; set; }

	private List<ZombieMoveComp> _zombies = new List<ZombieMoveComp>();
	
	protected override void OnAwake()
	{
		// RepouplateList();
	}

	// private void RepouplateList()
	// {
	// 	List<GameObject> thingToSpawn = SpawnArea;
	// 	for ( int i = 0; i < thingToSpawn.Count; i++ )
	// 	{
	// 		GameObject awsome = Game.Random.FromList( thingToSpawn );
	// 		_spawnAreas.Enqueue( awsome );
	// 		thingToSpawn.Remove( awsome );
	// 	}
	// }


	protected override void OnUpdate()
	{
		// if(MaxAmountOfZombies > AmountOfZombies)
		// {
		// 	GameObject randomSpawner = Game.Random.FromList(SpawnArea);
		// 	Zombie.Clone(randomSpawner.WorldPosition);
		//
		// 	++AmountOfZombies;
		// 	
		// 	
		// }
		
	}



	
}

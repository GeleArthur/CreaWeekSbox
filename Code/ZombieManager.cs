using Sandbox;
using System;

public sealed class ZombieManager : Component
{
	[Property] 
	private int ZombieAmount { get; set; }
	
	[Property]
	public List<GameObject> SpawnAreas { get; set; }

	[Property]
    private PrefabFile Zombie { get; set; }

	private readonly List<OnCollideRagDoll> _activeZombies = [];
	
	protected override void OnAwake()
	{
		foreach ( GameObject child in GameObject.Children )
		{
			SpawnAreas.Add( child );
		}
	}

	protected override void OnUpdate()
	{
		
		while ( _activeZombies.Count < ZombieAmount )
		{
			Vector3 spawnLocation = Random.Shared.FromList( SpawnAreas ).WorldPosition + Vector3.Random * 3;
			Vector3? pointOnNav = Scene.NavMesh.GetClosestPoint( spawnLocation );
			
			if ( pointOnNav.HasValue )
			{
				SceneTraceResult result = Scene.Trace.Ray( pointOnNav.Value, pointOnNav.Value + Vector3.Down * 3 ).Run();
				if ( result.Hit )
				{
					GameObject newZombie = GameObject.Clone( Zombie, new Transform(result.EndPosition, Rotation.Identity) );
					_activeZombies.Add( newZombie.GetComponent<OnCollideRagDoll>() );
					
				}
			}
		}
	}

	public void RemoveZombie(OnCollideRagDoll zombies)
	{
		_activeZombies.Remove( zombies );
	}
}

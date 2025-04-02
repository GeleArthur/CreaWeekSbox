using Sandbox;
using System;

public sealed class ZombieManager : Component
{
	[Property] 
	private int _zombieAmount { get; set; }
	
	[Property]
	public List<GameObject> SpawnAreas { get; set; }

	[Property]
    private PrefabFile Zombie { get; set; }

	private List<OnCollideRagDoll> _activeZombies = new List<OnCollideRagDoll>();
	
	protected override void OnAwake()
	{
		foreach ( GameObject child in GameObject.Children )
		{
			SpawnAreas.Add( child );
		}
	}

	protected override void OnUpdate()
	{
		while ( _activeZombies.Count < _zombieAmount )
		{
			GameObject spawnLocation = Random.Shared.FromList( SpawnAreas );
			var pointOnNav = Scene.NavMesh.GetClosestPoint( spawnLocation.WorldPosition );

			if ( pointOnNav.HasValue )
			{
				GameObject newZombie = GameObject.Clone( Zombie, new Transform(pointOnNav.Value, Rotation.Identity) );
				_activeZombies.Add( newZombie.GetComponent<OnCollideRagDoll>() );

			}
			
		}
	}
}

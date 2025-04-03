using Meteor.VehicleTool.Vehicle;
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

	public PlayerController Player;
	private readonly List<OnCollideRagDoll> _activeZombies = [];
	private TimeUntil _spawnInBetween;

	private VehicleController _car;

	protected override void OnAwake()
	{
		_car = Game.ActiveScene.GetAllComponents<VehicleController>().First();

		foreach ( GameObject child in GameObject.Children )
		{
			SpawnAreas.Add( child );
		}
	}

	protected override void OnStart()
	{
		Player = Game.ActiveScene.GetAllComponents<PlayerController>().First();
	}

	protected override void OnUpdate()
	{
		var orderedSpawnAreas = SpawnAreas.OrderBy( ( area1 ) => Vector3.DistanceBetween( area1.WorldPosition, _car.WorldPosition ) );
		//SpawnAreas.Sort(
		//	( x, y ) => Vector3.Distance( m_PC.transform.position, x.transform.position )
		//		.CompareTo( Vector3.Distance( m_PC.transform.position, y.transform.position ) )
		//);
		while ( _activeZombies.Count < ZombieAmount && _spawnInBetween )
		{
			_spawnInBetween = 0.01f;
			Vector3 spawnLocation = orderedSpawnAreas.First().WorldPosition + Vector3.Random * 100;
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

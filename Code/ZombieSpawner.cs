using Sandbox;



public sealed class ZombieSpawner : Component
{

	[Property]
	public List<GameObject> SpawnArea { get; set; }

	[Property]
	GameObject Zombie { get; set; }

	[Property]
	public int MaxAmountOfZombies { get; set; } = 10;

	[Property]
	public BoxCollider ColliderComp { get; set; }

	private bool _canSpawn;

	private float _deltaTime;
	float SpawnTimer = 3f;


	protected override void OnAwake()
	{
		ColliderComp.OnTriggerEnter = ( Collider other ) =>
		{
			if ( other.Tags.Has( "player" ) )
			{
				_canSpawn = true;

			}
		};

		ColliderComp.OnTriggerExit = ( Collider other ) =>
		{
			_canSpawn= false;
		};
	}
	protected override void OnUpdate()
	{
		if(_canSpawn)
		{
			_deltaTime += Time.Delta;
			if ( _deltaTime > SpawnTimer )
			{
				_deltaTime -=SpawnTimer;
			}
		}
		
		
	}
}

using System;
using Meteor.VehicleTool.Vehicle;
using Sandbox;

public sealed class HealthComponent : Component
{
	[Property]
	private int _maxHealth { get; set; }

	[Property] public Action OnDeath;
	public bool IsDeath => _health <= 0;
	private int _health;

	public int Health
	{
		get { return _health; }
		private set
		{
			_health = value;
			if ( _health <= 0 )
			{
				PlayerDies();
				OnDeath?.Invoke();
			}
		}
	}

	public void Damage( int damage )
	{
		Health = int.Clamp( _health - damage, 0, _maxHealth );
	}

	public void Heal( int healing )
	{
		Health = int.Clamp( _health + healing, 0, _maxHealth );
	}

	[Button]
	void DamageLol()
	{
		Damage(10);
	}

	protected override void OnStart()
	{
		Health = _maxHealth;
	}

	protected override void OnUpdate()
	{
		DebugOverlay.Text( WorldPosition + Vector3.Up * 90f, $"[{_health}/{_maxHealth}]" );
	}

	private void PlayerDies()
	{
		var player = GameObject.GetComponent<PlayerController>();
		if ( player == null ) return;
		var vehicle = Scene.GetAllComponents<VehicleController>().First();

		Damage(-_maxHealth);
		var vhHealth = vehicle.GetComponent<HealthComponent>();
		vhHealth.Damage( -vhHealth._maxHealth );

		vehicle.GameObject.WorldPosition = Scene.GetAllObjects( false ).Where( ( obj ) => obj.Name == "CarSpawnPoint" )
			.ToList().First().WorldPosition;
		vehicle.GameObject.WorldRotation = Scene.GetAllObjects( false ).Where( ( obj ) => obj.Name == "CarSpawnPoint" )
			.ToList().First().WorldRotation;
		player.GameObject.WorldPosition = Scene.GetAllObjects( false ).Where( ( obj ) => obj.Name == "PlayerRespawnPoint" )
			.ToList().First().WorldPosition;
	}
}

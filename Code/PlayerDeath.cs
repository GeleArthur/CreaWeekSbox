using System.Security.Cryptography;
using Meteor.VehicleTool.Vehicle;
using Sandbox;

public sealed class PlayerDeath : Component
{
	protected override void OnStart()
	{
		GameObject.GetComponent<HealthComponent>().OnDeath += DeathOfPLayer;
	}
	public void DeathOfPLayer()
	{
		Log.Info("I died");
		var player = GameObject.GetComponent<PlayerController>();
		var playerHeal = GameObject.GetComponent<HealthComponent>();
		if ( player == null ) return;
		var vehicle = Scene.GetAllComponents<VehicleController>().First();
		var fual = Scene.GetAllComponents<FuelTank>().First();
		var upgrades = vehicle.GameObject.GetComponent<CarUpgradeManager>();

		int randIdx = RandomNumberGenerator.GetInt32(0, upgrades.UpgradeList.Count);
		for ( int index = 0; index < upgrades.UpgradeList.Count; ++index )
		{
			if ( randIdx == index )
			{
				if ( upgrades.UpgradeList[index].UpgradeInfo.isActive && !upgrades.UpgradeList[index].UpgradeInfo.isBroken )
				{
					upgrades.UpgradeList[index].UpgradeInfo.isBroken = true;
					upgrades.UpgradeList[index].UpgradeInfo.visual.Enabled = false;
				}
				else
				{
					randIdx = RandomNumberGenerator.GetInt32( 0, upgrades.UpgradeList.Count );
					index = 0;
				}
			}
		}

		fual.AddFual( fual.MaxFuel);

		playerHeal.Damage( -playerHeal.MaxHealth );
		var vhHealth = vehicle.GameObject.GetComponent<HealthComponent>();
		vhHealth?.Damage( -vhHealth.MaxHealth );

		vehicle.GameObject.WorldPosition = Scene.GetAllObjects( true ).Where( ( obj ) => obj.Name == "CarSpawnPoint" )
			.ToList().First().WorldPosition;
		vehicle.GameObject.WorldRotation = Scene.GetAllObjects( true ).Where( ( obj ) => obj.Name == "CarSpawnPoint" )
			.ToList().First().WorldRotation;
		player.GameObject.WorldPosition = Scene.GetAllObjects( true ).Where( ( obj ) => obj.Name == "PlayerRespawnPoint" )
			.ToList().First().WorldPosition;

	}
}

using System;
using Sandbox;

public class CarUpgradeComponent : Component
{
	[Property] public UpgradeInfo UpgradeInfo;
	[Property] public Action OnBuy;
	[Property] public Action OnBreak;
	[Property] public Action OnRepair;

	//! DEBUG
	[Button( icon: "💸" )]
	public void Buy()
	{
		OnBuy.Invoke();
	}

	[Button( icon: "💥" )]
	public void Break()
	{
		OnBuy.Invoke();
	}

	[Button( icon: "🔧" )]
	public void Repair()
	{
		OnBuy.Invoke();
	}
}

public class UpgradeInfo
{
	public string Name { get; set; }
	public string Description { get; set; }

	public int buyCost { get; set; }
	public int repairCost { get; set; }

	public bool isActive { get; set; }
	public bool isBroken { get; set; }

	public GameObject visual { get; set; }
}

public sealed class CarUpgradeManager : Component
{
	[Property] private List<CarUpgradeComponent> _upgrades;

	// General methods
	public int GetCost(int upgrade)
	{
		if ( !_upgrades[upgrade].UpgradeInfo.isActive ) { return _upgrades[upgrade].UpgradeInfo.buyCost; }

		if ( _upgrades[upgrade].UpgradeInfo.isBroken ) { return _upgrades[upgrade].UpgradeInfo.repairCost; }
		return 0;
	}

	public void Unlock( int upgrade )
	{
		_upgrades[upgrade].UpgradeInfo.isActive = true;
	}

	public void Break( int upgrade )
	{
		_upgrades[upgrade].UpgradeInfo.isBroken = true;
	}

	public void Repair( int upgrade )
	{
		_upgrades[upgrade].UpgradeInfo.isBroken = false;
	}

	protected override void OnUpdate()
	{

	}
}

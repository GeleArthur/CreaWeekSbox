using Sandbox;
using System;

public sealed class GoalManager : Component
{

	[Property]
	public Goal[] Goals { get; set; }
	[Property]
	public Economy EconomyComp { get; set; }
	[Property]
	public FuelTank FuelTankComp { get; set; }
	[Property]
	public int FuelPerGoal {  get; set; }

	private Goal _currentGoal;

	public Goal CurrentGoal
	{
		get { return _currentGoal; }
		private set { _currentGoal = value; }
	}
	
	protected override void OnStart()
	{
		FuelTankComp = Game.ActiveScene.GetAllComponents<FuelTank>().First();
		EconomyComp = Game.ActiveScene.GetAllComponents<Economy>().First();
		SetNewGoal();

		foreach( var goal in Goals)
		{
			goal.GoalManager = this;
		}

	}
	
	public void Notify(Goal goal, bool isSellingGoal, bool isFuelGoal)
	{
		if ( goal == CurrentGoal )
		{
			if(isSellingGoal)
			{
				EconomyComp.SellMedicine();
			}
			else if (isFuelGoal)
			{
				FuelTankComp.AddFual(150);
			}
			else
			{
				EconomyComp.AddMedicine(FuelPerGoal);
			}
			
			CurrentGoal.EnableModel( false );
			SetNewGoal();
		}
	}
	private void SetNewGoal()
	{
		var prevGoal = CurrentGoal;
		base.OnStart();
		var rnd = new Random();
		while ( prevGoal == CurrentGoal )
		{
			if ( Goals.Length == 0 ) break;
			
			CurrentGoal = Goals[rnd.Int( 0, Goals.Length-1 )];
		}
		CurrentGoal.EnableModel(true);
		
	}
}

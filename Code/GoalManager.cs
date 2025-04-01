using Sandbox;
using System;

public sealed class GoalManager : Component
{

	[Property]
	public GameObject[] Goals { get; set; }
	[Property]
	public Economy EconomyComp { get; set; }

	GameObject _currentGoal;

	GameObject CurrentGoal
	{
		get { return _currentGoal; }
		set { _currentGoal = value; }
	}
	
	protected override void OnStart()
	{
		SetNewGoal();

		foreach( GameObject goal in Goals)
		{
			goal.GetComponent<Goal>().GoalManager = this;
		}

	}
	
	public void Notify(GameObject goal, bool isSellingGoal)
	{
		if (goal == CurrentGoal)
		{
			if(isSellingGoal)
			{
				EconomyComp.SellMedicine();
			}
			else
			{
				EconomyComp.AddMedicine(10);
			}
			
			CurrentGoal.GetComponent<Goal>().EnableModel( false );
			SetNewGoal();
		}
	}
	private void SetNewGoal()
	{
		var prevGoal = CurrentGoal;
		base.OnStart();
		Random rnd = new Random();
		while ( prevGoal == CurrentGoal )
		{
			CurrentGoal = Goals[rnd.Int( 0, Goals.Length-1 )];
		}
		CurrentGoal.GetComponent<Goal>().EnableModel(true);

	}
}

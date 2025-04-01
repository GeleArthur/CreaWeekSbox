using Sandbox;
using System;

public sealed class GoalManager : Component
{

	[Property]
	public GameObject[] Goals { get; set; }

	GameObject _currentGoal;

	GameObject CurrentGoal
	{
		get { return _currentGoal; }
		set { _currentGoal = value; }
	}

	private bool _goalReached = false;

	protected override void OnStart()
	{
		SetNewGoal();
		Log.Info( "started" );
	}
	
	public void Notify(GameObject goal)
	{
		if (goal == CurrentGoal)
		{
			Log.Info( "reached goal" );
			_goalReached = true;
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

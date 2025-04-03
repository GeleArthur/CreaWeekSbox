using Meteor.VehicleTool.Vehicle;
using Sandbox;

public enum GoalTypes
{
	MedicineGoal,
	FuelGoal,
	SellingGoal,
	Garage
}

public sealed class GoalArrow : Component
{
	[Property]
	public GoalTypes GoalType { get; set; }

	private GameObject _carLocation;
	private PlayerController _playerLoc;

	private Vector3 GoalPos;
	private Goal mygoal;
	private bool active = false;

	protected override void OnStart()
	{
		_carLocation = Scene.GetAllComponents<VehicleController>().First().GameObject;
		_playerLoc = Scene.GetAllComponents<PlayerController>().First();

		var goals = Scene.GetAllComponents<GoalManager>().First().Goals;

		if ( GoalType == GoalTypes.Garage )
		{
			GoalPos = Scene.GetAllComponents<GarageComp>().First().WorldPosition;
			return;
		}

		foreach ( var goal in goals )
		{
			if ( goal.SellingGoal && GoalType == GoalTypes.SellingGoal )
			{
				GoalPos = goal.WorldPosition;
				mygoal = goal;
				//GoalPos = new Vector3( 500, 500, 0 );
				break;
			}

			if ( goal.fuelGoal && GoalType == GoalTypes.FuelGoal )
			{
				GoalPos = goal.WorldPosition;
				mygoal = goal;
				//GoalPos = new Vector3( 100, 500, 0 );
				break;
			}

			if ( goal.SellingGoal == false && goal.fuelGoal == false && GoalType == GoalTypes.MedicineGoal )
			{
				GoalPos = goal.WorldPosition;
				mygoal = goal;
				//GoalPos = new Vector3( -200, -100, 0 );
				break;
			}
		}
	}

	protected override void OnUpdate()
	{
		if ( mygoal == null || GoalType == GoalTypes.Garage )
		{
			active = true;
		}
		else
		{
			active = mygoal.GameObject.Enabled;
		}

	}
	protected override void OnPreRender()
	{
		if ( !active )
			return;

		Vector3 playerPos = _playerLoc.GameObject.Enabled ? _playerLoc.GameObject.WorldPosition : _carLocation.WorldPosition;
		Vector3 worldPos = new Vector3( playerPos.x, playerPos.y, playerPos.z + 1700 );
		Vector3 vectorToGoal = new Vector3( GoalPos - playerPos );

		Rotation rotation = Rotation.LookAt( vectorToGoal );
		WorldTransform = new Transform( worldPos, new Angles( 0, rotation.Yaw(), 0 ) );
	}
}

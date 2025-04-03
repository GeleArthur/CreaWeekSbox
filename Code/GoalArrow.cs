using Meteor.VehicleTool.Vehicle;
using Sandbox;

public enum GoalTypes
{
	MedicineGoal,
	FuelGoal,
	SellingGoal
}

public sealed class GoalArrow : Component
{
	[Property]
	public GoalTypes GoalType { get; set; }

	private GameObject _carLocation;

	private Vector3 GoalPos;

	protected override void OnStart()
	{
		_carLocation = Scene.GetAllComponents<VehicleController>().First().GameObject;

		var goals = Scene.GetAllComponents<GoalManager>().First().Goals;
		
		foreach ( var goal in goals )
		{
			if ( goal.SellingGoal && GoalType == GoalTypes.SellingGoal )
			{
				GoalPos = goal.WorldPosition;
				//GoalPos = new Vector3( 500, 500, 0 );
				break;
			}

			if ( goal.fuelGoal && GoalType == GoalTypes.FuelGoal )
			{
				GoalPos = goal.WorldPosition;
				//GoalPos = new Vector3( 100, 500, 0 );
				break;
			}

			if ( goal.SellingGoal == false && goal.fuelGoal == false && GoalType == GoalTypes.MedicineGoal )
			{
				GoalPos = goal.WorldPosition;
				//GoalPos = new Vector3( -200, -100, 0 );
				break;
			}
		}
	}

	protected override void OnPreRender()
	{
		Vector3 playerPos = _carLocation.WorldPosition;
		Vector3 worldPos = new Vector3( playerPos.x, playerPos.y, playerPos.z + 1700 );
		Vector3 vectorToGoal = new Vector3( GoalPos - WorldPosition );

		Rotation rotation = Rotation.LookAt( vectorToGoal );
		WorldTransform = new Transform( worldPos, new Angles( 0, rotation.Yaw(), 0 ) );
	}
}

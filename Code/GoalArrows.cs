namespace Sandbox;
public enum GoalTypes
{
	MedicineGoal,
	FuelGoal,
	SellingGoal
}

public sealed class GoalArrows: Component
{
	[Property]
	public GoalTypes GoalType { get; set; }

	[Property]
	public GameObject PlayerToCircle { get; set; }

	private Vector3 GoalPos;

	protected override void OnStart()
	{
		PlayerToCircle = Scene.GetAllComponents<PlayerController>().First().GameObject;
		if ( PlayerToCircle == null )
		{
			Log.Info("Player To Circle null"  );
		}
		var playerPos = PlayerToCircle.WorldPosition;

		var goals = 
			Scene.Directory.FindByName( "GoalManager" ).First().
				GetComponent<GoalManager>().Goals;

		Log.Info( "Amount of goals: " + goals.Length);

		foreach ( var goal in goals )
		{
			if ( goal.SellingGoal && GoalType == GoalTypes.SellingGoal )
			{
				GoalPos = goal.WorldPosition;
				break;
			}

			if ( goal.fuelGoal && GoalType == GoalTypes.FuelGoal )
			{
				GoalPos = goal.WorldPosition;
				break;
			}

			if ( goal.SellingGoal == false && goal.fuelGoal == false && GoalType == GoalTypes.MedicineGoal )
			{
				GoalPos = goal.WorldPosition;
				break;
			}
		}
	}

	protected override void OnUpdate()
	{
		var playerPos = PlayerToCircle.WorldPosition;
		var worldPos = new Vector3(playerPos.x,playerPos.y,playerPos.z + 1800);
		Vector3 VectorToGoal = new Vector3( GoalPos - WorldPosition );

		var rotation = Rotation.LookAt( VectorToGoal );
		this.WorldTransform = new Transform( worldPos, new Angles( 0, rotation.Yaw(), 0 ) );
	}
}

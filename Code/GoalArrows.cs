using Sandbox;

using System;

public sealed class GoalArrows: Component
{
	[Property]
	public Vector3 GoalPos { get; set; }

	[Property]
	public GameObject PlayerToCircle { get; set; }

	protected override void OnStart()
	{
		var playerPos = PlayerToCircle.WorldPosition;
		WorldPosition = new Vector3(playerPos.x, playerPos.y, playerPos.z + 1400);

		GoalPos = 
			Scene.Directory.FindByName( "GoalManager" ).First().GetComponent<GoalManager>().CurrentGoal.WorldPosition;
	}

	protected override void OnUpdate()
	{
		WorldPosition = WorldPosition with { x = PlayerToCircle.WorldPosition.x , y = PlayerToCircle.WorldPosition.y };

		Vector2 VectorToGoal = new Vector2( GoalPos - WorldPosition );

		//Vector2 dotOfToGoal = Vector2.Dot( VectorToGoal, Vector2.Left );

		var angle = VectorToGoal.Degrees;

		WorldRotation = WorldRotation with { z = angle };
	}
}


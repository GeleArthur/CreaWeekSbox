using Sandbox;

public sealed class FollowPlayerCamera : Component
{
	[Property] public float HeightOffset = 100f;
	[Property] public PlayerController PlayerController = null;
	protected override void OnUpdate()
	{
		GameObject.WorldPosition = new Vector3( PlayerController.GameObject.WorldPosition.x, PlayerController.GameObject.WorldPosition.y, HeightOffset);
	}
}

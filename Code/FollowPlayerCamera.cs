using System;
using Meteor.VehicleTool.Vehicle;
using Sandbox;
using Sandbox.VR;

public sealed class FollowPlayerCamera : Component
{
	[Property] public float HeightOffset = 100f;
	[Property] public PlayerController PlayerController = null;
	[Property] public VehicleController VehicleController = null;
	[Property] public CameraComponent Cam = null;

	protected override void OnStart()
	{
		PlayerController = Scene.GetAllComponents<PlayerController>().First();
		VehicleController = Scene.GetAllComponents<VehicleController>().First();
		Cam = Scene.GetAllComponents<CameraComponent>().First((obj) => obj.IsMainCamera);
	}

	protected override void OnUpdate()
	{
		float angle = float.Atan2( Cam.WorldTransform.Forward.y, Cam.WorldTransform.Forward.x );
		if ( PlayerController.GameObject.Enabled )
		{
			GameObject.WorldPosition = new Vector3( PlayerController.GameObject.WorldPosition.x, PlayerController.GameObject.WorldPosition.y, HeightOffset );
		}
		else if ( VehicleController.GameObject.Enabled )
		{
			GameObject.WorldPosition = new Vector3( VehicleController.GameObject.WorldPosition.x, VehicleController.GameObject.WorldPosition.y, HeightOffset );
		}
		GameObject.WorldRotation = Rotation.From( 90, angle * 180 / Single.Pi, 0 ); // Looking straight down
	}
}

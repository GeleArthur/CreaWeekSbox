using Sandbox;

public sealed class OnCollideRagDoll : Component, Component.ICollisionListener
{
	public void OnCollisionStart( Collision collision )
	{
		Log.Info( collision.Other.GameObject.Name );
		GetComponent<ModelPhysics>().MotionEnabled = true;
	}
}

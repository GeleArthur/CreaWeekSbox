using Sandbox;

public sealed class OnCollideRagDoll : Component, Component.ICollisionListener
{
	private ModelPhysics _ragdollPhysics;
	private Rigidbody _rigidbody;
	private ModelCollider _modelCollider;
	
	protected override void OnStart()
	{
		_ragdollPhysics =  GetComponent<ModelPhysics>();
		_rigidbody = GetComponent<Rigidbody>();
		_modelCollider = GetComponent<ModelCollider>();
		_ragdollPhysics.Enabled = false;
		_rigidbody.Enabled = true;
		_modelCollider.Enabled = true;

	}

	public void OnCollisionStart( Collision collision )
	{
		// if(!collision.Other.GameObject.Tags.Has("car")) return;
		// Log.Info( collision.Contact.Speed.Length );
		
		if(collision.Contact.Speed.Length < 250) return;
		
		_ragdollPhysics.Enabled = true;
		_rigidbody.Enabled = false;
		_modelCollider.Enabled = false;

	}
}

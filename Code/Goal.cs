using Sandbox;

public sealed class Goal : Component
{
	[Property]
	public GoalManager GoalManager;

	[Property]
	public ModelRenderer Model;

	private Collider _collider;
	public Collider Collider
	{ 
		get { return _collider; } 
		set { _collider = value; }
	}

	protected override void OnStart()
	{
		Collider = GetComponent<Collider>();
		Collider.OnTriggerEnter = ( Collider other ) =>
		{
			if ( other.Tags.Has("player"))
			{
				GoalManager.Notify( this.GameObject );
			}
		};

		
	}

	public void EnableModel(bool enable)
	{
		Model.Enabled = enable;
	}
}

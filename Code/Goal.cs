using Sandbox;

public sealed class Goal : Component
{
	[Property]
	public ModelRenderer Model;
	[Property]
	public bool SellingGoal = true;

	private Collider _collider;
	public Collider Collider
	{ 
		get { return _collider; } 
		set { _collider = value; }
	}


	private GoalManager _goalManager;
	public GoalManager GoalManager
	{
		get { return _goalManager; }
		set { _goalManager = value; }
	}

	protected override void OnStart()
	{
		Collider = GetComponent<Collider>();
		Collider.OnTriggerEnter = ( Collider other ) =>
		{
			if ( other.Tags.Has("player") ||other.Tags.Has("car"))
			{
				GoalManager.Notify( this.GameObject , SellingGoal);
			}
		};
	}

	public void EnableModel(bool enable)
	{
		Model.Enabled = enable;
	}
}

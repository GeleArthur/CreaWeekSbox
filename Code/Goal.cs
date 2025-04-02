using Sandbox;
using System;

public sealed class Goal : Component
{
	[Property]
	public ModelRenderer Model {  get; set; }
	[Property]
	public bool SellingGoal { get; set; }
	[Property]
	public bool fuelGoal { get; set; }

	[Property]
	public TextRenderer TextRendererComp { get; set; }

	private Collider _collider;
	public Collider Collider
	{ 
		get { return _collider; } 
		set { _collider = value; }
	}

	private GoalManager _goalManager;
	public GoalManager GoalManager
	{
		private get { return _goalManager; }
		set { _goalManager = value; }
	}

	private GameObject _player;

	protected override void OnStart()
	{
		if ( SellingGoal && fuelGoal ) { Log.Warning( "both selling and fuelin goal are true" ); }
		_player = Scene.Directory.FindByName( "Player Controller" ).First();

		Collider = GetComponent<Collider>();
		Collider.OnTriggerEnter = ( Collider other ) =>
		{
			if ( other.Tags.Has("player") ||other.Tags.Has("car"))
			{
				if(GoalManager == null)
				{
					GoalManager = Scene.Directory.FindByName( "GoalManager" ).First().GetComponent<GoalManager>();
				}
				GoalManager.Notify( this.GameObject , SellingGoal, fuelGoal);
			}
		};
		if ( SellingGoal )
		{ 
			Model.Tint = new Color( 0f, 1f, 0f, 0.4f );
			TextRendererComp.Text = "🤑";
		}
		else if(fuelGoal)
		{
			Model.Tint = new Color( 1f, 0f, 0f, 0.4f );
			TextRendererComp.Text = "⛽";
		}
		else
		{
			Model.Tint = new Color( 0f, 0f, 1f, 0.4f );
			TextRendererComp.Text = "💊";
		}
			
	}

	protected override void OnUpdate()
	{
		if ( !TextRendererComp.IsValid )
		{
			Log.Info( "rip" );
			return;
		}

		if (TextRendererComp.Active)
		{
			var direction = _player.WorldPosition - WorldPosition;
			direction = direction / direction.Length;
			var angle = Math.Atan2( direction.y, direction.x );
			angle = angle / double.Pi * 180;

			TextRendererComp.LocalTransform = new Transform( TextRendererComp.LocalPosition, new Angles( 0, (float)angle, 0 ) );
		}
	}

	public void EnableModel(bool enable)
	{
		Model.Enabled = enable;
		if ( TextRendererComp == null )
		{
			Log.Info( "also here" );
			return;
		}
		TextRendererComp.Enabled = enable;
	}
}

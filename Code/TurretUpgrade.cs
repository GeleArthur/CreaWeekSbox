using Sandbox;
using System;

public sealed class TurretUpgrade : Component
{
	[Property]
	public int FireRate;
	[Property]
	public GameObject Bullet;
	[Property]
	public GameObject Nuzzle;
	[Property]
	private Collider ColliderComp;

	private List<GameObject> _targets = new List<GameObject>();
	public List<GameObject> Targets
	{ 
		get { return _targets; }
		set { _targets = value; }
	}

	private float _elapsedSec = 0f;
	protected override void OnAwake()
	{
		ColliderComp.OnTriggerEnter = ( Collider other ) =>
		{
			if ( other.Tags.Has( "player" ) && !Targets.Contains(other.GameObject))
			{
				Targets.Add( other.GameObject );
				Log.Info( Targets.Count );
			}
		};

		ColliderComp.OnTriggerExit = ( Collider other ) =>
		{
			Targets.Remove( other.GameObject );
		};
	}
	protected override void OnUpdate()
	{
		if ( Targets.Count < 1) return;
		if ( Targets[0].IsDestroyed ) Targets.RemoveAt( 0 );

		// facing target
		var direction = _targets[0].WorldPosition - WorldPosition;
		direction = direction / direction.Length;
		var angle = Math.Atan2(direction.y,direction.x);
		angle = angle / double.Pi * 180;

		var angle2 = Math.Atan2( direction.z, direction.x );
		angle2 = angle2 / double.Pi * 180;

		if ( angle > 90 || angle < -90 )
		{
			angle2 = (180 + angle2);
			angle2 = -angle2;
		}

		this.LocalTransform = new Transform( this.LocalPosition, new Angles( -(float)angle2, (float)angle, 0 ) );

		//shooting
		_elapsedSec = _elapsedSec + Time.Delta;
		if ( _elapsedSec > (1f/FireRate) )
		{
			_elapsedSec -= (1f / FireRate);

			var bullet = Bullet.Clone();
			var bulletScale = bullet.LocalScale;
			bullet.LocalTransform = Nuzzle.WorldTransform;
			bullet.LocalScale = bulletScale;
		}
		

	}
}

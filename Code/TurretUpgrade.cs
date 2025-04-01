using Sandbox;
using System;

public sealed class TurretUpgrade : Component
{
	[Property]
	public int FireRate { get; set; }
	[Property]
	public GameObject Bullet { get; set; }
	[Property]
	public GameObject Nuzzle { get; set; }
	[Property]
	private Collider ColliderComp { get; set; }

	private readonly List<GameObject> _targets = [];

	private float _elapsedSec = 0f;
	protected override void OnAwake()
	{
		ColliderComp.OnTriggerEnter = ( Collider other ) =>
		{
			if ( other.Tags.Has( "zombie" ) && !_targets.Contains(other.GameObject))
			{
				_targets.Add( other.GameObject );
			}
		};

		ColliderComp.OnTriggerExit = ( Collider other ) =>
		{
			_targets.Remove( other.GameObject );
		};
	}
	protected override void OnUpdate()
	{
		if ( _targets.Count < 1) return;
		if ( _targets[0].IsDestroyed ) _targets.RemoveAt( 0 );

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

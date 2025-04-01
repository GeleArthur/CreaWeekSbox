using Sandbox;

public sealed class Bullet : Component
{
	private float _speed = 100f;
	public float Speed
	{
		get { return _speed; }
		set { _speed = value; }
	}

	private float _lifeTime = 3f;
	private float _timeAlive = 0f;

	protected override void OnUpdate()
	{
		var finalMovementVec = LocalTransform.Forward * Time.Delta * Speed;
		this.GameObject.WorldPosition += finalMovementVec;

		_timeAlive = _timeAlive + Time.Delta;
		if (_timeAlive > _lifeTime)
		{
			this.GameObject.Destroy();
		}
	}

}

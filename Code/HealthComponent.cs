using System;
using Sandbox;

public sealed class HealthComponent : Component
{
	private int _health;
	private int _maxHealth;

	public Action OnDeath;

	public int Health
	{
		get { return _health; }
		private set
		{
			_health = value;
			if ( _health == 0 ) OnDeath?.Invoke();
		}
	}

	public void Damage( int damage )
	{
		_health = int.Clamp( _health - damage, 0, _maxHealth );
	}

	public void Heal( int healing )
	{
		_health = int.Clamp( _health + healing, 0, _maxHealth );
	}
}

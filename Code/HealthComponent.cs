using System;
using Sandbox;

public sealed class HealthComponent : Component
{
	[Property]
	private int _maxHealth;

	public Action OnDeath;
	private int _health;

	public int Health
	{
		get { return _health; }
		private set
		{
			_health = value;
			if ( _health <= 0 ) OnDeath?.Invoke();
		}
	}

	public void Damage( int damage )
	{
		Health = int.Clamp( _health - damage, 0, _maxHealth );
	}

	public void Heal( int healing )
	{
		Health = int.Clamp( _health + healing, 0, _maxHealth );
	}

}

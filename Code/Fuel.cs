using System;
using Sandbox;

public sealed class FuelTank : Component
{
	[Property] private float _defaultCapacity = 500;
	[Property] private float _upgradedCapacity = 1250;

	private float _maxCapacity;
	private float _currentFuel;

	[Range(0.1f, 1f, 0.01f)]private float _efficiency;
	private bool _isDriving;

	private float _fuelUsagePerSecond;

	protected override void OnUpdate()
	{
		if(!_isDriving) { return; }
		_currentFuel = float.Clamp( _currentFuel - Time.Delta * (1 -_efficiency) * _fuelUsagePerSecond, 0, _maxCapacity );
	}

	protected override void OnAwake()
	{
		_maxCapacity = _defaultCapacity;
	}

	public void SetActive( bool state )
	{
		_isDriving = state;
	}

	public float GetFuelAmount()
	{
		return _currentFuel;
	}

	public void SetEfficiency( float efficiency )
	{
		_efficiency = efficiency;
	}

	public void Upgrade()
	{
		_maxCapacity = _upgradedCapacity;
	}

	public void Downgrade()
	{
		_maxCapacity = _defaultCapacity;
	}
}

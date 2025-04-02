using System;
using Sandbox;

public sealed class FuelTank : Component
{
	[Property] private float _defaultCapacity = 500;
	[Property] private float _upgradedCapacity = 1250;

	[Property, ReadOnly] private float _maxCapacity;
	[Property, ReadOnly] private float _currentFuel;

	[Range(0f, 0.9f, 0.01f)]private float _efficiency = 0f;
	private bool _isDriving;

	[Property] private float _fuelUsagePerSecond = 1f;

	[Property] private Action _OnOutOfFuel;

	protected override void OnAwake()
	{
		_maxCapacity = _defaultCapacity;
		_currentFuel = 0.5f * _maxCapacity;
	}

	protected override void OnUpdate()
	{
		if(!_isDriving) { return; }

		if ( _currentFuel <= 0.001f )
		{
			_OnOutOfFuel?.Invoke();
			return;
		}

		_currentFuel = float.Clamp( _currentFuel - (Time.Delta * (1 - _efficiency) * _fuelUsagePerSecond), 0, _maxCapacity );
		DebugOverlay.Text( WorldPosition + Vector3.Up * 120f, $"Fuel:{_currentFuel:F1} / {_maxCapacity}" );
	}

	public void SetActive( bool state )
	{
		_isDriving = state;
	}

	public float GetFuelAmount()
	{
		return _currentFuel;
	}

    public void AddFual(float amount)
    {
        _currentFuel = float.Clamp( _currentFuel + amount, 0, _maxCapacity );
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

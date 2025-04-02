using Sandbox;
using System.Xml.Linq;

public sealed class Economy : Component
{
	public int Money { get => _money; }
	[Property] public int StartMoney = 0;
	private int _money = 0;

	public int Medicine { get => _medicine; }
	[Property] public int StartMedicine = 0;
	private int _medicine = 0;

	/// <summary>
	/// How much money the player gets per medicine
	/// </summary>
	[Property] public float ConversionRate = 2f;

	protected override void OnStart()
	{
		_money = StartMoney;
		_medicine = StartMedicine;
	}

	public void AddMedicine( int amount )
	{
		_medicine += amount;
	}

	[Button( "AddMedicine" )]
	private void DebugAddMedicine()
	{
		AddMedicine( 10 );
	}


	[Button( "Sell" )]
	public void SellMedicine(bool sellAll = true, int sellAmount = 0)
	{
		if ( sellAll )
		{
			_money += (int)(_medicine * ConversionRate);
			_medicine = 0;
		}
		else
		{
			sellAmount = (int)MathX.Clamp( sellAmount, 0, _medicine );
			_medicine -= sellAmount;
			_money += (int)(sellAmount * ConversionRate);

		}
	}

	public bool TryBuy( int price )
	{
		if ( _money >= price )
		{
			_money -= price;
			return true;
		}
		else
		{
			return false;
		}
	}
}

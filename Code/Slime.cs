using Sandbox;

public enum TeamType
{
	[Icon("🧑")]
	[Description("Player")]
	Player,
	[Icon("💩")]
	[Description("Player")]
	Slime
	
}

public sealed class Slime : Component
{
	[Property]
	public string Name { get; set; }
	
	[Property]
	public TeamType Team { get; set; }
	
	[Property]
	[Range(0,100)]
	public float MaxHealth { get; set; } = 100f;
	
	[Property]
	public SkinnedModelRenderer ModelRenderer { get; set; }

	private float _health;

	private TimeUntil _healthRegin;

	public float Health
	{
		get
		{
			return _health;
		}
		set
		{
			UpdateHealth( value );
		}
	}

	protected override void OnStart()
	{
		_health = MaxHealth;
	}

	protected override void OnUpdate()
	{
		if(!ModelRenderer.IsValid) return;
		if ( Team == TeamType.Slime )
		{
			float remmappedHealth = MathX.Remap( Health, 0f, MaxHealth, 0f, 100f );
			var currentHealth = ModelRenderer.GetFloat( "health" );
			var lerphalth = MathX.Lerp( currentHealth, remmappedHealth, Time.Delta*2f );
			ModelRenderer.Set( "health", lerphalth );
		}
	}

	[Button("Hurt 10", "💥")]
	public void Hurtdebug()
	{
		Damage( 10.0f );
	}

	[Button("heal 10", "❤️")]
	public void HealDebug()
	{
		Damage( -10.0f );
	}
	
	[Button("hit 30", "💥")]
	public void HurtMoreDebug()
	{
		Damage( 30.0f );
	}

	public void Damage( float damage )
	{
		Health -= damage;
	}

	private void UpdateHealth( float newHealth )
	{
		float diffrence = newHealth - _health;
		_health = MathX.Clamp(newHealth, 0, MaxHealth);
		
		if(!ModelRenderer.IsValid) return;
		if ( diffrence < 0f )
		{
			float remmappedDamage = MathX.Remap( -diffrence, 0f, MaxHealth, 0f, 300f );
			DamageAnimation( remmappedDamage );
		}


		if ( Health <= 0f )
		{
			DeathAnimtion();
		}
	}

	private async void DamageAnimation( float damage )
	{
		ModelRenderer.LocalScale *= 1.1f;
		ModelRenderer.Tint = Color.Red;

		await Task.DelaySeconds( damage / 150f );
		
		ModelRenderer.LocalScale /= 1.1f;
		ModelRenderer.Tint = Color.White;
	}

	private void DeathAnimtion()
	{
		ModelRenderer.Set( "dead", true );

	}
}

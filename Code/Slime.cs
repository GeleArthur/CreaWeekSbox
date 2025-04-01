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
	[Category("Health")]
	public float MaxHealth { get; set; } = 100f;

	[Property] 
	[Category("Health")]
	[Range(0, 30f, 1f)]
	public float HealthRegenration { get; set; } = 5f;
	
	[Property]
	[Category("Info")]
	public SkinnedModelRenderer ModelRenderer { get; set; }

	public bool Alive = true;

	private float _health;

	private TimeUntil _healthRegen = 1;
 
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

		DebugOverlay.Text( WorldPosition + Vector3.Up * 80f, $"{Name} [{Health} / {MaxHealth}]" );
	}

	protected override void OnFixedUpdate()
	{
		if(!Alive) return;
		if ( _healthRegen )
		{
			Damage( -HealthRegenration );
			_healthRegen = 1f;
		}
	}

	[Button("Hurt 10", "💥")]
	[Category("Health")]
	public void Hurtdebug()
	{
		Damage( 10.0f );
	}

	[Button("heal 10", "❤️")]
	[Category("Health")]
	public void HealDebug()
	{
		Damage( -10.0f );
	}
	
	[Button("hit 30", "💥")]
	[Category("Health")]
	public void HurtMoreDebug()
	{
		Damage( 30.0f );
	}

	public void Damage( float damage )
	{
		if(!Alive) return;
		Health -= damage;

		if ( damage >= 0 )
		{
			_healthRegen = 5f;
		}
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
			Kill();
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


	public async void Kill()
	{
		Alive = false;
		DeathAnimtion();

		await Task.DelaySeconds( 1f );

		GameObject.Destroy();
	}
}

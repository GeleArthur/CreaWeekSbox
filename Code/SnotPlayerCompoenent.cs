using Sandbox;

public sealed class SnotPlayerCompoenent : Component
{
	[Property]
	public PlayerController Controller { get; set; }
	
	[Property]
	public SkinnedModelRenderer ModelRenderer { get; set; }
	
	[Property]
	public Slime UnitComponent { get; set; }
	
	protected override void OnUpdate()
	{

	}

	[Button]
	public void Ragdoll()
	{
		if(!ModelRenderer.IsValid) return;
		
		var ragdoll = AddComponent<ModelPhysics>();
		ragdoll.Renderer = ModelRenderer;
		ragdoll.Model = ModelRenderer.Model;

		// Controller.UseInputControls = false;
	}
}

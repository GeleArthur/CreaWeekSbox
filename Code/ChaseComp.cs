using Sandbox;

public sealed class ChaseComp : Component
{
	[Property]
	public string Name { get; set; }
	
	[Property]
	private GameObject target;
	private NavMeshAgent agent;
	protected override void OnStart()
	{
		agent = GameObject.GetComponent<NavMeshAgent>();
	}
	protected override void OnUpdate()
	{
		agent.MoveTo( target.WorldPosition );
		//WorldPosition = (Vector3)Scene.NavMesh.GetRandomPoint();
	}


}

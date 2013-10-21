var target : Transform;

function Update()
{
	GetComponent(NavMeshAgent).destination = target.position;
}
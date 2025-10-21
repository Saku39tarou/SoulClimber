using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
	[SerializeField] Transform[] goals;
	private int destNum = 0;
	private NavMeshAgent agent;

	// Start is called before the first frame update
	void Start()
    {
        agent = GetComponent<NavMeshAgent>();
		agent.destination = goals[destNum].position;
    }

	void nextGoal()
	{
		destNum += 1;
		if(destNum == 4)
		{
			destNum = 0;
		}

		agent.destination = goals[destNum].position;

		//Debug.Log(destNum);
	}

    // Update is called once per frame
    void Update()
    {
		Debug.Log(agent.remainingDistance);
		if (agent.remainingDistance == 0)
		{
			nextGoal();
		}
    }

	private void OnTriggerStay(Collider other)
	{
		if(other.CompareTag("Palyer"))
		{
			agent.speed = 0;
			transform.LookAt(Vector3.Lerp(transform.forward + transform.position, other.transform.position, 0.05f), Vector3.down);
		}
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
	[SerializeField] Transform[] goals;
	[SerializeField] GameObject shotItem;
	[SerializeField] float shotSpeed;
	[SerializeField] GameObject enemyBody;

	private float wateTime = 2.0f;
	[SerializeField] float countTime;
	private int destNum = 0;
	private NavMeshAgent agent;

	enum Mode
	{
		Search,
		Attack,
	}
	[SerializeField] Mode mode;
	// Start is called before the first frame update
	void Start()
    {
        agent = GetComponent<NavMeshAgent>();
		agent.destination = goals[destNum].position;
    }

	void nextGoal()
	{
		destNum += 1;
		if(destNum == goals.Length)
		{
			destNum = 0;
		}

		agent.destination = goals[destNum].position;

		//Debug.Log(destNum);
	}

	void Shot()
	{
		GameObject shotObj = Instantiate(shotItem, enemyBody.transform.position, enemyBody.transform.rotation);
		Rigidbody bulletRigidbody = shotObj.GetComponent<Rigidbody>();
		bulletRigidbody.AddForce(enemyBody.transform.forward * shotSpeed);
	}

    // Update is called once per frame
    void Update()
    {
		if(mode == Mode.Search)
		{
			if (agent.remainingDistance == 0)
			{
				nextGoal();
			}
		}

		if(mode == Mode.Attack)
		{
			countTime -= Time.deltaTime;
			if (countTime <= 0)
			{
				Shot();
				countTime = wateTime;
			}
		}
    }

	private void OnTriggerStay(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			agent.speed = 0;
			enemyBody.transform.LookAt(other.transform);
			
			
			mode = Mode.Attack;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			agent.speed = 3.5f;
			enemyBody.gameObject.transform.rotation = Quaternion.identity;
			mode = Mode.Search;
		}
	}
}


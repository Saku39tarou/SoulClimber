using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWalk : MonoBehaviour
{
	[SerializeField] GameObject player; 
	[SerializeField] Transform[] goals;

	[SerializeField] int destNum = 0;
	[SerializeField] float distance;
	[SerializeField] float angle = 45f;

	bool attack;

	private NavMeshAgent agent;
	// Start is called before the first frame update
	void Start()
    {
		agent = GetComponent<NavMeshAgent>();
		agent.destination = goals[destNum].position;
		player = GameObject.Find("Player");
		attack = false;
	}

	void nextGoal()
	{
		destNum += 1;

		if (destNum == 3 || destNum == 5)
		{
			destNum = 0;
		}

	agent.destination = goals[destNum].position;

	Debug.Log(destNum);
	}

	void EnemyAttack()
	{
		attack = true;
	}

	// Update is called once per frame
	void Update()
	{
		if (agent.remainingDistance < 1)
		{
			nextGoal();
		}

		if (destNum == 4)
		{
			EnemyAttack();
		}

		if(attack)
		{
			Debug.Log("OK");
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player") //Ž‹ŠE‚Ì”ÍˆÍ“à‚Ì“–‚½‚è”»’è
		{
			//Ž‹ŠE‚ÌŠp“x“à‚ÉŽû‚Ü‚Á‚Ä‚¢‚é‚©
			Vector3 posDelta = other.transform.position - this.transform.position;
			float target_angle = Vector3.Angle(this.transform.forward, posDelta);

			if (target_angle < angle) //target_angle‚ªangle‚ÉŽû‚Ü‚Á‚Ä‚¢‚é‚©‚Ç‚¤‚©
			{
				if (Physics.Raycast(this.transform.position, posDelta, out RaycastHit hit)) //Ray‚ðŽg—p‚µ‚Ätarget‚É“–‚½‚Á‚Ä‚¢‚é‚©”»•Ê
				{
					if (hit.collider == other)
					{
						destNum = 3;
					}
				}
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player") //Ž‹ŠE‚Ì”ÍˆÍ“à‚Ì“–‚½‚è”»’è
		{
			attack = false;
			destNum = 0;
		}
	}
}

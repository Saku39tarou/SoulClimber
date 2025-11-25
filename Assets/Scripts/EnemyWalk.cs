using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWalk : MonoBehaviour
{
	[SerializeField] GameObject player; 
	[SerializeField] Transform[] goals;

	[SerializeField] float distance;
	[SerializeField] float angle = 45f;
	[SerializeField] float Speed = 3.5f;
	[SerializeField] float enemySpeed = 10f;
	[SerializeField] float attackTime;

	private float maxTime = 3;
	private int destNum = 0;
	bool attackEnemy;

	// アニメーション
	private Animator animator;

	private NavMeshAgent agent;
	// Start is called before the first frame update
	void Start()
    {
		agent = GetComponent<NavMeshAgent>();
		agent.destination = goals[destNum].position;
		attackEnemy = false;
		animator = GetComponent<Animator>();
		attackTime = maxTime;
	}

	void nextGoal()
	{
		if (attackEnemy) return;
		destNum += 1;

		if (destNum == goals.Length)
		{
			destNum = 0;
		}

	agent.destination = goals[destNum].position;
	}

	void EnemyAttack()
	{
		attackTime -= Time.deltaTime;
		agent.destination = player.transform.position;
		agent.speed = enemySpeed;
		animator.SetBool("Attack",true);

		if (attackTime <= 0)
		{
			attackEnemy = false;
			animator.SetBool("Attack", false);
			attackTime = maxTime;
			agent.speed = Speed;
			Debug.Log(destNum);
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (agent.remainingDistance < 1)
		{
			nextGoal();
		}

		if(attackEnemy)
		{
			EnemyAttack();
			Debug.Log("OK");
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player") //視界の範囲内の当たり判定
		{
			//視界の角度内に収まっているか
			Vector3 posDelta = other.transform.position - this.transform.position;
			float target_angle = Vector3.Angle(this.transform.forward, posDelta);

			if (target_angle < angle) //target_angleがangleに収まっているかどうか
			{
				if (Physics.Raycast(this.transform.position, posDelta, out RaycastHit hit)) //Rayを使用してtargetに当たっているか判別
				{
					attackEnemy = true;
					this.gameObject.transform.LookAt(other.transform);
				}
			}
		}
	}

	
}

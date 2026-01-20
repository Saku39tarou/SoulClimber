using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
	
	[SerializeField] GameObject shotItem;
	[SerializeField] float shotSpeed;
	[SerializeField] GameObject enemyBody;

	private float wateTime = 2.0f;
	[SerializeField] float countTime;
	
	

	enum Mode
	{
		Search,
		Attack,
	}
	[SerializeField] Mode mode;
	// Start is called before the first frame update
	void Start()
    {
       
		
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
			
			enemyBody.transform.LookAt(other.transform);
			
			
			mode = Mode.Attack;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			
			enemyBody.gameObject.transform.rotation = Quaternion.identity;
			mode = Mode.Search;
		}
	}
}


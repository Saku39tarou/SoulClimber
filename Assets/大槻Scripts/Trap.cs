using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			var status = other.GetComponent<HP>();
			status.Damage(10);
		}
	}
	/*private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Floor"))
		{
				var status = other.GetComponent<HP>();
				status.Damage(10);
		}

	}*/
}

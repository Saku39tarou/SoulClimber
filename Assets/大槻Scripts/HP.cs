using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class HP : MonoBehaviour
{/*
	public int healAmount = 20;

	private void OnTriggerEnter(Collider other)
	{
		PlayerHealth health = other.GetComponent<PlayerHealth>();
		if (health != null)
		{
			health.Heal(healAmount);
			Destroy(gameObject); // âÒïúÉAÉCÉeÉÄÇè¡Ç∑
		}
	}*/


	
	[SerializeField] UnityEvent onDieCallback = new UnityEvent();
	[SerializeField] float maxHp=100f;	
	[SerializeField] Slider HpBar;

    void Start()
    {
	
        if(HpBar != null)
		{
			HpBar.value = maxHp;
		}
    }
	
	public void Damage(float damage)
	{
		if (maxHp <= 0) return;
		
		maxHp -= damage;

		if(HpBar != null)
		{
			HpBar.value = maxHp;
		}
		if(maxHp<=0)
		{
			OnDie();
		}
	}
	
	
	private void OnTriggerEnter(Collider collision)
	{
		//if(collision.gameObject.tag=="recover")
		if(collision.CompareTag("recover"))
		{
			HpBar.value +=1;
			Destroy(collision.gameObject);
		}
		if(collision.CompareTag("NOrecover"))
		{
			HpBar.value -=1;
			Destroy(collision.gameObject);
		}
	
	}

	public void TakeDamage(float damage)
	{
		HpBar.value -= damage;
	}
	void OnDie()
	{
		onDieCallback.Invoke();
	}

	private bool IsGrounded()
	{
		return false;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class HP : MonoBehaviour
{
	[SerializeField] UnityEvent onDieCallback = new UnityEvent();
	[SerializeField] int life=100;	
	[SerializeField] Slider HpBar;

    void Start()
    {
	
        if(HpBar != null)
		{
			HpBar.value = life;
		}
    }

    public void Damage(int damage)
	{
		if (life <= 0) return;

		life -= damage;



		if(HpBar != null)
		{
			HpBar.value = life;
		}
		if(life<=0)
		{
			OnDie();
		}
	}

	private void OnTriggerEnter(Collider collision)
	{
		if(collision.gameObject.tag=="recover")
		{
			HpBar.value += 10;
			Destroy(collision.gameObject);
		}
	}

	public void TakeDamage(int damage)
	{
		HpBar.value -= damage;
	}
	void OnDie()
	{
		onDieCallback.Invoke();
	}
}

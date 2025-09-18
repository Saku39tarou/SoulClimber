using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HP : MonoBehaviour
{
	[SerializeField] UnityEvent onDieCallback = new UnityEvent();
	[SerializeField] int life = 100;
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

	void OnDie()
	{
		onDieCallback.Invoke();
	}
}

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class HP : MonoBehaviour
{
	
	[SerializeField] UnityEvent onDieCallback = new UnityEvent();	
	[SerializeField] float maxHp=1f;	
	//[SerializeField] float maxHp=100f;	
	[SerializeField] Slider HpBar;



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

	
	//Unity側のHPBarの設定しているValueの値で設定する必要
	private void OnTriggerEnter(Collider collision)
	{
		//if(collision.gameObject.tag=="recover")
		if(collision.CompareTag("recover"))
		{
			HpBar.value +=0.1f;
			Destroy(collision.gameObject);
		}
		if(collision.CompareTag("NOrecover"))
		{
			HpBar.value -=0.1f;
			Destroy(collision.gameObject);
		}
		
	}

	//毒ダメージ
	private int count;
	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("PoisonGas"))
		{
			count += 1;//秒ごとに
			
			if (count % 100 == 0)
			{
				HpBar.value -= 0.1f;//Sliderを減らす
			}
			
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

	void GoToGameover()
	{
		SceneManager.LoadScene("GameOver");
	}
}

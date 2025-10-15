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

	//[SerializeField] float startFallY;
	//[SerializeField] int FallDamage;	//プレイヤーが　m落ちたらダメージ

    void Start()
    {
	
        if(HpBar != null)
		{
			HpBar.value = life;
		}
    }

	//Rigidbodyで　地上の。
	/*private void Update()
	{
		if(GetComponent<Rigidbody>().velocity.y<0 && !IsGrounded())
		{
			startFallY = transform.position.y;
		}
	}*/
	
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
		//if(collision.gameObject.tag=="recover")
		if(collision.CompareTag("recover"))
		{
			HpBar.value += 1;
			Destroy(collision.gameObject);
		}
		if(collision.CompareTag("NOrecover"))
		{
			HpBar.value -= 1;
			Destroy(collision.gameObject);
		}

		/*if (collision.CompareTag("Ground"))
		{
			float fallDistance = startFallY - transform.position.y; // 落下距離を計算
			if (fallDistance > FallDamage)
			{
				// ダメージ計算と処理
				HpBar.value -= 10; // 落下距離に応じてダメージを与える
				//FallDamage -= 10;//
			}
			startFallY = transform.position.y; // 落下開始時の高さをリセット
		}*/
	
		/*if(collision.gameObject.tag=="ground")
		{
			HpBar.value -= 10;
		}*/
	}

	public void TakeDamage(int damage)
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

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
	[SerializeField] float maxHp = 1f;
	//[SerializeField] float maxHp=100f;	
	[SerializeField] Slider HpBar;



	public void Damage(float damage)
	{
		if (maxHp <= 0) return;

		maxHp -= damage;

		if (HpBar != null)
		{
			HpBar.value = maxHp;
		}
		if (maxHp <= 0)
		{
			OnDie();
		}
	}


	//Unity側のHPBarの設定しているValueの値で設定する必要
	private void OnTriggerEnter(Collider collision)
	{
		//if(collision.gameObject.tag=="recover")
		if (collision.CompareTag("recover"))
		{
			HpBar.value += 0.1f;
			Destroy(collision.gameObject);
		}
		if (collision.CompareTag("NOrecover"))
		{
			HpBar.value -= 0.1f;
			Destroy(collision.gameObject);
		}
		//playerが触れても消えない
		if(collision.CompareTag("EnemyDame"))
		{
			HpBar.value -= 0.1f;
		}

	}


	//毒ダメージ
	private int count;

	[SerializeField] GameObject gas;

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("PoisonGas"))
		{
			count += 1;//秒ごとに

			if (count % 100 == 0)
			{
				HpBar.value -= 0.1f;//Sliderを減らす
			}
			Destroy(gas,8);
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

	/*private bool IsGrounded()
	{
		return false;
	}*/

	void GoToGameover()
	{
		SceneManager.LoadScene("GameOver");
	}

	//落下ダメージ処理

	public float minFallDistance = 5f;         // この距離以下ならノーダメージ
	public float maxFallDistance = 20f;        // この距離で最大ダメージ
											   //public float maxDamage = 10f;             // 最大ダメージ 
	public float maxDamage = 0.5f;             // 最大ダメージ
	//public float maxDamage = 0.1f;

	//private float maxHP = 100f;
	private float maxHP = 1f;
	//private float currentHP;

	private float fallStartY;
	private bool isFalling = false;

	private Rigidbody rb;

	void Start()
	{
		//currentHP = maxHP;
		HpBar.value = maxHP;
		UpdateHPBar();
		rb = GetComponent<Rigidbody>();

		if (GameOverPanel != null)
		{
			GameOverPanel.SetActive(false);
		}
	}

	void Update()
	{
		// 地面から離れた瞬間Y座標を記録する
		if (!isFalling && !IsGrounded())
		{
			isFalling = true;
			fallStartY = transform.position.y;
		}

		// 地面に着地したとき
		if (isFalling && IsGrounded())
		{
			float fallDistance = fallStartY - transform.position.y;

			if (fallDistance > minFallDistance)
			{
				float damage = CalculateFallDamage(fallDistance);
				ApplyDamage(damage);
			}

			isFalling = false;
		}
		//HPが0になったらゲームを停止
		if(HpBar.value<=0)
		{
			GameOver();
		}
	}

	float CalculateFallDamage(float fallDistance)
	{
		if (fallDistance >= maxFallDistance)
			return maxDamage;

		float t = (fallDistance - minFallDistance) / (maxFallDistance - minFallDistance);
		return t * maxDamage;
	}

	//[SerializeField] GameObject GameOverShowPanel;
	void ApplyDamage(float amount)
	{
		//currentHP -= amount;
		HpBar.value -= amount;
		//currentHP = Mathf.Max(currentHP, 0);
		HpBar.value = Mathf.Max(HpBar.value, 0);
		UpdateHPBar();

		//if (currentHP <= 0)
		/*if (maxHp <= 0)
		{
			GameOver();
		}*/
	}

	void UpdateHPBar()
	{
		if (HpBar != null)
		{
			//HpBar.value = currentHP / maxHP;
			HpBar.value =  HpBar.value/ maxHP;
		}
	}

	bool IsGrounded()
	{
		// 地面との接地判定
		return Physics.Raycast(transform.position, Vector3.down, 1.1f);
	}

	[SerializeField] GameObject GameOverPanel;

	//HPが0になったらゲームを一時停止
	void GameOver()
	{
		
		if (GameOverPanel != null)
		{
			GameOverPanel.SetActive(true);
		}

		Time.timeScale = 0;


		Debug.Log("GameOver");
		//GameOverText.GameOverShowPanel();//パネル表示
		//SceneManager.LoadScene("GameOver");//シーン遷移
	}
}

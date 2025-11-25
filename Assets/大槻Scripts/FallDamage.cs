using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FallDamage : MonoBehaviour
{/*
	public float minFallDistance = 5f;         // この距離以下ならノーダメージ
	public float maxFallDistance = 20f;        // この距離で最大ダメージ
	//public float maxDamage = 10f;             // 最大ダメージ 
	public float maxDamage = 0.1f;             // 最大ダメージ
	public Slider HpBar;                       
	
	//private float maxHP = 100f;
	private float maxHP = 1f;
	private float currentHP;

	private float fallStartY;
	private bool isFalling = false;

	private Rigidbody rb;

	void Start()
	{
		currentHP = maxHP;
		UpdateHPBar();
		rb = GetComponent<Rigidbody>();
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
	}

	float CalculateFallDamage(float fallDistance)
	{
		if (fallDistance >= maxFallDistance)
			return maxDamage;

		float t = (fallDistance - minFallDistance) / (maxFallDistance - minFallDistance);
		return t * maxDamage;
	}

	void ApplyDamage(float amount)
	{
		currentHP -= amount;
		currentHP = Mathf.Max(currentHP, 0);
		UpdateHPBar();
	}

	void UpdateHPBar()
	{
		if (HpBar != null)
		{
			HpBar.value = currentHP / maxHP;
		}
	}

	bool IsGrounded()
	{
		// 地面との接地判定
		return Physics.Raycast(transform.position, Vector3.down, 1.1f);
	}*/
}





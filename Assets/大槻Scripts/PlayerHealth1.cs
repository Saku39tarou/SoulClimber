using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHealth1 : MonoBehaviour
{
	public Slider hpBar;
	//public float maxHP = 1;
	public float maxHP = 0.4f;
	public float currentHP;

	void Start()
	{
		//currentHP = maxHP;
		UpdateHPBar();
	}

	public void Heal(float amount)
	{
		currentHP += amount;
		if (currentHP > maxHP)
			currentHP = maxHP;
		//hpBar.value += 0.1f;
		UpdateHPBar();
	}

	void UpdateHPBar()
	{
		//hpBar.value = (float)currentHP / maxHP;
		hpBar.value += 0.5f;
	}
}

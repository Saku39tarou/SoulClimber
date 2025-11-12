using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{
	public Slider hpBar;
	//public float maxHP = 1;
	public float maxHP = 0.1f;
	public float currentHP;

	void Start()
	{
		currentHP = maxHP;
		UpdateHPBar();
	}

	public void Heal(float amount)
	{
		currentHP += amount;
		if (currentHP > maxHP)
			currentHP = maxHP;
		UpdateHPBar();
	}

	void UpdateHPBar()
	{
		hpBar.value = (float)currentHP / maxHP;
	}
	
}







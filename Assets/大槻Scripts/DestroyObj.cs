using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObj : MonoBehaviour
{
	// Destroyする時間を指定する
	public float time = 2;

	// DestoryしたいGameObject(基本はアタッチされたもの)
	public GameObject gameObject;

	// Use this for initialization
	void Start()
	{
		// Destory
		Destroy(gameObject, time);
	}

}

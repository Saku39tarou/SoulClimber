using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpornObj : MonoBehaviour
{
	[SerializeField] GameObject cubeObject;

	void Start()
	{
		cubeObject.SetActive(false);
		Invoke("CubeSet", 10.0f);
	}


	void CubeSet()
	{
		cubeObject.SetActive(true);
	}
}

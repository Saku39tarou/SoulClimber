using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
	[SerializeField] GameObject flag;
	[SerializeField] GameObject _clear;

    // Start is called before the first frame update
    void Start()
    {
		flag.SetActive(false);
		_clear.SetActive(false);
	}

	// Update is called once per frame
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			flag.SetActive(true);
			_clear.SetActive(true);
		}
	}
}

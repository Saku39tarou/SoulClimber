using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fog : MonoBehaviour
{
	// 変更したいマテリアルをアサイン
	[SerializeField] GameObject smoke;

	[SerializeField] float _maxTime = 3.0f;
	[SerializeField] float _time;

	// Start is called before the first frame update
	void Start()
	{
		smoke.SetActive(false);
		_time = _maxTime;
	}

	void Update()
	{
		if(!smoke)
		{
			if (_time <= 0)
			{
				smoke.SetActive(false);
				_time = _maxTime;
			}
		}
		
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			smoke.SetActive(true);
			_time -= Time.deltaTime; // 経過時間を計算		
		}
	}

	private void OnTriggerExit(Collider other)
	{
		smoke.SetActive(false);
	}
}

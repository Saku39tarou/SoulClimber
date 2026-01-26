using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fog : MonoBehaviour
{
	// 変更したいマテリアルをアサイン
	[SerializeField] GameObject smoke;

	// Start is called before the first frame update
	void Start()
	{
		smoke.SetActive(false);
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player") //視界の範囲内の当たり判定
		{
			smoke.SetActive(true);
		}

	}
}

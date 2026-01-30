using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.LookDev;

public class RandomSpawn : MonoBehaviour
{
	[SerializeField] GameObject obj;
	[SerializeField] GameObject[] objSpawnPos;
	[SerializeField] int spawnValue;
	[Header("true/falseValueの数値はspawnValueより低く設定してください")]
	[SerializeField] int trueValue;
	[SerializeField] int falseValue;
	private SkySystem.Sky skySystem;

	[SerializeField]List<GameObject> beeList = new List<GameObject>();
	int objCount;
	GameObject objs;
	//[SerializeField]GameObject instance;
	// Start is called before the first frame update


	void OnEnable()
    {

		//int truecount = 0;
		//int falsecount = 0;

		////9個のスポーン地点からランダムに4個敵を生成
		//for (int i = 0; i < 9; i++)
		//{
		//	bool draw = false;
		//	if (truecount < 4 && falsecount < 5)
		//	{
		//		int num = Random.Range(0, 2);
		//		if (num == 1)
		//		{
		//			draw = true;
		//			truecount++;
		//		}
		//		else
		//		{
		//			draw = false;
		//			falsecount++;
		//		}
		//	}
		//	else if (truecount >= 4)
		//	{
		//		draw = false;
		//	}
		//	else if (falsecount >= 5)
		//	{
		//		draw = true;
		//	}

		//	if (draw)
		//	{
		//		Instantiate(bird, birdSpawnPos[i].transform.position, birdSpawnPos[i].transform.rotation);
		//	}
		//}

		
	}

	// Update is called once per frame
	void Update()
    {
		//if(skySystem == SkySystem.Sky.Day)
		//{
		//	Destroy(bird);
		//}
    }

	public void Spawn()
	{
		int truecount = 0;
		int falsecount = 0;

		foreach (GameObject objs in beeList)
		{
			Destroy(objs);
		}
		beeList.Clear();

		//9個のスポーン地点からランダムに4個敵を生成
		for (int i = 0; i < spawnValue; i++)
		{
			bool draw = false;
			if (truecount < trueValue && falsecount < falseValue)
			{
				int num = Random.Range(0, 2);
				if (num == 1)
				{
					draw = true;
					truecount++;
				}
				else
				{
					draw = false;
					falsecount++;
				}
			}
			else if (truecount >= trueValue)
			{
				draw = false;
			}
			else if (falsecount >= falseValue)
			{
				draw = true;
			}

			
			if (draw)
			{
				objs = GameObject.Instantiate(obj, objSpawnPos[i].transform.position, objSpawnPos[i].transform.rotation);
				beeList.Add(objs);
				objCount++;
			}
		}


	}
}

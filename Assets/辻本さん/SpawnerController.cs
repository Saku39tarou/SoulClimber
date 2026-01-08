using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
	[SerializeField] GameObject enemyRandomSpawner;
	[SerializeField] SkySystem skyObject;

	
	
    // Start is called before the first frame update
    void Awake()
    {
		skyObject.GetComponent<SkySystem>();
        enemyRandomSpawner.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
		
        if(skyObject.skyState == SkySystem.Sky.Night)
		{
			enemyRandomSpawner.SetActive(true);
		}
		if(skyObject.skyState == SkySystem.Sky.Day)
		{
			enemyRandomSpawner.SetActive(false);
		}
    }
}

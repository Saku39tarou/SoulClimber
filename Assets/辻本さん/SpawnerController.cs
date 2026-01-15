using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
	[SerializeField] RandomSpawn randomSpawner;
	[SerializeField] SkySystem skyObject;

	bool spawn = false;
	
    // Start is called before the first frame update
    void Awake()
    {
		skyObject.GetComponent<SkySystem>();
		randomSpawner.GetComponent<RandomSpawn>();
    }

    // Update is called once per frame
    void Update()
    {
		
        if(skyObject.skyState == SkySystem.Sky.Night && spawn)
		{
			spawn = false;
		}
		if(skyObject.skyState == SkySystem.Sky.Day && !spawn)
		{
			
			randomSpawner.Spawn();
			
			spawn = true;
		}
    }
}

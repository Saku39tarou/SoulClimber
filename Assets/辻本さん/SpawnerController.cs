using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
	[SerializeField] RandomSpawn randomSpawner;
	[SerializeField] SkySystem skyObject;

	bool spawnDay = true;
	bool spawnNight = false;
	
    // Start is called before the first frame update
    void Awake()
    {
		skyObject.GetComponent<SkySystem>();
		randomSpawner.GetComponent<RandomSpawn>();
    }

    // Update is called once per frame
    void Update()
    {
		
        if(skyObject.skyState == SkySystem.Sky.Night && !spawnDay && spawnNight)
		{
			randomSpawner.Spawn();
			spawnDay = true;
			spawnNight = false;
		}
		if(skyObject.skyState == SkySystem.Sky.Day && spawnDay && !spawnNight)
		{
			randomSpawner.Spawn();
			spawnDay = false;
			spawnNight = true;
		}
    }
}

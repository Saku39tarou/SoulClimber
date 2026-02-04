using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
	[SerializeField] RandomSpawn BeeSpawner;
	[SerializeField] RandomSpawn AppleSpawner;
	[SerializeField] RandomSpawn CakeSpawner;
	[SerializeField] RandomSpawn WaterMelonSpawner;
	[SerializeField] SkySystem skyObject;

	[Header("êîílÇÕspawnValueÇ…âûÇ∂Çƒê›íËÇµÇƒÇ≠ÇæÇ≥Ç¢")]
	[SerializeField] int beeDayTrue;
	[SerializeField] int beeDayFalse;
	[SerializeField] int beeNightTrue;
	[SerializeField] int beeNightFalse;

	[SerializeField] int ItemDaytrue;
	[SerializeField] int ItemDayfalse;
	[SerializeField] int ItemNightTrue;
	[SerializeField] int ItemNightFalse;

	bool spawnDay = true;
	bool spawnNight = false;
	
    // Start is called before the first frame update
    void Awake()
    {
		skyObject.GetComponent<SkySystem>();
		BeeSpawner.GetComponent<RandomSpawn>();
		AppleSpawner.GetComponent<RandomSpawn>();
		CakeSpawner.GetComponent<RandomSpawn>();
		WaterMelonSpawner.GetComponent <RandomSpawn>();

	}

    // Update is called once per frame
    void Update()
    {
		if (skyObject.skyState == SkySystem.Sky.Day && spawnDay && !spawnNight)
		{
			BeeSpawner.trueValue = beeDayTrue;
			BeeSpawner.falseValue = beeDayFalse;
			BeeSpawner.Spawn();

			AppleSpawner.trueValue = ItemDaytrue;
			AppleSpawner.falseValue = ItemDayfalse;
			AppleSpawner.Spawn();

			WaterMelonSpawner.trueValue = ItemDaytrue;
			WaterMelonSpawner.falseValue = ItemDayfalse;
			WaterMelonSpawner.Spawn();

			CakeSpawner.Spawn();
			
			spawnDay = false;
			spawnNight = true;
		}

		if (skyObject.skyState == SkySystem.Sky.Night && !spawnDay && spawnNight)
		{
			BeeSpawner.trueValue = beeNightTrue;
			BeeSpawner.falseValue = beeNightFalse;
			BeeSpawner.Spawn();
			AppleSpawner.trueValue = ItemNightTrue;
			AppleSpawner.falseValue = ItemNightFalse;
			AppleSpawner.Spawn();

			WaterMelonSpawner.trueValue = ItemNightTrue;
			WaterMelonSpawner.falseValue = ItemNightFalse;
			WaterMelonSpawner.Spawn();

			CakeSpawner.Spawn();
			
			spawnDay = true;
			spawnNight = false;
		}
		
    }
}

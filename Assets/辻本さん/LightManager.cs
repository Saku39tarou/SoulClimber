using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
	[SerializeField]
	GameObject sun;
	[SerializeField]
	GameObject moon;
	[SerializeField]
	float sunStrength;
	[SerializeField]
	float moonStrength;

	[SerializeField] SkySystem skyObject;
	// Start is called before the first frame update
	void Start()
    {
		sunStrength = sun.GetComponent<Light>().intensity;
	}

    // Update is called once per frame
    void Update()
    {
		sun.GetComponent<Light>().intensity = sunStrength;



		if (skyObject.skyState == SkySystem.Sky.Night)
		{
			if(sunStrength >= 0.1f)
			{
				sunStrength -= 0.01f;
			}
		}
		if (skyObject.skyState == SkySystem.Sky.Day)
		{
			if (sunStrength <= 1.0f)
			{
				sunStrength += 0.01f;
			}
		}
	}
}

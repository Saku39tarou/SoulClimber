using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fogcolor : MonoBehaviour
{

	[SerializeField] GameObject fogObject;
	[SerializeField] Color fogColor;
	[SerializeField] float colorValue;

	[SerializeField] float _time = 180.0f;


	bool night;
	// Start is called before the first frame update
	void Start()
    {
		fogColor = fogObject.GetComponent<Light>().color;
		colorValue = 255.0f;
		night = false;
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKey(KeyCode.RightArrow))
		{
			fogObject.GetComponent<Light>().color = fogColor;
			fogColor = new Color(colorValue, colorValue, colorValue);
			if (colorValue < 1.0f)
			{
				colorValue += 0.01f;
			}
		}

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			fogObject.GetComponent<Light>().color = fogColor;
			fogColor = new Color(colorValue, colorValue, 1);
			if (colorValue > 0)
			{
				colorValue -= 0.01f;
			}
		}
	}
}

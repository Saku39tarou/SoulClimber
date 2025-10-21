using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightChecker : MonoBehaviour
{
	private TimeOfDay timeOfDay;
	private bool isDay;

	private void Start()
	{
		timeOfDay = FindAnyObjectByType<TimeOfDay>();
	}
}

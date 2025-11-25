using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
	AudioSource Audio;

	private void Start()
	{
		Audio = GetComponent<AudioSource>();
	}

	public void PlayStart()
	{
		Audio.PlayOneShot(Audio.clip);
	}
}

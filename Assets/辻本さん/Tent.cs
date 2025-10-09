using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering;

public class Tent : MonoBehaviour
{
	[SerializeField] GameObject climbPlayer;
	[SerializeField] GameObject ghostPlayer;
	bool onPlayer = false;

	private PlayerController.State controller;
	// Start is called before the first frame update
	void Start()
    {
        ghostPlayer.SetActive(false);
		ghostPlayer.GetComponent<PlayerController>();
		
    }

    // Update is called once per frame
    void Update()
    {

		if (onPlayer && Input.GetKeyDown(KeyCode.E))
		{
			Debug.Log("E‚ª‰Ÿ‚³‚ê‚½");
			ghostPlayer.SetActive(true);
			climbPlayer.SetActive(false);
		}

	}

	private void OnTriggerStay(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			onPlayer = true;
		}
	}
}

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
	bool onGhost = false;

	[SerializeField] PlayerController.State changeState;
	// Start is called before the first frame update
	void Start()
    {
        ghostPlayer.SetActive(false);
		
		
	}

    // Update is called once per frame
    void Update()
    {

		if (onPlayer && Input.GetKeyDown(KeyCode.E))
		{
			Debug.Log("E‚ª‰Ÿ‚³‚ê‚½");
			ghostPlayer.SetActive(true);
			climbPlayer.SetActive(false);
			GameObject.FindWithTag("Ghost").GetComponent<PlayerController>().SetState(changeState);
			changeState = PlayerController.State.Walk;
		}

		if (onGhost && Input.GetKeyDown(KeyCode.E))
		{
			Debug.Log("E‚ª‰Ÿ‚³‚ê‚½");
			ghostPlayer.SetActive(false);
			climbPlayer.SetActive(true);
			GameObject.FindWithTag("Player").GetComponent<PlayerController>().SetState(changeState);
			changeState = PlayerController.State.Ghost;
		}

	}

	private void OnTriggerStay(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			onPlayer = true;
		}

		if(other.gameObject.CompareTag("Ghost"))
		{
			onGhost = true;
		}
	}
}

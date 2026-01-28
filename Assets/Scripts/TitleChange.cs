using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleChange : MonoBehaviour
{
	private string selectButton;

	// Start is called before the first frame update
	public void PlessButon(string Name)
	{
		selectButton = Name;

		switch (Name)
		{
			case "START":
				SceneManager.LoadScene("GameScene");


				break;

		}
	}
}

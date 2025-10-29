using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log("player‚É“–‚½‚Á‚½");
			Destroy(this.gameObject);
		}
		if(other.CompareTag("Stage"))
		{
			Destroy(this.gameObject);
		}
	}

	
}

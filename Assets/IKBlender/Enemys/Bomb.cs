using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{ 
	[Header("”š”­‚Ü‚Å‚ÌŠÔ[s]")]
	[SerializeField] float _time = 3.0f;
	
	[Header("”š•—‚ÌPrefab")]
	[SerializeField]  Explosion _explosionPrefab;

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			Invoke(nameof(Explode), _time);
		}
	}
	
	private void Explode()
	{
		// ”š”­‚ğ¶¬
		var explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
		explosion.Explode();
	
		// ©g‚ÍÁ‚¦‚é
		Destroy(gameObject);
	}
}

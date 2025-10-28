using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowAwayObject : MonoBehaviour
{
	[Header("”š”­‚Ü‚Å‚ÌŠÔ[s]")]
	[SerializeField] private float _time = 3.0f;

	[Header("”š•—‚ÌPrefab")]
	[SerializeField] private Explosion _explosionPrefab;

	private void Start()
	{
		// ˆê’èŠÔŒo‰ßŒã‚É”­‰Î
		Invoke(nameof(Explode), _time);
	}

	private void Explode()
	{
		// ”š”­‚ğ¶¬
		Explosion explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
		explosion.Exploade();

		// ©M‚ÍÁ‚¦‚é
		Destroy(gameObject);
	}
}

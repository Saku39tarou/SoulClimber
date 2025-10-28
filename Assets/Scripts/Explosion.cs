using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Explosion : MonoBehaviour
{
	[Header("爆風に当たった時に吹っ飛ぶ力の強さ")]
	[SerializeField] private float _explosivePower;

	[Header("爆風の判定が実際に発生するまでのディレイ")]
	[SerializeField] private float _startDelaySeconds = 0.1f;

	[Header("爆風の持続フレーム数")]
	[SerializeField] private int _durationFrameCount = 1;

	[Header("エフェクト含めすべての再生が終了するまでの時間")]
	[SerializeField] private float _stopSeconds;

	[SerializeField] private ParticleSystem _effect;
	[SerializeField] private AudioSource _sfx;
	[SerializeField] private SphereCollider _collider;

	private void Awake()
	{
		_effect.Stop();
		_sfx.Stop();
		_collider.enabled = false;
	}

	public void Exploade()
	{
		// 当たり判定管理のコルーチン
		StartCoroutine(ExplodeCoroutine());
		// エフェクト含めて諸々を消すコルーチン
		StartCoroutine(StopCoroutine());

		// エフェクトと効果音再生
		_effect.Play();
		_sfx.Play();
	}

	private IEnumerator ExplodeCoroutine()
	{
		// 指定秒数が経過するまでFixedUpdate上で待つ
		Single delayCount = Mathf.Max(0, _startDelaySeconds);
		while(delayCount > 0)
		{
			yield return new WaitForFixedUpdate();
			delayCount -= Time.fixedDeltaTime;
		}

		// 時間経過したらコライダーを有効化して爆発の当たり判定が出る
		_collider.enabled = true;

		// 一定フレーム数有効化
		for (int i = 0; i < _durationFrameCount; i++)
		{
			yield return new WaitForFixedUpdate();
		}

		// 当たり判定無効化
		_collider.enabled = false;
	}

	private IEnumerator StopCoroutine()
	{
		// 時間経過後に消す
		yield return new WaitForSeconds(_stopSeconds);
		_effect.Stop();
		_sfx.Stop();
		_collider.enabled = false;

		Destroy(gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		// 衝突対象がRigidbodyの配下であるかを調べる
		Rigidbody rigidBody = other.GetComponentInParent<Rigidbody>();

		// Rigidbodyがついてないなら吹っ飛ばないの終わる
		if (rigidBody == null) return;

		// 爆風によって爆発中央から吹っ飛ぶ方向のベクトルを作る
		Vector3 direction = (other.transform.position - transform.position).normalized;

		// 吹っ飛ばす
		rigidBody.AddForce(direction * _explosivePower, ForceMode.VelocityChange);
	}
}

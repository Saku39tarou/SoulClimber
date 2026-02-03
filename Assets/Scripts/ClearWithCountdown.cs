using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ClearWithCountdown : MonoBehaviour
{
	[Header("Scene")]
	[SerializeField] private string clearSceneName = "ClearScene";

	[Header("Target")]
	[SerializeField] private string targetTag = "Player";

	[Header("Countdown")]
	[SerializeField] private float countdownSeconds = 3f;
	[SerializeField] private bool cancelIfExit = true; // 範囲から出たらキャンセルするか

	[Header("UI")]
	[SerializeField] private TextMeshProUGUI countdownText;

	private Coroutine countdownRoutine;
	private bool isCounting = false;

	private void Awake()
	{
		if (countdownText != null)
		{
			countdownText.text = "";
			countdownText.gameObject.SetActive(false);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag(targetTag)) return;

		if (isCounting) return; // 既にカウント中なら二重起動防止

		countdownRoutine = StartCoroutine(CountdownAndLoad());
	}

	private void OnTriggerExit(Collider other)
	{
		if (!other.CompareTag(targetTag)) return;

		if (cancelIfExit)
		{
			CancelCountdown();
		}
	}

	private IEnumerator CountdownAndLoad()
	{
		isCounting = true;

		if (countdownText != null)
		{
			countdownText.gameObject.SetActive(true);
		}

		float t = countdownSeconds;

		// 例：3.0〜2.0は「3」、2.0〜1.0は「2」…という表示
		while (t > 0f)
		{
			int display = Mathf.CeilToInt(t);

			if (countdownText != null)
				countdownText.text = display.ToString();

			t -= Time.deltaTime;
			yield return null;
		}

		// 少しだけ見せたい場合
		//yield return new WaitForSeconds(0.2f);

		SceneManager.LoadScene(clearSceneName);
	}

	private void CancelCountdown()
	{
		if (countdownRoutine != null)
		{
			StopCoroutine(countdownRoutine);
			countdownRoutine = null;
		}

		isCounting = false;

		if (countdownText != null)
		{
			countdownText.text = "";
			countdownText.gameObject.SetActive(false);
		}
	}
}

using UnityEngine;
using System.Collections;

public class FadeToBeContinued : MonoBehaviour {
	public MeshRenderer fade;
	public TextMesh fadeText;
	private int fading = 0;
	public float waitBeforeFade = 0;
	public float fadeSpeed = 0.01f;
	public GameObject attachedLevel = null;
	public string endingLoadScene = "Menu";
	public bool destroyOnLoad = true;

	public GameObject messagePrefab;
	public string menuSceneName = "Menu";
	public string targetSceneOverride = null;

	void Update()
	{
		if (fade != null)
		{
			if (fading != 0)
			{
				Color fadeColor = fade.material.color;
				fadeColor.a = Mathf.Clamp01(fadeColor.a + (fadeSpeed * fading));
				fade.material.color = fadeColor;
				if (fadeText != null)
				{
					Color fadeTextColor = fadeText.color;
					fadeTextColor.a = Mathf.Clamp01(fadeTextColor.a + (fadeSpeed * fading));
					fadeText.color = fadeTextColor;
				}

				if (fading > 0 && fadeColor.a >= 1 && Globals.Instance != null && attachedLevel != null)
				{
					Destroy(attachedLevel);
					Destroy(Globals.Instance.gameObject);
					StartCoroutine(FadeIn());
				}
				else if (fading < 0 && fadeColor.a <= 0)
				{
					Destroy(gameObject);
				}
			}
		}
	}

	public void StartFade()
	{
		StartCoroutine(FadeOut());
	}

	private IEnumerator FadeOut()
	{
		fade.transform.parent = null;
		if (fade.GetComponent<DontDestroyOnLoad>() == null)
		{
			fade.gameObject.AddComponent<DontDestroyOnLoad>();
		}
		
		yield return new WaitForSeconds(waitBeforeFade);
		fading = 1;
	}

	private IEnumerator FadeIn()
	{
		yield return new WaitForSeconds(waitBeforeFade / 2);
		//Debug.Log(endingLoadScene);
		//Application.LoadLevel(endingLoadScene);

		/*TODO make this work without going back to menu*/
		GameObject levelLoadObject = (GameObject)Instantiate(messagePrefab);
		levelLoadObject.name = "Level Load Message Insant";
		TranslevelMessage levelLoadMessage = levelLoadObject.GetComponent<TranslevelMessage>();
		if (levelLoadMessage != null)
		{
			levelLoadMessage.messageName = "LevelLoadInstant";
			if (targetSceneOverride == null || targetSceneOverride == "")
			{
				levelLoadMessage.message = Application.loadedLevelName;
			}
			else
			{
				levelLoadMessage.message = targetSceneOverride;
			}
		}
		Application.LoadLevel(menuSceneName);


		if (destroyOnLoad)
		{
			Destroy(gameObject);
		}
		else
		{
			yield return new WaitForSeconds(waitBeforeFade / 2);
			fading = -1;
		}
	}
}

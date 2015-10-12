using UnityEngine;
using System.Collections;

public class FadeToBeContinued : MonoBehaviour {
	public MeshRenderer fade;
	public TextMesh fadeText;
	private int fading = 0;
	public float waitBeforeFade = 0;
	public float fadeSpeed = 0.01f;
	public GameObject attachedLevel = null;
	//public string endingLoadScene = "Menu";
	public bool destroyOnLoad = true;
	private float t = 0.0f;

	private bool zoomOut = false;
	private bool zoomComplete = false;
	private Vector3 startingPos;
	private Vector3 endingPos;

	private GameObject cameraSystem;

	//public bool forceFromMenu = false;
	//public GameObject messagePrefab;
	//public string menuSceneName = "Menu";
	//public string targetSceneOverride = null;

	void Update()
	{
		if (zoomOut)
			ZoomOut ();
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

		if (!zoomOut && !zoomComplete) 
		{
			zoomOut = true;
			if(cameraSystem == null)
				cameraSystem = CameraSplitter.Instance.gameObject;
			startingPos = cameraSystem.transform.position;
			endingPos = new Vector3(startingPos.x, startingPos.y, -500.0f);
			CameraSplitter.Instance.followPlayers = false;
		}
		
		yield return new WaitForSeconds(waitBeforeFade);
		fading = 1;
	}

	private void ZoomOut()
	{
		if (t != 1)
		{
			Globals.Instance.perspectiveCamera = true;
			t = Mathf.Clamp(t + Time.deltaTime / waitBeforeFade, 0.0f, 1.0f);
			cameraSystem.transform.position = Vector3.Lerp(startingPos, endingPos, t);
		}
		else
		{
			zoomOut = false;
			zoomComplete = true;
		}
	}
	private IEnumerator FadeIn()
	{
		yield return new WaitForSeconds(waitBeforeFade / 2);
		//Debug.Log(endingLoadScene);
		//Application.LoadLevel(endingLoadScene);

		/*TODO make this work without going back to menu*/
		/*if (forceFromMenu)
		{
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
            Globals.Instance.ResetOrExit(false);
		}
		else
		{*/
			/*GameObject levelLoadObject = (GameObject)Instantiate(messagePrefab);
			levelLoadObject.name = "Level Load Message";
			TranslevelMessage levelLoadMessage = levelLoadObject.GetComponent<TranslevelMessage>();
			if (levelLoadMessage != null)
			{
				levelLoadMessage.messageName = "LevelLoad";
				if (targetSceneOverride == null || targetSceneOverride == "")
				{
					levelLoadMessage.message = Application.loadedLevelName;
				}
				else
				{
					levelLoadMessage.message = targetSceneOverride;
				}
			}*/
            
            Globals.Instance.ResetOrExit();
		//}


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

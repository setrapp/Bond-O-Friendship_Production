using UnityEngine;
using System.Collections;

public class ForceMenu : MonoBehaviour {
	public GameObject messagePrefab;
	public string menuSceneName = "Menu";
	public string targetSceneOverride = null;
	
	void Awake()
	{
		if (Globals.Instance == null)
		{
			GameObject levelLoadObject = (GameObject)Instantiate(messagePrefab);
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
			}
			Application.LoadLevel(menuSceneName);
		}
	}
}

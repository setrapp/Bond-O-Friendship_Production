using UnityEngine;
using System.Collections;

public class ForceMenu : MonoBehaviour {
	public GameObject messagePrefab;
	public string menuSceneName = "Menu";
	
	void Awake()
	{
		if (GameObject.FindGameObjectWithTag("Globals") == null)
		{
			GameObject levelLoadObject = (GameObject)Instantiate(messagePrefab);
			levelLoadObject.name = "Level Load Message";
			TranslevelMessage levelLoadMessage = levelLoadObject.GetComponent<TranslevelMessage>();
			if (levelLoadMessage != null)
			{
				levelLoadMessage.messageName = "LevelLoad";
				levelLoadMessage.message = Application.loadedLevelName;
			}
			Application.LoadLevel(menuSceneName);
		}
	}
}

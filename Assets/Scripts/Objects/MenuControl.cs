using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using InControl;

public class MenuControl : MonoBehaviour {

    public GameObject levelSelectButton;
    public GameObject startMenu;
    public GameObject levelSelect;

	public string startScene;
    InputDevice device;
	

	// Use this for initialization
	void Start () {
		if(InputManager.Devices.Count > 0)
		{
            Globals.usingController = false;
			Globals.numberOfControllers = InputManager.Devices.Count;
		}
    }


	// Update is called once per frame
    void Update()
    {
        GameObject[] messages = GameObject.FindGameObjectsWithTag("TranslevelMessage");
        bool levelLoadFound = false;
        for (int i = 0; i < messages.Length && !levelLoadFound; i++)
        {
            TranslevelMessage message = messages[i].GetComponent<TranslevelMessage>();
            if (message != null && message.messageName == "LevelLoad")
            {
                startScene = message.message;
                Destroy(message.gameObject);
            }
        }

        if(InputManager.Devices.Count > 0)
        {
            InputDevice device = InputManager.ActiveDevice;
            if (device.Action1.IsPressed || device.MenuWasPressed)//device.LeftTrigger.IsPressed && device.RightTrigger.IsPressed)
            {
                MainMenuLoadLevel();
                Globals.usingController = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))//Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.RightControl))
        {
            MainMenuLoadLevel();
            Globals.usingController = false;
        }
    }		

    public void MainMenuLoadLevel()
    {
		CameraSplitter splitter = Globals.Instance.GetComponentInChildren<CameraSplitter>();
		if (splitter != null)
		{
			splitter.splittable = true;
		}
        Application.LoadLevel(startScene);
    }

    public void MainMenuLoadLevel(string levelName)
    {
        if(levelName != null)
        {
            //Application.LoadLevel(levelName);
            startScene = levelName;
        }
    }

    
    public void LevelSelect()
    {
        startMenu.SetActive(false);
        levelSelect.SetActive(true);
        EventSystem.current.SetSelectedGameObject(levelSelectButton);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
		        Application.Quit();
        #endif
    }

}

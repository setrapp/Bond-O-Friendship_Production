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
    private int deviceCount;

	// Use this for initialization
	void Start () {
    }


	// Update is called once per frame
    void Update()
    {
        //device = InputManager.ActiveDevice;
        //Debug.Log(device.Name);
        deviceCount = InputManager.controllerCount;

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
			if (message != null && message.messageName == "LevelLoadInstant")
			{
				startScene = message.message;
				Destroy(message.gameObject);
				MainMenuLoadLevel();
			}
        }

        if(deviceCount > 0)
        {
            device = InputManager.ActiveDevice;
            if (device.AnyButton)//device.LeftTrigger.IsPressed && device.RightTrigger.IsPressed)
            {
                Globals.Instance.leftControllerIndex = InputManager.Devices.IndexOf(device);
                Globals.Instance.leftControllerPreviousIndex = Globals.Instance.leftControllerIndex;
                Globals.Instance.rightContollerIndex = -2;
                Globals.Instance.rightControllerPreviousIndex = -2;

                if(deviceCount == 1)
                {
                    Globals.Instance.player1Controls.controlScheme = Globals.ControlScheme.SharedLeft;
                    Globals.Instance.player2Controls.controlScheme = Globals.ControlScheme.SharedRight;
                    Globals.Instance.player1Controls.inputNameSelected = Globals.InputNameSelected.LeftController;
                    Globals.Instance.player2Controls.inputNameSelected = Globals.InputNameSelected.LeftController;
                }
                else
                {
                    Globals.Instance.player1Controls.controlScheme = Globals.ControlScheme.Solo;
                    Globals.Instance.player2Controls.controlScheme = Globals.ControlScheme.Solo;
                    Globals.Instance.player1Controls.inputNameSelected = Globals.InputNameSelected.LeftController;
                    Globals.Instance.player2Controls.inputNameSelected = Globals.InputNameSelected.RightController;
                }

                //Debug.Log(Globals.Instance.player1ControlScheme);
                MainMenuLoadLevel();
                return;
               // Globals.usingController = true;
            }
        }
        if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))//Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.RightControl))
        {
            if (deviceCount == 0)
            {
                Globals.Instance.leftControllerIndex = -3;
                Globals.Instance.rightContollerIndex = -3;
            }
            else if(deviceCount == 1)
            {
                Globals.Instance.leftControllerIndex = -2;
                Globals.Instance.rightContollerIndex = -3;
            }
            else
            {
                Globals.Instance.leftControllerIndex = -2;
                Globals.Instance.rightContollerIndex = -2;
            }

            Globals.Instance.player1Controls.controlScheme = Globals.ControlScheme.SharedLeft;
            Globals.Instance.player2Controls.controlScheme = Globals.ControlScheme.SharedRight;
            Globals.Instance.player1Controls.inputNameSelected = Globals.InputNameSelected.Keyboard;
            Globals.Instance.player2Controls.inputNameSelected = Globals.InputNameSelected.Keyboard;
            MainMenuLoadLevel();
           // Globals.usingController = false;
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

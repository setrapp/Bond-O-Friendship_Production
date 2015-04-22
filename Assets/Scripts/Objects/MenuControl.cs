using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using InControl;

public class MenuControl : MonoBehaviour {

    public GameObject startMenu;

    public GameObject inputSelect;

	public string startScene;
    InputDevice device;
    private int deviceCount;

    public bool player1Ready = false;
    public bool player2Ready = false;
    private bool inputSelected = false;

    private float t = 1f;
    public float duration = 3.0f;

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

        if (!inputSelected)
        {
            if (deviceCount > 0)
            {

                device = InputManager.ActiveDevice;
                if (device.AnyButton || device.LeftStick.HasChanged || device.RightStick.HasChanged)
                {
                    Globals.Instance.leftControllerIndex = InputManager.Devices.IndexOf(device);
                    Globals.Instance.leftControllerPreviousIndex = Globals.Instance.leftControllerIndex;
                    Globals.Instance.rightContollerIndex = -2;
                    Globals.Instance.rightControllerPreviousIndex = -2;

                    if (deviceCount == 1)
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

                    inputSelected = true;
                    inputSelect.SetActive(true);
                    return;
                }
            }
            if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))//Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.RightControl))
            {
                if (deviceCount == 0)
                {
                    Globals.Instance.leftControllerIndex = -3;
                    Globals.Instance.rightContollerIndex = -3;
                }
                else if (deviceCount == 1)
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

                inputSelected = true;
                inputSelect.SetActive(true);
                return;
            }
        }
        else
        {
            if(startMenu.activeSelf)
                FadeStartMenu();
        }

        if (player1Ready && player2Ready && inputSelected)
        {
            MainMenuLoadLevel();           
        }
        else
		{
            CameraSplitter.Instance.followPlayers = false;
		}

    }	


    private void FadeStartMenu()
    {
        if (startMenu.GetComponent<CanvasGroup>().alpha != 0.0f)
        {
            t -= Time.deltaTime / duration;
            t = Mathf.Clamp(t, 0.0f, 1.0f);
            startMenu.GetComponent<CanvasGroup>().alpha = t;
        }
        else
            startMenu.SetActive(false);
    }


    public void MainMenuLoadLevel()
    {
		CameraSplitter.Instance.splittable = true;
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


    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
		        Application.Quit();
        #endif
    }

   

}

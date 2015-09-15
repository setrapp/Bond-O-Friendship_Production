using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using InControl;

public class MenuControl : MonoBehaviour {

    public ClusterNodePuzzle newGameNodePuzzle;

    public InputFill inputFill;

    public GameObject startMenu;

    public GameObject obscureMenuPanel;

	public GameObject mainMenu;

    public GameObject begin;

    public GameObject inputSelect;

	public GameObject options;

	public GameObject exitGameConfirm;

	public enum MenuState{TitleScreen, MainMenu, Options, InputSelect, QuitGame, StartGame};

	public MenuState menuState = MenuState.TitleScreen;

	public FadeMainMenu fMainMenu;
	public FadeInputSelect fInputSelect;
	public FadeQuitGame fQuitGame;
	public FadeOptions fOptions;
	public MainMenuInputOutlines mainMenuInputOutlines;
	public FollowPlayerInputKeyboard keyboardInputFollowing;
    //public GameObject levelCover;
    //public GameObject levelCover2;

	public string startScene;
    InputDevice device;
    private int deviceCount;

    public bool player1Ready = false;
    public bool player2Ready = false;

    private bool readyUp = false;
    private bool startGame = false;
    private bool toggled = false;

    private bool inputSelected = false;

    private float f = 0f;
    private float t = 1f;
    public float duration = 3.0f;

    public float fadeInDuration = 1.5f;

    public bool fadeIn = true;

    private Color startColor;
    private Color fadeColor;

	public ClusterNodePuzzle optionsNodePuzzle;
	public ClusterNodePuzzle exitGameNodePuzzle;

	public ClusterNodePuzzle optionsBackNodePuzzle;
	public ClusterNodePuzzle optionsInputSelectNodePuzzle;

	public ClusterNodePuzzle inputSelectBackNodePuzzle;

	public ClusterNodePuzzle confirmQuitNodePuzzle;
	public ClusterNodePuzzle cancelQuitNodePuzzle;

    private bool startLevelLoaded = false;

    private bool startPanelFade = false;
    private bool zoom = true;
	private bool startZoom = false;

	// Use this for initialization
	void Awake () 
    {
        //startColor = levelCover.renderer.material.color;
       // fadeColor = new Color(startColor.r, startColor.g, startColor.b, 0.0f);
        //inputSelectRenderers = inputSelect.GetComponentsInChildren<Renderer>();   
       // Globals.Instance.allowInput = false;
		//mainMenu.SetActive (false);
		//inputSelect.SetActive (false);

        if (Application.isEditor && !Globals.Instance.zoomIntroInEditor)
        {
            fadeInDuration = .1f;
        }

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
                StartCoroutine(MainMenuLoadLevel());
            }
        }
        if (!startLevelLoaded)
            StartCoroutine(MainMenuLoadLevel());

        Globals.Instance.inMainMenu = true;
    }


	// Update is called once per frame
    void Update()
    {
		if (menuState == MenuState.TitleScreen) 
		{
			if(mainMenu.activeInHierarchy)
			mainMenu.SetActive(false);
			if(inputSelect.activeInHierarchy)
			inputSelect.SetActive(false);
			if(exitGameConfirm.activeInHierarchy)
				exitGameConfirm.SetActive(false);
			if(options.activeInHierarchy)
				options.SetActive(false);
		}
        if (obscureMenuPanel.activeSelf && startPanelFade)
        {
            FadeInFadeOut();
        }
        else
        {
            deviceCount = InputManager.controllerCount;
            if (!inputSelected)
            {
				SetInputSelected();
            }
            else
            {
                if (startMenu.activeSelf)
                    FadeStartMenu();
            }


			switch(menuState)
			{

	//Main Menu/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				case MenuState.MainMenu:

				
				if(!mainMenu.activeInHierarchy)
						mainMenu.SetActive(true);

				if(fMainMenu.f != 1)
						fMainMenu.FadeIn();


			if(!inputSelect.activeInHierarchy)
			inputSelect.SetActive(true);

            if (fInputSelect.f != 1)
                fInputSelect.FadeIn();

			if(!exitGameConfirm.activeInHierarchy)
				exitGameConfirm.SetActive(true);

            if (fQuitGame.f != 1)
                fQuitGame.FadeIn();

			if(!options.activeInHierarchy)
				options.SetActive(true);

            if (fOptions.f != 1)
                fOptions.FadeIn();

				if (newGameNodePuzzle != null && newGameNodePuzzle.solved)
				{
					newGameNodePuzzle.solved = false;
				     Globals.Instance.allowInput = false;
					menuState = MenuState.StartGame;      
				}

                if (fOptions.f == 1)
                {
                    if (!options.GetComponent<OptionsMenu>().soundChecked)
                        options.GetComponent<OptionsMenu>().CheckSoundSettings();
                }

                    if(!inputFill.allowFill)
						inputFill.allowFill = true;
					keyboardInputFollowing.setColor = true;

                    if (confirmQuitNodePuzzle != null && confirmQuitNodePuzzle.solved)
                    {
                        confirmQuitNodePuzzle.solved = false;
                        Application.Quit();
                    }

                //options.GetComponent<OptionsMenu>().soundChecked = false;
                    break;
	//Options   /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			case MenuState.Options:				
				break;
	//Input Select/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				case MenuState.InputSelect:					
					break;
	//Quit Game Confirm////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				case MenuState.QuitGame:
				
					break;
			case MenuState.StartGame:
				ToggleFadeMainMenu();
				ToggleFadeOptionsMenu();
				ToggleFadeInputSelectMenu();
				ToggleFadeExitGameConfirm();


					if (!toggled)
					{
						  Invoke("StartGame", .5f);
                          CameraSplitter.Instance.JumpToPlayers();
                          
						  toggled = true;
					}

                    if (startGame)
                    {
                        
                         CameraSplitter.Instance.followPlayers = false;
                         CameraSplitter.Instance.movePlayers = true;
                        
                        FadeControls();
                    }

				break;
			}

			if(startZoom)
			{
				if(CameraSplitter.Instance.zoomState != CameraSplitter.ZoomState.ZoomedIn)
					CameraSplitter.Instance.Zoom(false, true);
				else
				{
					CameraSplitter.Instance.followPlayers = true;
					CameraSplitter.Instance.splittable = true;
					CameraSplitter.Instance.zCameraOffset = -300.0f;
					CameraSplitter.Instance.duration = 3.0f;
					Globals.Instance.allowInput = true;

                    Destroy(GameObject.FindGameObjectWithTag("Main Menu"));
                    Globals.Instance.inMainMenu = false;

					startZoom = false;
				}
			}
        }

    }	

	private void ToggleFadeMainMenu ()
	{
		if(fMainMenu.f == 0)
		{
			if(mainMenu.activeInHierarchy)
				mainMenu.SetActive(false);
		}
		else
		{
			fMainMenu.FadeOut();
		}
	}
	private void ToggleFadeOptionsMenu()
	{
		if(fOptions.f == 0)
		{
			if(options.activeInHierarchy)
				options.SetActive(false);
		}
		else
		{
			fOptions.FadeOut();
		}
	}

	private void ToggleFadeInputSelectMenu()
	{
		if(fInputSelect.f == 0)
		{
			if(inputSelect.activeInHierarchy)
				inputSelect.SetActive(false);
		}
		else
		{
			fInputSelect.FadeOut();
		}
	}
	private void ToggleFadeExitGameConfirm()
	{
		if(fQuitGame.f == 0)
		{
			if(exitGameConfirm.activeInHierarchy)
				exitGameConfirm.SetActive(false);
		}
		else
		{
			fQuitGame.FadeOut();
		}
	}
	
	private void StartGame()
    {
        startGame = true;
    }

    private void FadeStartMenu()
    {
        if (startMenu.GetComponent<CanvasGroup>().alpha != 0.0f)
        {
            t = Mathf.Clamp(t - Time.deltaTime/1.0f, 0.0f, 1.0f);
            startMenu.GetComponent<CanvasGroup>().alpha = t;            
        }
        else
        {
            if (f != 1)
            {
               
            }
            else
            {
                inputFill.allowFill = true;
                startMenu.SetActive(false);
                t = 1.0f;
            }
        }
    }

    private void FadeControls()
    {            

		if (zoom) 
		{
			if (!Application.isEditor || Globals.Instance.zoomIntroInEditor) 
			{

				Invoke ("ZoomCamera", 0.5f);
			} 
			else 
			{
				CameraSplitter.Instance.transform.position = new Vector3 (CameraSplitter.Instance.transform.position.x, CameraSplitter.Instance.transform.position.y, CameraSplitter.Instance.startPos.z);
				CameraSplitter.Instance.splittable = true;
				CameraSplitter.Instance.followPlayers = true;
                Destroy(GameObject.FindGameObjectWithTag("Main Menu"));
                Globals.Instance.inMainMenu = false;
				Globals.Instance.allowInput = true;
			}

			zoom = false;
		}

        
	}

    private void FadeInFadeOut()
    {
        
            if (t != 0)
            {
                t = Mathf.Clamp(t - Time.deltaTime / fadeInDuration, 0.0f, 1.0f);
                obscureMenuPanel.GetComponent<CanvasGroup>().alpha = t;
            }
            else
            {

                if (f != 1)
                {
                    f = Mathf.Clamp(f + Time.deltaTime / fadeInDuration, 0.0f, 1.0f);
                    begin.GetComponent<CanvasGroup>().alpha = f;
                }
                else
                {
                    t = 1;
                    f = 0;
                    obscureMenuPanel.SetActive(false);
                }
                
            }
        
	}

    public void ZoomCamera()
    {
		CameraSplitter.Instance.SetZoomTarget(false);
		startZoom = true;
    }

    public IEnumerator MainMenuLoadLevel()
    {
        AsyncOperation async = Application.LoadLevelAdditiveAsync(startScene);
        startLevelLoaded = true;

        yield return async;

        startPanelFade = true;
    }

    public void MainMenuLoadLevel(string levelName)
    {
        if(levelName != null)
        {
            startScene = levelName;
        }
    }

	private void SetInputSelected()
	{
		
		if (deviceCount > 0)
		{
			device = InputManager.ActiveDevice;
			if (device.AnyButton || device.LeftStick.HasChanged || device.RightStick.HasChanged || device.MenuWasPressed)
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
				//inputSelect.SetActive(true);
				//mainMenu.SetActive(true);
				menuState = MenuState.MainMenu;
				return;
			}
		}
		if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
		{
			if(!Input.GetKeyDown(KeyCode.F9))
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
				//inputSelect.SetActive(true);
				//mainMenu.SetActive(true);
				menuState = MenuState.MainMenu;
				return;
			}
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

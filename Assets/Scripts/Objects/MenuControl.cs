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

	public GameObject logo;

	public GameObject beginTitle;
	public GameObject continueTitle;

	public GameObject inputSelect;

	public GameObject options;

	public GameObject exitGameConfirm;

	public GameObject continueGame;

	public enum MenuState{TitleScreen, MainMenu, StartGame, ContinueGame};

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
	private float t = 0f;
	public float duration = 3.0f;

	public float fadeInDuration = 1.5f;

	public bool fadeIn = true;

	private Color startColor;
	private Color fadeColor;

	public ClusterNodePuzzle continueGameNodePuzzle;
	public ClusterNodePuzzle confirmQuitNodePuzzle;

	private bool startLevelLoaded = false;

	private bool startPanelFade = false;
	private bool zoom = true;
	private bool startZoom = false;

	private bool fadeStartScreen = false;

    public GameObject gameControls;
    private float s = 1.0f;
    private bool toggleFadeOutControls = false;
    private bool toggleInvoke = true;

    private Vector3 posNoZ;
    private Vector3 player1NoZ;
    private Vector3 player2NoZ;

    public float distance = 2.0f;
    private float distancePow = 0.0f;

    public float disToPlayer1;
    public float disToPlayer2;

    public bool player1Toggled = false;
    public bool player2Toggled = false;

    public bool moveCameraOffset = false;
    public Vector3 cameraOffset = new Vector3(0.0f, -12.0f, 0.0f);
    private float x = 1.0f;

	// Use this for initialization
	void Awake () 
	{
		//startColor = levelCover.renderer.material.color;
	   // fadeColor = new Color(startColor.r, startColor.g, startColor.b, 0.0f);
		//inputSelectRenderers = inputSelect.GetComponentsInChildren<Renderer>();   
	    Globals.Instance.allowInput = false;
		//mainMenu.SetActive (false);
		//inputSelect.SetActive (false);

		if (Application.isEditor && Globals.Instance.quickFade)
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
        distancePow = Mathf.Pow(distance, 2);
	}


	// Update is called once per frame
	void Update()
	{
		if (!fadeStartScreen && startPanelFade)
		{
			FadeInFadeOut();
		}
		else
		{
			//deviceCount = InputManager.controllerCount;
			if (!inputSelected)
			{
				SetInputSelected();
			}
			else
			{
				if (startMenu.activeSelf)
					FadeStartMenu();
			}

            if(moveCameraOffset)
            {
                AdjustCameraOffset();
            }

			switch(menuState)
			{

	//Main Menu/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				case MenuState.MainMenu:

                    if(toggleInvoke)
                    {
                        posNoZ = new Vector3(transform.position.x, transform.position.y, 0.0f);


                        if (!Player1InRange() || !Player2InRange())
                        {
                            ToggleControlsFadeInvoke();
                        }
                        
                    }
                    if(toggleFadeOutControls)
                    {
                        FadeOutControls();
                    }
				
				if(!mainMenu.activeInHierarchy)
						mainMenu.SetActive(true);

				if(fMainMenu.f != 1)
						fMainMenu.FadeIn();


			if(!inputSelect.activeInHierarchy)
			inputSelect.SetActive(true);           

			if(!exitGameConfirm.activeInHierarchy)
				exitGameConfirm.SetActive(true);

			if(!options.activeInHierarchy)
				options.SetActive(true);

		   

				if (newGameNodePuzzle != null && newGameNodePuzzle.solved)
				{
					//CameraSplitter.Instance.player1Target.transform.position = Globals.Instance.player1.transform.position;
					//CameraSplitter.Instance.player2Target.transform.position = Globals.Instance.player2.transform.position;
					newGameNodePuzzle.solved = false;
                    newGameNodePuzzle.UnlightNodes();
					Globals.Instance.allowInput = false;
					menuState = MenuState.StartGame;      
				}

				if (continueGameNodePuzzle != null && continueGameNodePuzzle.solved)
				{
					//CameraSplitter.Instance.player1Target.transform.position = Globals.Instance.player1.transform.position;
					//CameraSplitter.Instance.player2Target.transform.position = Globals.Instance.player2.transform.position;
					continueGameNodePuzzle.solved = false;
                    continueGameNodePuzzle.UnlightNodes();
					Globals.Instance.allowInput = false;
					menuState = MenuState.ContinueGame;
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
                        confirmQuitNodePuzzle.UnlightNodes();
						Application.Quit();
					}

				//options.GetComponent<OptionsMenu>().soundChecked = false;
					break;
	
			case MenuState.StartGame:
				ToggleFadeMainMenu();


					if (!toggled)
					{
						  Invoke("StartGame", .5f);
						  CameraSplitter.Instance.JumpToPlayers();
						  
						  toggled = true;
					}

					if (startGame)
					{
						
						 
						Globals.Instance.fromContinue = false;
						Globals.Instance.ResetLevels();
						 CameraSplitter.Instance.movePlayers = true;
						
						FadeControls();
					}

				break;

			case MenuState.ContinueGame:
				ToggleFadeMainMenu();


				if (!toggled)
				{
					StartGame();
					CameraSplitter.Instance.JumpToPlayers();

					toggled = true;
				}

				if (startGame)
				{
					Globals.Instance.fromContinue = true;
					Globals.Instance.Player1.character.bondAttachable.enabled = true;
					Globals.Instance.Player2.character.bondAttachable.enabled = true;
					CameraSplitter.Instance.movePlayers = true;

					FadeControls();
				}

				break;

			}


			if(startZoom)
			{
				if (CameraSplitter.Instance.zoomState != CameraSplitter.ZoomState.ZoomedIn)
				{
					CameraSplitter.Instance.MovePlayers(Globals.Instance.player1PositionBeforePause, Globals.Instance.player2PositionBeforePause, false);
					CameraSplitter.Instance.Zoom(false);
					
				}
				else
				{
					CameraSplitter.Instance.followPlayers = true;
					CameraSplitter.Instance.splittable = true;
					CameraSplitter.Instance.zCameraOffset = -300.0f;
					CameraSplitter.Instance.duration = 3.0f;
					Globals.Instance.allowInput = true;
					CameraSplitter.Instance.player1Target.transform.localPosition = CameraSplitter.Instance.player1TargetStartPosition;
					CameraSplitter.Instance.player2Target.transform.localPosition = CameraSplitter.Instance.player2TargetStartPosition;
					Destroy(GameObject.FindGameObjectWithTag("Main Menu"));
					Globals.Instance.inMainMenu = false;
                    //Globals.Instance.pauseMenuFloors.SetActive(true);

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

    private void ToggleControlsFadeInvoke()
    {
        toggleFadeOutControls = true;
        toggleInvoke = false;
    }

    private void FadeOutControls()
    {
        if(gameControls.GetComponent<CanvasGroup>().alpha != 0.0f)
        {
            s = Mathf.Clamp(s - Time.deltaTime / 2.0f, 0.0f, 1.0f);
            gameControls.GetComponent<CanvasGroup>().alpha = s;

            
        }
        else
        {
            toggleFadeOutControls = false;
        }
    }


    private bool Player1InRange()
    {
        player1NoZ = new Vector3(Globals.Instance.Player1.transform.position.x, Globals.Instance.Player1.transform.position.y, 0.0f);
        disToPlayer1 = Vector3.SqrMagnitude(player1NoZ - posNoZ);
        player1Toggled = disToPlayer1 < distancePow;
        return player1Toggled;
    }

    private bool Player2InRange()
    {
        player2NoZ = new Vector3(Globals.Instance.Player2.transform.position.x, Globals.Instance.Player2.transform.position.y, 0.0f);
        disToPlayer2 = Vector3.SqrMagnitude(player2NoZ - posNoZ);
        player2Toggled = disToPlayer2 < distancePow;
        return player2Toggled;
    }

	private void StartGame()
	{
		startGame = true;
	}

	private void FadeStartMenu()
	{
		if (startMenu.GetComponent<CanvasGroup>().alpha != 0.0f)
		{
			t = Mathf.Clamp(t - Time.deltaTime/2.0f, 0.0f, 1.0f);
			startMenu.GetComponent<CanvasGroup>().alpha = t;            
		}
		else
		{
			//if (f != 1)
			//{
			   
			//}
			//else
			//{
				inputFill.allowFill = true;
				startMenu.SetActive(false);
				t = 1.0f;
                moveCameraOffset = true;
			//}
		}
	}

    private void AdjustCameraOffset()
    {
        if(x != 0)
        {
            x = Mathf.Clamp(x - Time.deltaTime / 2.0f, 0.0f, 1.0f);
            CameraSplitter.Instance.mainCameraFollow.centerOffset = Vector3.Lerp(Vector3.zero, cameraOffset, x);
        }
        else
        {
            moveCameraOffset = false;
            Helper.FirePulse(Globals.Instance.Player1.transform.position, Globals.Instance.defaultPulseStats);
            Helper.FirePulse(Globals.Instance.Player2.transform.position, Globals.Instance.defaultPulseStats);
            Globals.Instance.allowInput = true;
        }
    }

	private void FadeControls()
	{            

		if (zoom) 
		{
			if (!Application.isEditor || Globals.Instance.zoomIntroInEditor) 
			{

				ZoomCamera();
			} 
			else 
			{
				CameraSplitter.Instance.transform.position = new Vector3 (CameraSplitter.Instance.transform.position.x, CameraSplitter.Instance.transform.position.y, CameraSplitter.Instance.startPos.z);
				CameraSplitter.Instance.splittable = true;
				CameraSplitter.Instance.followPlayers = true;
				Destroy(GameObject.FindGameObjectWithTag("Main Menu"));
				Globals.Instance.inMainMenu = false;
                //Globals.Instance.pauseMenuFloors.SetActive(true);
				Globals.Instance.allowInput = true;
			}

			zoom = false;
		}

		
	}

	private void FadeInFadeOut()
	{
		if (logo.GetComponent<CanvasGroup>().alpha != 1.0f)
		{
            t = Globals.Instance.quickFade ? Mathf.Clamp(t + Time.deltaTime / fadeInDuration, 0.0f, 1.0f) : Mathf.Clamp(t + Time.deltaTime / 5.0f, 0.0f, 1.0f);
			logo.GetComponent<CanvasGroup>().alpha = t;
		}
		else
		{

			//if (t != 0)
		   // {
			//    t = Mathf.Clamp(t - Time.deltaTime / fadeInDuration, 0.0f, 1.0f);
			//    obscureMenuPanel.GetComponent<CanvasGroup>().alpha = t;
			//}
			//else
		   // {

				if (f != 1)
				{
					f = Mathf.Clamp(f + Time.deltaTime / fadeInDuration, 0.0f, 1.0f);
					if (beginTitle.activeSelf)
					{
						beginTitle.GetComponent<CanvasGroup>().alpha = f;
					}
					else if (continueTitle.activeSelf)
					{
						continueTitle.GetComponent<CanvasGroup>().alpha = f;
					}
					
				}
				else
				{
					t = 1;
					f = 0;
					fadeStartScreen = true;
					//obscureMenuPanel.SetActive(false);
				}

			//}
		}
		
	}

	public void ZoomCamera()
	{

		CameraSplitter.Instance.player1Target.transform.position = Globals.Instance.Player1.transform.position;
		CameraSplitter.Instance.player2Target.transform.position = Globals.Instance.Player2.transform.position;
		CameraSplitter.Instance.SetZoomTarget(false);
		Globals.Instance.camera1PositionBeforePause = CameraSplitter.Instance.startPos;
		Globals.Instance.camera2PositionBeforePause = CameraSplitter.Instance.startPos;
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
		
		if (InputManager.Devices.Count > 0)
		{
			device = InputManager.ActiveDevice;
			if (device.AnyButton || device.LeftStick.HasChanged || device.RightStick.HasChanged || device.Command.HasChanged)
			{
				Globals.Instance.leftControllerInputDevice = device;
				//Globals.Instance.leftControllerIndex = InputManager.Devices.IndexOf(device);
				//Globals.Instance.leftControllerPreviousIndex = Globals.Instance.leftControllerIndex;
				Globals.Instance.rightControllerInputDevice = null;
				//Globals.Instance.rightContollerIndex = -2;
				//Globals.Instance.rightControllerPreviousIndex = -2;

				if (InputManager.Devices.Count == 1)
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

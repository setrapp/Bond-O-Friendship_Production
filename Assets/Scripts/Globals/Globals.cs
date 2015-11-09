using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class Globals : MonoBehaviour {
	private static Globals instance = null;
	public static Globals Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject instanceObject = GameObject.FindGameObjectWithTag("Globals");
				if (instanceObject != null)
				{
					instance = instanceObject.GetComponent<Globals>();
				}
			}
			return instance;
		}
	}

	public bool perspectiveCamera = false;
	public float perspectiveFOV = 25;
	[HideInInspector]
	public float startingPerspectiveFOV;
	public float orthographicSize = 20;
	[HideInInspector]
	public float startingOrthographicSize;

	public bool zoomIntroInEditor = true;
	public bool earlyBondInEditor = false;
	public float editorFakeStreamRate = 0;
	public bool editorIgnoreSpecialStart = false;


	public bool allowInput = true;
	public bool titleScreenFaded = false;

	public float audioVolume = -1;
	public bool mute = false;
	public AudioSource bgm;
	public AudioSource[] levelsBackgroundAudio;

	public RingPulse defaultPulsePrefab;
	public PulseStats defaultPulseStats;

	public enum ControlScheme{None, SharedLeft, SharedRight, Solo};

	public enum InputNameSelected {None, Keyboard, LeftController, RightController };

	public enum GameState { Unpaused, Unpausing, Paused, Pausing};
	public GameState gameState = GameState.Unpaused;

	public ControlsAndInput player1Controls;
	public ControlsAndInput player2Controls;


	// public ControlScheme player1ControlScheme;
	//public ControlScheme player2ControlScheme;

	private PlayerInput player1;
	public PlayerInput Player1
	{
		get
		{
			if(player1 == null)
			{
				GameObject[] players = GameObject.FindGameObjectsWithTag("Character");
				for (int i = 0; i < players.Length && player1 == null; i++)
				{
					PlayerInput player = players[i].GetComponent<PlayerInput>();
					if (player != null && player.playerNumber == PlayerInput.Player.Player1)
					{
						player1 = player;
					}
				}
			}
			return player1;
		}
		set { player1 = value; }
	}

	private PlayerInput player2;
	public PlayerInput Player2
	{
		get
		{
			if (player2 == null)
			{
				GameObject[] players = GameObject.FindGameObjectsWithTag("Character");
				for (int i = 0; i < players.Length && player2 == null; i++)
				{
					PlayerInput player = players[i].GetComponent<PlayerInput>();
					if (player != null && player.playerNumber == PlayerInput.Player.Player1)
					{
						player2 = player;
					}
				}
			}
			return player2;
		}
		set { player2 = value; }
	}

	// public InputNameSelected player1InputNameSelected;
	// public InputNameSelected player2InputNameSelected;

	public GameObject initialPlayerHolder = null;

	public bool autoAttractor = false;
	public bool fluffsThrowable = true;
	public float fluffLeaveDistance = 1.0f;
	public float fluffLeaveAttractWait = 3.0f;
	public float fluffLeaveEmbed = 1.0f;

	public bool inMainMenu = true;

	//Index of the player's controller, -1 means keyboard, -2 means waiting for input
	public int leftControllerIndex;
	public int rightContollerIndex;
	public int leftControllerPreviousIndex = -2;
	public int rightControllerPreviousIndex = -2;

	public InputDevice leftControllerInputDevice;
	public InputDevice rightControllerInputDevice;

	public bool allowPreviousController = false;



	public static bool isPaused;

	public Vector3 player1PositionBeforePause;
	public Vector3 player2PositionBeforePause;
	public Vector3 camera1PositionBeforePause;
	public Vector3 camera2PositionBeforePause;

	public GameObject canvasPaused;

	[Header("Fluff Depth Mask")]
	public bool visibilityDepthMaskNeeded = false;
	[Header("Fluff Depth Mask")]
	public GameObject depthMaskPrefab;
	[Header("Fluff Depth Mask")]
	public GameObject depthMaskHolderPrefab;

	public GameObject orphanFluffHolderPrefab;

	[SerializeField]
	public List<Fluff> allFluffs;
	public AudioSource fluffAudio;

	public bool updatePlayersOnLoad = true;

	public static bool sharing = false;

	public EtherRing existingEther = null;

	public bool bondAllowed = false;
	public bool bondSoundPlayable = true;
	public bool playersBonded = false;


	public GameObject pauseMenu;
    public GameObject pauseMenuFloors;

	public SetShaderData_DarkAlphaMasker darknessMask = null;
	public float playerLuminIntensity = 1;
	public float defaultPlayerLuminIntensity = 1;

    public bool configureControls = false;
    public bool notifyControlsChangeOnDisconnect = false;

	// Flags from completed levels [None, Tutorial, Harmony, Intimacy, Asymmetry]
	public bool[] levelsCompleted = new bool[5];

    public GameObject startSpawnLocation = null;
    public GameObject continueSpawnLocation = null;
    public bool fromContinue = false;


    public bool quickFade = false;
    void OnEnable()
    {
        InputManager.OnDeviceDetached += OnDeviceDetached;
    }

    void OnDisable()
    {
        InputManager.OnDeviceDetached -= OnDeviceDetached;
    }

	void Awake()
	{
		if (CheckExistingGlobals())
		{
			return;
		}

		if (!Application.isEditor)
		{
			Cursor.visible = false;
		}
		//Debug.Log(leftControllerIndex);

		if (Application.isEditor && earlyBondInEditor)
		{
			bondAllowed = true;
		}

		//ResetLevels();

		startingPerspectiveFOV = perspectiveFOV;
		startingOrthographicSize = orthographicSize;

		CheckCameraPerspective();

		if (audioVolume < 0)
		{
			audioVolume = AudioListener.volume;
		}

		CheckCameraPerspective();
		CheckVolume();

		if (bgm != null && !bgm.isPlaying)
		{
			bgm.Play();
		}

		defaultPlayerLuminIntensity = playerLuminIntensity;
	}

	void Update()
	{
		if (Player1 == null || Player2 == null)
		{
			CameraSplitter.Instance.SetPlayers();
		}

        if((Input.GetKeyDown(KeyCode.Escape) || InputManager.ActiveDevice.MenuWasPressed) && !inMainMenu)
		{
			if(gameState == GameState.Unpaused)
			{
				gameState = GameState.Pausing;
				OnPause();
			}
			if(gameState == GameState.Paused && Application.isEditor)
			{
				allowInput = false;
				CameraSplitter.Instance.splittable = true;
				gameState = GameState.Unpausing;
			}
		}

		if (Input.GetKeyDown(KeyCode.M))
		{
			mute = !mute;
		}

		if (pauseMenu == null)
		{
			pauseMenu = GetComponentInChildren<PauseMenuControl>().gameObject;
		}

		if (gameState == GameState.Pausing) 
		{
			
			if(CameraSplitter.Instance.zoomState != CameraSplitter.ZoomState.ZoomedOut)
			{
				CameraSplitter.Instance.Zoom(true);
				CameraSplitter.Instance.MovePlayers(player1PositionBeforePause, player2PositionBeforePause);
			}
			if(CameraSplitter.Instance.zoomState == CameraSplitter.ZoomState.ZoomedOut)
			{
				gameState = GameState.Paused;
				if (player1 != null) { player1.character.bondAttachable.enabled = false; }
				if (player2 != null) { player2.character.bondAttachable.enabled = false; }
			}
		}

		if (gameState == GameState.Paused) 
		{
			//if(!pauseMenu.activeInHierarchy)
				//pauseMenu.SetActive(true);
			//CameraSplitter.Instance.splittable = false;
			allowInput = true;
			if (darknessMask != null && darknessMask.gameObject.activeSelf)
			{
				darknessMask.gameObject.SetActive(false);
			}
		}

		if (gameState == GameState.Unpausing) 
		{
			//if(pauseMenu.activeInHierarchy)
				//pauseMenu.SetActive(false);
			if(CameraSplitter.Instance.zoomState != CameraSplitter.ZoomState.ZoomedIn)
			{
				if (darknessMask != null && !darknessMask.gameObject.activeSelf)
				{
					darknessMask.gameObject.SetActive(true);
				}
				CameraSplitter.Instance.Zoom(false);
				CameraSplitter.Instance.MovePlayers(player1PositionBeforePause, player2PositionBeforePause, false);
			}
			if(CameraSplitter.Instance.zoomState == CameraSplitter.ZoomState.ZoomedIn)
			{
				pauseMenuFloors.SetActive(false);
				gameState = GameState.Unpaused;
                CameraSplitter.Instance.player1Target.transform.localPosition = CameraSplitter.Instance.player1TargetStartPosition;
                CameraSplitter.Instance.player2Target.transform.localPosition = CameraSplitter.Instance.player2TargetStartPosition;
				if (bondAllowed)
				{
					if (player1 != null) { player1.character.bondAttachable.enabled = true; }
					if (player2 != null) { player2.character.bondAttachable.enabled = true; }
				}
				allowInput = true;
			}
		}
		
		//leftControllerIndex = HandleDeviceDisconnect(leftControllerIndex);
		//rightContollerIndex = HandleDeviceDisconnect(rightContollerIndex);
		//ResetDeviceIndex();
		WaitForInput();
        ResetDevices();
		CheckCameraPerspective();
		CheckVolume();

		/* if(Input.GetKeyDown(KeyCode.Z))
		 {
			 Debug.Log("Left Controller Index: " + leftControllerIndex);
			 Debug.Log("Previous Left: " + leftControllerPreviousIndex);
			 Debug.Log("Right Controller Index: " + rightContollerIndex);
			 Debug.Log("Previous Right: " + rightControllerPreviousIndex);
		 }*/

		// Ensure that tutorial is always considered complete if any other level is completed (this should only affect testing).
		levelsCompleted[1] = levelsCompleted[1] || levelsCompleted[2] || levelsCompleted[3] || levelsCompleted[4];
    }

	public void ResetLevels()
	{
		for (int i = 0; i < levelsCompleted.Length; i++)
		{
			levelsCompleted[i] = false;
		}
		// The first element is garbage data.
		levelsCompleted[0] = true;
	}

	public void OnPause()
	{
		SetPauseLocations ();
		CameraSplitter.Instance.SetZoomTarget ();
		pauseMenuFloors.SetActive (true);
        pauseMenuFloors.transform.position = new Vector3(CameraSplitter.Instance.transform.position.x, CameraSplitter.Instance.transform.position.y, pauseMenuFloors.transform.position.z);
		allowInput = false;
	}

	public void SetPauseLocations()
	{
		player1PositionBeforePause = Player1.transform.position;
		player2PositionBeforePause = Player2.transform.position;
		camera1PositionBeforePause = CameraSplitter.Instance.mainCameraFollow.transform.position;
		camera2PositionBeforePause = CameraSplitter.Instance.splitCameraFollow.transform.position;
	}


    public void ResetOrExit()
    {
		titleScreenFaded = false;
        if (inMainMenu)
        {
           // if (Application.isEditor)
           //     UnityEditor.EditorApplication.isPlaying = false;
           // else
                Application.Quit();
        }
        else
        {
			Globals.Instance.gameState = Globals.GameState.Unpaused;
			Application.LoadLevel(0);
        }
    }

	private void CheckCameraPerspective()
	{
		if (CameraSplitter.Instance.splitCamera1.orthographic == perspectiveCamera || CameraSplitter.Instance.splitCamera2.orthographic == perspectiveCamera)
		{
			CameraSplitter.Instance.splitCamera1.orthographic = CameraSplitter.Instance.splitCamera2.orthographic = !perspectiveCamera;
		}
		if (perspectiveCamera && CameraSplitter.Instance.splitCamera1.fieldOfView != perspectiveFOV || CameraSplitter.Instance.splitCamera2.fieldOfView != perspectiveFOV)
		{
			CameraSplitter.Instance.splitCamera1.fieldOfView = CameraSplitter.Instance.splitCamera2.fieldOfView = perspectiveFOV;
		}
        if (!perspectiveCamera && CameraSplitter.Instance.splitCamera1.orthographicSize != orthographicSize || CameraSplitter.Instance.splitCamera2.orthographicSize != orthographicSize)
        {
            CameraSplitter.Instance.splitCamera1.orthographicSize = CameraSplitter.Instance.splitCamera2.orthographicSize = orthographicSize;
        }
	}
	
	private void CheckVolume()
	{
		audioVolume = Mathf.Max(audioVolume, 0);

		if (mute && AudioListener.volume > 0)
		{
			AudioListener.volume = 0;
		}
		else if (!mute && AudioListener.volume != audioVolume)
		{
			AudioListener.volume = audioVolume;
		}
	}

	public void BondFormed(Bond bond)
	{
		// Track if a bond between the players has been formed.
		if (bond.OtherPartner(Player1.character.bondAttachable) == Player2.character.bondAttachable
			&& bond.OtherPartner(Player2.character.bondAttachable) == Player1.character.bondAttachable)
		{
			if (!playersBonded)
			{
				Helper.FirePulse(Player1.transform.position, defaultPulseStats);
				Helper.FirePulse(Player2.transform.position, defaultPulseStats);
			}

			playersBonded = true;
		}
	}

	public void BondBroken(Bond bond)
	{
		// Track if a bond between the players has been broken.
		if (bond.OtherPartner(Player1.character.bondAttachable) == Player2.character.bondAttachable
			&& bond.OtherPartner(Player2.character.bondAttachable) == Player1.character.bondAttachable)
		{
			playersBonded = false;
		}
	}

	private bool CheckExistingGlobals()
	{
		if (instance != null && instance != this)
		{
			// Reference new camera system from this globals to replace the camera system of the existing globals.
			CameraSplitter newCameraSystem = GetComponentInChildren<CameraSplitter>();

			// Swap old audio listener into new camera system.
			AudioListener trashAudioListener = newCameraSystem.GetComponentInChildren<AudioListener>();
			CameraSplitter.Instance.GetComponentInChildren<AudioListener>().transform.parent = trashAudioListener.transform.parent;
			Destroy(trashAudioListener.gameObject);
			CameraSplitter.Instance.gameObject.SetActive(false);

			// Choose the correct starting background music by applying this global's defaults to the existing globals.
			int bgmIndex = -1;
			for (int i = 0; i < levelsBackgroundAudio.Length && bgmIndex < 0; i++)
			{
				if (bgm == levelsBackgroundAudio[i])
				{
					bgmIndex = i;
				}
			}
			if (bgmIndex > 0)
			{
				Globals.Instance.bgm = Globals.Instance.levelsBackgroundAudio[bgmIndex];
			}

			// Move new camera system into existing globals and destroy old camera system.
			newCameraSystem.transform.parent = CameraSplitter.Instance.transform.parent;
			Destroy(CameraSplitter.Instance.gameObject);
			newCameraSystem.gameObject.SetActive(true);
			CameraSplitter.Instance = newCameraSystem;

			// Move new pause menu controls into existing globals and destroy old controls UI.
			GameObject newControls = pauseMenu.GetComponent<PauseMenuControl>().gameControls;
			PauseMenuControl existingPause = Globals.instance.pauseMenu.GetComponent<PauseMenuControl>();
			newControls.transform.parent = existingPause.gameControls.transform.parent;
			Destroy(existingPause.gameControls.gameObject);
			existingPause.gameControls = newControls;
			existingPause.gameObject.SetActive(false);

			// Ensure that all background music is at the correct volume.
			for (int i = 0; i < Globals.Instance.levelsBackgroundAudio.Length; i++)
			{
				Globals.Instance.levelsBackgroundAudio[i].volume = 0;
			}
			Globals.Instance.bgm.volume = 1;
			Globals.Instance.bgm.Play();

			// Ensure that no empty levels are still considered loaded.
			LevelHandler.Instance.loadedIslands = new List<Island>();

			// Disallow bond making.
			if (Globals.Instance.earlyBondInEditor && Application.isEditor)
			{
				Globals.Instance.bondAllowed = false;
				if (Globals.Instance.Player1 != null && Globals.Instance.Player2 != null)
				{
					Globals.Instance.Player1.character.bondAttachable.enabled = false;
					Globals.Instance.Player2.character.bondAttachable.enabled = false;
				}
				Globals.Instance.bondSoundPlayable = true;
			}

			// Reset player placement info
			Globals.Instance.updatePlayersOnLoad = true;
			if (Globals.Instance.Player1 != null && Globals.Instance.Player2 != null && Globals.Instance.initialPlayerHolder != null)
			{
				Globals.Instance.Player1.transform.parent = Globals.instance.initialPlayerHolder.transform;
				Globals.Instance.Player2.transform.parent = Globals.instance.initialPlayerHolder.transform;
			}

			if (darknessMask != null)
			{
				Globals.Instance.darknessMask.fadeIn = false;
				Globals.Instance.darknessMask.gameObject.SetActive(true);
			}

			// Destoy this globals and allow the existing one to continue.
			Destroy(gameObject);
			return true;
		}
		return false;
	}

	private void ResetDevices()
	{
        if (configureControls)
        {
            if (InputManager.Devices.Count == 1)
            {
                if (player1Controls.inputNameSelected == InputNameSelected.RightController)
                {
                    player1Controls.inputNameSelected = InputNameSelected.LeftController;
                    player1Controls.controlScheme = ControlScheme.SharedLeft;
                    player1Controls.controlScheme = CheckSoloInput(player1Controls, player2Controls);
                    player2Controls.controlScheme = CheckSharedInput(player2Controls, player1Controls);
                    player2Controls.controlScheme = CheckSoloInput(player2Controls, player1Controls);
                }

                if (player2Controls.inputNameSelected == InputNameSelected.RightController)
                {
                    player2Controls.inputNameSelected = InputNameSelected.LeftController;
                    player2Controls.controlScheme = ControlScheme.SharedRight;
                    player2Controls.controlScheme = CheckSoloInput(player2Controls, player1Controls);
                    player1Controls.controlScheme = CheckSharedInput(player1Controls, player2Controls);
                    player1Controls.controlScheme = CheckSoloInput(player1Controls, player2Controls);
                }
            }
            if(InputManager.Devices.Count == 0)
            {
                if (player1Controls.inputNameSelected != InputNameSelected.Keyboard)
                {
                    player1Controls.inputNameSelected = InputNameSelected.Keyboard;
                    player1Controls.controlScheme = ControlScheme.SharedLeft;
                    player1Controls.controlScheme = CheckSoloInput(player1Controls, player2Controls);
                    player2Controls.controlScheme = CheckSharedInput(player2Controls, player1Controls);
                    player2Controls.controlScheme = CheckSoloInput(player2Controls, player1Controls);
                }
                if (player2Controls.inputNameSelected != InputNameSelected.Keyboard)
                {
                    player2Controls.inputNameSelected = InputNameSelected.Keyboard;
                    player2Controls.controlScheme = ControlScheme.SharedRight;
                    player2Controls.controlScheme = CheckSoloInput(player2Controls, player1Controls);
                    player1Controls.controlScheme = CheckSharedInput(player1Controls, player2Controls);
                    player1Controls.controlScheme = CheckSoloInput(player1Controls, player2Controls);
                }
            }
            configureControls = false;
        }
		//if (player1Controls.inputNameSelected != InputNameSelected.LeftController && player2Controls.inputNameSelected != InputNameSelected.LeftController && InputManager.controllerCount >= 1)
		//	leftControllerIndex = -2;
		//if (player1Controls.inputNameSelected != InputNameSelected.RightController && player2Controls.inputNameSelected != InputNameSelected.RightController && InputManager.controllerCount >=2)
		//	rightContollerIndex = -2;
        //if (InputManager.controllerCount < 2)
         //   rightContollerIndex = -3;
      //  if (InputManager.controllerCount == 0)
           // leftControllerIndex = -3;

	}

	private void WaitForInput()
	{
		if (leftControllerInputDevice == null && (player1Controls.inputNameSelected == InputNameSelected.LeftController || player2Controls.inputNameSelected == InputNameSelected.LeftController))
		{
			var device = InputManager.ActiveDevice;

            if (device != rightControllerInputDevice && device.Name != "")
            {
                if (device.AnyButton || device.LeftStick.HasChanged || device.RightStick.HasChanged || device.Command.HasChanged)
                {
                    leftControllerInputDevice = device;
                    //leftControllerPreviousIndex = leftControllerIndex;
                }
            }
		}

		if (rightControllerInputDevice == null && (player1Controls.inputNameSelected == InputNameSelected.RightController || player2Controls.inputNameSelected == InputNameSelected.RightController))
		{
			var device = InputManager.ActiveDevice;
			if (device != leftControllerInputDevice && device.Name != "")
			{
                if (device.AnyButton || device.LeftStick.HasChanged || device.RightStick.HasChanged || device.Command.HasChanged)
                {
                    rightControllerInputDevice = device;
                }
					//rightControllerPreviousIndex = rightContollerIndex;

			}
		}
	}

	/*private int HandleDeviceDisconnect(int deviceIndex)
	{
		var device = deviceIndex >= 0 && deviceIndex < InputManager.Devices.Count ? InputManager.Devices[deviceIndex] : null;

		if (device != null)
		{
			if (device.Name == "")
			{
				//if (InputManager.controllerCount < 2)
					//return -3;
				//else
					return -2;
			}
		}
		return deviceIndex;
	}*/

    void OnDeviceDetached(InputDevice inputDevice)
    {
        leftControllerInputDevice = null;
        rightControllerInputDevice = null;

        if(gameState == GameState.Unpaused && !inMainMenu)
        {
            gameState = GameState.Pausing;
            OnPause();
        }

        notifyControlsChangeOnDisconnect = true;
        configureControls = true;

        //player1Controls.inputNameSelected = InputNameSelected.Keyboard;
        //player1Controls.controlScheme = ControlScheme.SharedLeft;
        //player2Controls.inputNameSelected = InputNameSelected.Keyboard;
        //player2Controls.controlScheme = ControlScheme.SharedRight;
    }

	public ControlScheme CheckSoloInput(ControlsAndInput playerCAI, ControlsAndInput otherPlayerCAI)
	{
		//If they aren't using the keyboard and they aren't using the same device as the other player, they are using a controller solo
		if (playerCAI.inputNameSelected != otherPlayerCAI.inputNameSelected)
			return ControlScheme.Solo;

		return playerCAI.controlScheme;
	}

	public ControlScheme CheckSharedInput(ControlsAndInput playerCAI, ControlsAndInput otherPlayerCAI)
	{
		
		//First Check if using the same device
		if ((playerCAI.inputNameSelected == otherPlayerCAI.inputNameSelected))
		{
			if (otherPlayerCAI.controlScheme == ControlScheme.SharedLeft)
				playerCAI.controlScheme = ControlScheme.SharedRight;
			else if (otherPlayerCAI.controlScheme == ControlScheme.SharedRight)
				playerCAI.controlScheme = ControlScheme.SharedLeft;
		}
		return playerCAI.controlScheme;
	}

	public enum BackgroundAudio
	{
		TUTORIAL = 0,
		HARMONY,
		INTIMACY,
		ASYMMETRY
	}
}

[System.Serializable]
public class ControlsAndInput
{
	public Globals.ControlScheme controlScheme;
	public Globals.InputNameSelected inputNameSelected;
}

public class InputActionSet : PlayerActionSet
{
	public PlayerAction LLeft;
	public PlayerAction LRight;
	public PlayerAction LUp;
	public PlayerAction LDown;
	public PlayerTwoAxisAction LMove;

	public PlayerAction RLeft;
	public PlayerAction RRight;
	public PlayerAction RUp;
	public PlayerAction RDown;
	public PlayerTwoAxisAction RMove;

	public InputActionSet()
	{
		LLeft = CreatePlayerAction("L Move Left");
		LRight = CreatePlayerAction("L Move Right");
		LUp = CreatePlayerAction("L Move Up");
		LDown = CreatePlayerAction("L Move Down");
		LMove = CreateTwoAxisPlayerAction(LLeft, LRight, LDown, LUp);
		RLeft = CreatePlayerAction("R Move Left");
		RRight = CreatePlayerAction("R Move Right");
		RUp = CreatePlayerAction("R Move Up");
		RDown = CreatePlayerAction("R Move Down");
		RMove = CreateTwoAxisPlayerAction(RLeft, RRight, RDown, RUp);
	}
}

/*public class SharedController : PlayerActionSet
{
	public PlayerAction LLeft;
	public PlayerAction LRight;
	public PlayerAction LUp;
	public PlayerAction LDown;
	public PlayerTwoAxisAction LMove;

	public PlayerAction RLeft;
	public PlayerAction RRight;
	public PlayerAction RUp;
	public PlayerAction RDown;
	public PlayerTwoAxisAction RMove;

	public SharedController()
	{
		LLeft = CreatePlayerAction("L Move Left");
		LRight = CreatePlayerAction("L Move Right");
		LUp = CreatePlayerAction("L Move Up");
		LDown = CreatePlayerAction("L Move Down");
		LMove = CreateTwoAxisPlayerAction(LLeft, LRight, LDown, LUp);

		RLeft = CreatePlayerAction("R Move Left");
		RRight = CreatePlayerAction("R Move Right");
		RUp = CreatePlayerAction("R Move Up");
		RDown = CreatePlayerAction("R Move Down");
		RMove = CreateTwoAxisPlayerAction(RLeft, RRight, RDown, RUp);
	}
}

public class SeparateController : PlayerActionSet
{
	public PlayerAction Left;
	public PlayerAction Right;
	public PlayerAction Up;
	public PlayerAction Down;
	public PlayerTwoAxisAction Move;

	public SeparateController()
	{
		Left = CreatePlayerAction("Move Left");
		Right = CreatePlayerAction("Move Right");
		Up = CreatePlayerAction("Move Up");
		Down = CreatePlayerAction("Move Down");
		Move = CreateTwoAxisPlayerAction(Left, Right, Down, Up);
	}
}

public class SeparateKeyboard : PlayerActionSet
{
	public PlayerAction Left;
	public PlayerAction Right;
	public PlayerAction Up;
	public PlayerAction Down;
	public PlayerTwoAxisAction Move;

	public SeparateKeyboard()
	{
		Left = CreatePlayerAction("Move Left");
		Right = CreatePlayerAction("Move Right");
		Up = CreatePlayerAction("Move Up");
		Down = CreatePlayerAction("Move Down");
		Move = CreateTwoAxisPlayerAction(Left, Right, Down, Up);
	}
}*/

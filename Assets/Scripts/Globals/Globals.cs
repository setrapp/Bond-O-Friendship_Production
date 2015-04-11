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

	public enum JoyStick{Joy1, Joy2, Joy3, Joy4};

    [HideInInspector]
    public KeyCode leftKeyboardUp;
    [HideInInspector]
    public KeyCode leftKeyboardDown;
    [HideInInspector]
    public KeyCode leftKeyboardLeft;
    [HideInInspector]
    public KeyCode leftKeyboardRight;

    [HideInInspector]
    public KeyCode rightKeyboardUp;
    [HideInInspector]
    public KeyCode rightKeyboardDown;
    [HideInInspector]
    public KeyCode rightKeyboardLeft;
    [HideInInspector]
    public KeyCode rightKeyboardRight;



	public enum ControlScheme{KeyboardSharedLeft, KeyboardSharedRight, ControllerSharedLeft, ControllerSharedRight, KeyboardSolo, ControllerSolo};

    public enum InputNameSelected { Keyboard, LeftController, RightController };


    public ControlScheme player1ControlScheme;
    public ControlScheme player2ControlScheme;

	public PlayerInput player1;
	public PlayerInput player2;

    public InputNameSelected player1InputNameSelected;
    public InputNameSelected player2InputNameSelected;

	public GameObject initialPlayerHolder = null;

	public bool autoAttractor = false;
	public bool fluffsThrowable = true;
	public float fluffLeaveDistance = 1.0f;
	public float fluffLeaveAttractWait = 3.0f;
	public float fluffLeaveEmbed = 1.0f;

   // public static InputDevice startingDevice;
   // public static InputDevice playerOneDevice;
   // public static InputDevice playerTwoDevice;

    //Index of the player's controller, -1 means keyboard, -2 means waiting for input
    public int player1Device;
    public int player2Device;
    public int player1PreviousDevice = -2;
    public int player2PreviousDevice = -2;

    public static int numberOfControllers;

    public static bool isPaused;

    public static bool usingController;

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

	public bool updatePlayersOnLoad = true;

	public static bool sharing = false;

	public EtherRing existingEther = null;

	public bool playersBonded = false;

	void Awake()
	{
		if (!Application.isEditor)
		{
			Screen.showCursor = false;
		}
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	public void BondFormed(Bond bond)
	{
		// Track if a bond between the players has been formed.
		if (bond.OtherPartner(player1.character.bondAttachable) == player2.character.bondAttachable
		    && bond.OtherPartner(player2.character.bondAttachable) == player1.character.bondAttachable)
		{
			playersBonded = true;
		}
	}

	public void BondBroken(Bond bond)
	{
		// Track if a bond between the players has been broken.
		if (bond.OtherPartner(player1.character.bondAttachable) == player2.character.bondAttachable
		    && bond.OtherPartner(player2.character.bondAttachable) == player1.character.bondAttachable)
		{
			playersBonded = false;
		}
	}
}

public class SharedKeyboard : PlayerActionSet
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

    public SharedKeyboard()
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

public class SharedController : PlayerActionSet
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
}

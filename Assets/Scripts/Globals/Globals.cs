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
    
	public enum ControlScheme{None, SharedLeft, SharedRight, Solo};

    public enum InputNameSelected {None, Keyboard, LeftController, RightController };

    public ControlsAndInput player1Controls;
    public ControlsAndInput player2Controls;


   // public ControlScheme player1ControlScheme;
    //public ControlScheme player2ControlScheme;

	public PlayerInput player1;
	public PlayerInput player2;

   // public InputNameSelected player1InputNameSelected;
   // public InputNameSelected player2InputNameSelected;

	public GameObject initialPlayerHolder = null;

	public bool autoAttractor = false;
	public bool fluffsThrowable = true;
	public float fluffLeaveDistance = 1.0f;
	public float fluffLeaveAttractWait = 3.0f;
	public float fluffLeaveEmbed = 1.0f;

    //Index of the player's controller, -1 means keyboard, -2 means waiting for input
    public int leftControllerIndex;
    public int rightContollerIndex;
    public int leftControllerPreviousIndex = -2;
    public int rightControllerPreviousIndex = -2;

    public bool allowPreviousController = false;



    public static bool isPaused;

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

	void Awake()
	{
		if (!Application.isEditor)
		{
			Screen.showCursor = false;
		}
	}

	void Update()
	{

        leftControllerIndex = HandleDeviceDisconnect(leftControllerIndex);
        rightContollerIndex = HandleDeviceDisconnect(rightContollerIndex);
        ResetDeviceIndex();
        WaitForInput();

       /* if(Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Left Controller Index: " + leftControllerIndex);
            Debug.Log("Previous Left: " + leftControllerPreviousIndex);
            Debug.Log("Right Controller Index: " + rightContollerIndex);
            Debug.Log("Previous Right: " + rightControllerPreviousIndex);
        }*/

		if (Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

    private void ResetDeviceIndex()
    {
        if (player1Controls.inputNameSelected != InputNameSelected.LeftController && player2Controls.inputNameSelected != InputNameSelected.LeftController && (InputManager.controllerCount >= 2 || (InputManager.controllerCount == 1 && rightContollerIndex == -3)))
            leftControllerIndex = -2;
        if (player1Controls.inputNameSelected != InputNameSelected.RightController && player2Controls.inputNameSelected != InputNameSelected.RightController && InputManager.controllerCount >=2)
            rightContollerIndex = -2;
    }

    private void WaitForInput()
    {
        if (leftControllerIndex == -2 && (player1Controls.inputNameSelected == InputNameSelected.LeftController || player2Controls.inputNameSelected == InputNameSelected.LeftController))
        {
            var device = InputManager.ActiveDevice;

            if (InputManager.Devices.IndexOf(device) != rightContollerIndex && device.Name != "")
            {
                if (InputManager.Devices.IndexOf(device) == rightControllerPreviousIndex && !allowPreviousController)
                {
                }
                else
                {
                    leftControllerIndex = InputManager.Devices.IndexOf(device);
                    leftControllerPreviousIndex = leftControllerIndex;
                }
            }
        }

        if (rightContollerIndex == -2 && (player1Controls.inputNameSelected == InputNameSelected.RightController || player2Controls.inputNameSelected == InputNameSelected.RightController))
        {
            var device = InputManager.ActiveDevice;
            if (InputManager.Devices.IndexOf(device) != leftControllerIndex && device.Name != "")
            {
                if (InputManager.Devices.IndexOf(device) == leftControllerPreviousIndex && !allowPreviousController)
                {
                }
                else
                {
                    rightContollerIndex = InputManager.Devices.IndexOf(device);
                    rightControllerPreviousIndex = rightContollerIndex;

                }
            }
        }
    }

    private int HandleDeviceDisconnect(int deviceIndex)
    {
        var device = deviceIndex >= 0 ? InputManager.Devices[deviceIndex] : null;

        if (device != null)
        {
            if (device.Name == "")
            {
                if (InputManager.controllerCount < 2)
                    return -3;
                else
                    return -2;
            }
        }
        if(deviceIndex == -2)
        {
            if (InputManager.controllerCount < 2)
                return -3;
        }
        return deviceIndex;
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

    
}


[System.Serializable]
public class ControlsAndInput
{
    public Globals.ControlScheme controlScheme;
    public Globals.InputNameSelected inputNameSelected;
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

using UnityEngine;
using System.Collections;
using InControl;

public class PlayerInput : MonoBehaviour {
	public CharacterComponents character;
	public enum Player{Player1, Player2};

	public Player playerNumber;
	public GameObject canvasStart;

	public GameObject canvasPaused;

	public GameObject geometry;
	public float deadZone = .75f;

	private bool fireFluffReady = true;
	private Vector3 velocityChange;



	private Vector3 target;
	public Vector3 desiredLook;
	public bool joystickDetermined = false;

	//private bool paused = false;
	
    InputDevice device;

    private bool oneController;

	void Awake()
	{
		if (character == null)
		{
			character = GetComponent<CharacterComponents>();
		}
	}

	void Start()
	{

	}

	void Update () 
    {

        CheckDevices();
        device = playerNumber == Player.Player1 ? Globals.playerOneDevice : Globals.playerTwoDevice;
        //device = InputManager.Devices.Count == 1 ? Globals.playerOneDevice : device;
        
       // Debug.Log(device.Name);
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

        

        if ((oneController && playerNumber == Player.Player1) || !oneController)
        {
            if (device.Action4.WasPressed)
            {
                if (Globals.isPaused)
                {
                    canvasPaused.SetActive(false);
                    Time.timeScale = 1;
                }
                else
                {
                    canvasPaused.SetActive(true);
                    Time.timeScale = 0;
                }
                Globals.isPaused = !Globals.isPaused;
            }
        }

        

        if (!Globals.isPaused)
		{
			AttemptFluffThrow();
			AttemptFluffAttract();

			velocityChange =  PlayerJoystickMovement();
			// Movement
			if (velocityChange.sqrMagnitude > 0)
			{
				character.mover.Accelerate(velocityChange, true, true);
				character.mover.slowDown = false;
			}
			else
			{
				character.mover.slowDown = true;
			}
			// Turn towards velocity change.
			transform.LookAt(transform.position + velocityChange, transform.up);
		}
	}
    
	private Vector3 PlayerJoystickMovement()
	{

        Vector2 stickInput = Vector2.zero;

        //Debug.Log(device.LeftStick.Vector.ToString());

        if (oneController)
        {
            if(playerNumber == Player.Player1)
            {
                stickInput = device.LeftStick.Vector;
                return stickInput.sqrMagnitude > Mathf.Pow(deadZone, 2f) ? new Vector3(device.LeftStick.X, device.LeftStick.Y, 0) : Vector3.zero;
            }
            if (playerNumber == Player.Player2)
            {
                stickInput = device.RightStick.Vector;
                return stickInput.sqrMagnitude > Mathf.Pow(deadZone, 2f) ? new Vector3(device.RightStick.X, device.RightStick.Y, 0) : Vector3.zero;
            }
        }
        else
        {
            stickInput = device.RightStick.Vector;

            stickInput = device.LeftStick.Vector != Vector2.zero ? device.LeftStick.Vector : stickInput;

            if (device.LeftStick.Vector != Vector2.zero)
            {
                return stickInput.sqrMagnitude > Mathf.Pow(deadZone, 2f) ? new Vector3(device.LeftStick.X, device.LeftStick.Y, 0) : Vector3.zero;
            }
            else
                return stickInput.sqrMagnitude > Mathf.Pow(deadZone, 2f) ? new Vector3(device.RightStick.X, device.RightStick.Y, 0) : Vector3.zero;
        }

        return Vector3.zero;
	}
    
	private void AttemptFluffAttract()
	{
		bool canAttract = false;

        if (oneController)
        {
            if (playerNumber == Player.Player1 && device.LeftBumper.IsPressed)
                canAttract = true;
            else if (playerNumber == Player.Player2 && device.RightBumper.IsPressed)
                canAttract = true;
        }
        else
        {
            if (device.LeftBumper.IsPressed || device.RightBumper.IsPressed)
                canAttract = true;
        }

		if (!fireFluffReady)
		{
			canAttract = false;
		}

		if (canAttract)
		{
			character.attractor.AttractFluffs();
		}
		else
		{
			character.attractor.StopAttracting();
		}
	}

    
	private void AttemptFluffThrow()
	{
        Vector2 lookAt = Vector2.zero;
		float minToFire = deadZone;

        if (oneController)
        {
            if (playerNumber == Player.Player1)
            {
                if (device.LeftTrigger.WasPressed)
                {
                    lookAt = transform.forward;
                    minToFire = 0;
                }
            }
            else if (playerNumber == Player.Player2)
            {
                if (device.RightTrigger.WasPressed)
                {
                    lookAt = transform.forward;
                    minToFire = 0;
                }
            }
        }
        else
        {
            if (device.LeftTrigger.WasPressed || device.RightTrigger.WasPressed)
            {
                lookAt = transform.forward;
                minToFire = 0;
            }
        }
		

		if(lookAt.sqrMagnitude > Mathf.Pow(minToFire, 2f))
		{
			lookAt.Normalize();

			if(fireFluffReady)
			{
				Vector3 throwDirection = new Vector3(lookAt.x, lookAt.y, 0);
				Vector3 velocityBoost = Vector3.zero;

				if (Vector3.Dot(character.mover.velocity, throwDirection) > 0)
				{
					velocityBoost += character.mover.velocity;
				}
                character.fluffThrow.Throw(throwDirection, velocityBoost);
				fireFluffReady = false;
			}
		}
		else
		{
			fireFluffReady = true;
		}
    }


    #region Helper Methods

    private void CheckDevices()
    {
        
        if (InputManager.Devices.Count != Globals.numberOfControllers)
        {
            if (Globals.numberOfControllers < InputManager.Devices.Count)
            {
                Globals.numberOfControllers++;
            }
            else
            {
                canvasPaused.SetActive(true);
                Time.timeScale = 0;
                Globals.isPaused = true;
            }
        }

        if (Globals.numberOfControllers == 1)
        {
            oneController = true;
        }
        else
            oneController = false;


        if (InputManager.Devices.Count > 1)
        {
            Globals.playerOneDevice = InputManager.Devices[0];
            Globals.playerTwoDevice = InputManager.Devices[1];
        }
        else if(InputManager.Devices.Count == 1)
        {
            Globals.playerOneDevice = InputManager.Devices[0];
            Globals.playerTwoDevice = InputManager.Devices[0];
        }

    }

   

    
    #endregion



}

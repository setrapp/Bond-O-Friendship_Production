using UnityEngine;
using System.Collections;
using InControl;

public class PlayerInput : MonoBehaviour {

	public CharacterComponents character;

    public enum Player { Player1, Player2 };
    public Player playerNumber;

    public ControlsAndInput controlScheme;

	public float deadZone = .75f;

	public Vector3 velocityChange;

    public Vector3 moveDir;

	InputActionSet keyboardInput;
	InputActionSet controllerInput;

    //SharedKeyboard sharedKeyboard;
    //SharedController sharedController;
    //SeparateController separateController;
    //SeparateKeyboard separateKeyboard;

    public bool allowPreviousController = true;

    public int deviceNumber;


	InputDevice device;

	void Awake()
	{
		if (character == null)
		{
			character = GetComponent<CharacterComponents>();
		}

        if(Globals.Instance != null)
        {
            if(playerNumber == Player.Player1)
            {
                Globals.Instance.Player1 = this;
            }
            if (playerNumber == Player.Player2)
            {
                Globals.Instance.Player2 = this;
            }
        }

		CreateActionSets();
	}

	void FixedUpdate () 
	{
        controlScheme = playerNumber == Player.Player1 ? Globals.Instance.player1Controls : Globals.Instance.player2Controls;

        if (controlScheme.inputNameSelected == Globals.InputNameSelected.LeftController)
            device = Globals.Instance.leftControllerInputDevice;
        else if (controlScheme.inputNameSelected == Globals.InputNameSelected.RightController)
            device = Globals.Instance.rightControllerInputDevice;
        else
            device = null;

       // device = deviceNumber >= 0 && deviceNumber < InputManager.Devices.Count ? InputManager.Devices[deviceNumber] : null;

        if (device != null)
        {
			controllerInput.Device = device;
        }

        /*if(deviceNumber == -3)
        {
            if (playerNumber == Player.Player1)
            {
                Globals.Instance.player1Controls = new ControlsAndInput { controlScheme = Globals.ControlScheme.SharedLeft, inputNameSelected = Globals.InputNameSelected.Keyboard };
                Globals.Instance.player1Controls.controlScheme = Globals.Instance.CheckSoloInput(Globals.Instance.player1Controls, Globals.Instance.player2Controls);
                Globals.Instance.player2Controls.controlScheme = Globals.Instance.CheckSharedInput(Globals.Instance.player2Controls, Globals.Instance.player1Controls);
                Globals.Instance.player2Controls.controlScheme = Globals.Instance.CheckSoloInput(Globals.Instance.player2Controls, Globals.Instance.player1Controls);
            }
            if (playerNumber == Player.Player2)
            {
                Globals.Instance.player2Controls = new ControlsAndInput { controlScheme = Globals.ControlScheme.SharedRight, inputNameSelected = Globals.InputNameSelected.Keyboard };
                Globals.Instance.player2Controls.controlScheme = Globals.Instance.CheckSoloInput(Globals.Instance.player2Controls, Globals.Instance.player1Controls);
                Globals.Instance.player1Controls.controlScheme = Globals.Instance.CheckSharedInput(Globals.Instance.player1Controls, Globals.Instance.player2Controls);
                Globals.Instance.player1Controls.controlScheme = Globals.Instance.CheckSoloInput(Globals.Instance.player1Controls, Globals.Instance.player2Controls);
            }
        }*/

        #region Pause
        /*if (player1ControlScheme == Globals.ControlScheme.ControllerSolo)//Globals.usingController)
		{
			if (device != null && device.MenuWasPressed)
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
		else
		{
			if (Input.GetKeyDown(KeyCode.Escape))
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
        }*/
        #endregion

        if ((device != null || controlScheme.inputNameSelected == Globals.InputNameSelected.Keyboard) && Globals.Instance.allowInput)
		{
            if (controlScheme.inputNameSelected != Globals.InputNameSelected.Keyboard)
			{
				controllerInput.Enabled = true;
				keyboardInput.Enabled = false;
                velocityChange = controlScheme.controlScheme == Globals.ControlScheme.Solo ? PlayerControllerSoloMovement() : PlayerControllerSharedMovement();
			}
            else
			{
				controllerInput.Enabled = false;
				keyboardInput.Enabled = true;
                velocityChange = controlScheme.controlScheme == Globals.ControlScheme.Solo ? PlayerKeyboardSoloMovement() : PlayerKeyboardSharedMovement();
			}

			velocityChange = character.mover.ClampMovementChange(velocityChange, true, true);

			// Movement
			if (velocityChange.sqrMagnitude > 0)
			{
				character.mover.Accelerate(velocityChange, false);
				character.mover.slowDown = false;
			}
			else
			{
				character.mover.slowDown = true;
			}

			// Turn towards velocity change.
			moveDir = character.mover.velocity;
			if (moveDir.sqrMagnitude <= 0)
			{
				moveDir = transform.forward + (velocityChange.normalized * Time.deltaTime);
			}
			transform.LookAt(transform.position + moveDir, -Vector3.forward);
		}
	}

   
    public Vector3 PlayerControllerSharedMovement()
    {
        Vector2 stickInput = Vector2.zero;
        if (controlScheme.controlScheme == Globals.ControlScheme.SharedLeft)
        {
            stickInput = controllerInput.LMove;
			return stickInput.sqrMagnitude > Mathf.Pow(deadZone, 2f) ? new Vector3(controllerInput.LMove.X, controllerInput.LMove.Y, 0) : Vector3.zero;
        }
        else if (controlScheme.controlScheme == Globals.ControlScheme.SharedRight)
        {
			stickInput = controllerInput.RMove;
			return stickInput.sqrMagnitude > Mathf.Pow(deadZone, 2f) ? new Vector3(controllerInput.RMove.X, controllerInput.RMove.Y, 0) : Vector3.zero;
        }

        return Vector3.zero;
    }

    public Vector3 PlayerControllerSoloMovement()
    {
		Vector2 stickInput = controllerInput.LMove.Vector + controllerInput.RMove.Vector;//separateController.Move;
        return stickInput.sqrMagnitude > Mathf.Pow(deadZone, 2f) ? new Vector3(stickInput.x, stickInput.y, 0) : Vector3.zero;
    }

    public Vector3 PlayerKeyboardSharedMovement()
    {
        if (controlScheme.controlScheme == Globals.ControlScheme.SharedLeft)
        {
			return new Vector3(keyboardInput.LMove.X, keyboardInput.LMove.Y, 0);
        }
        else if (controlScheme.controlScheme == Globals.ControlScheme.SharedRight)
        {
			return new Vector3(keyboardInput.RMove.X, keyboardInput.RMove.Y, 0);
        }
        return Vector3.zero;
    }

    public Vector3 PlayerKeyboardSoloMovement()
    {
		Vector2 keyInput = keyboardInput.LMove.Vector + keyboardInput.RMove.Vector;
		return new Vector3(keyInput.x, keyInput.y,0);
    }	
	
	private void OnCollisionEnter(Collision col)
	{
		if (col.collider.gameObject.tag == "Character")
		{
			CharacterComponents partnerCharacter = col.collider.GetComponent<CharacterComponents>();
			if (!character.bondAttachable.IsBondMade(partnerCharacter.bondAttachable))
			{
				character.bondAttachable.volleyPartner = partnerCharacter.bondAttachable;
				character.SetFlashAndFill(partnerCharacter.colors.baseColor);
				partnerCharacter.bondAttachable.volleyPartner = character.bondAttachable;
				partnerCharacter.SetFlashAndFill(character.colors.baseColor);
				character.bondAttachable.AttemptBond(partnerCharacter.bondAttachable, col.contacts[0].point, true);
			}
		}
		/*else if (Globals.Instance != null && !Globals.Instance.fluffsThrowable)
		{
			LeaveFluff(col);
		}*/
	}

	private void OnTriggerStay(Collider col)
	{
		if (col.gameObject.tag != "Character" && Globals.Instance != null && !Globals.Instance.fluffsThrowable)
		{
			FluffStick fluffStick = col.GetComponent<FluffStick>();
			if (fluffStick != null && fluffStick.CanStick())
			{
				LeaveFluff(fluffStick);
			}
			
		}
	}

	private void LeaveFluff(FluffStick fluffStick)
	{
		// Check if any fluffs are on the character.
		Fluff firstFluff = null;
		if (character.fluffHandler.fluffs.Count > 0)
		{
			firstFluff = character.fluffHandler.fluffs[0];
		}

		Vector3 leavePos = fluffStick.transform.position + fluffStick.transform.TransformDirection(fluffStick.stickOffset);

		if (firstFluff != null && !Physics.GetIgnoreLayerCollision(firstFluff.gameObject.layer, fluffStick.gameObject.layer))
		{
			Fluff leavee = null;
			float bestDot = -Mathf.Infinity;
			for (int i = 0; i < character.fluffHandler.fluffs.Count; i++)
			{
				float fluffDotCol = Vector3.Dot(character.fluffHandler.fluffs[i].transform.position - transform.position, leavePos - transform.position);
				if (fluffDotCol >= bestDot && character.fluffHandler.fluffs[i] != character.fluffHandler.spawnedFluff)
				{
					bestDot = fluffDotCol;
					leavee = character.fluffHandler.fluffs[i];
				}
			}

			if (leavee != null)
			{
				/*bool tooClose = false;
				for (int i = 0; i < Globals.Instance.allFluffs.Count && !tooClose; i++)
				{
					Fluff checkFluff = Globals.Instance.allFluffs[i];
					if (checkFluff.attachee != null && checkFluff.attachee.gameObject == col.collider.gameObject
						&& (col.contacts[0].point - checkFluff.transform.position).sqrMagnitude < Mathf.Pow(Globals.Instance.fluffLeaveDistance, 2))
					{
						tooClose = true;
					}
				}*/

				//if (!tooClose)
				//{
					character.fluffHandler.fluffs.Remove(leavee);
					leavee.attachee = null;
					leavee.nonAttractTime = Globals.Instance.fluffLeaveAttractWait;
					leavee.attractable = false;
					if (OrphanFluffHolder.Instance != null)
					{
						leavee.transform.parent = OrphanFluffHolder.Instance.transform;
					}
					else
					{
						leavee.transform.parent = transform.parent;
					}
					//leavee.Pass((leavePos - transform.position) * character.fluffThrow.passForce, gameObject, Globals.Instance.fluffLeaveAttractWait);
					leavee.Attach(fluffStick, true);
				//}
			}
		}
	}



	#region Helper Methods

	private void CreateActionSets()
	{
		keyboardInput = new InputActionSet();
		controllerInput = new InputActionSet ();
		
		//Shared Keyboard
		keyboardInput.LUp.AddDefaultBinding(Key.W);
		keyboardInput.LDown.AddDefaultBinding(Key.S);
		keyboardInput.LLeft.AddDefaultBinding(Key.A);
		keyboardInput.LRight.AddDefaultBinding(Key.D);
		keyboardInput.RUp.AddDefaultBinding(Key.UpArrow);
		keyboardInput.RDown.AddDefaultBinding(Key.DownArrow);
		keyboardInput.RLeft.AddDefaultBinding(Key.LeftArrow);
		keyboardInput.RRight.AddDefaultBinding(Key.RightArrow);
		
		//Shared Controller
		controllerInput.LUp.AddDefaultBinding(InputControlType.LeftStickUp);
		controllerInput.LDown.AddDefaultBinding(InputControlType.LeftStickDown);
		controllerInput.LLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
		controllerInput.LRight.AddDefaultBinding(InputControlType.LeftStickRight);
		controllerInput.RUp.AddDefaultBinding(InputControlType.RightStickUp);
		controllerInput.RDown.AddDefaultBinding(InputControlType.RightStickDown);
		controllerInput.RLeft.AddDefaultBinding(InputControlType.RightStickLeft);
		controllerInput.RRight.AddDefaultBinding(InputControlType.RightStickRight);
	}


	private void CheckDevices()
	{
        /*
		//TODO: TEXT for pause menu
        if (!Globals.usingController && InputManager.Devices.Count == 0)
            Globals.numberOfControllers = InputManager.Devices.Count;

        if (Globals.numberOfControllers > 0 && InputManager.Devices.Count == 0)
		{
			canvasPaused.SetActive(true);
			Time.timeScale = 0;
			Globals.isPaused = true;

			Globals.numberOfControllers = 0;
		}
        else if (Globals.numberOfControllers == 0 && InputManager.Devices.Count > 0)
		{
			canvasPaused.SetActive(true);
			Time.timeScale = 0;
			Globals.isPaused = true;
            Globals.numberOfControllers = InputManager.Devices.Count;
		}*/
	}

  /*  private void WaitForInput(Player playerNumber)
    {
        if(playerNumber == Player.Player1)
        {
            if(Globals.Instance.player1Device == -2)
            {
                //Debug.Log("Here 1");
                device = InputManager.ActiveDevice;

                if(InputManager.Devices.IndexOf(device) != Globals.Instance.player2Device)
                {
                    if (InputManager.Devices.IndexOf(device) == Globals.Instance.player1PreviousDevice && !allowPreviousController)
                    { 
                    }
                    else
                        Globals.Instance.player1Device = InputManager.Devices.IndexOf(device);
                }
            }
        }
        if (playerNumber == Player.Player2)
        {
           // Debug.Log("Here 2");
            if (Globals.Instance.player2Device == -2)
            {
                device = InputManager.ActiveDevice;
               // Debug.Log(InputManager.Devices.IndexOf(device));
                if (InputManager.Devices.IndexOf(device) != Globals.Instance.player1Device)
                {
                    if (InputManager.Devices.IndexOf(device) == Globals.Instance.player2PreviousDevice && !allowPreviousController)
                    {
                    }
                    else
                    Globals.Instance.player2Device = InputManager.Devices.IndexOf(device);
                }
            }
        }
    }*/

	/*private void CheckInputMethod()
	{

		if (InputManager.Devices.Count > 0)
		{
			InputDevice device = InputManager.ActiveDevice;
			if (device.LeftTrigger.IsPressed && device.RightTrigger.IsPressed)
			{
				Globals.usingController = true;
				canvasPaused.SetActive(false);
				Time.timeScale = 1;
				Globals.isPaused = false;
			}
		}

		if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.RightControl))
		{
			Globals.usingController = false;
			canvasPaused.SetActive(false);
			Time.timeScale = 1;
			Globals.isPaused = false;
		}
	}*/

    /*private void ResetDeviceNumber()
    {
        Globals.Instance.player2Device = -2;
        if (InputManager.controllerCount < 2)
        {
            Globals.Instance.player2Device = -1;
            Globals.Instance.player2ControlScheme = Globals.ControlScheme.KeyboardSharedRight;
            Globals.Instance.player2InputNameSelected = Globals.InputNameSelected.Keyboard;

            if (Globals.Instance.player1ControlScheme == Globals.ControlScheme.KeyboardSolo)
                Globals.Instance.player1ControlScheme = Globals.ControlScheme.KeyboardSharedLeft;
        }
    }*/


   

	
	#endregion



}

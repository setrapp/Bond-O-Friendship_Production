using UnityEngine;
using System.Collections;
using InControl;

public class PlayerInput : MonoBehaviour {

	public CharacterComponents character;

    public enum Player { Player1, Player2 };
    public Player playerNumber;

    public ControlsAndInput controlScheme;

	public float deadZone = .75f;

	private Vector3 velocityChange;

    SharedKeyboard sharedKeyboard;
    SharedController sharedController;
    SeparateController separateController;
    SeparateKeyboard separateKeyboard;

    public bool allowPreviousController = true;

    public int deviceNumber;


	InputDevice device;

	void Awake()
	{
		if (character == null)
		{
			character = GetComponent<CharacterComponents>();
		}

        sharedKeyboard = new SharedKeyboard();
        sharedController = new SharedController();
        separateController = new SeparateController();
        separateKeyboard = new SeparateKeyboard();

        //Shared Keyboard
        sharedKeyboard.LUp.AddDefaultBinding(Key.W);
        sharedKeyboard.LDown.AddDefaultBinding(Key.S);
        sharedKeyboard.LLeft.AddDefaultBinding(Key.A);
        sharedKeyboard.LRight.AddDefaultBinding(Key.D);
        sharedKeyboard.RUp.AddDefaultBinding(Key.UpArrow);
        sharedKeyboard.RDown.AddDefaultBinding(Key.DownArrow);
        sharedKeyboard.RLeft.AddDefaultBinding(Key.LeftArrow);
        sharedKeyboard.RRight.AddDefaultBinding(Key.RightArrow);

        //Shared Controller
        sharedController.LUp.AddDefaultBinding(InputControlType.LeftStickUp);
        sharedController.LDown.AddDefaultBinding(InputControlType.LeftStickDown);
        sharedController.LLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
        sharedController.LRight.AddDefaultBinding(InputControlType.LeftStickRight);
        sharedController.RUp.AddDefaultBinding(InputControlType.RightStickUp);
        sharedController.RDown.AddDefaultBinding(InputControlType.RightStickDown);
        sharedController.RLeft.AddDefaultBinding(InputControlType.RightStickLeft);
        sharedController.RRight.AddDefaultBinding(InputControlType.RightStickRight);

        //Separate Controller
        separateController.Up.AddDefaultBinding(InputControlType.LeftStickUp);
        separateController.Down.AddDefaultBinding(InputControlType.LeftStickDown);
        separateController.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
        separateController.Right.AddDefaultBinding(InputControlType.LeftStickRight);
        separateController.Up.AddDefaultBinding(InputControlType.RightStickUp);
        separateController.Down.AddDefaultBinding(InputControlType.RightStickDown);
        separateController.Left.AddDefaultBinding(InputControlType.RightStickLeft);
        separateController.Right.AddDefaultBinding(InputControlType.RightStickRight);

        //Sepatate Keyboard
        separateKeyboard.Up.AddDefaultBinding(Key.W);
        separateKeyboard.Down.AddDefaultBinding(Key.S);
        separateKeyboard.Left.AddDefaultBinding(Key.A);
        separateKeyboard.Right.AddDefaultBinding(Key.D);
        separateKeyboard.Up.AddDefaultBinding(Key.UpArrow);
        separateKeyboard.Down.AddDefaultBinding(Key.DownArrow);
        separateKeyboard.Left.AddDefaultBinding(Key.LeftArrow);
        separateKeyboard.Right.AddDefaultBinding(Key.RightArrow);
	}

	void Update () 
	{
        controlScheme = playerNumber == Player.Player1 ? Globals.Instance.player1Controls : Globals.Instance.player2Controls;

        if (controlScheme.inputNameSelected == Globals.InputNameSelected.LeftController)
            deviceNumber = Globals.Instance.leftControllerIndex;
        else if (controlScheme.inputNameSelected == Globals.InputNameSelected.RightController)
            deviceNumber = Globals.Instance.rightContollerIndex;
        else
            deviceNumber = -1;

        device = deviceNumber >= 0 ? InputManager.Devices[deviceNumber] : null;

        if (device != null)
        {
            sharedController.Device = device;
            separateController.Device = device;
        }

        if(deviceNumber == -3)
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
        }

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

        if (!Globals.isPaused && (device != null || controlScheme.inputNameSelected == Globals.InputNameSelected.Keyboard) && Globals.Instance.allowInput)
		{
            if (controlScheme.inputNameSelected != Globals.InputNameSelected.Keyboard)
                velocityChange = controlScheme.controlScheme == Globals.ControlScheme.Solo ? PlayerControllerSoloMovement() : PlayerControllerSharedMovement();
            else
                velocityChange = controlScheme.controlScheme == Globals.ControlScheme.Solo ? PlayerKeyboardSoloMovement() : PlayerKeyboardSharedMovement();

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
			Vector3 moveDir = character.mover.velocity;
			if (moveDir.sqrMagnitude <= 0)
			{
				moveDir = transform.forward + (velocityChange.normalized * Time.deltaTime);
			}
			transform.LookAt(transform.position + moveDir, -Vector3.forward);
		}
	}

   
    private Vector3 PlayerControllerSharedMovement()
    {
        Vector2 stickInput = Vector2.zero;
        if (controlScheme.controlScheme == Globals.ControlScheme.SharedLeft)
        {
            stickInput = sharedController.LMove;
            return stickInput.sqrMagnitude > Mathf.Pow(deadZone, 2f) ? new Vector3(sharedController.LMove.X, sharedController.LMove.Y, 0) : Vector3.zero;
        }
        else if (controlScheme.controlScheme == Globals.ControlScheme.SharedRight)
        {
            stickInput = sharedController.RMove;
            return stickInput.sqrMagnitude > Mathf.Pow(deadZone, 2f) ? new Vector3(sharedController.RMove.X, sharedController.RMove.Y, 0) : Vector3.zero;
        }

        return Vector3.zero;
    }

    private Vector3 PlayerControllerSoloMovement()
    {
        Vector2 stickInput = Vector2.zero;
        stickInput = separateController.Move;
        return stickInput.sqrMagnitude > Mathf.Pow(deadZone, 2f) ? new Vector3(separateController.Move.X, separateController.Move.Y, 0) : Vector3.zero;
    }

    private Vector3 PlayerKeyboardSharedMovement()
    {
        if (controlScheme.controlScheme == Globals.ControlScheme.SharedLeft)
        {
            return new Vector3(sharedKeyboard.LMove.X, sharedKeyboard.LMove.Y, 0);
        }
        else if (controlScheme.controlScheme == Globals.ControlScheme.SharedRight)
        {
            return new Vector3(sharedKeyboard.RMove.X, sharedKeyboard.RMove.Y, 0);
        }
        return Vector3.zero;
    }

    private Vector3 PlayerKeyboardSoloMovement()
    {
        return new Vector3(separateKeyboard.Move.X, separateKeyboard.Move.Y,0);
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

using UnityEngine;
using System.Collections;
using InControl;

public class PlayerInput : MonoBehaviour {
	public CharacterComponents character;
	public enum Player{Player1, Player2};

    public Globals.ControlScheme controlScheme;
    public Globals.ControlScheme otherPlayerControlScheme;

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

    SharedKeyboard sharedKeyboard;
    SharedController sharedController;
    SeparateController separateController;
    SeparateKeyboard separateKeyboard;

    public bool allowPreviousController = true;
	//private bool paused = false;
	
	InputDevice device;

	private bool oneController;

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
        /*This is for debug purposes, will be cleaned up and moved elsewhere later*
        if ((Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSharedLeft || Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSharedRight) && Globals.Instance.player2ControlScheme == Globals.Instance.player1ControlScheme)
        {
            Globals.Instance.player2ControlScheme = Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSharedLeft ? Globals.ControlScheme.ControllerSharedRight : Globals.ControlScheme.ControllerSharedLeft;
        }
        if ((Globals.Instance.player1ControlScheme == Globals.ControlScheme.KeyboardSharedLeft || Globals.Instance.player1ControlScheme == Globals.ControlScheme.KeyboardSharedRight) && Globals.Instance.player2ControlScheme == Globals.Instance.player1ControlScheme)
        {
            Globals.Instance.player2ControlScheme = Globals.Instance.player1ControlScheme == Globals.ControlScheme.KeyboardSharedLeft ? Globals.ControlScheme.KeyboardSharedRight : Globals.ControlScheme.KeyboardSharedLeft;
        }
        //End Debug comment area*/

       // Debug.Log(Globals.Instance.player1ControlScheme);

        controlScheme = playerNumber == Player.Player1 ? Globals.Instance.player1ControlScheme : Globals.Instance.player2ControlScheme;
        otherPlayerControlScheme = playerNumber == Player.Player1 ? Globals.Instance.player2ControlScheme : Globals.Instance.player1ControlScheme;

        if((controlScheme == Globals.ControlScheme.ControllerSharedLeft || controlScheme == Globals.ControlScheme.ControllerSharedRight) && (otherPlayerControlScheme != Globals.ControlScheme.ControllerSharedLeft && otherPlayerControlScheme != Globals.ControlScheme.ControllerSharedRight))
        {
            if (playerNumber == Player.Player1)
                Globals.Instance.player1ControlScheme = Globals.ControlScheme.ControllerSolo;
            else
                Globals.Instance.player2ControlScheme = Globals.ControlScheme.ControllerSolo;
        }
        

		if(playerNumber == Player.Player1)
			CheckDevices();


        WaitForInput(playerNumber);

        if(playerNumber == Player.Player1)
            device = Globals.Instance.player1Device >= 0 ? InputManager.Devices[Globals.Instance.player1Device] : null;
        if (playerNumber == Player.Player2)
            device = Globals.Instance.player2Device >= 0 ? InputManager.Devices[Globals.Instance.player2Device] : null;

        if(device != null)
        {
            sharedController.Device = device;
            separateController.Device = device;
            if(device.Name == "")
            {
                if (playerNumber == Player.Player1)
                    Globals.Instance.player1Device = -2;
                if (playerNumber == Player.Player2)
                    Globals.Instance.player2Device = -2;
            }
                
        }
        //device = playerNumber == Player.Player1 ? InputManager.Devices[Globals.Instance.playerOneDevice] : InputManager.Devices[Globals.Instance.playerTwoDevice];
        /*
        if (InputManager.Devices.Count > 0 && (controlScheme == Globals.ControlScheme.ControllerSharedLeft || controlScheme == Globals.ControlScheme.ControllerSharedRight))
            device = InputManager.ActiveDevice;
        if (InputManager.Devices.Count > 0 && (controlScheme == Globals.ControlScheme.ControllerSolo) && (otherPlayerControlScheme == Globals.ControlScheme.KeyboardSolo))
            device = InputManager.ActiveDevice;
        if (InputManager.Devices.Count > 1 && (controlScheme == Globals.ControlScheme.ControllerSolo) && (otherPlayerControlScheme == Globals.ControlScheme.ControllerSolo))
        {
            if(playerNumber == Player.Player1)
                device = InputManager.Devices[0];
            if (playerNumber == Player.Player2)
                device = InputManager.Devices[1];
        }*/

        #region Pause
        if (controlScheme == Globals.ControlScheme.ControllerSolo)//Globals.usingController)
		{
			if (playerNumber == Player.Player1 && device != null && device.MenuWasPressed)
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
			if (Input.GetKeyDown(KeyCode.Escape) && playerNumber == Player.Player1)
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
        #endregion

        if (!Globals.isPaused && (device != null || controlScheme == Globals.ControlScheme.KeyboardSolo || controlScheme == Globals.ControlScheme.KeyboardSharedLeft || controlScheme == Globals.ControlScheme.KeyboardSharedRight))
		{
			//AttemptFluffThrow();
			AttemptFluffAttract();

            if(controlScheme == Globals.ControlScheme.ControllerSharedLeft || controlScheme == Globals.ControlScheme.ControllerSharedRight || controlScheme == Globals.ControlScheme.ControllerSolo)
                velocityChange = controlScheme == Globals.ControlScheme.ControllerSolo ? PlayerControllerSoloMovement() : PlayerControllerSharedMovement();
            else
                velocityChange = controlScheme == Globals.ControlScheme.KeyboardSolo ? PlayerKeyboardSoloMovement() : PlayerKeyboardSharedMovement();//PlayerJoystickMovement();

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
		else
		{
			//if(playerNumber == Player.Player1)
			//CheckInputMethod();
		}
	}

   
    private Vector3 PlayerControllerSharedMovement()
    {
        Vector2 stickInput = Vector2.zero;
        if(controlScheme == Globals.ControlScheme.ControllerSharedLeft)
        {
            stickInput = sharedController.LMove;
            return stickInput.sqrMagnitude > Mathf.Pow(deadZone, 2f) ? new Vector3(sharedController.LMove.X, sharedController.LMove.Y, 0) : Vector3.zero;
        }
        else if(controlScheme == Globals.ControlScheme.ControllerSharedRight)
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
        if (controlScheme == Globals.ControlScheme.KeyboardSharedLeft)
        {
            return new Vector3(sharedKeyboard.LMove.X, sharedKeyboard.LMove.Y, 0);
        }
        else if (controlScheme == Globals.ControlScheme.KeyboardSharedRight)
        {
            return new Vector3(sharedKeyboard.RMove.X, sharedKeyboard.RMove.Y, 0);
        }
        return Vector3.zero;
    }

    private Vector3 PlayerKeyboardSoloMovement()
    {
        return new Vector3(separateKeyboard.Move.X, separateKeyboard.Move.Y,0);
    }

    /*No longer Needed
	private Vector3 PlayerJoystickMovement()
	{

		Vector2 stickInput = Vector2.zero;

		if (Globals.usingController)
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
			Vector3 keyVector = Vector3.zero;
			if(playerNumber == Player.Player1)
			{
				if (Input.GetKey(KeyCode.W))
					keyVector += Vector3.up;
				if (Input.GetKey(KeyCode.S))
					keyVector -= Vector3.up;
				if (Input.GetKey(KeyCode.A))
					keyVector -= Vector3.right;
				if (Input.GetKey(KeyCode.D))
					keyVector += Vector3.right;
			}

			if (playerNumber == Player.Player2)
			{
				if (Input.GetKey(KeyCode.UpArrow))
					keyVector += Vector3.up;
				if (Input.GetKey(KeyCode.DownArrow))
					keyVector -= Vector3.up;
				if (Input.GetKey(KeyCode.LeftArrow))
					keyVector -= Vector3.right;
				if (Input.GetKey(KeyCode.RightArrow))
					keyVector += Vector3.right;
			}

			return keyVector;
		}

		return Vector3.zero;
	}*/
	
	private void AttemptFluffAttract()
	{
		// If attractor is automatic, skip this input.
		if (Globals.Instance.autoAttractor)
		{
			return;
		}

		bool canAttract = false;

		if (Globals.usingController)
		{
			if (playerNumber == Player.Player1 && device.LeftBumper.IsPressed)
				canAttract = true;
			else if (playerNumber == Player.Player2 && device.RightBumper.IsPressed)
				canAttract = true;
		}
		else
		{
			if (playerNumber == Player.Player1 && Input.GetKey(KeyCode.LeftShift))
				canAttract = true;
			else if (playerNumber == Player.Player2 && Input.GetKey(KeyCode.RightShift))
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

		if (Globals.usingController)
		{
			if (playerNumber == Player.Player1)
			{
				if (device.LeftTrigger.WasPressed || (Globals.Instance.autoAttractor && device.LeftBumper.WasPressed))
				{
					lookAt = transform.forward;
					minToFire = 0;
				}
			}
			else if (playerNumber == Player.Player2 || (Globals.Instance.autoAttractor && device.RightBumper.WasPressed))
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
			if (playerNumber == Player.Player1 && (Input.GetKey(KeyCode.LeftControl) || (Globals.Instance.autoAttractor && Input.GetKey(KeyCode.Space))))
			{
				lookAt = transform.forward;
				minToFire = 0;
			}
			else if (playerNumber == Player.Player2 && (Input.GetKey(KeyCode.RightControl) || (Globals.Instance.autoAttractor && Input.GetKey(KeyCode.Return))))
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

	private void OnCollisionEnter(Collision col)
	{
		if (col.collider.gameObject.tag == "Character")
		{
			CharacterComponents partnerCharacter = col.collider.GetComponent<CharacterComponents>();
			if (!character.bondAttachable.IsBondMade(partnerCharacter.bondAttachable))
			{
				character.bondAttachable.volleyPartner = partnerCharacter.bondAttachable;
				character.SetFlashAndFill(partnerCharacter.colors.attachmentColor);
				partnerCharacter.bondAttachable.volleyPartner = character.bondAttachable;
				partnerCharacter.SetFlashAndFill(character.colors.attachmentColor);
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
			float bestDot = 0;
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

    private void WaitForInput(Player playerNumber)
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
    }

	private void CheckInputMethod()
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
	}

	

   

	
	#endregion



}

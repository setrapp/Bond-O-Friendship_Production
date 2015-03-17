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
		if(playerNumber == Player.Player1)
			CheckDevices();
		if(Globals.usingController && InputManager.controllerCount > 0)
			device = InputManager.ActiveDevice;
		
 
		//if (Input.GetKeyDown(KeyCode.Escape))
		//{
		//	Application.Quit();
		//}

		if (Globals.usingController)
		{
			if (device.MenuWasPressed && playerNumber == Player.Player1)
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

		if (!Globals.isPaused)
		{
			AttemptFluffThrow();
			AttemptFluffAttract();
			velocityChange =  PlayerJoystickMovement();
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
			transform.LookAt(transform.position + moveDir, transform.up);
		}
		else
		{
			if(playerNumber == Player.Player1)
			CheckInputMethod();
		}
	}

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
	}
	
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
		else if (Globals.Instance != null && !Globals.Instance.fluffsThrowable)
		{
			LeaveFluff(col);
		}
	}

	private void OnCollisionStay(Collision col)
	{
		if (col.collider.gameObject.tag != "Character" && Globals.Instance != null && !Globals.Instance.fluffsThrowable)
		{
			LeaveFluff(col);
		}
	}

	private void LeaveFluff(Collision col)
	{
		// Check if any fluffs are on the character.
		Fluff firstFluff = null;
		if (character.fluffHandler.fluffs.Count > 0)
		{
			firstFluff = character.fluffHandler.fluffs[0];
		}
		
		
		if (firstFluff != null && !Physics.GetIgnoreLayerCollision(firstFluff.gameObject.layer, col.collider.gameObject.layer) && col.contacts.Length > 0)
		{
			Fluff leavee = null;
			float bestDot = 0;
			for (int i = 0; i < character.fluffHandler.fluffs.Count; i++)
			{
				float fluffDotCol = Vector3.Dot(character.fluffHandler.fluffs[i].transform.position - transform.position, col.contacts[0].point - transform.position);
				if (fluffDotCol >= bestDot && character.fluffHandler.fluffs[i] != character.fluffHandler.spawnedFluff)
				{
					bestDot = fluffDotCol;
					leavee = character.fluffHandler.fluffs[i];
				}
			}

			if (leavee != null)
			{
				bool tooClose = false;
				for (int i = 0; i < Globals.Instance.allFluffs.Count && !tooClose; i++)
				{
					Fluff checkFluff = Globals.Instance.allFluffs[i];
					if (checkFluff.attachee != null && checkFluff.attachee.gameObject == col.collider.gameObject
						&& (col.contacts[0].point - checkFluff.transform.position).sqrMagnitude < Mathf.Pow(Globals.Instance.fluffLeaveDistance, 2))
					{
						tooClose = true;
					}
				}

				if (!tooClose)
				{
					character.fluffHandler.fluffs.Remove(leavee);
					if (OrphanFluffHolder.Instance != null)
					{
						leavee.transform.parent = OrphanFluffHolder.Instance.transform;
					}
					else
					{
						leavee.transform.parent = transform.parent;
					}
					leavee.Pass((col.contacts[0].point - transform.position) * character.fluffThrow.passForce, gameObject, Globals.Instance.fluffLeaveAttractWait);
				}
			}
		}
	}



	#region Helper Methods

	private void CheckDevices()
	{
		//TODO: TEXT for pause menu
		if (!Globals.usingController && InputManager.controllerCount == 0)
			Globals.numberOfControllers = InputManager.controllerCount;

		if (Globals.numberOfControllers > 0 && InputManager.controllerCount == 0)
		{
			canvasPaused.SetActive(true);
			Time.timeScale = 0;
			Globals.isPaused = true;

			Globals.numberOfControllers = 0;
		}
		else if(Globals.numberOfControllers == 0 && InputManager.controllerCount > 0)
		{
			canvasPaused.SetActive(true);
			Time.timeScale = 0;
			Globals.isPaused = true;
			Globals.numberOfControllers = InputManager.controllerCount;
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

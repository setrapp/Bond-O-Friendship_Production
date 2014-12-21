using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
	public CharacterComponents character;
	public enum Player{Player1, Player2};

	public Globals.ControlScheme controlScheme;

	public Player playerNumber;
	public Globals.JoyStick joystickNumber;
	public GameObject canvasStart;

	public GameObject canvasPaused;

	public bool useKeyboard = false;

	public PlayerInput otherPlayerInput;

	public bool sharing = false;

	public GameObject geometry;
	public float deadZone = .75f;

	private bool fireFluffReady = true;
	private Vector3 velocityChange;


	public bool swapJoysticks = false;

	private Vector3 target;
	public Vector3 desiredLook;
	public bool joystickDetermined = false;

	private bool paused = false;

	private float i = 0;

	void Awake()
	{
		if (character == null)
		{
			character = GetComponent<CharacterComponents>();
		}
	}

	void Start()
	{
		if (otherPlayerInput == null && Globals.Instance != null)
		{
			if (Globals.Instance.player1 == this)
			{
				otherPlayerInput = Globals.Instance.player2;
			}
			else if (Globals.Instance.player2 == this)
			{
				otherPlayerInput = Globals.Instance.player1;
			}
		}

		sharing = Globals.sharing;
		if(playerNumber == Player.Player1)
		{
			joystickNumber = Globals.playerOneJoystickNumber;
			controlScheme = Globals.playerOneControlScheme;
		}
		else
		{
			if(!sharing)
			{
				joystickNumber = Globals.playerTwoJoystickNumber;
				controlScheme = Globals.playerTwoControlScheme;
			}
			else
			{
				joystickNumber = Globals.playerOneJoystickNumber;
				controlScheme = Globals.playerTwoControlScheme;
			}
		}


	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		if (GetPause())
		{
			if (paused)
			{
				canvasPaused.SetActive(false);
				Time.timeScale = 1;
			}
			else
			{
				canvasPaused.SetActive(true);
				Time.timeScale = 0;
			}

			paused = !paused;
		}
				
		if(!paused)
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

		Vector2 leftStickInput = new Vector2(GetAxisMoveHorizontal(), GetAxisMoveVertical());

		if(sharing && playerNumber == Player.Player2)
		{
			leftStickInput = new Vector2(GetAxisAimHorizontal(), GetAxisAimVertical());
			return leftStickInput.sqrMagnitude > Mathf.Pow(deadZone, 2f) ? new Vector3(GetAxisAimHorizontal(),GetAxisAimVertical(),0) : Vector3.zero;
		}

		return leftStickInput.sqrMagnitude > Mathf.Pow(deadZone, 2f) ? new Vector3(GetAxisMoveHorizontal(),GetAxisMoveVertical(),0) : Vector3.zero;
	}

	private void AttemptFluffAttract()
	{
		bool canAttract = false;

		if (sharing && playerNumber == Player.Player1 && GetRightBumperAbsorb() > 0.5f)
			canAttract = true;
		else if (sharing && playerNumber == Player.Player2 && GetLeftBumperAbsorb() > 0.5f)
			canAttract = true;
		else if (!sharing && (GetAbsorb() || GetRightBumperAbsorb() > 0.5f || GetLeftBumperAbsorb() > 0.5f))
			canAttract = true;

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
		Vector2 lookAt = CalculateThrowDirection();	


		if(controlScheme == Globals.ControlScheme.triggers || sharing)
		{
			lookAt = Vector2.zero;
		}

		float minToFire = deadZone;

		if(!sharing && controlScheme == Globals.ControlScheme.triggers && (GetAxisTriggers() > deadZone|| GetAxisTriggers() < -deadZone))
		{
			lookAt = transform.forward;
			minToFire = 0;
		}
		
		if(sharing)
		{
			if(playerNumber == Player.Player1)
			{
				if(GetAxisLeftTrigger() > deadZone)
				{
					lookAt = transform.forward;
					minToFire = 0;
				}
			}
			else if(playerNumber == Player.Player2)
			{
				if(GetAxisRightTrigger() > deadZone)
				{
					lookAt = transform.forward;
					minToFire = 0;
				}
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
	
	private float GetAxisMoveHorizontal(){if(!swapJoysticks)return Input.GetAxis(joystickNumber.ToString() + "LeftStickHorizontal"); else return Input.GetAxis(joystickNumber.ToString() +"RightStickHorizontal");}
	private float GetAxisMoveVertical(){if(!swapJoysticks)return Input.GetAxis(joystickNumber.ToString() +"LeftStickVertical"); else return Input.GetAxis(joystickNumber.ToString() +"RightStickVertical");}
	private float GetAxisAimHorizontal(){if(!swapJoysticks)return Input.GetAxis(joystickNumber.ToString() +"RightStickHorizontal"); else return Input.GetAxis(joystickNumber.ToString() + "LeftStickHorizontal");}
	private float GetAxisAimVertical(){if(!swapJoysticks)return Input.GetAxis(joystickNumber.ToString() +"RightStickVertical"); else return Input.GetAxis(joystickNumber.ToString() +"LeftStickVertical");}
	private float GetAxisTriggers(){return Input.GetAxis(joystickNumber.ToString() + "Triggers");}
	private float GetAxisLeftTrigger() {return Input.GetAxis(joystickNumber.ToString() + "LeftTrigger");}
	private float GetAxisRightTrigger() {return Input.GetAxis(joystickNumber.ToString() + "RightTrigger");}
	private float GetAxisStickThrow(){return Input.GetAxis(joystickNumber.ToString() + "StickThrow");}
	private bool GetAbsorb() { return Input.GetButton(joystickNumber.ToString() + "Absorb");}
	private float GetRightBumperAbsorb() { return Input.GetAxis(joystickNumber.ToString() + "RightBumperAbsorb");}
	private float GetLeftBumperAbsorb() { return Input.GetAxis(joystickNumber.ToString() + "LeftBumperAbsorb");}
	private bool GetPause() { return Input.GetButtonDown(joystickNumber.ToString() + "Pause");}


	private Vector2 CalculateThrowDirection()
	{
		Vector2 lookAt = Vector2.zero; 
		lookAt = new Vector2(GetAxisAimHorizontal(), GetAxisAimVertical());

		return lookAt;
	}

	#endregion



}

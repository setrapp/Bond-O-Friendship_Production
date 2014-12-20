using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
	public SimpleMover mover;
	public PartnerLink partnerLink;
	protected Collider tailTrigger;
	public enum Player{Player1, Player2};

	//public enum JoyStick{Joy1, Joy2, Joy3, Joy4};

	public Globals.ControlScheme controlScheme;

	public Player playerNumber;
	public Globals.JoyStick joystickNumber;
	public GameObject canvasStart;

	public GameObject canvasPaused;

	public ParticleSystem absorbPrefab;

	public bool useKeyboard = false;

	public PlayerInput otherPlayerInput;

	public bool sharing = false;

	public GameObject geometry;
	public float deadZone = .75f;

	private bool fireFluffReady = true;
	private Vector3 velocityChange;


	public bool swapJoysticks = false;

	private ParticleSystem absorb;
	private Vector3 target;
	public Vector3 desiredLook;
	public bool joystickDetermined = false;

	private bool paused = false;

	public float pullSpeed;
	private float i = 0;

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

//		if(playerNumber == Player.Player2)
//			Debug.Log(joystickDetermined);

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

		//if(playerNumber == Player.Player2)
		//{
			//Debug.Log(controlScheme.ToString());
		//}

		//var gamepads = Input.GetJoystickNames();
		//useKeyboard = (gamepads.Length == 1 && playerNumber == Player.Player1) || gamepads.Length > 1 ? false : true;
		//if (useKeyboard && canvasStart.activeInHierarchy)
		//{
		//	canvasStart.SetActive(false);
		//}

		/*if(!useKeyboard && !joystickDetermined)
		{
			if(playerNumber == Player.Player1)
			{
				if(Input.GetButtonDown("Joy1Absorb") || Input.GetButtonDown("Joy1Pause") || Input.GetButtonDown("Joy1StickThrow"))
				{
					joystickNumber = JoyStick.Joy1;
					joystickDetermined = true;
				}
				if(Input.GetButtonDown("Joy2Absorb") || Input.GetButtonDown("Joy2Pause") || Input.GetButtonDown("Joy2StickThrow"))
				{
					joystickNumber = JoyStick.Joy2;
					joystickDetermined = true;
				}
				if(Input.GetButtonDown("Joy3Absorb") || Input.GetButtonDown("Joy3Pause") || Input.GetButtonDown("Joy3StickThrow"))
				{
					joystickNumber = JoyStick.Joy3;
					joystickDetermined = true;
				}
				if(Input.GetButtonDown("Joy4Absorb") || Input.GetButtonDown("Joy4Pause") || Input.GetButtonDown("Joy4StickThrow"))
				{
					joystickNumber = JoyStick.Joy4;
					joystickDetermined = true;
				}

				if(joystickDetermined)
					canvasStart.SetActive(false);
				//Debug.Log(joystickNumber.ToString());
			}
			else if(otherPlayerInput != null && otherPlayerInput.joystickDetermined)
			{
				if((Input.GetButtonDown("Joy1Absorb") || Input.GetButtonDown("Joy1Pause") || Input.GetButtonDown("Joy1StickThrow")) && otherPlayerInput.joystickNumber != JoyStick.Joy1)
				{
					joystickNumber = JoyStick.Joy1;
					joystickDetermined = true;
				}
				if((Input.GetButtonDown("Joy2Absorb") || Input.GetButtonDown("Joy2Pause") || Input.GetButtonDown("Joy2StickThrow")) && otherPlayerInput.joystickNumber != JoyStick.Joy2)
				{
					joystickNumber = JoyStick.Joy2;
					joystickDetermined = true;
				}
				if((Input.GetButtonDown("Joy3Absorb") || Input.GetButtonDown("Joy3Pause") || Input.GetButtonDown("Joy3StickThrow")) && otherPlayerInput.joystickNumber != JoyStick.Joy3)
				{
					joystickNumber = JoyStick.Joy3;
					joystickDetermined = true;
				}
				if((Input.GetButtonDown("Joy4Absorb") || Input.GetButtonDown("Joy4Pause") || Input.GetButtonDown("Joy4StickThrow")) && otherPlayerInput.joystickNumber != JoyStick.Joy4)
				{
					joystickNumber = JoyStick.Joy4;
					joystickDetermined = true;
				}

				if(joystickDetermined)
					canvasStart.SetActive(false);
			}
		}*/


				
			if(!paused)
			{
				PlayerLookAt();
				partnerLink.absorbing = Absorbing();

				velocityChange =  PlayerJoystickMovement();
				// Movement
				if (velocityChange.sqrMagnitude > 0)
				{
					mover.Accelerate(velocityChange, true, true);
					mover.slowDown = false;
				}
				else
				{
					mover.slowDown = true;
				}
				// Turn towards velocity change.
				transform.LookAt(transform.position + velocityChange, transform.up);

				if(absorb != null)
				{
					absorb.transform.position = transform.position;
				}
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

	bool Absorbing()
	{
		bool canAbsorb = false;

		if(sharing && playerNumber == Player.Player1 && GetRightBumperAbsorb() > .5)
			canAbsorb = true;
		else if(sharing && playerNumber == Player.Player2 && GetLeftBumperAbsorb() > .5)
			canAbsorb = true;
		else if(!sharing && (GetAbsorb() || GetRightBumperAbsorb() > .5 || GetLeftBumperAbsorb() > .5))
			canAbsorb = true;

		//Debug.Log(GetBumperAbsorb());

		if(!fireFluffReady)
			canAbsorb = false;

		if (canAbsorb)
		{
			if(absorb == null)
			{
				absorb = (ParticleSystem)Instantiate(absorbPrefab);
				absorb.transform.position = transform.position;
				absorb.startColor = GetComponent<ConnectionAttachable>().attachmentColor / 2;
				absorb.startColor = new Color(absorb.startColor.r, absorb.startColor.g, absorb.startColor.b, 0.1f);
			}
			GameObject[] fluffArray = GameObject.FindGameObjectsWithTag("Fluff");
			foreach(GameObject liveFluffObject in fluffArray)
			{
				Fluff liveFluff = liveFluffObject.GetComponent<Fluff>();
				if (liveFluff != null)
				{
					bool fluffAttachedToSelf = (liveFluff.attachee != null && liveFluff.attachee.gameObject == gameObject);
					if (!fluffAttachedToSelf)
					{
						float fluffSqrDist = (liveFluff.transform.position - transform.position).sqrMagnitude;
						Vector3 absorbOffset = Vector3.zero;
						// If the fluff is too far to be absorbed directly and absorption through the connection is enabled, attempt connection absorption.
						if (fluffSqrDist > Mathf.Pow(partnerLink.absorbStrength, 2) && partnerLink.connectionAbsorb)
						{
							float nearSqrDist = fluffSqrDist;
							for (int i = 0; i < partnerLink.connectionAttachable.connections.Count; i++)
							{
								// Only check connection distance to fluff if the connection is at least as long as the distance from this to the fluff.
								if (Mathf.Pow(partnerLink.connectionAttachable.connections[i].ConnectionLength, 2) >= fluffSqrDist)
								{
									Vector3 nearConnection = partnerLink.connectionAttachable.connections[i].NearestPoint(liveFluffObject.transform.position);
									float sqrDist = (liveFluffObject.transform.position - nearConnection).sqrMagnitude;
									if (sqrDist < nearSqrDist)
									{
										nearSqrDist = sqrDist;
										absorbOffset = (nearConnection - transform.position) * partnerLink.connectionOffsetFactor;
									}
								}
							}
							fluffSqrDist = nearSqrDist;
						}
						if (fluffSqrDist <= Mathf.Pow(partnerLink.absorbStrength, 2))
						{
							liveFluff.Pull(gameObject, absorbOffset, pullSpeed);
						}
					}
				}
			}
			return true;
		}
		else if(absorb != null)
		{
			absorb.startColor = Color.Lerp(absorb.startColor, new Color(0, 0, 0, 0), 0.5f);
			Destroy(absorb.gameObject, 1.0f);
		}
		return false;
	}

	void PlayerLookAt()
	{
		Vector2 lookAt = FireDirection();	


		if(controlScheme == Globals.ControlScheme.triggers || sharing)
		{
			lookAt = Vector2.zero;
		}

		float minToFire = deadZone;

		//bool stickFire = GetAxisStickThrow() != 0;//!swapJoysticks ? GetAxisStickThrow() > 0 : GetAxisStickThrow() < 0;

		//Debug.Log(GetAxisTriggers())
		if(!sharing && controlScheme == Globals.ControlScheme.triggers && (GetAxisTriggers() > deadZone|| GetAxisTriggers() < -deadZone))
		{
			lookAt = transform.forward;
			minToFire = 0;
		}

		//if((sharing && playerNumber == Player.Player1 && GetAxisTriggers() < -deadZone) || (sharing && playerNumber == Player.Player2 && GetAxisTriggers() < -deadZone))
		//{
		//	minToFire = 0;
		//}

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
					//Debug.Log(i);
					//i++;
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
				//Vector3 target = transform.position + new Vector3(lookAt.x, lookAt.y, 0);
				Vector3 throwDirection = new Vector3(lookAt.x, lookAt.y, 0);
				Vector3 velocityBoost = Vector3.zero;

				if (Vector3.Dot(mover.velocity, throwDirection) > 0)
				{
					velocityBoost += mover.velocity;
				}
                partnerLink.fluffThrow.Throw(throwDirection, velocityBoost);
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


	private Vector2 FireDirection()
	{
		Vector2 lookAt = Vector2.zero; 
		//if (!useKeyboard)
		//{
			lookAt = new Vector2(GetAxisAimHorizontal(), GetAxisAimVertical());
		//}
		///else
		//{
		//	if ((playerNumber == Player.Player1 && Input.GetMouseButtonUp(0)) || (playerNumber == Player.Player2 && Input.GetMouseButtonUp(1)))
		//	{
		//		Vector3 mousePos = Input.mousePosition;
		//		mousePos.z = transform.position.z;
		//		mousePos = CameraSplitter.Instance.GetFollowingCamera(gameObject).ScreenToWorldPoint(mousePos);
		//		lookAt = mousePos - transform.position;
	//			lookAt.Normalize();
	//		}
		//	else if ((playerNumber == Player.Player1 && Input.GetKeyUp(KeyCode.Space)) || (playerNumber == Player.Player2 && Input.GetKeyUp("[0]")))
		//	{
		////		lookAt = geometry.transform.forward;
		//	}
		//}

		return lookAt;
	}

	#endregion



}

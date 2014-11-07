using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
	public SimpleMover mover;
	public PartnerLink partnerLink;
	protected Collider tailTrigger;
	public enum Player{Player1, Player2};
	public Player playerNumber;
	public ParticleSystem absorbPrefab;

	public bool useKeyboard = false;

	public GameObject geometry;
	public float deadZone = .75f;

	private bool firePulse = true;
	private float startChargingPulse = 0f;
	private Vector3 velocityChange;
	public float basePulsePower = 10;
	public float timedPulsePower = 10;
	public float basePulseDrain = 0.1f;
	public float timedPulseDrain = 0.1f;
	private ParticleSystem absorb;
	private Vector3 target;
	public float absorbStrength = 20.0f;

	private bool paused = false;

	void Update () {


		var gamepads = Input.GetJoystickNames();
		useKeyboard = (gamepads.Length == 1 && playerNumber == Player.Player1) || gamepads.Length > 1 ? false : true;

		if(Input.GetButtonDown("Pause"))
		{
			if(paused)
				Time.timeScale = 1;
			else
				Time.timeScale = 0;

			paused = !paused;
		}

		if(!paused)
		{
			velocityChange = !useKeyboard ? PlayerJoystickMovement() : Vector3.zero;
			// Movement
			if(useKeyboard)
			{
				if ((playerNumber == Player.Player1 && Input.GetKey("w")) || (playerNumber == Player.Player2 && Input.GetKey(KeyCode.UpArrow)))
				{
					velocityChange += Vector3.up;
				}
				if ((playerNumber == Player.Player1 && Input.GetKey("a")) || (playerNumber == Player.Player2 && Input.GetKey(KeyCode.LeftArrow)))
				{
					velocityChange -= Vector3.right;
				}
				if ((playerNumber == Player.Player1 && Input.GetKey("s")) || (playerNumber == Player.Player2 && Input.GetKey(KeyCode.DownArrow)))
				{
					velocityChange -= Vector3.up;
				}
				if ((playerNumber == Player.Player1 && Input.GetKey("d")) || (playerNumber == Player.Player2 && Input.GetKey(KeyCode.RightArrow)))
				{
					velocityChange += Vector3.right;
				}
				transform.LookAt(transform.position + velocityChange, transform.up);
			}

			PlayerLookAt();

			if (velocityChange.sqrMagnitude > 0)
			{
				mover.Accelerate(velocityChange);
			}
			else
			{
				mover.SlowDown();
			}

			geometry.transform.LookAt(transform.position + mover.velocity, geometry.transform.up);
			if(absorb != null)
			{
				absorb.transform.position = transform.position;
			}
		}
	}

	private Vector3 PlayerJoystickMovement()
	{
		Vector2 leftStickInput = new Vector2(GetAxisMoveHorizontal(), GetAxisMoveVertical());
		return leftStickInput.sqrMagnitude > Mathf.Pow(deadZone, 2f) ? new Vector3(GetAxisMoveHorizontal(),GetAxisMoveVertical(),0) : Vector3.zero;
	}

	void PlayerLookAt()
	{
		Vector2 lookAt = FireDirection();

		var chargeTime = Time.time - startChargingPulse;

		if (IsChargingPulse() && !partnerLink.chargingPulse)
		{
			partnerLink.chargingPulse = true;
			startChargingPulse = Time.time;
		}

		if (partnerLink.chargingPulse)
		{
			if (CanFire(basePulseDrain + timedPulseDrain * Time.deltaTime))
			{
				//transform.localScale -= new Vector3(timedPulseDrain * Time.deltaTime, timedPulseDrain * Time.deltaTime, timedPulseDrain * Time.deltaTime);
				if(absorb == null)
				{
					absorb = (ParticleSystem)Instantiate(absorbPrefab);
					absorb.transform.position = transform.position;
					absorb.startColor = GetComponent<PartnerLink>().headRenderer.material.color / 2;
					absorb.startColor = new Color(absorb.startColor.r, absorb.startColor.g, absorb.startColor.b, 0.1f);
				}
				GameObject[] pulseArray = GameObject.FindGameObjectsWithTag("Pulse");
				foreach(GameObject livePulse in pulseArray)
					if(Vector3.SqrMagnitude(livePulse.transform.position - transform.position) < 100.0f && livePulse.GetComponent<MovePulse>() != null && livePulse.GetComponent<MovePulse>().creator != partnerLink.pulseShot)
						livePulse.GetComponent<MovePulse>().target = Vector3.MoveTowards(livePulse.GetComponent<MovePulse>().target, transform.position, 20.0f*Time.deltaTime);

			}
		}
		else if(absorb != null)
		{
			absorb.startColor = Color.Lerp(absorb.startColor, new Color(0, 0, 0, 0), 0.5f);
			Destroy(absorb.gameObject, 1.0f);
		}

		
		float minToFire = useKeyboard ? 0 : deadZone;
		if(lookAt.sqrMagnitude > Mathf.Pow(minToFire, 2f))
		{
			lookAt.Normalize();
			if(firePulse)
			{
				Vector3 target = transform.position + new Vector3(lookAt.x, lookAt.y, 0);
				transform.LookAt(target, transform.up);
				Vector3 pulseDirection = new Vector3(lookAt.x, lookAt.y, 0);
				Vector3 velocityBoost = Vector3.zero;
				if (Vector3.Dot(mover.velocity, pulseDirection) > 0)
				{
					velocityBoost += mover.velocity;
				}

				if (!partnerLink.chargingPulse && !useKeyboard && CanFire(basePulseDrain))
				{
					pulseDirection *= basePulsePower;
					//transform.localScale -= new Vector3(basePulseDrain, basePulseDrain, basePulseDrain);
					partnerLink.pulseShot.Shoot(transform.position + velocityBoost + pulseDirection, basePulseDrain);
				}
				else if (!IsChargingPulse() && startChargingPulse > 0 && CanFire(basePulseDrain))
				{
					pulseDirection *= basePulsePower +timedPulsePower * chargeTime;
					//transform.localScale -= new Vector3(basePulseDrain, basePulseDrain, basePulseDrain);
					partnerLink.pulseShot.Shoot(transform.position + velocityBoost + pulseDirection, basePulseDrain + timedPulseDrain * Time.deltaTime);
				}
				firePulse = false;
				partnerLink.chargingPulse = false;
				partnerLink.preChargeScale = transform.localScale.x;
				startChargingPulse = 0f;
			}
		}
		else
		{
			firePulse = true;
			if (partnerLink.chargingPulse && !IsChargingPulse())
			{
				partnerLink.chargingPulse = false;
				startChargingPulse = 0f;
			}
		}
	}

	bool CanFire(float costToFire)
	{
		return transform.localScale.x - costToFire >= partnerLink.minScale;
	}
	
	
	
	#region Helper Methods
	
	private float GetAxisMoveHorizontal(){return Input.GetAxis("MoveHorizontal" + playerNumber.ToString());}
	private float GetAxisMoveVertical(){return Input.GetAxis("MoveVertical" + playerNumber.ToString());}
	private float GetAxisAimHorizontal(){return Input.GetAxis("AimHorizontal" + playerNumber.ToString());}
	private float GetAxisAimVertical(){return Input.GetAxis("AimVertical" + playerNumber.ToString());}

	private bool IsChargingPulse()
	{
		if (!useKeyboard)
		{
			return Input.GetButton("FirePulse" + playerNumber.ToString());
		}
		else
		{
			bool keyboardCharge = (playerNumber == Player.Player1 && Input.GetKey(KeyCode.Space)) || (playerNumber == Player.Player2 && Input.GetKey("[0]"));
			bool mouseCharge = (playerNumber == Player.Player1 && Input.GetMouseButton(0)) || (playerNumber == Player.Player2 && Input.GetMouseButton(1));
			return keyboardCharge || mouseCharge;
		}
	}

	private Vector2 FireDirection()
	{
		Vector2 lookAt = Vector2.zero; 
		if (!useKeyboard)
		{
			lookAt = new Vector2(GetAxisAimHorizontal(), GetAxisAimVertical());
		}
		else
		{
			if ((playerNumber == Player.Player1 && Input.GetMouseButtonUp(0)) || (playerNumber == Player.Player2 && Input.GetMouseButtonUp(1)))
			{
				Vector3 mousePos = Input.mousePosition;
				mousePos.z = transform.position.z;
				mousePos = CameraSplitter.Instance.GetFollowingCamera(gameObject).ScreenToWorldPoint(mousePos);
				lookAt = mousePos - transform.position;
				lookAt.Normalize();
			}
			else if ((playerNumber == Player.Player1 && Input.GetKeyUp(KeyCode.Space)) || (playerNumber == Player.Player2 && Input.GetKeyUp("[0]")))
			{
				lookAt = geometry.transform.forward;
			}
		}

		return lookAt;
	}

	#endregion



}

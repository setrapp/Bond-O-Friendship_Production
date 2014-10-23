using UnityEngine;
using System.Collections;

public class KeyboardSeek : SimpleSeek {

	public GameObject pulsePrefab;
	private GameObject pulse;
	public enum Player{Player1, Player2};
	public Player playerNumber;
	public ParticleSystem pulseParticlePrefab;

	public bool useKeyboard = false;

	public GameObject geometry;	
	public float deadZone = .75f;

	private bool firePulse = true;
	private float startChargingPulse = 0f;
	private Vector3 velocityChange;
	public float basePulseSize = 0.5f;
	public float basePulsePower = 10;
	public float timedPulsePower = 10;
	public float basePulseDrain = 0.1f;
	public float timedPulseDrain = 0.1f;
	private ParticleSystem pulseParticle;
	private ParticleSystem absorb;
	private Vector3 target;
	public float absorbStrength = 20.0f;

	private bool paused = false;
	private Vector3 particleRotation;
	private Color pOneCol;
	private Color pTwoCol;

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

			PlayerLookAt(useKeyboard);

			//Draw the line
			if (velocityChange.sqrMagnitude > 0)
			{
				mover.Accelerate(velocityChange);
				if (tracer.lineRenderer == null)
					tracer.StartLine();
				else
					tracer.AddVertex(transform.position);
			}
			else
			{
				mover.SlowDown();
				tracer.DestroyLine();
			}

			geometry.transform.LookAt(transform.position + mover.velocity, geometry.transform.up);
			if(pulseParticle != null && pulse != null)
			{
				pulseParticle.transform.position = pulse.transform.position;
			}
			if(absorb != null)
			{
				absorb.transform.position = transform.position;
			}
		}

		pOneCol = new Color(0,Random.Range(0.0f,0.5f),Random.Range(0.5f,1.0f));
		pTwoCol = new Color(0,Random.Range(0.5f,1.0f),Random.Range(0.0f,0.5f));

	}

	private Vector3 PlayerJoystickMovement()
	{
		Vector2 leftStickInput = new Vector2(GetAxisMoveHorizontal(), GetAxisMoveVertical());
		return leftStickInput.sqrMagnitude > Mathf.Pow(deadZone, 2f) ? new Vector3(GetAxisMoveHorizontal(),GetAxisMoveVertical(),0) : Vector3.zero;
	}

	void PlayerLookAt(bool assumeForward)
	{
		Vector2 lookAt = geometry.transform.forward;
		if (!assumeForward)
		{
			lookAt = new Vector2(GetAxisAimHorizontal(), GetAxisAimVertical());
		}
		else if (velocityChange.sqrMagnitude == 0 || !partnerLink.chargingPulse || GetButtonFirePulse())
		{
			lookAt = new Vector2();
		}

		var chargeTime = Time.time - startChargingPulse;

		if (GetButtonFirePulse() && !partnerLink.chargingPulse)
		{
			partnerLink.chargingPulse = true;
			startChargingPulse = Time.time;
		}

		if (partnerLink.chargingPulse)
		{
			if (CanFire(basePulseDrain + timedPulseDrain * Time.deltaTime))
			{
				transform.localScale -= new Vector3(timedPulseDrain * Time.deltaTime, timedPulseDrain * Time.deltaTime, timedPulseDrain * Time.deltaTime);
				if(pulse != null)
				{
					GameObject[] pulseArray = GameObject.FindGameObjectsWithTag("Pulse");
					foreach(GameObject livePulse in pulseArray)
						if(Vector3.Distance(livePulse.transform.position, transform.position) < 10.0f)
							livePulse.GetComponent<MovePulse>().target = Vector3.MoveTowards(livePulse.GetComponent<MovePulse>().target, transform.position, 20.0f*Time.deltaTime);
				}

			}
		}

		if(lookAt.sqrMagnitude > Mathf.Pow(deadZone, 2f))
		{
			if(firePulse)
			{
				Vector3 target = transform.position + new Vector3(lookAt.x, lookAt.y, 0);
				transform.LookAt(target, transform.up);
				Vector3 joystickPos = new Vector3(lookAt.x, lookAt.y, 0);

				if (!partnerLink.chargingPulse && !useKeyboard && CanFire(basePulseDrain))
				{
					joystickPos *= basePulsePower;
					FirePulse(transform.position + mover.velocity + joystickPos, basePulseDrain);
					transform.localScale -= new Vector3(basePulseDrain, basePulseDrain, basePulseDrain);
					partnerLink.preChargeScale = transform.localScale.x;
					firePulse = false;
				}
				else if(!GetButtonFirePulse() && startChargingPulse > 0)
				{
					joystickPos *= basePulsePower +timedPulsePower * chargeTime;
					transform.localScale -= new Vector3(basePulseDrain, basePulseDrain, basePulseDrain);
					FirePulse(transform.position + mover.velocity + joystickPos, basePulseDrain + timedPulseDrain * Time.deltaTime);
					firePulse = false;
					partnerLink.chargingPulse = false;
					partnerLink.preChargeScale = transform.localScale.x;
					startChargingPulse = 0f;
				}
			}
		}
		else
		{
			firePulse = true;
			if (!GetButtonFirePulse() && partnerLink.chargingPulse)
			{
				partnerLink.chargingPulse = false;
				startChargingPulse = 0f;
			}
		}
	}

	void FirePulse(Vector3 pulseTarget, float pulseCapacity)
	{
		pulse = Instantiate(pulsePrefab, transform.position, Quaternion.identity) as GameObject;
		MovePulse movePulse = pulse.GetComponent<MovePulse>();
		movePulse.target = pulseTarget;
		movePulse.creator = gameObject;
		movePulse.capacity = pulseCapacity;
		pulse.transform.localScale = new Vector3(basePulseSize + pulseCapacity, basePulseSize + pulseCapacity, basePulseSize + pulseCapacity);
		//pulse.renderer.material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
		pulseParticle = (ParticleSystem)Instantiate(pulseParticlePrefab);

		if(gameObject.name == "Player 1")
		{
			pulse.renderer.material.color = pOneCol;
		}
		if(gameObject.name == "Player 2")
		{
			pulse.renderer.material.color = pTwoCol;
		}

		//particleRotation = pulse.GetComponent<MovePulse>().moveVector;
		pulseParticle.transform.forward = particleRotation;
		pulseParticle.startColor = pulse.renderer.material.color;
		pulseParticle.startSpeed = particleRotation.magnitude;
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

	private bool GetButtonFirePulse()
	{
		if (!useKeyboard)
		{
			return Input.GetButton("FirePulse" + playerNumber.ToString());
		}
		else
		{
			return (playerNumber == Player.Player1 && Input.GetKey(KeyCode.Space)) || (playerNumber == Player.Player2 && Input.GetKey("[0]"));
		}
	}

	#endregion



}

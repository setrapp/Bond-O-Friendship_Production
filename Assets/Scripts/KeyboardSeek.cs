using UnityEngine;
using System.Collections;

public class KeyboardSeek : SimpleSeek {

	public GameObject pulse;
	public enum Player{Player1, Player2};
	public Player playerNumber;

	public bool useKeyboard = false;

	public GameObject geometry;	
	public float deadZone = .75f;

	private bool firePulse = true;
	private bool chargingPulse = false;
	private float startChargingPulse = 0f;

	void Update () {

		Vector3 acceleration = !useKeyboard ? PlayerJoystickMovement() : Vector3.zero;
		// Movement
		if(!useKeyboard)
		{
			PlayerJoystickLookAt();
		}
		else
		{
			if ((playerNumber == Player.Player1 && Input.GetKey("w")) || (playerNumber == Player.Player2 && Input.GetKey(KeyCode.UpArrow)))
			{
				acceleration += Vector3.up;
			}
			if ((playerNumber == Player.Player1 && Input.GetKey("a")) || (playerNumber == Player.Player2 && Input.GetKey(KeyCode.LeftArrow)))
			{
				acceleration -= Vector3.right;
			}
			if ((playerNumber == Player.Player1 && Input.GetKey("s")) || (playerNumber == Player.Player2 && Input.GetKey(KeyCode.DownArrow)))
			{
				acceleration -= Vector3.up;
			}
			if ((playerNumber == Player.Player1 && Input.GetKey("d")) || (playerNumber == Player.Player2 && Input.GetKey(KeyCode.RightArrow)))
			{
				acceleration += Vector3.right;
			}
			transform.LookAt(transform.position + acceleration, transform.up);
			// Sharing.
			if((playerNumber == Player.Player1 && Input.GetKeyDown(KeyCode.LeftControl)) || (playerNumber == Player.Player2 && Input.GetKeyDown(KeyCode.RightControl)))
			{
				partnerLink.connection.SendPulse(partnerLink, partnerLink.partner);
			}
			if ((playerNumber == Player.Player1 && Input.GetKey(KeyCode.Space)) || (playerNumber == Player.Player2 && Input.GetKey("[0]")))
			{
				partnerLink.preparingPulse = true;
			}
			if ((playerNumber == Player.Player1 && Input.GetKeyUp(KeyCode.Space)) || (playerNumber == Player.Player2 && Input.GetKeyUp("[0]")))
			{
				partnerLink.preparingPulse = false;
			}
		}

		//Draw the line
		if (acceleration.sqrMagnitude > 0)
		{
			mover.Accelerate(acceleration);
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
	}

	private Vector3 PlayerJoystickMovement()
	{
		Vector2 leftStickInput = new Vector2(GetAxisMoveHorizontal(), GetAxisMoveVertical());
		return leftStickInput.sqrMagnitude > Mathf.Pow(deadZone, 2f) ? new Vector3(GetAxisMoveHorizontal(),GetAxisMoveVertical(),0) : Vector3.zero;
	}

	void PlayerJoystickLookAt()
	{
		Vector2 rightStickInput = new Vector2(GetAxisAimHorizontal(), GetAxisAimVertical());
		if(GetButtonFirePulse() && !chargingPulse)
		{
			chargingPulse = true;
			startChargingPulse = Time.time;
		}

		if(rightStickInput.sqrMagnitude > Mathf.Pow(deadZone, 2f))
		{
			if(firePulse)
			{
				Vector3 target = transform.position + new Vector3(GetAxisAimHorizontal(), GetAxisAimVertical(), 0);
				transform.LookAt(target, transform.up);
				Vector3 joystickPos = new Vector3(GetAxisAimHorizontal(), GetAxisAimVertical(), 0);	

				if(!chargingPulse)
				{	
					joystickPos *= (5);
					FirePulse(transform.position + joystickPos);
					firePulse = false;
				}
				else if(!GetButtonFirePulse())
				{
					var chargeTime = Time.time - startChargingPulse;
					joystickPos *= (5 * (2+chargeTime));
					FirePulse(transform.position + joystickPos);
					firePulse = false;
					chargingPulse = false;
					startChargingPulse = 0f;
				}
			}
		}
		else
		{
			firePulse = true;
			if(!GetButtonFirePulse() && chargingPulse)
			{
				chargingPulse = false;
				startChargingPulse = 0f;
			}
		}
	}

	void FirePulse(Vector3 pulseTarget)
	{
		GameObject go = Instantiate(pulse,transform.position, Quaternion.identity) as GameObject;
		go.GetComponent<MovePulse>().target = pulseTarget;
	}


	
	
	
	#region Helper Methods
	
	private float GetAxisMoveHorizontal(){return Input.GetAxis("MoveHorizontal" + playerNumber.ToString());}
	private float GetAxisMoveVertical(){return Input.GetAxis("MoveVertical" + playerNumber.ToString());}
	private float GetAxisAimHorizontal(){return Input.GetAxis("AimHorizontal" + playerNumber.ToString());}
	private float GetAxisAimVertical(){return Input.GetAxis("AimVertical" + playerNumber.ToString());}
	private float GetAxisGive(){return Input.GetAxis("Give" + playerNumber.ToString());}
	private float GetAxisTake(){return Input.GetAxis("Take" + playerNumber.ToString());}

	private bool GetButtonFirePulse(){return Input.GetButton("FirePulse" +playerNumber.ToString());}

	#endregion



}

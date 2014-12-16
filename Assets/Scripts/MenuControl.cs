using UnityEngine;
using System.Collections;

public class MenuControl : MonoBehaviour {


	public GameObject startArrow;
	public GameObject optionsArrow;
	public GameObject quitArrow;
	public GameObject mainMenu;
	public GameObject controllerSelectMenu;
	public GameObject pOneStick;
	public GameObject pOneTriggers;
	public GameObject pTwoStick;
	public GameObject pTwoTriggers;
	public GameObject pSharedController;
	public GameObject pressButtonToStartText;

	private bool movingThumbstick = false;
	//private bool sharing = false;


	private bool pOneMovingThumbstick = false;
	private bool pTwoMovingThumbstick = false;


	
	private bool playerTwoIn = false;

	public GameObject playerOneReadyText;
	public GameObject playerTwoReadyText;

	private bool playerOneReady = false;
	private bool playerTwoReady = false;
	private int gamepadCount = 0;
	// Use this for initialization
	void Start () {
		startArrow.SetActive(true);
		optionsArrow.SetActive(false);
		quitArrow.SetActive(false);
	
	}


	// Update is called once per frame
	void Update () {
	
		checkReady();
	
		if(startArrow.activeSelf)
		{
			if(Input.GetButtonDown("Joy1Absorb"))
			{
				Globals.playerOneJoystickNumber = Globals.JoyStick.Joy1;
				mainMenu.SetActive(false);
				controllerSelectMenu.SetActive(true);
				startArrow.SetActive(false);
			}
			if(Input.GetButtonDown("Joy2Absorb"))
			{
				Globals.playerOneJoystickNumber = Globals.JoyStick.Joy2;
				mainMenu.SetActive(false);
				controllerSelectMenu.SetActive(true);
				startArrow.SetActive(false);
			}
			if(Input.GetButtonDown("Joy3Absorb"))
			{
				Globals.playerOneJoystickNumber = Globals.JoyStick.Joy3;
				mainMenu.SetActive(false);
				controllerSelectMenu.SetActive(true);
				startArrow.SetActive(false);
			}
			if(Input.GetButtonDown("Joy4Absorb"))
			{
				Globals.playerOneJoystickNumber = Globals.JoyStick.Joy4;
				mainMenu.SetActive(false);
				controllerSelectMenu.SetActive(true);
				startArrow.SetActive(false);
			}

			if(controllerSelectMenu.activeSelf)
			{
				var gamepads = Input.GetJoystickNames();
				gamepadCount = gamepads.Length;
				if(gamepadCount == 1)
				{
					pOneStick.SetActive(false);
					pOneTriggers.SetActive(false);
					pTwoStick.SetActive(false);
					pTwoTriggers.SetActive(false);
					pSharedController.SetActive(true);
					Globals.sharing = true;
				}
				else
				{
					pOneStick.SetActive(true);
					pressButtonToStartText.SetActive(true);
				}
			}

		}

		if(pressButtonToStartText.activeSelf)
		{

			//Debug.Log(playerOneJoystickNumber.ToString());
			if(Input.GetButtonDown("Joy1Absorb") && Globals.playerOneJoystickNumber != Globals.JoyStick.Joy1)
			{
				Globals.playerTwoJoystickNumber = Globals.JoyStick.Joy1;
				pressButtonToStartText.SetActive(false);
				pTwoStick.SetActive(true);
				playerTwoIn = true;
				Globals.playerTwoControlScheme = Globals.ControlScheme.sticks;
			}
			if(Input.GetButtonDown("Joy2Absorb")&& Globals.playerOneJoystickNumber != Globals.JoyStick.Joy2)
			{
				Globals.playerTwoJoystickNumber = Globals.JoyStick.Joy2;
				pressButtonToStartText.SetActive(false);
				pTwoStick.SetActive(true);
				playerTwoIn = true;
				Globals.playerTwoControlScheme = Globals.ControlScheme.sticks;
			}
			if(Input.GetButtonDown("Joy3Absorb")&& Globals.playerOneJoystickNumber != Globals.JoyStick.Joy3)
			{
				Globals.playerTwoJoystickNumber = Globals.JoyStick.Joy3;
				pressButtonToStartText.SetActive(false);
				pTwoStick.SetActive(true);
				playerTwoIn = true;
				Globals.playerTwoControlScheme = Globals.ControlScheme.sticks;
			}
			if(Input.GetButtonDown("Joy4Absorb")&& Globals.playerOneJoystickNumber != Globals.JoyStick.Joy4)
			{
				Globals.playerTwoJoystickNumber = Globals.JoyStick.Joy4;
				pressButtonToStartText.SetActive(false);
				pTwoStick.SetActive(true);
				playerTwoIn = true;
				Globals.playerTwoControlScheme = Globals.ControlScheme.sticks;
			}


			//Debug.Log(playerTwoControlScheme.ToString());

		}


		if(Input.GetButtonDown("ToggleShare") && !playerOneReady && !playerTwoReady)
		{

			if(Globals.sharing && gamepadCount > 1)
			{
				if(Globals.playerOneControlScheme == Globals.ControlScheme.sticks)
					pOneStick.SetActive(true);
				else
					pOneTriggers.SetActive(true);

				if(playerTwoIn)
				{
					if(Globals.playerTwoControlScheme == Globals.ControlScheme.sticks)
						pTwoStick.SetActive(true);
					else 
						pTwoTriggers.SetActive(true);
				}
				else
				{
					pressButtonToStartText.SetActive(true);
				}

				pSharedController.SetActive(false);	
				Globals.sharing = false;
			}
			else{
				pOneStick.SetActive(false);
				pOneTriggers.SetActive(false);
				pTwoStick.SetActive(false);
				pTwoTriggers.SetActive(false);
				pressButtonToStartText.SetActive(false);
				pSharedController.SetActive(true);
				Globals.sharing = true;
			}


		}

		if(mainMenu.activeSelf)
			checkMenuInput();
		if(controllerSelectMenu.activeSelf && !Globals.sharing)
		{
			if(!playerOneReady)
				checkPlayerOneInput();
			if(!playerTwoReady)
				checkPlayerTwoInput();
		}

		gamepadCount = Input.GetJoystickNames().Length;

		if(Globals.sharing && (Input.GetButtonDown(Globals.playerOneJoystickNumber.ToString() + "Pause") || Input.GetButtonDown(Globals.playerTwoJoystickNumber.ToString() + "Pause")))
		{
			Application.LoadLevel("Cradle 3");
		}
		if(!Globals.sharing && playerTwoReady && playerOneReady)
		{
			Application.LoadLevel("Cradle 3");
		}
			
	}

	void checkPlayerOneInput()
	{
		if(Input.GetAxis(Globals.playerOneJoystickNumber.ToString() + "LeftStickHorizontal") != 1 && Input.GetAxis(Globals.playerOneJoystickNumber.ToString() + "LeftStickHorizontal") != -1) 
			pOneMovingThumbstick = false;

		if((Input.GetAxis(Globals.playerOneJoystickNumber.ToString() + "LeftStickHorizontal") == 1 || Input.GetAxis(Globals.playerOneJoystickNumber.ToString() + "LeftStickHorizontal") == -1) && !pOneMovingThumbstick)
		{
			pOneMovingThumbstick = true;
			if(pOneStick.activeSelf)
			{
				pOneStick.SetActive(false);
				pOneTriggers.SetActive(true);
				Globals.playerOneControlScheme = Globals.ControlScheme.triggers;
			}
			else if(pOneTriggers.activeSelf)
			{
				pOneStick.SetActive(true);
				pOneTriggers.SetActive(false);
				Globals.playerOneControlScheme = Globals.ControlScheme.sticks;
			}
		}
	}

	void checkPlayerTwoInput()
	{
		if(Input.GetAxis(Globals.playerTwoJoystickNumber.ToString() + "LeftStickHorizontal") != 1 && Input.GetAxis(Globals.playerTwoJoystickNumber.ToString() + "LeftStickHorizontal") != -1) 
			pTwoMovingThumbstick = false;
		
		if((Input.GetAxis(Globals.playerTwoJoystickNumber.ToString() + "LeftStickHorizontal") == 1 || Input.GetAxis(Globals.playerTwoJoystickNumber.ToString() + "LeftStickHorizontal") == -1) && !pTwoMovingThumbstick)
		{
			pTwoMovingThumbstick = true;
			if(pTwoStick.activeSelf)
			{
				pTwoStick.SetActive(false);
				pTwoTriggers.SetActive(true);
				Globals.playerTwoControlScheme = Globals.ControlScheme.triggers;
			}
			else if(pTwoTriggers.activeSelf)
			{
				pTwoStick.SetActive(true);
				pTwoTriggers.SetActive(false);
				Globals.playerTwoControlScheme = Globals.ControlScheme.sticks;
			}
		}
	}


	void checkReady()
	{

		if(Input.GetButtonDown(Globals.playerOneJoystickNumber.ToString() + "Pause") && !Globals.sharing)
		{
			playerOneReady = !playerOneReady;
			playerOneReadyText.SetActive(playerOneReady);
		}


		if(Input.GetButtonDown(Globals.playerTwoJoystickNumber.ToString() + "Pause") && playerTwoIn &&!Globals.sharing)
		{
			playerTwoReady = !playerTwoReady;
			playerTwoReadyText.SetActive(playerTwoReady);
		}
		
	}
	void checkMenuInput()
	{
		if(Input.GetAxis("MenuVertical") != 1 && Input.GetAxis("MenuVertical") != -1)
			movingThumbstick = false;

		if(Input.GetAxis("MenuVertical") == 1 && !movingThumbstick)
		{
			movingThumbstick = true;
			if(startArrow.activeSelf)
			{
				startArrow.SetActive(false);
				quitArrow.SetActive(true);
			}
			else if(optionsArrow.activeSelf)
			{
				optionsArrow.SetActive(false);
				startArrow.SetActive(true);
			}
			else if(quitArrow.activeSelf)
			{
				quitArrow.SetActive(false);
				optionsArrow.SetActive(true);
			}
		}

		if(Input.GetAxis("MenuVertical") == -1 && !movingThumbstick)
		{
			movingThumbstick = true;
			if(startArrow.activeSelf)
			{
				startArrow.SetActive(false);
				optionsArrow.SetActive(true);
			}
			else if(optionsArrow.activeSelf)
			{
				optionsArrow.SetActive(false);
				quitArrow.SetActive(true);
			}
			else if(quitArrow.activeSelf)
			{
				quitArrow.SetActive(false);
				startArrow.SetActive(true);
			}
		}

	}
}

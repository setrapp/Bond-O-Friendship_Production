using UnityEngine;
using System.Collections;

public class MainMenuInputOutlines : MonoBehaviour {

	//public GameObject controlsPlayer1;
	public GameObject gamepadPlayer1;
	public GameObject keyboardPlayer1;

	//public GameObject controlsPlayer2;
	public GameObject gamepadPlayer2;
	public GameObject keyboardPlayer2;

	//public GameObject controlsShared;
	public GameObject gamepadSharedPlayer1Left;
	public GameObject gamepadSharedPlayer2Left;
	public GameObject keyboardSharedPlayer1Left;
	public GameObject keyboardSharedPlayer2Left;

	private bool player1Solo = false;
	private bool player1Keyboard = false;
	private bool player1SharedLeft = false;

	private bool player2Solo = false;
	private bool player2Keyboard = false;
	private bool player2SharedLeft = false;

	public bool forceCheck = false;

	// Use this for initialization
	void Start () 
	{
		player1Solo = Globals.Instance.player1Controls.controlScheme == Globals.ControlScheme.Solo;
		player1Keyboard = Globals.Instance.player1Controls.inputNameSelected == Globals.InputNameSelected.Keyboard;
		player1SharedLeft = Globals.Instance.player1Controls.controlScheme == Globals.ControlScheme.SharedLeft;

		player2Solo = Globals.Instance.player2Controls.controlScheme == Globals.ControlScheme.Solo;
		player2Keyboard = Globals.Instance.player2Controls.inputNameSelected == Globals.InputNameSelected.Keyboard;
		player2SharedLeft = Globals.Instance.player2Controls.controlScheme == Globals.ControlScheme.SharedLeft;

        //PositionOutlines();

		ToggleInputOutlines ();

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (CheckInputChange() || forceCheck)
			ToggleInputOutlines();	
	}

    

	private bool CheckInputChange()
	{
		bool inputChanged = false;

		if (player1Solo != (Globals.Instance.player1Controls.controlScheme == Globals.ControlScheme.Solo) || player2Solo != (Globals.Instance.player2Controls.controlScheme == Globals.ControlScheme.Solo))
			inputChanged = true;
		if (player1Keyboard != (Globals.Instance.player1Controls.inputNameSelected == Globals.InputNameSelected.Keyboard) || player2Keyboard != (Globals.Instance.player2Controls.inputNameSelected == Globals.InputNameSelected.Keyboard))
			inputChanged = true;
		if (player1SharedLeft != (Globals.Instance.player1Controls.controlScheme == Globals.ControlScheme.SharedLeft) || player2SharedLeft != (Globals.Instance.player2Controls.controlScheme == Globals.ControlScheme.SharedLeft))
			inputChanged = true;

		return inputChanged;
	}

	private void UpdateBools()
	{
		player1Solo = Globals.Instance.player1Controls.controlScheme == Globals.ControlScheme.Solo;
		player1Keyboard = Globals.Instance.player1Controls.inputNameSelected == Globals.InputNameSelected.Keyboard;
		player1SharedLeft = Globals.Instance.player1Controls.controlScheme == Globals.ControlScheme.SharedLeft;
		
		player2Solo = Globals.Instance.player2Controls.controlScheme == Globals.ControlScheme.Solo;
		player2Keyboard = Globals.Instance.player2Controls.inputNameSelected == Globals.InputNameSelected.Keyboard;
		player2SharedLeft = Globals.Instance.player2Controls.controlScheme == Globals.ControlScheme.SharedLeft;
	}

	private void ToggleInputOutlines()
	{
		if (player1Solo && player2Solo) 
		{
			//controlsPlayer1.SetActive (true);
			//controlsPlayer2.SetActive (true);
			//controlsShared.SetActive (false);
            gamepadSharedPlayer1Left.SetActive(false);
            gamepadSharedPlayer2Left.SetActive(false);

            keyboardSharedPlayer1Left.SetActive(false);
            keyboardSharedPlayer2Left.SetActive(false);
		
			gamepadPlayer1.SetActive (!player1Keyboard);
			keyboardPlayer1.SetActive (player1Keyboard);

			gamepadPlayer2.SetActive (!player2Keyboard);
			keyboardPlayer2.SetActive (player2Keyboard);
		
		}
		else if(!player1Solo && !player2Solo)
		{

			//controlsPlayer1.SetActive (false);
			//controlsPlayer2.SetActive (false);
			//controlsShared.SetActive (true);
            gamepadPlayer1.SetActive(false);
            keyboardPlayer1.SetActive(false);

            gamepadPlayer2.SetActive(false);
            keyboardPlayer2.SetActive(false);

			if(player1Keyboard && player2Keyboard)
			{
				gamepadSharedPlayer1Left.SetActive(false);
				gamepadSharedPlayer2Left.SetActive(false);

				keyboardSharedPlayer1Left.SetActive(player1SharedLeft);
				keyboardSharedPlayer2Left.SetActive(player2SharedLeft);

			}

			if(!player1Keyboard && !player2Keyboard)
			{
				gamepadSharedPlayer1Left.SetActive(player1SharedLeft);
				gamepadSharedPlayer2Left.SetActive(player2SharedLeft);
				
				keyboardSharedPlayer1Left.SetActive(false);
				keyboardSharedPlayer2Left.SetActive(false);
			}
		}

		UpdateBools();
		forceCheck = true;
	}
}

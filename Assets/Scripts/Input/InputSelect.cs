using UnityEngine;
using System.Collections;
using InControl;

public class InputSelect : MonoBehaviour {

    public enum InputName { LeftStickLeftController, RightStickLeftController, LeftStickRightController, RightStickRightController, LeftSideKeyboard, RightSideKeyboard }

    public InputName inputName;

    public Globals.InputNameSelected inputNameSelected;

    public float player1Timer = 4f;
    public float player2Timer = 4f;

    public Globals.ControlScheme player1ControlScheme;
    public Globals.ControlScheme player2ControlScheme;

    public int deviceIndex = -2;

	// Use this for initialization
	void Start () 
    {
        if (inputName == InputName.LeftStickLeftController || inputName == InputName.LeftStickRightController)
            inputNameSelected = Globals.InputNameSelected.LeftController;

        if (inputName == InputName.LeftStickRightController || inputName == InputName.RightStickRightController)
            inputNameSelected = Globals.InputNameSelected.RightController;

        if (inputName == InputName.LeftSideKeyboard || inputName == InputName.RightSideKeyboard)
            inputNameSelected = Globals.InputNameSelected.Keyboard;

        if (inputNameSelected == Globals.Instance.player1InputNameSelected)
            deviceIndex = Globals.Instance.player1Device;

        if (inputNameSelected == Globals.Instance.player2InputNameSelected)
            deviceIndex = Globals.Instance.player2Device;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (inputNameSelected == Globals.Instance.player1InputNameSelected)
            deviceIndex = Globals.Instance.player1Device;
        else if (inputNameSelected == Globals.Instance.player2InputNameSelected)
            deviceIndex = Globals.Instance.player2Device;
        else
            deviceIndex = -2;

        if (player1Timer < 1f)
        {
            //ChangePlayerInput(1);
            ChangePlayerOneInput();
            CancelInvoke("PlayerOneTimer");
            player1Timer = 4f;
        }

        if (player2Timer < 1f)
        {
           // ChangePlayerInput(2);
            ChangePlayerTwoInput();
            CancelInvoke("PlayerTwoTimer");
            player2Timer = 4f;
        }
	
	}

    void OnTriggerEnter(Collider collide)
    {
        Debug.Log("Enter");
        if (collide.gameObject.name == "Player 1")
        {
            InvokeRepeating("PlayerOneTimer", 1f, 1f);
        }
        if (collide.gameObject.name == "Player 2")
        {
            InvokeRepeating("PlayerTwoTimer", 1f, 1f);
        }
    }

    void OnTriggerExit(Collider collide)
    {
        Debug.Log("Exit");
        if (collide.gameObject.name == "Player 1")
        {
            CancelInvoke("PlayerOneTimer");
            player1Timer = 4f;
        }
        if (collide.gameObject.name == "Player 2")
        {
            CancelInvoke("PlayerTwoTimer");
            player2Timer = 4f;
        }
    }

    void ChangePlayerOneInput()
    {
        switch(inputName)
        {
            case InputName.LeftSideKeyboard:
                if (Globals.Instance.player1ControlScheme == Globals.ControlScheme.KeyboardSharedLeft || Globals.Instance.player1ControlScheme == Globals.ControlScheme.KeyboardSolo)
                    break;
                if (Globals.Instance.player2ControlScheme == Globals.ControlScheme.KeyboardSolo || Globals.Instance.player2ControlScheme == Globals.ControlScheme.KeyboardSharedLeft)
                {
                    Globals.Instance.player1ControlScheme = Globals.ControlScheme.KeyboardSharedLeft;
                    Globals.Instance.player2ControlScheme = Globals.ControlScheme.KeyboardSharedRight;
                }
                else
                    Globals.Instance.player1ControlScheme = Globals.ControlScheme.KeyboardSolo;

                Globals.Instance.player1PreviousDevice = Globals.Instance.player1Device;
                Globals.Instance.player1Device = -1;
                Globals.Instance.player1InputNameSelected = Globals.InputNameSelected.Keyboard;
                break;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case InputName.RightSideKeyboard:
                if (Globals.Instance.player1ControlScheme == Globals.ControlScheme.KeyboardSharedRight || Globals.Instance.player1ControlScheme == Globals.ControlScheme.KeyboardSolo)
                    break;
                if (Globals.Instance.player2ControlScheme == Globals.ControlScheme.KeyboardSolo || Globals.Instance.player2ControlScheme == Globals.ControlScheme.KeyboardSharedRight)
                {
                    Globals.Instance.player1ControlScheme = Globals.ControlScheme.KeyboardSharedRight;
                    Globals.Instance.player2ControlScheme = Globals.ControlScheme.KeyboardSharedLeft;
                }
                else
                    Globals.Instance.player1ControlScheme = Globals.ControlScheme.KeyboardSolo;

                Globals.Instance.player1PreviousDevice = Globals.Instance.player1Device;
                Globals.Instance.player1Device = -1;
                Globals.Instance.player1InputNameSelected = Globals.InputNameSelected.Keyboard;
                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case InputName.LeftStickLeftController:
                if (Globals.Instance.player1InputNameSelected == Globals.InputNameSelected.LeftController && (Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSharedLeft || Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSolo))
                    break;
                if (Globals.Instance.player2InputNameSelected == Globals.InputNameSelected.LeftController && (Globals.Instance.player2ControlScheme == Globals.ControlScheme.ControllerSharedLeft || Globals.Instance.player2ControlScheme == Globals.ControlScheme.ControllerSolo))
                {
                    Globals.Instance.player1ControlScheme = Globals.ControlScheme.ControllerSharedLeft;
                    Globals.Instance.player2ControlScheme = Globals.ControlScheme.ControllerSharedRight;
                }
                else
                    Globals.Instance.player1ControlScheme = Globals.ControlScheme.ControllerSolo;

                Globals.Instance.player1PreviousDevice = Globals.Instance.player1Device;
                Globals.Instance.player1Device = deviceIndex;
                Globals.Instance.player1InputNameSelected = Globals.InputNameSelected.LeftController;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                break;
            case InputName.RightStickLeftController:
                if (Globals.Instance.player1InputNameSelected == Globals.InputNameSelected.LeftController && (Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSharedRight || Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSolo))
                    break;
                if (Globals.Instance.player2InputNameSelected == Globals.InputNameSelected.LeftController && (Globals.Instance.player2ControlScheme == Globals.ControlScheme.ControllerSharedRight || Globals.Instance.player2ControlScheme == Globals.ControlScheme.ControllerSolo))
                {
                    Globals.Instance.player1ControlScheme = Globals.ControlScheme.ControllerSharedRight;
                    Globals.Instance.player2ControlScheme = Globals.ControlScheme.ControllerSharedLeft;
                }
                else
                    Globals.Instance.player1ControlScheme = Globals.ControlScheme.ControllerSolo;

                Globals.Instance.player1PreviousDevice = Globals.Instance.player1Device;
                Globals.Instance.player1Device = deviceIndex;
                Globals.Instance.player1InputNameSelected = Globals.InputNameSelected.LeftController;
                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case InputName.LeftStickRightController:
                if (Globals.Instance.player1InputNameSelected == Globals.InputNameSelected.RightController && (Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSharedLeft || Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSolo))
                    break;
                if (Globals.Instance.player2InputNameSelected == Globals.InputNameSelected.RightController && (Globals.Instance.player2ControlScheme == Globals.ControlScheme.ControllerSharedLeft || Globals.Instance.player2ControlScheme == Globals.ControlScheme.ControllerSolo))
                {
                    Globals.Instance.player1ControlScheme = Globals.ControlScheme.ControllerSharedLeft;
                    Globals.Instance.player2ControlScheme = Globals.ControlScheme.ControllerSharedRight;
                }
                else
                    Globals.Instance.player1ControlScheme = Globals.ControlScheme.ControllerSolo;

                Globals.Instance.player1PreviousDevice = Globals.Instance.player1Device;
                Globals.Instance.player1Device = deviceIndex;
                Globals.Instance.player1InputNameSelected = Globals.InputNameSelected.RightController;
                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case InputName.RightStickRightController:
                if (Globals.Instance.player1InputNameSelected == Globals.InputNameSelected.RightController && (Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSharedRight || Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSolo))
                    break;
                if (Globals.Instance.player2InputNameSelected == Globals.InputNameSelected.RightController && (Globals.Instance.player2ControlScheme == Globals.ControlScheme.ControllerSharedRight || Globals.Instance.player2ControlScheme == Globals.ControlScheme.ControllerSolo))
                {
                    Globals.Instance.player1ControlScheme = Globals.ControlScheme.ControllerSharedRight;
                    Globals.Instance.player2ControlScheme = Globals.ControlScheme.ControllerSharedLeft;
                }
                else
                    Globals.Instance.player1ControlScheme = Globals.ControlScheme.ControllerSolo;

                Globals.Instance.player1PreviousDevice = Globals.Instance.player1Device;
                Globals.Instance.player1Device = deviceIndex;
                Globals.Instance.player1InputNameSelected = Globals.InputNameSelected.RightController;
                break;
        }
    }

    void ChangePlayerTwoInput()
    {
        
        switch (inputName)
        {
            case InputName.LeftSideKeyboard:
                if (Globals.Instance.player2ControlScheme == Globals.ControlScheme.KeyboardSharedLeft || Globals.Instance.player2ControlScheme == Globals.ControlScheme.KeyboardSolo)
                    break;
                if (Globals.Instance.player1ControlScheme == Globals.ControlScheme.KeyboardSolo || Globals.Instance.player1ControlScheme == Globals.ControlScheme.KeyboardSharedLeft)
                {
                    Globals.Instance.player2ControlScheme = Globals.ControlScheme.KeyboardSharedLeft;
                    Globals.Instance.player1ControlScheme = Globals.ControlScheme.KeyboardSharedRight;
                }
                else
                    Globals.Instance.player2ControlScheme = Globals.ControlScheme.KeyboardSolo;

                Globals.Instance.player2PreviousDevice = Globals.Instance.player2Device;
                Globals.Instance.player2Device = -1;
                Globals.Instance.player2InputNameSelected = Globals.InputNameSelected.Keyboard;
                break;
            case InputName.RightSideKeyboard:
                if (Globals.Instance.player2ControlScheme == Globals.ControlScheme.KeyboardSharedRight || Globals.Instance.player2ControlScheme == Globals.ControlScheme.KeyboardSolo)
                    break;
                if (Globals.Instance.player1ControlScheme == Globals.ControlScheme.KeyboardSolo || Globals.Instance.player1ControlScheme == Globals.ControlScheme.KeyboardSharedRight)
                {
                    Globals.Instance.player2ControlScheme = Globals.ControlScheme.KeyboardSharedRight;
                    Globals.Instance.player1ControlScheme = Globals.ControlScheme.KeyboardSharedLeft;
                }
                else
                    Globals.Instance.player2ControlScheme = Globals.ControlScheme.KeyboardSolo;

                Globals.Instance.player2PreviousDevice = Globals.Instance.player2Device;
                Globals.Instance.player2Device = -1;
                Globals.Instance.player2InputNameSelected = Globals.InputNameSelected.Keyboard;
                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case InputName.LeftStickLeftController:
                if (Globals.Instance.player2InputNameSelected == Globals.InputNameSelected.LeftController && (Globals.Instance.player2ControlScheme == Globals.ControlScheme.ControllerSharedLeft || Globals.Instance.player2ControlScheme == Globals.ControlScheme.ControllerSolo))
                    break;
                if (Globals.Instance.player1InputNameSelected == Globals.InputNameSelected.LeftController && (Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSharedLeft || Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSolo))
                {
                    Globals.Instance.player2ControlScheme = Globals.ControlScheme.ControllerSharedLeft;
                    Globals.Instance.player1ControlScheme = Globals.ControlScheme.ControllerSharedRight;
                }
                else
                {
                    Globals.Instance.player2ControlScheme = Globals.ControlScheme.ControllerSolo;
                }

                Globals.Instance.player2PreviousDevice = Globals.Instance.player2Device;
                Globals.Instance.player2Device = deviceIndex;
                Globals.Instance.player2InputNameSelected = Globals.InputNameSelected.LeftController;
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                break;
            case InputName.RightStickLeftController:
                if (Globals.Instance.player2InputNameSelected == Globals.InputNameSelected.LeftController && (Globals.Instance.player2ControlScheme == Globals.ControlScheme.ControllerSharedRight || Globals.Instance.player2ControlScheme == Globals.ControlScheme.ControllerSolo))
                    break;
                if (Globals.Instance.player1InputNameSelected == Globals.InputNameSelected.LeftController && (Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSharedRight || Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSolo))
                {
                    Globals.Instance.player2ControlScheme = Globals.ControlScheme.ControllerSharedRight;
                    Globals.Instance.player1ControlScheme = Globals.ControlScheme.ControllerSharedLeft;
                }
                else
                    Globals.Instance.player2ControlScheme = Globals.ControlScheme.ControllerSolo;

                Globals.Instance.player2PreviousDevice = Globals.Instance.player2Device;
                Globals.Instance.player2Device = deviceIndex;
                Globals.Instance.player2InputNameSelected = Globals.InputNameSelected.LeftController;
                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case InputName.LeftStickRightController:
                if (Globals.Instance.player2InputNameSelected == Globals.InputNameSelected.RightController && (Globals.Instance.player2ControlScheme == Globals.ControlScheme.ControllerSharedLeft || Globals.Instance.player2ControlScheme == Globals.ControlScheme.ControllerSolo))
                    break;
                if (Globals.Instance.player1InputNameSelected == Globals.InputNameSelected.RightController && (Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSharedLeft || Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSolo))
                {
                    Globals.Instance.player2ControlScheme = Globals.ControlScheme.ControllerSharedLeft;
                    Globals.Instance.player1ControlScheme = Globals.ControlScheme.ControllerSharedRight;
                }
                else
                    Globals.Instance.player2ControlScheme = Globals.ControlScheme.ControllerSolo;

                Globals.Instance.player2PreviousDevice = Globals.Instance.player2Device;
                Globals.Instance.player2Device = deviceIndex;
                Globals.Instance.player2InputNameSelected = Globals.InputNameSelected.RightController;
                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case InputName.RightStickRightController:
                if (Globals.Instance.player2InputNameSelected == Globals.InputNameSelected.RightController && (Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSharedRight || Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSolo))
                    break;
                if (Globals.Instance.player1InputNameSelected == Globals.InputNameSelected.RightController && (Globals.Instance.player2ControlScheme == Globals.ControlScheme.ControllerSharedRight || Globals.Instance.player2ControlScheme == Globals.ControlScheme.ControllerSolo))
                {
                    Globals.Instance.player2ControlScheme = Globals.ControlScheme.ControllerSharedRight;
                    Globals.Instance.player1ControlScheme = Globals.ControlScheme.ControllerSharedLeft;
                }
                else
                    Globals.Instance.player2ControlScheme = Globals.ControlScheme.ControllerSolo;

                Globals.Instance.player2PreviousDevice = Globals.Instance.player2Device;
                Globals.Instance.player2Device = deviceIndex;
                Globals.Instance.player2InputNameSelected = Globals.InputNameSelected.RightController;
                break;
        }
    }




    void PlayerOneTimer()
    {
        player1Timer--;
    }
    void PlayerTwoTimer()
    {
        player2Timer--;
    }
}

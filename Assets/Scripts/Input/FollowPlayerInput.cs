using UnityEngine;
using System.Collections;

public class FollowPlayerInput : MonoBehaviour 
{
    public bool isLeftController = false;

    public GameObject LeftStick;
    public GameObject RightStick;

    private Vector3 stickStartPos;

    private PlayerInput player1;
    private PlayerInput player2;

    private float scale = 0.1f;
    private ControlsAndInput p1ControlScheme;
    private ControlsAndInput p2ControlScheme;

    private bool leftStickMoved = false;
    private bool rightStickMoved = false;

	// Use this for initialization
	void Start () 
    {
        SetPlayers();
        stickStartPos = new Vector3(0.0f, 0.0f, -0.1f);	
	}
	
	// Update is called once per frame
	void Update () 
    {
        p1ControlScheme = Globals.Instance.player1Controls;
        p2ControlScheme = Globals.Instance.player2Controls;
        

        if (player1 == null || player2 == null)
            SetPlayers();

        HandleControllers(isLeftController);

        leftStickMoved = false;
        rightStickMoved = false;
	}

    void HandleControllers(bool isLeftController)
    {
        
        if (p1ControlScheme.inputNameSelected == Globals.InputNameSelected.LeftController && isLeftController && Globals.Instance.leftControllerInputDevice != null)
        {
            if (p1ControlScheme.controlScheme == Globals.ControlScheme.SharedLeft)
            {
                LeftStick.transform.localPosition = stickStartPos + Vector3.Scale(new Vector3(scale, scale, 1.0f), player1.PlayerControllerSharedMovement());
                leftStickMoved = true;
            }
            if (p1ControlScheme.controlScheme == Globals.ControlScheme.SharedRight)
            {
                RightStick.transform.localPosition = stickStartPos + Vector3.Scale(new Vector3(scale, scale, 1.0f), player1.PlayerControllerSharedMovement());
                rightStickMoved = true;
            }
            if (p1ControlScheme.controlScheme == Globals.ControlScheme.Solo)
            {
                LeftStick.transform.localPosition = stickStartPos + Vector3.Scale(new Vector3(scale, scale, 1.0f), player1.PlayerControllerSoloMovement());
                RightStick.transform.localPosition = stickStartPos + Vector3.Scale(new Vector3(scale, scale, 1.0f), player1.PlayerControllerSoloMovement());

                leftStickMoved = true;
                rightStickMoved = true;
            }
        }
        else if (p1ControlScheme.inputNameSelected == Globals.InputNameSelected.RightController && !isLeftController && Globals.Instance.rightControllerInputDevice != null)
        {
            if (p1ControlScheme.controlScheme == Globals.ControlScheme.SharedLeft)
            {
                LeftStick.transform.localPosition = stickStartPos + Vector3.Scale(new Vector3(scale, scale, 1.0f), player1.PlayerControllerSharedMovement());
                leftStickMoved = true;
            }
            if (p1ControlScheme.controlScheme == Globals.ControlScheme.SharedRight)
            {
                RightStick.transform.localPosition = stickStartPos + Vector3.Scale(new Vector3(scale, scale, 1.0f), player1.PlayerControllerSharedMovement());
                rightStickMoved = true;
            }
            if (p1ControlScheme.controlScheme == Globals.ControlScheme.Solo)
            {
                LeftStick.transform.localPosition = stickStartPos + Vector3.Scale(new Vector3(scale, scale, 1.0f), player1.PlayerControllerSoloMovement());
                RightStick.transform.localPosition = stickStartPos + Vector3.Scale(new Vector3(scale, scale, 1.0f), player1.PlayerControllerSoloMovement());

                leftStickMoved = true;
                rightStickMoved = true;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        if (p2ControlScheme.inputNameSelected == Globals.InputNameSelected.LeftController && isLeftController && Globals.Instance.leftControllerInputDevice != null)
        {
            if (p2ControlScheme.controlScheme == Globals.ControlScheme.SharedLeft)
            {
                LeftStick.transform.localPosition = stickStartPos + Vector3.Scale(new Vector3(scale, scale, 1.0f), player2.PlayerControllerSharedMovement());
                leftStickMoved = true;
            }
            if (p2ControlScheme.controlScheme == Globals.ControlScheme.SharedRight)
            {
                RightStick.transform.localPosition = stickStartPos + Vector3.Scale(new Vector3(scale, scale, 1.0f), player2.PlayerControllerSharedMovement());
                rightStickMoved = true;
            }
            if (p2ControlScheme.controlScheme == Globals.ControlScheme.Solo)
            {
                LeftStick.transform.localPosition = stickStartPos + Vector3.Scale(new Vector3(scale, scale, 1.0f), player2.PlayerControllerSoloMovement());
                RightStick.transform.localPosition = stickStartPos + Vector3.Scale(new Vector3(scale, scale, 1.0f), player2.PlayerControllerSoloMovement());

                leftStickMoved = true;
                rightStickMoved = true;
            }
        }
        else if (p2ControlScheme.inputNameSelected == Globals.InputNameSelected.RightController && !isLeftController && Globals.Instance.rightControllerInputDevice != null)
        {
            
            if (p2ControlScheme.controlScheme == Globals.ControlScheme.SharedLeft)
            {
                LeftStick.transform.localPosition = stickStartPos + Vector3.Scale(new Vector3(scale, scale, 1.0f), player2.PlayerControllerSharedMovement());
                leftStickMoved = true;
            }
            if (p2ControlScheme.controlScheme == Globals.ControlScheme.SharedRight)
            {
                RightStick.transform.localPosition = stickStartPos + Vector3.Scale(new Vector3(scale, scale, 1.0f), player2.PlayerControllerSharedMovement());
                rightStickMoved = true;
            }
            if (p2ControlScheme.controlScheme == Globals.ControlScheme.Solo)
            {
                LeftStick.transform.localPosition = stickStartPos + Vector3.Scale(new Vector3(scale, scale, 1.0f), player2.PlayerControllerSoloMovement());
                RightStick.transform.localPosition = stickStartPos + Vector3.Scale(new Vector3(scale, scale, 1.0f), player2.PlayerControllerSoloMovement());

                leftStickMoved = true;
                rightStickMoved = true;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (!leftStickMoved)
            LeftStick.transform.localPosition = stickStartPos;
        if (!rightStickMoved)
            RightStick.transform.localPosition = stickStartPos;
    }

 

    void SetPlayers()
    {
        if (Globals.Instance.Player1 != null)
            player1 = Globals.Instance.Player1;
        if (Globals.Instance.Player2 != null)
            player2 = Globals.Instance.Player2;
    }
}

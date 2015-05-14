using UnityEngine;
using System.Collections;

public class FollowPlayerInputKeyboard : MonoBehaviour {

    public GameObject leftUpKeyP1;
    public GameObject leftUpKeyP2;
    public GameObject leftLeftKeyP1;
    public GameObject leftLeftKeyP2;
    public GameObject leftDownKeyP1;
    public GameObject leftDownKeyP2;
    public GameObject leftRightKeyP1;
    public GameObject leftRightKeyP2;

    public GameObject rightUpKeyP1;
    public GameObject rightUpKeyP2;
    public GameObject rightLeftKeyP1;
    public GameObject rightLeftKeyP2;
    public GameObject rightDownKeyP1;
    public GameObject rightDownKeyP2;
    public GameObject rightRightKeyP1;
    public GameObject rightRightKeyP2;


    private Color player1StartColor;
    private Color player2StartColor;

    public Color player1PressedColor;
    public Color player2PressedColor;

    private PlayerInput player1;
    private PlayerInput player2;

    private ControlsAndInput p1ControlScheme;
    private ControlsAndInput p2ControlScheme;

    private bool leftUpKeyMoved = false;
    private bool leftRightKeyMoved = false;
    private bool leftDownKeyMoved = false;
    private bool leftLeftKeyMoved = false;

    private bool rightUpKeyMoved = false;
    private bool rightRightKeyMoved = false;
    private bool rightDownKeyMoved = false;
    private bool rightLeftKeyMoved = false;

    // Use this for initialization
    void Start()
    {
        SetPlayers();
        player1StartColor = leftUpKeyP1.renderer.material.color;
        player2StartColor = leftUpKeyP2.renderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        p1ControlScheme = Globals.Instance.player1Controls;
        p2ControlScheme = Globals.Instance.player2Controls;


        if (player1 == null || player2 == null)
            SetPlayers();

        HandleKeyboard();

        leftUpKeyMoved = false;
        leftRightKeyMoved = false;
        leftDownKeyMoved = false;
        leftLeftKeyMoved = false;

        rightUpKeyMoved = false;
        rightRightKeyMoved = false;
        rightDownKeyMoved = false;
        rightLeftKeyMoved = false;
    }

    void HandleKeyboard()
    {

        if (p1ControlScheme.inputNameSelected == Globals.InputNameSelected.Keyboard)
        {
            if (p1ControlScheme.controlScheme == Globals.ControlScheme.SharedLeft)
            {
                Vector3 input = player1.PlayerKeyboardSharedMovement();

                leftUpKeyMoved = input.y == 1.0f;
                leftRightKeyMoved = input.x == 1.0f;
                leftDownKeyMoved = input.y == -1.0f;
                leftLeftKeyMoved = input.x == -1.0f;
            }
            if (p1ControlScheme.controlScheme == Globals.ControlScheme.SharedRight)
            {
                Vector3 input = player1.PlayerKeyboardSharedMovement();
                rightUpKeyMoved = input.y == 1.0f;
                rightRightKeyMoved = input.x == 1.0f;
                rightDownKeyMoved = input.y == -1.0f;
                rightLeftKeyMoved = input.x == -1.0f;
            }
            if (p1ControlScheme.controlScheme == Globals.ControlScheme.Solo)
            {
                Vector3 input = player1.PlayerKeyboardSoloMovement();

                leftUpKeyMoved = input.y == 1.0f;
                leftRightKeyMoved = input.x == 1.0f;
                leftDownKeyMoved = input.y == -1.0f;
                leftLeftKeyMoved = input.x == -1.0f;
                rightUpKeyMoved = input.y == 1.0f;
                rightRightKeyMoved = input.x == 1.0f;
                rightDownKeyMoved = input.y == -1.0f;
                rightLeftKeyMoved = input.x == -1.0f;
            }
        }
       

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        if (p2ControlScheme.inputNameSelected == Globals.InputNameSelected.Keyboard)
        {
            if (p2ControlScheme.controlScheme == Globals.ControlScheme.SharedLeft)
            {
                Vector3 input = player2.PlayerKeyboardSharedMovement();

                leftUpKeyMoved = input.y == 1.0f;
                leftRightKeyMoved = input.x == 1.0f;
                leftDownKeyMoved = input.y == -1.0f;
                leftLeftKeyMoved = input.x == -1.0f;
            }
            if (p2ControlScheme.controlScheme == Globals.ControlScheme.SharedRight)
            {
                Vector3 input = player2.PlayerKeyboardSharedMovement();
                rightUpKeyMoved = input.y == 1.0f;
                rightRightKeyMoved = input.x == 1.0f;
                rightDownKeyMoved = input.y == -1.0f;
                rightLeftKeyMoved = input.x == -1.0f;
            }
            if (p2ControlScheme.controlScheme == Globals.ControlScheme.Solo)
            {
                Vector3 input = player2.PlayerKeyboardSoloMovement();

                leftUpKeyMoved = input.y == 1.0f;
                leftRightKeyMoved = input.x == 1.0f;
                leftDownKeyMoved = input.y == -1.0f;
                leftLeftKeyMoved = input.x == -1.0f;
                rightUpKeyMoved = input.y == 1.0f;
                rightRightKeyMoved = input.x == 1.0f;
                rightDownKeyMoved = input.y == -1.0f;
                rightLeftKeyMoved = input.x == -1.0f;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if(leftUpKeyMoved)
        {
            leftUpKeyP1.renderer.material.color = player1PressedColor;
            leftUpKeyP2.renderer.material.color = player2PressedColor;
        }
        else
        {
            leftUpKeyP1.renderer.material.color = player1StartColor;
            leftUpKeyP2.renderer.material.color = player2StartColor;
        }

        if (leftLeftKeyMoved)
        {
            leftLeftKeyP1.renderer.material.color = player1PressedColor;
            leftLeftKeyP2.renderer.material.color = player2PressedColor;
        }
        else
        {
            leftLeftKeyP1.renderer.material.color = player1StartColor;
            leftLeftKeyP2.renderer.material.color = player2StartColor;
        }

        if (leftDownKeyMoved)
        {
            leftDownKeyP1.renderer.material.color = player1PressedColor;
            leftDownKeyP2.renderer.material.color = player2PressedColor;
        }
        else
        {
            leftDownKeyP1.renderer.material.color = player1StartColor;
            leftDownKeyP2.renderer.material.color = player2StartColor;
        }

        if (leftRightKeyMoved)
        {
            leftRightKeyP1.renderer.material.color = player1PressedColor;
            leftRightKeyP2.renderer.material.color = player2PressedColor;
        }
        else
        {
            leftRightKeyP1.renderer.material.color = player1StartColor;
            leftRightKeyP2.renderer.material.color = player2StartColor;
        }

        ////

        if (rightUpKeyMoved)
        {
            rightUpKeyP1.renderer.material.color = player1PressedColor;
            rightUpKeyP2.renderer.material.color = player2PressedColor;
        }
        else
        {
            rightUpKeyP1.renderer.material.color = player1StartColor;
            rightUpKeyP2.renderer.material.color = player2StartColor;
        }

        if (rightLeftKeyMoved)
        {
            rightLeftKeyP1.renderer.material.color = player1PressedColor;
            rightLeftKeyP2.renderer.material.color = player2PressedColor;
        }
        else
        {
            rightLeftKeyP1.renderer.material.color = player1StartColor;
            rightLeftKeyP2.renderer.material.color = player2StartColor;
        }

        if (rightDownKeyMoved)
        {
            rightDownKeyP1.renderer.material.color = player1PressedColor;
            rightDownKeyP2.renderer.material.color = player2PressedColor;
        }
        else
        {
            rightDownKeyP1.renderer.material.color = player1StartColor;
            rightDownKeyP2.renderer.material.color = player2StartColor;
        }

        if (rightRightKeyMoved)
        {
            rightRightKeyP1.renderer.material.color = player1PressedColor;
            rightRightKeyP2.renderer.material.color = player2PressedColor;
        }
        else
        {
            rightRightKeyP1.renderer.material.color = player1StartColor;
            rightRightKeyP2.renderer.material.color = player2StartColor;
        }

        leftUpKeyMoved = false;
        leftRightKeyMoved = false;
        leftLeftKeyMoved = false;
        rightUpKeyMoved = false;
        rightRightKeyMoved = false;
        rightDownKeyMoved = false;
        rightLeftKeyMoved = false;

    }



    void SetPlayers()
    {
        if (Globals.Instance.player1 != null)
            player1 = Globals.Instance.player1;
        if (Globals.Instance.player2 != null)
            player2 = Globals.Instance.player2;
    }
}

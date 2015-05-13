using UnityEngine;
using System.Collections;
using InControl;

public class InputSelect : MonoBehaviour {

    public InputFill inputFill;

    private Vector3 posNoZ;

    public float distance = 2.0f;
    private float distancePow = 0.0f;

    private float disToPlayer1;
    private float disToPlayer2;

    public bool player1Toggled = false;
    public bool player2Toggled = false;

    [SerializeField]
    public ControlsAndInput inputNameAndControlScheme;

    private Vector3 startingSize;
    private Vector3 bigger;

    void Start()
    {
        distancePow = Mathf.Pow(distance, 2);
    }

    void Update()
    {

        posNoZ = new Vector3(transform.position.x, transform.position.y, 0.0f);

        if (Player1InRange() && !Player2InRange())
        {
            inputFill.player1FutureControls.controlScheme = inputNameAndControlScheme.controlScheme;
            inputFill.player1FutureControls.inputNameSelected = inputNameAndControlScheme.inputNameSelected;

            if (inputFill.player2FutureControls.controlScheme == inputNameAndControlScheme.controlScheme && inputFill.player2FutureControls.inputNameSelected == inputNameAndControlScheme.inputNameSelected)
            {
                inputFill.player2FutureControls.controlScheme = Globals.ControlScheme.None;
                inputFill.player2FutureControls.inputNameSelected = Globals.InputNameSelected.None;
            }

        }
        else if (Player2InRange() && !Player1InRange())
        {
            if (inputFill.player1FutureControls.controlScheme == inputNameAndControlScheme.controlScheme && inputFill.player1FutureControls.inputNameSelected == inputNameAndControlScheme.inputNameSelected)
            {
                inputFill.player1FutureControls.controlScheme = Globals.ControlScheme.None;
                inputFill.player1FutureControls.inputNameSelected = Globals.InputNameSelected.None;
            }

            inputFill.player2FutureControls.controlScheme = inputNameAndControlScheme.controlScheme;
            inputFill.player2FutureControls.inputNameSelected = inputNameAndControlScheme.inputNameSelected;
        }
        else if (!Player1InRange() && !Player2InRange())
        {
            if (inputFill.player1FutureControls.controlScheme == inputNameAndControlScheme.controlScheme && inputFill.player1FutureControls.inputNameSelected == inputNameAndControlScheme.inputNameSelected)
            {
                inputFill.player1FutureControls.controlScheme = Globals.ControlScheme.None;
                inputFill.player1FutureControls.inputNameSelected = Globals.InputNameSelected.None;
            }
            if (inputFill.player2FutureControls.controlScheme == inputNameAndControlScheme.controlScheme && inputFill.player2FutureControls.inputNameSelected == inputNameAndControlScheme.inputNameSelected)
            {
                inputFill.player2FutureControls.controlScheme = Globals.ControlScheme.None;
                inputFill.player2FutureControls.inputNameSelected = Globals.InputNameSelected.None;
            }
        }


    }

    private bool Player1InRange()
    {
        disToPlayer1 = Vector3.SqrMagnitude(inputFill.player1PosNoZ - posNoZ);
        player1Toggled = disToPlayer1 < distancePow;
        return player1Toggled;
    }

    private bool Player2InRange()
    {
        disToPlayer2 = Vector3.SqrMagnitude(inputFill.player2PosNoZ - posNoZ);
        player2Toggled = disToPlayer2 < distancePow;
        return player2Toggled;
    }


   
}

using UnityEngine;
using System.Collections;
using InControl;

public class InputSelect : MonoBehaviour {

    public InputFill inputFill;

    [SerializeField]
    public ControlsAndInput inputNameAndControlScheme;


    void OnTriggerEnter(Collider collide)
    {
        if (collide.gameObject.name == "Player 1")
        {
            inputFill.player1FutureControls.controlScheme = inputNameAndControlScheme.controlScheme;
            inputFill.player1FutureControls.inputNameSelected = inputNameAndControlScheme.inputNameSelected;
        }
        if (collide.gameObject.name == "Player 2")
        {
            inputFill.player2FutureControls.controlScheme = inputNameAndControlScheme.controlScheme;
            inputFill.player2FutureControls.inputNameSelected = inputNameAndControlScheme.inputNameSelected;
        }
    }

    void OnTriggerExit(Collider collide)
    {
        if (collide.gameObject.name == "Player 1")
        {
            inputFill.player1FutureControls.controlScheme = Globals.ControlScheme.None;
            inputFill.player1FutureControls.inputNameSelected = Globals.InputNameSelected.None;
        }
        if (collide.gameObject.name == "Player 2")
        {
            inputFill.player2FutureControls.controlScheme = Globals.ControlScheme.None;
            inputFill.player2FutureControls.inputNameSelected = Globals.InputNameSelected.None;
        }

    }

   

  


}

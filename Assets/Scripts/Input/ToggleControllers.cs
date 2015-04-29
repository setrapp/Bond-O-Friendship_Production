using UnityEngine;
using System.Collections;
using InControl;


public class ToggleControllers : MonoBehaviour {

    public GameObject rightController;
    public GameObject leftController;

    public InputFill inputFill;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Globals.Instance.leftControllerIndex == -3)
        {
            leftController.SetActive(false);
            if(inputFill.player1FutureControls.inputNameSelected == Globals.InputNameSelected.LeftController)
            {
                inputFill.player1FutureControls.controlScheme = Globals.ControlScheme.None;
                inputFill.player1FutureControls.inputNameSelected = Globals.InputNameSelected.None;
            }
            if (inputFill.player2FutureControls.inputNameSelected == Globals.InputNameSelected.LeftController)
            {
                inputFill.player2FutureControls.controlScheme = Globals.ControlScheme.None;
                inputFill.player2FutureControls.inputNameSelected = Globals.InputNameSelected.None;
            }

        }
        else
            leftController.SetActive(true);
        if (Globals.Instance.rightContollerIndex == -3)
        {
            rightController.SetActive(false);
            if (inputFill.player1FutureControls.inputNameSelected == Globals.InputNameSelected.RightController)
            {
                inputFill.player1FutureControls.controlScheme = Globals.ControlScheme.None;
                inputFill.player1FutureControls.inputNameSelected = Globals.InputNameSelected.None;
            }
            if (inputFill.player2FutureControls.inputNameSelected == Globals.InputNameSelected.RightController)
            {
                inputFill.player2FutureControls.controlScheme = Globals.ControlScheme.None;
                inputFill.player2FutureControls.inputNameSelected = Globals.InputNameSelected.None;
            }
        }
        else
            rightController.SetActive(true);

	}
}

using UnityEngine;
using System.Collections;
using InControl;


public class ToggleControllers : MonoBehaviour {

    public GameObject rightController;
    public GameObject leftController;

    public InputFill inputFill;

    private Vector3 leftControllerLocation;
    private Vector3 rightControllerLocation;

    private Vector3 centerLocation;

    private float duration = 1.0f;
    private float t = 0;

    private bool moving = false;

//    private bool rightControllerActive = false;



	// Use this for initialization
	void Start () 
    {
        leftControllerLocation = leftController.transform.localPosition;
        rightControllerLocation = rightController.transform.localPosition;

        centerLocation = (leftControllerLocation + rightControllerLocation) / 2.0f;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (InputManager.Devices.Count == 0)
        {
            leftController.SetActive(false);
            if (inputFill.player1FutureControls.inputNameSelected == Globals.InputNameSelected.LeftController)
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
        {
            leftController.SetActive(true);
        }
        if (InputManager.Devices.Count < 2)
        {
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


            t = Mathf.Clamp(t - Time.deltaTime / duration, 0.0f, 1.0f);
        }
        else
        {
            rightController.SetActive(true);
            t = Mathf.Clamp(t + Time.deltaTime / duration, 0.0f, 1.0f);
        }

        if (t == 1 || t == 0)
            moving = false;
        else
            moving = true;

        if(t == 0)
            rightController.SetActive(false);





        leftController.transform.localPosition = Vector3.Lerp(centerLocation, leftControllerLocation, t);
        rightController.transform.localPosition = Vector3.Lerp(centerLocation, rightControllerLocation, t);

    }
}

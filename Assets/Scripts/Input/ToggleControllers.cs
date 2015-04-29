using UnityEngine;
using System.Collections;
using InControl;


public class ToggleControllers : MonoBehaviour {

    public GameObject rightController;
    public GameObject leftController;

    public GameObject rightControllerMask;

    public InputFill inputFill;

    private Vector3 leftControllerLocation;
    private Vector3 rightControllerLocation;

    public Collider lcltCol;
    public Collider lcrtCol;

    public Collider rcltCol;
    public Collider rcrtCol;

    private Vector3 centerLocation;

    private float duration = 1.0f;
    private float t = 0;

    private bool moving = false;

    private bool rightControllerActive = false;

	// Use this for initialization
	void Start () 
    {
        leftControllerLocation = leftController.transform.position;
        rightControllerLocation = rightController.transform.position;

        centerLocation = (leftControllerLocation + rightControllerLocation) / 2.0f;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Globals.Instance.leftControllerIndex == -3)
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

            inputFill.LL.transform.localScale = inputFill.emptyController;
            inputFill.LL2.transform.localScale = inputFill.emptyController;
            inputFill.LR.transform.localScale = inputFill.emptyController;
            inputFill.LR2.transform.localScale = inputFill.emptyController;

        }
        else
        {
            leftController.SetActive(true);
        }
        if (Globals.Instance.rightContollerIndex == -3)
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

            inputFill.RL.transform.localScale = inputFill.emptyController;
            inputFill.RL2.transform.localScale = inputFill.emptyController;
            inputFill.RR.transform.localScale = inputFill.emptyController;
            inputFill.RR2.transform.localScale = inputFill.emptyController;

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

        lcltCol.enabled = !moving;
        lcrtCol.enabled = !moving;
        rcltCol.enabled = !moving;
        rcrtCol.enabled = !moving;

        rightControllerMask.SetActive(!moving);



        leftController.transform.position = Vector3.Lerp(centerLocation, leftControllerLocation, t);
        rightController.transform.position = Vector3.Lerp(centerLocation, rightControllerLocation, t);

    }
}

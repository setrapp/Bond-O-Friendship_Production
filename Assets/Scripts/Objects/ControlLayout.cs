using UnityEngine;
using System.Collections;
using InControl;

public class ControlLayout : MonoBehaviour {


    private bool useController;

    public GameObject controller;
    public GameObject keyboard;

    public GameObject playerOneMoveController;
    public GameObject playerOneShootController;
    public GameObject playerTwoMoveController;
    public GameObject playerTwoShootController;
    public GameObject controllerOutline;

    public GameObject playerOneMoveKeyboard;
    public GameObject playerOneShootKeyboard;
    public GameObject playerTwoMoveKeyboard;
    public GameObject playerTwoShootKeyboard;
    public GameObject keyboardOutline;

    InputDevice device;

    private bool playerOneMovePressed = false;
    private bool playerOneShootPressed = false;
    private bool playerTwoMovePressed = false;
    private bool playerTwoShootPressed = false;

	// Use this for initialization
	void Start () {
        useController = Globals.usingController;

		if(useController)
		{
			keyboard.SetActive(false);
			controller.SetActive(true);
			if (Globals.Instance != null && !Globals.Instance.fluffsThrowable)
			{
				playerOneShootController.SetActive(false);
				playerTwoShootController.SetActive(false);
			}
		}
		else
		{
			keyboard.SetActive(true);
			controller.SetActive(false);
			if (Globals.Instance != null && !Globals.Instance.fluffsThrowable)
			{
				playerOneShootKeyboard.SetActive(false);
				playerTwoShootKeyboard.SetActive(false);
			}
		}	
	}
	
	// Update is called once per frame
	void Update () {

        if (useController)
        {
            device = InputManager.ActiveDevice;
            CheckControllerInput();
        }
        else
            CheckKeyboardInput();
        
	
	}
    private void CheckKeyboardInput()
    {
        if (!playerOneMoveKeyboard.activeInHierarchy && !playerOneShootKeyboard.activeInHierarchy && !playerTwoMoveKeyboard.activeInHierarchy && !playerTwoShootKeyboard.activeInHierarchy)
        {
            StartCoroutine(FadeOut(keyboardOutline, keyboardOutline.renderer.material.color, 1.0f));
            if (!keyboardOutline.activeInHierarchy)
                this.gameObject.SetActive(false);
        }

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)) && !playerOneMovePressed)
        {
            StartCoroutine(FadeOut(playerOneMoveKeyboard, playerOneMoveKeyboard.renderer.material.color, 1.0f));
            playerOneMovePressed = true;
        }
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow)) && !playerTwoMovePressed)
        {
            StartCoroutine(FadeOut(playerTwoMoveKeyboard, playerTwoMoveKeyboard.renderer.material.color, 1.0f));
            playerTwoMovePressed = true;
        }

        if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Space)) && !playerOneShootPressed)
        {
            StartCoroutine(FadeOut(playerOneShootKeyboard, playerOneShootKeyboard.renderer.material.color, 1.0f));
            playerOneShootPressed = true;
        }
        if ((Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.Return)) && !playerTwoShootPressed)
        {
            StartCoroutine(FadeOut(playerTwoShootKeyboard, playerTwoShootKeyboard.renderer.material.color, 1.0f));
            playerTwoShootPressed = true;
        }       
    }

    private void CheckControllerInput()
    {
        if (!playerOneMoveController.activeInHierarchy && !playerOneShootController.activeInHierarchy && !playerTwoMoveController.activeInHierarchy && !playerTwoShootController.activeInHierarchy)
        {
            StartCoroutine(FadeOut(controllerOutline, controllerOutline.renderer.material.color, 1.0f));
            if (!controllerOutline.activeInHierarchy)
                this.gameObject.SetActive(false);
        }

        if(device.LeftStick.HasChanged && !playerOneMovePressed)
        {   
            StartCoroutine(FadeOut(playerOneMoveController, playerOneMoveController.renderer.material.color, 1.0f));
            playerOneMovePressed = true;
        }
        
        if (device.RightStick.HasChanged && !playerTwoMovePressed)
        {
            StartCoroutine(FadeOut(playerTwoMoveController, playerTwoMoveController.renderer.material.color, 1.0f));
            playerTwoMovePressed = true;
        }

        if ((device.LeftBumper.IsPressed || device.LeftTrigger.IsPressed) && !playerOneShootPressed)
        {
            StartCoroutine(FadeOut(playerOneShootController, playerOneShootController.renderer.material.color, 1.0f));
            playerOneShootPressed = true;
        }
        if ((device.RightBumper.IsPressed || device.RightTrigger.IsPressed) && !playerTwoShootPressed)
        {
            StartCoroutine(FadeOut(playerTwoShootController, playerTwoShootController.renderer.material.color, 1.0f));
            playerTwoShootPressed = true;
        }       
    }

    IEnumerator FadeOut(GameObject go, Color goStartingColor, float duration)
    {
        for (float t = 0.0f; t < duration; t += Time.deltaTime)
        {            
            go.renderer.material.color = Color.Lerp(goStartingColor, new Color(goStartingColor.r, goStartingColor.g, goStartingColor.b, 0.0f), t / duration);
            if (go.renderer.material.color.a < 0.1f)
                go.SetActive(false);
            yield return null;
        }        

    }

  
}

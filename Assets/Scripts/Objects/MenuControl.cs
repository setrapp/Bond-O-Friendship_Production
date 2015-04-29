using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using InControl;

public class MenuControl : MonoBehaviour {

    public GameObject startMenu;

    public GameObject obscureCanvas;

    public GameObject inputSelect;

	public string startScene;
    InputDevice device;
    private int deviceCount;

    public bool player1Ready = false;
    public bool player2Ready = false;

    private bool readyUp = false;

    private bool inputSelected = false;

    private float f = 0f;
    private float t = 1f;
    public float duration = 3.0f;

    public bool fadeIn = true;

    private Color startColor;
    private Color fadeColor;

    private Component[] inputSelectRenderers;
    private List<Color> inputSelectColorsEmpty = new List<Color>();
    private List<Color> inputSelectColorsFull = new List<Color>();

	// Use this for initialization
	void Start () 
    {
        startColor = obscureCanvas.renderer.material.color;
        fadeColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f);

        inputSelectRenderers = inputSelect.GetComponentsInChildren<Renderer>();
       
        foreach(Renderer renderer in inputSelectRenderers)
        {
            if (renderer.material.HasProperty("_Color"))
            {
                renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.0f);
                inputSelectColorsEmpty.Add(renderer.material.color);
                inputSelectColorsFull.Add(new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1.0f));
            }
            else
            {
                inputSelectColorsEmpty.Add(Color.white);
                inputSelectColorsFull.Add(Color.white);
            }
        }
       // Debug.Log(inputSelectRenderers.Length);
        //Debug.Log(inputSelectColorsEmpty.Count);
        //Debug.Log(inputSelectColorsFull.Count);
    }


	// Update is called once per frame
    void Update()
    {
        //Debug.Log(inputSelectRenderers.Length);
        //device = InputManager.ActiveDevice;
        //Debug.Log(device.Name);
        deviceCount = InputManager.controllerCount;

        GameObject[] messages = GameObject.FindGameObjectsWithTag("TranslevelMessage");
        bool levelLoadFound = false;
        for (int i = 0; i < messages.Length && !levelLoadFound; i++)
        {
            TranslevelMessage message = messages[i].GetComponent<TranslevelMessage>();
            if (message != null && message.messageName == "LevelLoad")
            {
                startScene = message.message;
                Destroy(message.gameObject);
            }
			if (message != null && message.messageName == "LevelLoadInstant")
			{
				startScene = message.message;
				Destroy(message.gameObject);
				MainMenuLoadLevel();
			}
        }

        if (!inputSelected)
        {
            if (deviceCount > 0)
            {

                device = InputManager.ActiveDevice;
                if (device.AnyButton || device.LeftStick.HasChanged || device.RightStick.HasChanged)
                {
                    Globals.Instance.leftControllerIndex = InputManager.Devices.IndexOf(device);
                    Globals.Instance.leftControllerPreviousIndex = Globals.Instance.leftControllerIndex;
                    Globals.Instance.rightContollerIndex = -2;
                    Globals.Instance.rightControllerPreviousIndex = -2;

                    if (deviceCount == 1)
                    {
                        Globals.Instance.player1Controls.controlScheme = Globals.ControlScheme.SharedLeft;
                        Globals.Instance.player2Controls.controlScheme = Globals.ControlScheme.SharedRight;
                        Globals.Instance.player1Controls.inputNameSelected = Globals.InputNameSelected.LeftController;
                        Globals.Instance.player2Controls.inputNameSelected = Globals.InputNameSelected.LeftController;
                    }
                    else
                    {
                        Globals.Instance.player1Controls.controlScheme = Globals.ControlScheme.Solo;
                        Globals.Instance.player2Controls.controlScheme = Globals.ControlScheme.Solo;
                        Globals.Instance.player1Controls.inputNameSelected = Globals.InputNameSelected.LeftController;
                        Globals.Instance.player2Controls.inputNameSelected = Globals.InputNameSelected.RightController;
                    }

                    inputSelected = true;
                    inputSelect.SetActive(true);
                    return;
                }
            }
            if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))//Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.RightControl))
            {
                if (deviceCount == 0)
                {
                    Globals.Instance.leftControllerIndex = -3;
                    Globals.Instance.rightContollerIndex = -3;
                }
                else if (deviceCount == 1)
                {
                    Globals.Instance.leftControllerIndex = -2;
                    Globals.Instance.rightContollerIndex = -3;
                }
                else
                {
                    Globals.Instance.leftControllerIndex = -2;
                    Globals.Instance.rightContollerIndex = -2;
                }

                Globals.Instance.player1Controls.controlScheme = Globals.ControlScheme.SharedLeft;
                Globals.Instance.player2Controls.controlScheme = Globals.ControlScheme.SharedRight;
                Globals.Instance.player1Controls.inputNameSelected = Globals.InputNameSelected.Keyboard;
                Globals.Instance.player2Controls.inputNameSelected = Globals.InputNameSelected.Keyboard;

                inputSelected = true;
                inputSelect.SetActive(true);
                return;
            }
        }
        else
        {
            if (startMenu.activeSelf)
                FadeStartMenu();
                
        }

        if (player1Ready && player2Ready && inputSelected)
        {
            Globals.Instance.allowInput = false;
            readyUp = true;
            //if(obscureCanvas.activeSelf)
                //FadeInFadeOut();           
             
            //MainMenuLoadLevel();           
        }
        else
		{
            CameraSplitter.Instance.followPlayers = false;
		}

        if(readyUp)
            FadeControls();

    }	


    private void FadeStartMenu()
    {
        if (startMenu.GetComponent<CanvasGroup>().alpha != 0.0f)
        {
            t -= Time.deltaTime / duration;
            t = Mathf.Clamp(t, 0.0f, 1.0f);
            f = 1.0f - t;
            startMenu.GetComponent<CanvasGroup>().alpha = t;
            for (int i = 0; i < inputSelectRenderers.Length; i++ )
            {
                if (inputSelectRenderers[i].renderer.material.HasProperty("_Color"))
                    inputSelectRenderers[i].renderer.material.color = Color.Lerp(inputSelectColorsEmpty[i], inputSelectColorsFull[i], f);
            }
        }
        else
        {
            startMenu.SetActive(false);
            t = 1.0f;
            MainMenuLoadLevel();
        }
    }

    private void FadeControls()
    {
        if (t != 0)
        {
            
            CameraSplitter.Instance.movePlayers = true;
           // inputSelect.SetActive(false);
          //  Debug.Log(t);
            t -= Time.deltaTime / duration;
            t = Mathf.Clamp(t, 0.0f, 1.0f);
            for (int i = 0; i < inputSelectRenderers.Length; i++)
            {
                if (inputSelectRenderers[i].renderer.material.HasProperty("_Color"))
                    inputSelectRenderers[i].renderer.material.color = Color.Lerp(inputSelectColorsEmpty[i], inputSelectColorsFull[i], t);
            }
        }
        else
        {
            Invoke("ZoomCamera", 1.0f);
        }
    }

    private void FadeInFadeOut()
    {
        if(fadeIn)
        {
            if (t != 1)
            {
                t = Mathf.Clamp(t + Time.deltaTime / duration, 0.0f, 1.0f);
                obscureCanvas.renderer.material.color = Color.Lerp(startColor, fadeColor, t);
                //obscureCanvas.GetComponent<CanvasGroup>().alpha = t;
            }
            else
            {
                fadeIn = false;
                Globals.Instance.perspectiveCamera = true;
                
            }
        }
        else
        {
            if (t != 0)
            {
                t = Mathf.Clamp(t - Time.deltaTime / duration, 0.0f, 1.0f);
                obscureCanvas.renderer.material.color = Color.Lerp(startColor, fadeColor, t);
                //obscureCanvas.GetComponent<CanvasGroup>().alpha = t;
            }
            else
            {
                obscureCanvas.SetActive(false);
                
            }
        }
    }

    public void ZoomCamera()
    {
        
        CameraSplitter.Instance.zoom = true;
    }

    public void MainMenuLoadLevel()
    {
		//CameraSplitter.Instance.splittable = true;
        //Application.LoadLevel(startScene);
        Application.LoadLevelAdditiveAsync(startScene);
    }

    public void MainMenuLoadLevel(string levelName)
    {
        if(levelName != null)
        {
            //Application.LoadLevel(levelName);
            startScene = levelName;
        }
    }


    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
		        Application.Quit();
        #endif
    }

   

}

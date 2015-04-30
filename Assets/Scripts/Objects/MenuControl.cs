using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using InControl;

public class MenuControl : MonoBehaviour {

    public GameObject startMenu;

    public GameObject obscureMenuPanel;

    public GameObject inputSelect;

    public GameObject begin;

    //public GameObject levelCover;
    //public GameObject levelCover2;

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

    private bool startLevelLoaded = false;

    private bool startPanelFade = false;
    private bool zoom = true;

	// Use this for initialization
	void Start () 
    {
        //startColor = levelCover.renderer.material.color;
       // fadeColor = new Color(startColor.r, startColor.g, startColor.b, 0.0f);
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
                StartCoroutine(MainMenuLoadLevel());
            }
        }
        if (!startLevelLoaded)
            StartCoroutine(MainMenuLoadLevel());

        Globals.Instance.inMainMenu = true;
    }


	// Update is called once per frame
    void Update()
    {
        //Debug.Log(inputSelectRenderers.Length);
        //device = InputManager.ActiveDevice;
        //Debug.Log(device.Name);


        if (obscureMenuPanel.activeSelf && startPanelFade)
        {
            FadeInFadeOut();
        }
        else
        {
            deviceCount = InputManager.controllerCount;
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

            if (readyUp)
                FadeControls();
        }

    }	


    private void FadeStartMenu()
    {
        if (startMenu.GetComponent<CanvasGroup>().alpha != 0.0f)
        {
            t -= Time.deltaTime / duration;
            t = Mathf.Clamp(t, 0.0f, 1.0f);
            f = 1.0f - t;
            startMenu.GetComponent<CanvasGroup>().alpha = t;
            //levelCover.renderer.material.color = Color.Lerp(fadeColor, startColor, t);
            //levelCover2.renderer.material.color = Color.Lerp(fadeColor, startColor, t);
            for (int i = 0; i < inputSelectRenderers.Length; i++ )
            {
                if (inputSelectRenderers[i].renderer.material.HasProperty("_Color"))
                    inputSelectRenderers[i].renderer.material.color = Color.Lerp(inputSelectColorsEmpty[i], inputSelectColorsFull[i], f);
            }
        }
        else
        {
            startMenu.SetActive(false);
            //levelCover.SetActive(false);
           // levelCover2.SetActive(false);
            t = 1.0f;
        }
    }

    private void FadeControls()
    {
        if (t != 0)
        {            
            CameraSplitter.Instance.movePlayers = true;
            t -= Time.deltaTime / duration;
            t = Mathf.Clamp(t, 0.0f, 1.0f);
            for (int i = 0; i < inputSelectRenderers.Length; i++)
            {
                if (inputSelectRenderers[i].renderer.material.HasProperty("_Color"))
                    inputSelectRenderers[i].renderer.material.color = Color.Lerp(inputSelectColorsEmpty[i], inputSelectColorsFull[i], t);
            }

            if (zoom)
            {
                if (!Application.isEditor || Globals.Instance.zoomIntroInEditor)
                {
                    Invoke("ZoomCamera", 0.5f);
                }
                else
                {
                    CameraSplitter.Instance.EndZoom();
                    CameraSplitter.Instance.splittable = true;
                }

                zoom = false;
            }

        }
        else
        {
			
        }
    }

    private void FadeInFadeOut()
    {
        
            if (t != 0)
            {
                t = Mathf.Clamp(t - Time.deltaTime / 2.0f, 0.0f, 1.0f);
                float f = 1 - t;
                obscureMenuPanel.GetComponent<CanvasGroup>().alpha = t;
                begin.GetComponent<CanvasGroup>().alpha = f;
            }
            else
            {
                t = 1;
                obscureMenuPanel.SetActive(false);
                
            }
        
	}

    public void ZoomCamera()
    {
		CameraSplitter.Instance.zoom = true;
    }

    public IEnumerator MainMenuLoadLevel()
    {
        AsyncOperation async = Application.LoadLevelAdditiveAsync(startScene);
        startLevelLoaded = true;

        yield return async;

        startPanelFade = true;
    }

    public void MainMenuLoadLevel(string levelName)
    {
        if(levelName != null)
        {
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

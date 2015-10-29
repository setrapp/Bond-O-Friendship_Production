using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using InControl;

public class PauseMenuControl : MonoBehaviour {

    public ClusterNodePuzzle resumeGameNodePuzzle;

    public InputFill inputFill;

    public GameObject gameControls;

	public GameObject mainMenu;

    public GameObject inputSelect;

	public GameObject options;

	public GameObject exitGameConfirm;

	public enum MenuState{PauseMenu, Options, InputSelect, QuitGame};

	public MenuState menuState = MenuState.PauseMenu;

	public FadePauseMenu fMainMenu;
	public FadeInputSelect fInputSelect;
	public FadeQuitGame fQuitGame;
	public FadeOptions fOptions;


	public ClusterNodePuzzle confirmQuitNodePuzzle;

    private float s = 1.0f;
    private bool toggleFadeOutControls = false;
    private bool toggleInvoke = true;
	
	// Update is called once per frame
    void Update()
    {    

	//Main Menu/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

			if(Globals.Instance.gameState == Globals.GameState.Paused && !Globals.Instance.inMainMenu)
			{

				
				if(!mainMenu.activeInHierarchy)
					mainMenu.SetActive(true);
                if (!exitGameConfirm.activeInHierarchy)
                    exitGameConfirm.SetActive(true);
                if (!options.activeInHierarchy)
                    options.SetActive(true);
                if (!inputSelect.activeInHierarchy)
                    inputSelect.SetActive(true);
                if (!gameControls.activeInHierarchy)
                {
                    toggleFadeOutControls = false;
                    toggleInvoke = true;
                    gameControls.GetComponent<CanvasGroup>().alpha = 1.0f;
                    s = 1.0f;
                    gameControls.SetActive(true);
                }

                if (toggleInvoke)
                {
                    Invoke("ToggleControlsFadeInvoke", 5.0f);
                }
                if (toggleFadeOutControls)
                {
                    FadeOutControls();
                }
				//if(fMainMenu.f != 1)
					//fMainMenu.FadeIn();

                
               // if (fQuitGame.f != 1)
                   // fQuitGame.FadeIn();

               
                //if (fOptions.f != 1)
                    //fOptions.FadeIn();


              //  if (fInputSelect.f != 1)
                   // fInputSelect.FadeIn();

                //i//f(Globals.Instance.notifyControlsChangeOnDisconnect)
               // {

               // }
                

				if (resumeGameNodePuzzle != null && resumeGameNodePuzzle.solved)
				{
					resumeGameNodePuzzle.solved = false;
                    resumeGameNodePuzzle.UnlightNodes();
                    CameraSplitter.Instance.player1Target.transform.position = Globals.Instance.Player1.transform.position;
                    CameraSplitter.Instance.player2Target.transform.position = Globals.Instance.Player2.transform.position;
					Globals.Instance.gameState = Globals.GameState.Unpausing;
				}

                if (confirmQuitNodePuzzle != null && confirmQuitNodePuzzle.solved)
                {
                    confirmQuitNodePuzzle.solved = false;
                    Globals.Instance.pauseMenuFloors.SetActive(false);
                    Globals.Instance.ResetOrExit();
                    
                }

                if (fOptions.f == 1)
                {
                    if (!options.GetComponent<OptionsMenu>().soundChecked)
                        options.GetComponent<OptionsMenu>().CheckSoundSettings();
                }

                if (!inputFill.allowFill)
                    inputFill.allowFill = true;
				
			}
			else
			{
                if (mainMenu.activeInHierarchy)
                    mainMenu.SetActive(false);
                if (exitGameConfirm.activeInHierarchy)
                    exitGameConfirm.SetActive(false);
                if (options.activeInHierarchy)
                    options.SetActive(false);
                if (inputSelect.activeInHierarchy)
                    inputSelect.SetActive(false);
                if (gameControls.activeInHierarchy)
                    gameControls.SetActive(false);
			}
					


			

        

    }

    private void ToggleControlsFadeInvoke()
    {
        toggleFadeOutControls = true;
        toggleInvoke = false;
    }

    private void FadeOutControls()
    {
        if (gameControls.GetComponent<CanvasGroup>().alpha != 0.0f)
        {
            s = Mathf.Clamp(s - Time.deltaTime / 2.0f, 0.0f, 1.0f);
            gameControls.GetComponent<CanvasGroup>().alpha = s;


        }
        else
        {
            toggleFadeOutControls = false;
        }
    }

	private void ToggleFadeMainMenu ()
	{
		//if(fMainMenu.f == 0)
		//{
		//	if(mainMenu.activeInHierarchy && fMainMenu.colorsSet)
		//		mainMenu.SetActive(false);
		//}
		//else
		//{
		//	fMainMenu.FadeOut();
		//}
	}
	private void ToggleFadeOptionsMenu()
	{
		//if(fOptions.f == 0)
		//{
		//	if(options.activeInHierarchy && fOptions.colorsSet)
		//		options.SetActive(false);
		//}
		//else
		//{
		//	fOptions.FadeOut();
		//}
	}

	private void ToggleFadeInputSelectMenu()
	{
		//if(fInputSelect.f == 0)
		//{
		//	if(inputSelect.activeInHierarchy)
		//		inputSelect.SetActive(false);
		//}
		//else
		//{
		//	fInputSelect.FadeOut();
		//}
	}
	private void ToggleFadeExitGameConfirm()
	{
		//if(fQuitGame.f == 0)
		//{
		//	if(exitGameConfirm.activeInHierarchy)
		//		exitGameConfirm.SetActive(false);
		//}
		//else
		//{
		//	fQuitGame.FadeOut();
		//}
	}   

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using InControl;

public class PauseMenuControl : MonoBehaviour {

    public ClusterNodePuzzle resumeGameNodePuzzle;

    public InputFill inputFill;


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

	public ClusterNodePuzzle optionsNodePuzzle;
	public ClusterNodePuzzle exitGameNodePuzzle;

	public ClusterNodePuzzle optionsBackNodePuzzle;
	public ClusterNodePuzzle optionsInputSelectNodePuzzle;

	public ClusterNodePuzzle inputSelectBackNodePuzzle;

	public ClusterNodePuzzle confirmQuitNodePuzzle;
	public ClusterNodePuzzle cancelQuitNodePuzzle;
	
	// Update is called once per frame
    void Update()
    {    


			switch(menuState)
			{

	//Main Menu/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				case MenuState.PauseMenu:

				if(Globals.Instance.gameState == Globals.GameState.Paused)
			{

				ToggleFadeInputSelectMenu();
				ToggleFadeExitGameConfirm();
				ToggleFadeOptionsMenu();
				
				if(!mainMenu.activeInHierarchy)
						mainMenu.SetActive(true);

					if(fMainMenu.f != 1 && fInputSelect.f == 0 && fQuitGame.f == 0)
						fMainMenu.FadeIn();
				if (resumeGameNodePuzzle != null && resumeGameNodePuzzle.solved)
				{
					resumeGameNodePuzzle.solved = false;
					Globals.Instance.gameState = Globals.GameState.Unpausing;
				}
				//Puzzle to switch to Input Select
				if(optionsNodePuzzle != null && optionsNodePuzzle.solved)
				{
					optionsNodePuzzle.solved = false;
					menuState = MenuState.Options;
				}

				//Puzzle to switch to Exit Game Confirm
				if(exitGameNodePuzzle != null && exitGameNodePuzzle.solved)
				{
					exitGameNodePuzzle.solved = false;
					menuState = MenuState.QuitGame;
				}
			}
			else
			{
				ToggleFadeMainMenu();
				ToggleFadeInputSelectMenu();
				ToggleFadeExitGameConfirm();
				ToggleFadeOptionsMenu();
			}
					break;
	//Options   /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			case MenuState.Options:
				
				ToggleFadeMainMenu();
				ToggleFadeExitGameConfirm();
				ToggleFadeInputSelectMenu();
				if(!options.activeInHierarchy)
					options.SetActive(true);						
				if(fOptions.f != 1 && fMainMenu.f == 0 && fQuitGame.f == 0 && fInputSelect.f == 0)
					fOptions.FadeIn();
				
				if(fOptions.f == 1)
				{
					if(!options.GetComponent<OptionsMenu>().soundChecked)
						options.GetComponent<OptionsMenu>().CheckSoundSettings();

					if(optionsInputSelectNodePuzzle != null && optionsInputSelectNodePuzzle.solved)
					{
						options.GetComponent<OptionsMenu>().soundChecked = false;
						optionsInputSelectNodePuzzle.solved = false;
						menuState = MenuState.InputSelect;
					}

					if(optionsBackNodePuzzle != null && optionsBackNodePuzzle.solved)
					{
						options.GetComponent<OptionsMenu>().soundChecked = false;
						optionsBackNodePuzzle.solved = false;
						menuState = MenuState.PauseMenu;
					}
				}
				
				
				break;
	//Input Select/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				case MenuState.InputSelect:

					ToggleFadeMainMenu();
					ToggleFadeOptionsMenu();
					ToggleFadeExitGameConfirm();
					if(!inputSelect.activeInHierarchy)
						inputSelect.SetActive(true);						
					if(fInputSelect.f != 1 && fMainMenu.f == 0 && fQuitGame.f == 0)
						fInputSelect.FadeIn();
					
				if(fInputSelect.f == 1)
				{
					if(!inputFill.allowFill)
						inputFill.allowFill = true;
					//keyboardInputFollowing.setColor = true;

					if(inputSelectBackNodePuzzle != null && inputSelectBackNodePuzzle.solved)
					{
						//keyboardInputFollowing.setColor = false;
						inputSelectBackNodePuzzle.solved = false;
						inputFill.allowFill = false;
						menuState = MenuState.Options;
					}
				}

					
					break;
	//Quit Game Confirm////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				case MenuState.QuitGame:


				ToggleFadeMainMenu();
				ToggleFadeOptionsMenu();
				ToggleFadeInputSelectMenu();
				if(!exitGameConfirm.activeInHierarchy)
					exitGameConfirm.SetActive(true);
				if(fQuitGame.f != 1 && fMainMenu.f == 0 && fInputSelect.f == 0)
					fQuitGame.FadeIn();

				if(confirmQuitNodePuzzle != null && confirmQuitNodePuzzle.solved)
				{
					confirmQuitNodePuzzle.solved = false;
				Globals.Instance.ResetOrExit();
				}
				if(cancelQuitNodePuzzle != null && cancelQuitNodePuzzle.solved)
				{
					cancelQuitNodePuzzle.solved = false;
					menuState = MenuState.PauseMenu;
				}
				
					break;

			}

        

    }	

	private void ToggleFadeMainMenu ()
	{
		if(fMainMenu.f == 0)
		{
			if(mainMenu.activeInHierarchy)
				mainMenu.SetActive(false);
		}
		else
		{
			fMainMenu.FadeOut();
		}
	}
	private void ToggleFadeOptionsMenu()
	{
		if(fOptions.f == 0)
		{
			if(options.activeInHierarchy)
				options.SetActive(false);
		}
		else
		{
			fOptions.FadeOut();
		}
	}

	private void ToggleFadeInputSelectMenu()
	{
		if(fInputSelect.f == 0)
		{
			if(inputSelect.activeInHierarchy)
				inputSelect.SetActive(false);
		}
		else
		{
			fInputSelect.FadeOut();
		}
	}
	private void ToggleFadeExitGameConfirm()
	{
		if(fQuitGame.f == 0)
		{
			if(exitGameConfirm.activeInHierarchy)
				exitGameConfirm.SetActive(false);
		}
		else
		{
			fQuitGame.FadeOut();
		}
	}   

}

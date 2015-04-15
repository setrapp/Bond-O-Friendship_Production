using UnityEngine;
using System.Collections;
using InControl;

public class InputSelect : MonoBehaviour {

   // public enum InputName { LeftStickLeftController, RightStickLeftController, LeftStickRightController, RightStickRightController, LeftSideKeyboard, RightSideKeyboard }

   // public InputName inputName;

   // public Globals.InputNameSelected inputNameSelected;

    public float player1Timer = 4f;
    public float player2Timer = 4f;

   // public Globals.ControlScheme player1ControlScheme;
    //public Globals.ControlScheme player2ControlScheme;

   // public GameObject player1Fill;
    //public GameObject player2Fill;
    public InputFill inputFill;
    

    [SerializeField]
    public ControlsAndInput inputNameAndControlScheme;

	// Use this for initialization
	void Start () 
    {


        /*if (inputName == InputName.LeftStickLeftController || inputName == InputName.LeftStickRightController)
            inputNameSelected = Globals.InputNameSelected.LeftController;

        if (inputName == InputName.LeftStickRightController || inputName == InputName.RightStickRightController)
            inputNameSelected = Globals.InputNameSelected.RightController;

        if (inputName == InputName.LeftSideKeyboard || inputName == InputName.RightSideKeyboard)
            inputNameSelected = Globals.InputNameSelected.Keyboard;

        if (inputNameSelected == Globals.Instance.player1InputNameSelected)
            deviceIndex = Globals.Instance.player1Device;

        if (inputNameSelected == Globals.Instance.player2InputNameSelected)
            deviceIndex = Globals.Instance.player2Device;*/
	}
	
	// Update is called once per frame
	void Update () 
    {
        /*if (inputNameSelected == Globals.Instance.player1InputNameSelected)
            deviceIndex = Globals.Instance.player1Device;
        else if (inputNameSelected == Globals.Instance.player2InputNameSelected)
            deviceIndex = Globals.Instance.player2Device;
        else
            deviceIndex = -2;*/

        //CheckPlayerControl();

        if (player1Timer < 1f)
        {
            //ChangePlayerOneInput();
            CancelInvoke("PlayerOneTimer");
            //inputFill.player1FutureControls.controlScheme = Globals.ControlScheme.None;
            //inputFill.player1FutureControls.inputNameSelected = Globals.InputNameSelected.None;
            //inputFill.player1FutureControls = new ControlsAndInput { controlScheme = Globals.ControlScheme.None, inputNameSelected = Globals.InputNameSelected.None };
            player1Timer = 4f;
        }

        if (player2Timer < 1f)
        {
            //ChangePlayerTwoInput();
            CancelInvoke("PlayerTwoTimer");
            //inputFill.player2FutureControls.controlScheme = Globals.ControlScheme.None;
            //inputFill.player2FutureControls.inputNameSelected = Globals.InputNameSelected.None;
            player2Timer = 4f;
        }
	

        if(inputFill != null)
        {
            if(Input.GetKeyDown(KeyCode.Z))
            {
                inputFill.LL.SetActive(false);
            }
        }
	}

    void OnTriggerEnter(Collider collide)
    {
        if (collide.gameObject.name == "Player 1")
        {
            Debug.Log("Enter");
            inputFill.player1FutureControls.controlScheme = inputNameAndControlScheme.controlScheme;
            inputFill.player1FutureControls.inputNameSelected = inputNameAndControlScheme.inputNameSelected;
            InvokeRepeating("PlayerOneTimer", 1f, 1f);
        }
        if (collide.gameObject.name == "Player 2")
        {
            Debug.Log("Enter");
            inputFill.player2FutureControls.controlScheme = inputNameAndControlScheme.controlScheme;
            inputFill.player2FutureControls.inputNameSelected = inputNameAndControlScheme.inputNameSelected;
            InvokeRepeating("PlayerTwoTimer", 1f, 1f);
        }
    }

    void OnTriggerExit(Collider collide)
    {
        if (collide.gameObject.name == "Player 1")
        {
            inputFill.player1FutureControls.controlScheme = Globals.ControlScheme.None;
            inputFill.player1FutureControls.inputNameSelected = Globals.InputNameSelected.None;
            CancelInvoke("PlayerOneTimer");
            player1Timer = 4f;
        }
        if (collide.gameObject.name == "Player 2")
        {
            inputFill.player2FutureControls.controlScheme = Globals.ControlScheme.None;
            inputFill.player2FutureControls.inputNameSelected = Globals.InputNameSelected.None;
            CancelInvoke("PlayerTwoTimer");
            player2Timer = 4f;
        }

    }

    void ChangePlayerOneInput()
    {
       // Globals.Instance.player1Controls.inputNameSelected = inputNameAndControlScheme.inputNameSelected;
        //Globals.Instance.player1Controls.controlScheme = inputNameAndControlScheme.controlScheme;
        //Globals.Instance.player1Controls.controlScheme = Globals.Instance.CheckSoloInput(Globals.Instance.player1Controls, Globals.Instance.player2Controls);
        //Globals.Instance.player2Controls.controlScheme = Globals.Instance.CheckSharedInput(Globals.Instance.player2Controls, Globals.Instance.player1Controls);
        //Globals.Instance.player2Controls.controlScheme = Globals.Instance.CheckSoloInput(Globals.Instance.player2Controls, Globals.Instance.player1Controls);
       
    }

    void ChangePlayerTwoInput()
    {

        Globals.Instance.player2Controls.inputNameSelected = inputNameAndControlScheme.inputNameSelected;
        Globals.Instance.player2Controls.controlScheme = inputNameAndControlScheme.controlScheme;
        Globals.Instance.player2Controls.controlScheme = Globals.Instance.CheckSoloInput(Globals.Instance.player2Controls, Globals.Instance.player1Controls);
        Globals.Instance.player1Controls.controlScheme = Globals.Instance.CheckSharedInput(Globals.Instance.player1Controls, Globals.Instance.player2Controls);
        Globals.Instance.player1Controls.controlScheme = Globals.Instance.CheckSoloInput(Globals.Instance.player1Controls, Globals.Instance.player2Controls);
        
    }

    void PlayerOneTimer()
    {
        player1Timer--;
    }
    void PlayerTwoTimer()
    {
        player2Timer--;
    }

    /*void CheckPlayerControl()
    {
        switch(inputName)
        {
            case InputName.LeftStickLeftController:
                //Player 1
                if ((Globals.Instance.player1InputNameSelected == Globals.InputNameSelected.LeftController) && (Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSharedLeft || Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSolo))
                    player1Fill.SetActive(true);
                else
                    player1Fill.SetActive(false);
                //Player 2
                if ((Globals.Instance.player2InputNameSelected == Globals.InputNameSelected.LeftController) && (Globals.Instance.player2ControlScheme == Globals.ControlScheme.ControllerSharedLeft || Globals.Instance.player2ControlScheme == Globals.ControlScheme.ControllerSolo))
                    player2Fill.SetActive(true);
                else
                    player2Fill.SetActive(false);
                break;
            case InputName.RightStickLeftController:
                //Player 1
                if ((Globals.Instance.player1InputNameSelected == Globals.InputNameSelected.LeftController) && (Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSharedRight || Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSolo))
                    player1Fill.SetActive(true);
                else
                    player1Fill.SetActive(false);
                //Player 2
                if ((Globals.Instance.player2InputNameSelected == Globals.InputNameSelected.LeftController) && (Globals.Instance.player2ControlScheme == Globals.ControlScheme.ControllerSharedRight || Globals.Instance.player2ControlScheme == Globals.ControlScheme.ControllerSolo))
                    player2Fill.SetActive(true);
                else
                    player2Fill.SetActive(false);
                break;
            case InputName.LeftStickRightController:
                //Player 1
                if ((Globals.Instance.player1InputNameSelected == Globals.InputNameSelected.RightController) && (Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSharedLeft || Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSolo))
                    player1Fill.SetActive(true);
                else
                    player1Fill.SetActive(false);
                //Player 2
                if ((Globals.Instance.player2InputNameSelected == Globals.InputNameSelected.RightController) && (Globals.Instance.player2ControlScheme == Globals.ControlScheme.ControllerSharedLeft || Globals.Instance.player2ControlScheme == Globals.ControlScheme.ControllerSolo))
                    player2Fill.SetActive(true);
                else
                    player2Fill.SetActive(false);
                break;
            case InputName.RightStickRightController:
                //Player 1
                if ((Globals.Instance.player1InputNameSelected == Globals.InputNameSelected.RightController) && (Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSharedRight || Globals.Instance.player1ControlScheme == Globals.ControlScheme.ControllerSolo))
                    player1Fill.SetActive(true);
                else
                    player1Fill.SetActive(false);
                //Player 2
                if ((Globals.Instance.player2InputNameSelected == Globals.InputNameSelected.RightController) && (Globals.Instance.player2ControlScheme == Globals.ControlScheme.ControllerSharedRight || Globals.Instance.player2ControlScheme == Globals.ControlScheme.ControllerSolo))
                    player2Fill.SetActive(true);
                else
                    player2Fill.SetActive(false);
                break;
            case InputName.LeftSideKeyboard:
                //Player 1
                if ((Globals.Instance.player1InputNameSelected == Globals.InputNameSelected.Keyboard) && (Globals.Instance.player1ControlScheme == Globals.ControlScheme.KeyboardSharedLeft || Globals.Instance.player1ControlScheme == Globals.ControlScheme.KeyboardSolo))
                    player1Fill.SetActive(true);
                else
                    player1Fill.SetActive(false);
                //Player 2
                if ((Globals.Instance.player2InputNameSelected == Globals.InputNameSelected.Keyboard) && (Globals.Instance.player2ControlScheme == Globals.ControlScheme.KeyboardSharedLeft || Globals.Instance.player2ControlScheme == Globals.ControlScheme.KeyboardSolo))
                    player2Fill.SetActive(true);
                else
                    player2Fill.SetActive(false);
                break;
            case InputName.RightSideKeyboard:
                //Player 1
                if ((Globals.Instance.player1InputNameSelected == Globals.InputNameSelected.Keyboard) && (Globals.Instance.player1ControlScheme == Globals.ControlScheme.KeyboardSharedRight || Globals.Instance.player1ControlScheme == Globals.ControlScheme.KeyboardSolo))
                    player1Fill.SetActive(true);
                else
                    player1Fill.SetActive(false);
                //Player 2
                if ((Globals.Instance.player2InputNameSelected == Globals.InputNameSelected.Keyboard) && (Globals.Instance.player2ControlScheme == Globals.ControlScheme.KeyboardSharedRight || Globals.Instance.player2ControlScheme == Globals.ControlScheme.KeyboardSolo))
                    player2Fill.SetActive(true);
                else
                    player2Fill.SetActive(false);
                break;


        }*/
  //  }


}

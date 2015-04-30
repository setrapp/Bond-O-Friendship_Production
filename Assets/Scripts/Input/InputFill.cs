using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputFill : MonoBehaviour {

    //Player1
    public GameObject LL;
    public GameObject LR;
    public GameObject RL;
    public GameObject RR;
    public GameObject LK;
    public GameObject RK;

    private float LLF;
    private float LRF;
    private float RLF;
    private float RRF;
    private float LKF;
    private float RKF;

    private float previousLLF;
    private float previousLRF;
    private float previousRLF;
    private float previousRRF;
    private float previousLKF;
    private float previousRKF;

    private bool LLFilling = false;
    private bool LRFilling = false;
    private bool RLFilling = false;
    private bool RRFilling = false;
    private bool LKFilling = false;
    private bool RKFilling = false;

    //Player 2
    public GameObject LL2;
    public GameObject LR2;
    public GameObject RL2;
    public GameObject RR2;
    public GameObject LK2;
    public GameObject RK2;

    private float LLF2;
    private float LRF2;
    private float RLF2;
    private float RRF2;
    private float LKF2;
    private float RKF2;

    private float previousLLF2;
    private float previousLRF2;
    private float previousRLF2;
    private float previousRRF2;
    private float previousLKF2;
    private float previousRKF2;

    private bool LLFilling2 = false;
    private bool LRFilling2 = false;
    private bool RLFilling2 = false;
    private bool RRFilling2 = false;
    private bool LKFilling2 = false;
    private bool RKFilling2 = false;

    public ControlsAndInput player1FutureControls = new ControlsAndInput { controlScheme = Globals.ControlScheme.None, inputNameSelected = Globals.InputNameSelected.None };
    public ControlsAndInput player2FutureControls = new ControlsAndInput { controlScheme = Globals.ControlScheme.None, inputNameSelected = Globals.InputNameSelected.None };

    private float duration = 2f;
    
    private Vector3 filledInLeft = new Vector3(2.8f, 3f, 1f);
    private Vector3 filledInRight = new Vector3(2.9f, 3f, 1f);
    private Vector3 filledKey = new Vector3(2.45f, 1.8f, 1f);
    private Vector3 emptyKey = new Vector3(0f, 1.8f, 1f);
    public Vector3 emptyController = new Vector3(0f, 3f, 1f);
    
	// Use this for initialization
	void Start () 
    {
        switch (Globals.Instance.player1Controls.inputNameSelected)
        {
            case Globals.InputNameSelected.LeftController:
                switch (Globals.Instance.player1Controls.controlScheme)
                {
                    case Globals.ControlScheme.SharedLeft:
                        LL.transform.localScale = filledInLeft;
                        LLF = 1;
                        break;
                    case Globals.ControlScheme.SharedRight:
                        LR.transform.localScale = filledInRight;
                        LRF = 1;
                        break;
                    case Globals.ControlScheme.Solo:
                        LL.transform.localScale = filledInLeft;
                        LLF = 1;
                        LR.transform.localScale = filledInRight;
                        LRF = 1;
                        break;
                }
                break;

            case Globals.InputNameSelected.RightController:
                switch (Globals.Instance.player1Controls.controlScheme)
                {
                    case Globals.ControlScheme.SharedLeft:
                        RL.transform.localScale = filledInLeft;
                        RLF = 1;
                        break;
                    case Globals.ControlScheme.SharedRight:
                        RR.transform.localScale = filledInRight;
                        RRF = 1;
                        break;
                    case Globals.ControlScheme.Solo:
                        RL.transform.localScale = filledInLeft;
                        RLF = 1;
                        RR.transform.localScale = filledInRight;
                        RRF = 1;
                        break;
                }
                break;

            case Globals.InputNameSelected.Keyboard:
                switch (Globals.Instance.player1Controls.controlScheme)
                {
                    case Globals.ControlScheme.SharedLeft:
                        LK.transform.localScale = filledKey;
                        LKF = 1;
                        break;
                    case Globals.ControlScheme.SharedRight:
                        RK.transform.localScale = filledKey;
                        RKF = 1;
                        break;
                    case Globals.ControlScheme.Solo:
                        LK.transform.localScale = filledKey;
                        LKF = 1;
                        RK.transform.localScale = filledKey;
                        RKF = 1;
                        break;
                }
                break;
        }

        //////////////////Player 2/////////////////////////////////
        switch (Globals.Instance.player2Controls.inputNameSelected)
        {
            case Globals.InputNameSelected.LeftController:
                switch (Globals.Instance.player2Controls.controlScheme)
                {
                    case Globals.ControlScheme.SharedLeft:
                        LL2.transform.localScale = filledInLeft;
                        LLF2 = 1;
                        break;
                    case Globals.ControlScheme.SharedRight:
                        LR2.transform.localScale = filledInRight;
                        LRF2 = 1;
                        break;
                    case Globals.ControlScheme.Solo:
                        LL2.transform.localScale = filledInLeft;
                        LLF2 = 1;
                        LR2.transform.localScale = filledInRight;
                        LRF2 = 1;
                        break;
                }
                break;

            case Globals.InputNameSelected.RightController:
                switch (Globals.Instance.player2Controls.controlScheme)
                {
                    case Globals.ControlScheme.SharedLeft:
                        RL2.transform.localScale = filledInLeft;
                        RLF2 = 1;
                        break;
                    case Globals.ControlScheme.SharedRight:
                        RR2.transform.localScale = filledInRight;
                        RRF2 = 1;
                        break;
                    case Globals.ControlScheme.Solo:
                        RL2.transform.localScale = filledInLeft;
                        RLF2 = 1;
                        RR2.transform.localScale = filledInRight;
                        RRF2 = 1;
                        break;
                }
                break;

            case Globals.InputNameSelected.Keyboard:
                switch (Globals.Instance.player2Controls.controlScheme)
                {
                    case Globals.ControlScheme.SharedLeft:
                        LK2.transform.localScale = filledKey;
                        LKF2 = 1;
                        break;
                    case Globals.ControlScheme.SharedRight:
                        RK2.transform.localScale = filledKey;
                        RKF2 = 1;
                        break;
                    case Globals.ControlScheme.Solo:
                        LK2.transform.localScale = filledKey;
                        LKF2 = 1;
                        RK2.transform.localScale = filledKey;
                        RKF2 = 1;
                        break;
                }
                break;
        }
       
        //Debug.Log(LCLTP2.GetComponentInChildren<Renderer>().material.color);
	}
	
	// Update is called once per frame
	void Update () 
    {
        PlayerInputFill(SetControlsToUse(true), SetControlsToUse(false));
	}

    private ControlsAndInput SetControlsToUse(bool isPlayer1)
    {
        ControlsAndInput controlsToUse = new ControlsAndInput();

        if (isPlayer1)
        {
            if (player1FutureControls.controlScheme != Globals.ControlScheme.None && player1FutureControls.inputNameSelected != Globals.InputNameSelected.None)
            {
                controlsToUse.controlScheme = player1FutureControls.controlScheme;
                controlsToUse.inputNameSelected = player1FutureControls.inputNameSelected;
            }
            else
            {
                controlsToUse.controlScheme = Globals.Instance.player1Controls.controlScheme;
                controlsToUse.inputNameSelected = Globals.Instance.player1Controls.inputNameSelected;
            }
        }
        else
        {
            if (player2FutureControls.controlScheme != Globals.ControlScheme.None && player2FutureControls.inputNameSelected != Globals.InputNameSelected.None)
            {
                controlsToUse.controlScheme = player2FutureControls.controlScheme;
                controlsToUse.inputNameSelected = player2FutureControls.inputNameSelected;
            }
            else
            {
                controlsToUse.controlScheme = Globals.Instance.player2Controls.controlScheme;
                controlsToUse.inputNameSelected = Globals.Instance.player2Controls.inputNameSelected;
            }
        }

        return controlsToUse;
    }


    void PlayerInputFill(ControlsAndInput player1Controls, ControlsAndInput player2Controls)
    {
        float t = Time.deltaTime / duration;

        # region Tweak Control Scheme

        player1Controls.controlScheme = player1Controls.inputNameSelected == player2Controls.inputNameSelected ? player1Controls.controlScheme : Globals.ControlScheme.Solo;
        player2Controls.controlScheme = player2Controls.inputNameSelected == player1Controls.inputNameSelected ? player2Controls.controlScheme : Globals.ControlScheme.Solo;

        if(player1Controls.inputNameSelected == player2Controls.inputNameSelected)
        {
            if (player1Controls.controlScheme == player2Controls.controlScheme)
            {
                //Check if swapping sides. If so, set control scheme to say so.
                if (player1Controls.controlScheme == Globals.Instance.player1Controls.controlScheme && player1Controls.inputNameSelected == Globals.Instance.player1Controls.inputNameSelected)
                {
                    if (player1Controls.controlScheme == Globals.ControlScheme.SharedLeft)
                        player1Controls.controlScheme = Globals.ControlScheme.SharedRight;
                    else
                        player1Controls.controlScheme = Globals.ControlScheme.SharedLeft;
                }
                if (player2Controls.controlScheme == Globals.Instance.player2Controls.controlScheme && player2Controls.inputNameSelected == Globals.Instance.player2Controls.inputNameSelected)
                {
                    if (player2Controls.controlScheme == Globals.ControlScheme.SharedLeft)
                        player2Controls.controlScheme = Globals.ControlScheme.SharedRight;
                    else
                        player2Controls.controlScheme = Globals.ControlScheme.SharedLeft;
                }
            }

            if(player1Controls.controlScheme == Globals.ControlScheme.Solo)
            {
                if (player2Controls.controlScheme == Globals.ControlScheme.SharedLeft)
                    player1Controls.controlScheme = Globals.ControlScheme.SharedRight;
                if(player2Controls.controlScheme == Globals.ControlScheme.SharedRight)
                    player1Controls.controlScheme = Globals.ControlScheme.SharedLeft;
            }
            if (player2Controls.controlScheme == Globals.ControlScheme.Solo)
            {
                if (player1Controls.controlScheme == Globals.ControlScheme.SharedLeft)
                    player2Controls.controlScheme = Globals.ControlScheme.SharedRight;
                if (player1Controls.controlScheme == Globals.ControlScheme.SharedRight)
                    player2Controls.controlScheme = Globals.ControlScheme.SharedLeft;
            }
        }

        #endregion

        CheckControlScheme(player1Controls, 1);        

        LLF += LLFilling ? t : -t;
        LRF += LRFilling ? t : -t;
        RLF += RLFilling ? t : -t;
        RRF += RRFilling ? t : -t;
        LKF += LKFilling ? t : -t;
        RKF += RKFilling ? t : -t;

        LLF = Mathf.Clamp(LLF, 0, 1f);
        LRF = Mathf.Clamp(LRF, 0, 1f);
        RLF = Mathf.Clamp(RLF, 0, 1f);
        RRF = Mathf.Clamp(RRF, 0, 1f);
        LKF = Mathf.Clamp(LKF, 0, 1f);
        RKF = Mathf.Clamp(RKF, 0, 1f);

        CheckControlScheme(player1Controls, 1);

        previousLLF = LLF;
        previousLRF = LRF;
        previousRLF = RLF;
        previousRRF = RRF;
        previousLKF = LKF;
        previousRKF = RKF;

        CheckControlScheme(player2Controls, 2);

        LLF2 += LLFilling2 ? t : -t;
        LRF2 += LRFilling2 ? t : -t;
        RLF2 += RLFilling2 ? t : -t;
        RRF2 += RRFilling2 ? t : -t;
        LKF2 += LKFilling2 ? t : -t;
        RKF2 += RKFilling2 ? t : -t;

        LLF2 = Mathf.Clamp(LLF2, 0, 1f);
        LRF2 = Mathf.Clamp(LRF2, 0, 1f);
        RLF2 = Mathf.Clamp(RLF2, 0, 1f);
        RRF2 = Mathf.Clamp(RRF2, 0, 1f);
        LKF2 = Mathf.Clamp(LKF2, 0, 1f);
        RKF2 = Mathf.Clamp(RKF2, 0, 1f);

        CheckControlScheme(player2Controls, 2);

        previousLLF2 = LLF2;
        previousLRF2 = LRF2;
        previousRLF2 = RLF2;
        previousRRF2 = RRF2;
        previousLKF2 = LKF2;
        previousRKF2 = RKF2;

        if(LL.activeInHierarchy)
            LL.transform.localScale = Vector3.Lerp(emptyController, filledInLeft, LLF);
        if(LR.activeInHierarchy)
            LR.transform.localScale = Vector3.Lerp(emptyController, filledInRight, LRF);
        if (RL.activeInHierarchy)
            RL.transform.localScale = Vector3.Lerp(emptyController, filledInLeft, RLF);
        if (RR.activeInHierarchy)
            RR.transform.localScale = Vector3.Lerp(emptyController, filledInRight, RRF);
        if (LK.activeInHierarchy)
            LK.transform.localScale = Vector3.Lerp(emptyKey, filledKey, LKF);
        if (RK.activeInHierarchy)
            RK.transform.localScale = Vector3.Lerp(emptyKey, filledKey, RKF);

        if (LL2.activeInHierarchy)
            LL2.transform.localScale = Vector3.Lerp(emptyController, filledInLeft, LLF2);
        if (LR2.activeInHierarchy)
            LR2.transform.localScale = Vector3.Lerp(emptyController, filledInRight, LRF2);
        if (RL2.activeInHierarchy)
            RL2.transform.localScale = Vector3.Lerp(emptyController, filledInLeft, RLF2);
        if (RR2.activeInHierarchy)
            RR2.transform.localScale = Vector3.Lerp(emptyController, filledInRight, RRF2);
        if (LK2.activeInHierarchy)
            LK2.transform.localScale = Vector3.Lerp(emptyKey, filledKey, LKF2);
        if (RK2.activeInHierarchy)
            RK2.transform.localScale = Vector3.Lerp(emptyKey, filledKey, RKF2);        

        SetValuesToFalse();

    }

    void ChangePlayerOneInput(ControlsAndInput controlsAndInput)
    {
        if (controlsAndInput.controlScheme == Globals.Instance.player1Controls.controlScheme && controlsAndInput.inputNameSelected == Globals.Instance.player1Controls.inputNameSelected)
            return;
        
        Globals.Instance.player1Controls.inputNameSelected = controlsAndInput.inputNameSelected;
        Globals.Instance.player1Controls.controlScheme = controlsAndInput.controlScheme;
        Globals.Instance.player1Controls.controlScheme = Globals.Instance.CheckSoloInput(Globals.Instance.player1Controls, Globals.Instance.player2Controls);
        Globals.Instance.player2Controls.controlScheme = Globals.Instance.CheckSharedInput(Globals.Instance.player2Controls, Globals.Instance.player1Controls);
        Globals.Instance.player2Controls.controlScheme = Globals.Instance.CheckSoloInput(Globals.Instance.player2Controls, Globals.Instance.player1Controls);

    }

    void ChangePlayerTwoInput(ControlsAndInput controlsAndInput)
    {
        if (controlsAndInput.controlScheme == Globals.Instance.player2Controls.controlScheme && controlsAndInput.inputNameSelected == Globals.Instance.player2Controls.inputNameSelected)
            return;

        Globals.Instance.player2Controls.inputNameSelected = controlsAndInput.inputNameSelected;
        Globals.Instance.player2Controls.controlScheme = controlsAndInput.controlScheme;
        Globals.Instance.player2Controls.controlScheme = Globals.Instance.CheckSoloInput(Globals.Instance.player2Controls, Globals.Instance.player1Controls);
        Globals.Instance.player1Controls.controlScheme = Globals.Instance.CheckSharedInput(Globals.Instance.player1Controls, Globals.Instance.player2Controls);
        Globals.Instance.player1Controls.controlScheme = Globals.Instance.CheckSoloInput(Globals.Instance.player1Controls, Globals.Instance.player2Controls);

    }

    void CheckControlScheme(ControlsAndInput controlsAndInput, int playerNumber, bool isDebug = false)
    {
        switch (controlsAndInput.inputNameSelected)
        {
            case Globals.InputNameSelected.LeftController:
                switch (controlsAndInput.controlScheme)
                {
                    case Globals.ControlScheme.SharedLeft:
                    //////////////////////////////////////////////////
                        if (playerNumber == 1)
                        {
                            LLFilling = true;
                            if(LLF == 1 && LLF != previousLLF)
                                ChangePlayerOneInput(controlsAndInput);
                        }
                        else if (playerNumber == 2)
                        {
                            LLFilling2 = true;
                            if (LLF2 == 1 && LLF2 != previousLLF2)
                                ChangePlayerTwoInput(controlsAndInput);
                        }
                        break;
                    //////////////////////////////////////////////////
                    case Globals.ControlScheme.SharedRight:
                    //////////////////////////////////////////////////
                        if (playerNumber == 1)
                        {
                            LRFilling = true;
                            if (LRF == 1 && LRF != previousLRF)
                                ChangePlayerOneInput(controlsAndInput);
                        }
                        else if (playerNumber == 2)
                        {
                            LRFilling2 = true;
                            if (LRF2 == 1 && LRF2 != previousLRF2)
                                ChangePlayerTwoInput(controlsAndInput);
                        }
                        break;
                    //////////////////////////////////////////////////
                    case Globals.ControlScheme.Solo:
                    //////////////////////////////////////////////////
                        if (playerNumber == 1)
                        {
                            LLFilling = true;
                            LRFilling = true;
                            if ((LLF == 1 && LLF != previousLLF) && (LRF == 1 && LRF != previousLRF))                                
                                ChangePlayerOneInput(controlsAndInput);                            
                        }
                        else if (playerNumber == 2)
                        {
                            LLFilling2 = true;
                            LRFilling2 = true;
                            if ((LLF2 == 1 && LLF2 != previousLLF2) && (LRF2 == 1 && LRF2 != previousLRF2))
                                ChangePlayerTwoInput(controlsAndInput);
                        }
                        break;
                    //////////////////////////////////////////////////
                }
                break;

            case Globals.InputNameSelected.RightController:
                switch (controlsAndInput.controlScheme)
                {
                    case Globals.ControlScheme.SharedLeft:
                    //////////////////////////////////////////////////
                        if (playerNumber == 1)
                        {
                            RLFilling = true;
                            if (RLF == 1 && RLF != previousRLF)
                                ChangePlayerOneInput(controlsAndInput);
                        }
                        else if (playerNumber == 2)
                        {
                            RLFilling2 = true;
                            if (RLF2 == 1 && RLF2 != previousRLF2)
                                ChangePlayerTwoInput(controlsAndInput);
                        }
                        break;
                    //////////////////////////////////////////////////
                    case Globals.ControlScheme.SharedRight:
                    //////////////////////////////////////////////////
                        if (playerNumber == 1)
                        {
                            RRFilling = true;
                            if (RRF == 1 && RRF != previousRRF)
                                ChangePlayerOneInput(controlsAndInput);
                        }
                        else if (playerNumber == 2)
                        {
                            RRFilling2 = true;
                            if (RRF2 == 1 && RRF2 != previousRRF2)
                                ChangePlayerTwoInput(controlsAndInput);
                        }
                        break;
                    //////////////////////////////////////////////////
                    case Globals.ControlScheme.Solo:
                    //////////////////////////////////////////////////
                        if (playerNumber == 1)
                        {
                            RLFilling = true;
                            RRFilling = true;
                            if ((RLF == 1 && RLF != previousRLF) && (RRF == 1 && RRF != previousRRF))
                                ChangePlayerOneInput(controlsAndInput);
                        }
                        else if (playerNumber == 2)
                        {
                            RLFilling2 = true;
                            RRFilling2 = true;
                            if ((RLF2 == 1 && RLF2 != previousRLF) && (RRF2 == 1 && RRF2 != previousRRF))
                                ChangePlayerTwoInput(controlsAndInput);
                        }                        
                        break;
                }
                break;

            case Globals.InputNameSelected.Keyboard:
                switch (controlsAndInput.controlScheme)
                {
                    case Globals.ControlScheme.SharedLeft:
                        if (playerNumber == 1)
                        {
                            LKFilling = true;
                            if (LKF == 1 && LKF != previousLKF)
                                ChangePlayerOneInput(controlsAndInput);
                        }
                        else if (playerNumber == 2)
                        {
                            LKFilling2 = true;
                            if (LKF2 == 1 && LKF2 != previousLKF2)
                                ChangePlayerTwoInput(controlsAndInput);
                        }
                        break;
                    case Globals.ControlScheme.SharedRight:
                        if (playerNumber == 1)
                        {
                            RKFilling = true;
                            if (RKF == 1 && RKF != previousRKF)
                                ChangePlayerOneInput(controlsAndInput);
                        }
                        else if (playerNumber == 2)
                        {
                            RKFilling2 = true;
                            if (RKF2 == 1 && RKF2 != previousRKF2)
                                ChangePlayerTwoInput(controlsAndInput);
                        }
                        break;
                    case Globals.ControlScheme.Solo:
                        if (playerNumber == 1)
                        {
                            LKFilling = true;
                            RKFilling = true;
                            if ((LKF == 1 && LKF != previousRLF) && (RKF == 1 && RKF != previousRKF))
                                ChangePlayerOneInput(controlsAndInput);
                        }
                        else if (playerNumber == 2)
                        {
                            LKFilling2 = true;
                            RKFilling2 = true;
                            if ((LKF2 == 1 && LKF2 != previousRLF2) && (RKF2 == 1 && RKF2 != previousRKF2))
                                ChangePlayerTwoInput(controlsAndInput);
                        }
                        break;
                }
                break;
        }


    }

    void SetValuesToFalse()
    {
        LLFilling = false;
        LRFilling = false;
        RLFilling = false;
        RRFilling = false;
        LKFilling = false;
        RKFilling = false;
        LLFilling2 = false;
        LRFilling2 = false;
        RLFilling2 = false;
        RRFilling2 = false;
        LKFilling2 = false;
        RKFilling2 = false;
    }

    
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputFill : MonoBehaviour 
{

    public PulseStats pulseStats;

#region Input Objects


    public GameObject leftControllerLeftThumbstick;
    public GameObject leftControllerRightThumbstick;
    public GameObject rightControllerLeftThumbstick;
    public GameObject rightControllerRightThumbstick;
    public GameObject keyboardLeftKeys;
    public GameObject keyboardRightKeys;

    private Vector3 thumbstickSizeDefault;
    private Vector3 thumbstickSizeActive;
    private Vector3 keySizeDefault;
    private Vector3 keySizeActive;

    private float lcLtF = 0.0f;
    private float lcRtF = 0.0f;
    private float rcLtF = 0.0f;
    private float rcRtF = 0.0f;
    private float kLkF = 0.0f;
    private float kRkF = 0.0f;

    private bool lcLtFilling = false;
    private bool lcRtFilling = false;
    private bool rcLtFilling = false;
    private bool rcRtFilling = false;
    private bool kLkFilling = false;
    private bool kRkFilling = false;

    private float scaleDuration = 0.5f;

    private bool p1UsingGlobalControls = true;
    private bool p2UsingGlobalControls = true;

    public bool player1ControlsChanged = false;
    public bool player2ControlsChanged = false;

    private bool lcLtPulsing = false;
    private bool lcRtPulsing = false;
    public bool rcLtPulsing = false;
    public bool rcRtPulsing = false;
    private bool kLkPulsing = false;
    private bool kRkPulsing = false;

    private bool lcLtPulsingUp = true;
    private bool lcRtPulsingUp = true;
    private bool rcLtPulsingUp = true;
    private bool rcRtPulsingUp = true;
    private bool kLkPulsingUp = true;
    private bool kRkPulsingUp = true;

#endregion

    private PlayerInput playerHolder;

    [HideInInspector]
    public PlayerInput player1;
    [HideInInspector]
    public PlayerInput player2;
    [HideInInspector]
    public Vector3 player1PosNoZ;
    [HideInInspector]
    public Vector3 player2PosNoZ;

# region Player1
    public GameObject LcLeftBaseFillP1;
    public GameObject LcLeftThumbstickFillP1;
    public GameObject LcRightBaseFillP1;
    public GameObject LcRightThumbstickFillP1;

    public GameObject RcLeftBaseFillP1;
    public GameObject RcLeftThumbstickFillP1;
    public GameObject RcRightBaseFillP1;
    public GameObject RcRightThumbstickFillP1;

    public GameObject LkUpFillP1;
    public GameObject LkLeftFillP1;
    public GameObject LkDownFillP1;
    public GameObject LkRightFillP1;

    public GameObject rkUpFillP1;
    public GameObject rkLeftFillP1;
    public GameObject rkDownFillP1;
    public GameObject rkRightFillP1;

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

#endregion

#region Player 2
    public GameObject LcLeftBaseFillP2;
    public GameObject LcLeftThumbstickFillP2;
    public GameObject LcRightBaseFillP2;
    public GameObject LcRightThumbstickFillP2;

    public GameObject RcLeftBaseFillP2;
    public GameObject RcLeftThumbstickFillP2;
    public GameObject RcRightBaseFillP2;
    public GameObject RcRightThumbstickFillP2;

    public GameObject LkUpFillP2;
    public GameObject LkLeftFillP2;
    public GameObject LkDownFillP2;
    public GameObject LkRightFillP2;

    public GameObject rkUpFillP2;
    public GameObject rkLeftFillP2;
    public GameObject rkDownFillP2;
    public GameObject rkRightFillP2;

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

#endregion


    [HideInInspector]
    public ControlsAndInput player1FutureControls = new ControlsAndInput { controlScheme = Globals.ControlScheme.None, inputNameSelected = Globals.InputNameSelected.None };
    [HideInInspector]
    public ControlsAndInput player2FutureControls = new ControlsAndInput { controlScheme = Globals.ControlScheme.None, inputNameSelected = Globals.InputNameSelected.None };

    private ControlsAndInput player1ControlsToUse = new ControlsAndInput { controlScheme = Globals.ControlScheme.None, inputNameSelected = Globals.InputNameSelected.None };
    private ControlsAndInput player2ControlsToUse = new ControlsAndInput { controlScheme = Globals.ControlScheme.None, inputNameSelected = Globals.InputNameSelected.None };

    public ControlsAndInput player1PreviousControls = new ControlsAndInput { controlScheme = Globals.ControlScheme.None, inputNameSelected = Globals.InputNameSelected.None };
    public ControlsAndInput player2PreviousControls = new ControlsAndInput { controlScheme = Globals.ControlScheme.None, inputNameSelected = Globals.InputNameSelected.None };

    private float duration = 1.0f;
    private bool firstFill = true;


    private Vector3 emptyFill = new Vector3(0.0f, 0.0f, 1.0f);
    private Vector3 fullFill = new Vector3(1.0f, 1.0f, 1.0f);

   

    public GameObject leftController;
    public GameObject rightController;
    public GameObject keyboard;

    public GameObject leftControllerSplitLine;
    public GameObject rightControllerSplitLine;
    public GameObject keyboardSplitLine;

    public bool allowFill = false;

	// Use this for initialization
	void Start () 
    {
        SetPlayers();
        SetInputSizes();
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (player1 == null || player2 == null)
        {
            SetPlayers();
        }
        else
        {
            if (allowFill)
            {
                GetPlayerPosNoZ();

                SetControlsToUse(true);
                SetControlsToUse(false);
                TweakControlScheme();

                if (player1ControlsChanged)
                    WaitForPlayerInput(player1PreviousControls, true);
                if (player2ControlsChanged)
                    WaitForPlayerInput(player2PreviousControls, false);

                if (!player1ControlsChanged && !player2ControlsChanged)
                    PlayerInputFill(player1ControlsToUse, player2ControlsToUse);
                else
                    ChangeInputSizes();

                if (firstFill)
                    CheckFirstFill();
            }
           
        }

	}

    void GetPlayerPosNoZ()
    {
        player1PosNoZ = new Vector3(player1.transform.position.x, player1.transform.position.y, 0.0f);
        player2PosNoZ = new Vector3(player2.transform.position.x, player2.transform.position.y, 0.0f);
    }

    //Make sure players are set
    void SetPlayers()
    {
        if (Globals.Instance.Player1 != null)
            player1 = Globals.Instance.Player1;
        if (Globals.Instance.Player2 != null)
            player2 = Globals.Instance.Player2;
    }

    void SetInputSizes()
    {
        thumbstickSizeDefault = leftControllerLeftThumbstick.transform.localScale;
        keySizeDefault = keyboardLeftKeys.transform.localScale;

        thumbstickSizeActive = thumbstickSizeDefault * 1.5f;
        keySizeActive = keySizeDefault * 1.2f;
    }

    void ChangeInputSizes()
    {
        float f = Time.deltaTime / scaleDuration;

        if (!p1UsingGlobalControls)
        {
            lcLtFilling = LLFilling;
            lcRtFilling = LRFilling;
            rcLtFilling = RLFilling;
            rcRtFilling = RRFilling;
            kLkFilling = LKFilling;
            kRkFilling = RKFilling;
        }
        if (!p2UsingGlobalControls)
        {
            lcLtFilling = LLFilling2;
            lcRtFilling = LRFilling2;
            rcLtFilling = RLFilling2;
            rcRtFilling = RRFilling2;
            kLkFilling = LKFilling2;
            kRkFilling = RKFilling2;
        }

        lcLtF += lcLtPulsing ? lcLtPulsingUp ? f : -f : lcLtFilling ? f : -f;
        lcRtF += lcRtPulsing ? lcRtPulsingUp ? f : -f : lcRtFilling ? f : -f;
        rcLtF += rcLtPulsing ? rcLtPulsingUp ? f : -f : rcLtFilling ? f : -f;
        rcRtF += rcRtPulsing ? rcRtPulsingUp ? f : -f : rcRtFilling ? f : -f;
        kLkF += kLkPulsing ? kLkPulsingUp ? f : -f : kLkFilling ? f : -f;
        kRkF += kRkPulsing ? kRkPulsingUp ? f : -f : kRkFilling ? f : -f;

        lcLtF = Mathf.Clamp(lcLtF, 0, 1f);
        lcRtF = Mathf.Clamp(lcRtF, 0, 1f);
        rcLtF = Mathf.Clamp(rcLtF, 0, 1f);
        rcRtF = Mathf.Clamp(rcRtF, 0, 1f);
        kLkF = Mathf.Clamp(kLkF, 0, 1f);
        kRkF = Mathf.Clamp(kRkF, 0, 1f);

        if (leftController.activeInHierarchy)
        {
            leftControllerLeftThumbstick.transform.localScale = Vector3.Lerp(thumbstickSizeDefault, thumbstickSizeActive, lcLtF);
            leftControllerRightThumbstick.transform.localScale = Vector3.Lerp(thumbstickSizeDefault, thumbstickSizeActive, lcRtF);
        }
        if (rightController.activeInHierarchy)
        {
            rightControllerLeftThumbstick.transform.localScale = Vector3.Lerp(thumbstickSizeDefault, thumbstickSizeActive, rcLtF);
            rightControllerRightThumbstick.transform.localScale = Vector3.Lerp(thumbstickSizeDefault, thumbstickSizeActive, rcRtF);
        }
        if (keyboard.activeInHierarchy)
        {
            keyboardLeftKeys.transform.localScale = Vector3.Lerp(keySizeDefault, keySizeActive, kLkF);
            keyboardRightKeys.transform.localScale = Vector3.Lerp(keySizeDefault, keySizeActive, kRkF);
        }

        if ((lcLtF == 1 && lcLtPulsingUp) || (lcLtF == 0 && !lcLtPulsingUp))
            lcLtPulsingUp = !lcLtPulsingUp;
        if ((lcRtF == 1 && lcRtPulsingUp) || (lcRtF == 0 && !lcRtPulsingUp))
            lcRtPulsingUp = !lcRtPulsingUp;

        if ((rcLtF == 1 && rcLtPulsingUp) || (rcLtF == 0 && !rcLtPulsingUp))
            rcLtPulsingUp = !rcLtPulsingUp;
        if ((rcRtF == 1 && rcRtPulsingUp) || (rcRtF == 0 && !rcRtPulsingUp))
            rcRtPulsingUp = !rcRtPulsingUp;

        if ((kLkF == 1 && kLkPulsingUp) || (kLkF == 0 && !kLkPulsingUp))
            kLkPulsingUp = !kLkPulsingUp;
        if ((kRkF == 1 && kRkPulsingUp) || (kRkF == 0 && !kRkPulsingUp))
            kRkPulsingUp = !kRkPulsingUp;

    }

    void CheckFirstFill()
    {
        if(LLF == 1.0 || LRF == 1.0 || RLF == 1.0 || RRF == 1.0 || LKF == 1.0f || RKF == 1.0f)
        {
            firstFill = false;
            CheckForInputChange(true);
            //Helper.FirePulse(Globals.Instance.Player1.transform.position, Globals.Instance.defaultPulseStats);
            //Helper.FirePulse(Globals.Instance.Player2.transform.position, Globals.Instance.defaultPulseStats);
            //Globals.Instance.allowInput = true;
			Globals.Instance.titleScreenFaded = true;
            duration = 2.0f;
        }
    }

    void WaitForPlayerInput(ControlsAndInput controlScheme, bool isPlayer1)
    {
        playerHolder = isPlayer1 ? player1 : player2;

        if (controlScheme.inputNameSelected == Globals.InputNameSelected.LeftController && Globals.Instance.leftControllerInputDevice != null)
        {
            if (controlScheme.controlScheme == Globals.ControlScheme.SharedLeft && playerHolder.PlayerControllerSharedMovement() != Vector3.zero)
            {
                lcLtPulsing = false;

                if (isPlayer1)
                    player1ControlsChanged = false;
                else
                    player2ControlsChanged = false;
            }

            if (controlScheme.controlScheme == Globals.ControlScheme.SharedRight && playerHolder.PlayerControllerSharedMovement() != Vector3.zero)
            {
                lcRtPulsing = false;

                if (isPlayer1)
                    player1ControlsChanged = false;
                else
                    player2ControlsChanged = false;
            }

            if (controlScheme.controlScheme == Globals.ControlScheme.Solo && playerHolder.PlayerControllerSoloMovement() != Vector3.zero)
            {
                lcLtPulsing = false;
                lcRtPulsing = false;

                if (isPlayer1)
                    player1ControlsChanged = false;
                else
                    player2ControlsChanged = false;
            }
        }
        else if (controlScheme.inputNameSelected == Globals.InputNameSelected.RightController && Globals.Instance.rightControllerInputDevice != null)
        {
            if (controlScheme.controlScheme == Globals.ControlScheme.SharedLeft && playerHolder.PlayerControllerSharedMovement() != Vector3.zero)
            {
                rcLtPulsing = false;

                if (isPlayer1)
                    player1ControlsChanged = false;
                else
                    player2ControlsChanged = false;
            }

            if (controlScheme.controlScheme == Globals.ControlScheme.SharedRight && playerHolder.PlayerControllerSharedMovement() != Vector3.zero)
            {
                rcRtPulsing = false;

                if (isPlayer1)
                    player1ControlsChanged = false;
                else
                    player2ControlsChanged = false;
            }

            if (controlScheme.controlScheme == Globals.ControlScheme.Solo && playerHolder.PlayerControllerSoloMovement() != Vector3.zero)
            {
                rcLtPulsing = false;
                rcRtPulsing = false;

                if (isPlayer1)
                    player1ControlsChanged = false;
                else
                    player2ControlsChanged = false;
            }
        }
        else if(controlScheme.inputNameSelected == Globals.InputNameSelected.Keyboard)
        {
            if (controlScheme.controlScheme == Globals.ControlScheme.SharedLeft && playerHolder.PlayerKeyboardSharedMovement() != Vector3.zero)
            {
                kLkPulsing = false;

                if (isPlayer1)
                    player1ControlsChanged = false;
                else
                    player2ControlsChanged = false;
            }

            if (controlScheme.controlScheme == Globals.ControlScheme.SharedRight && playerHolder.PlayerKeyboardSharedMovement() != Vector3.zero)
            {
                kRkPulsing = false;

                if (isPlayer1)
                    player1ControlsChanged = false;
                else
                    player2ControlsChanged = false;
            }

            if (controlScheme.controlScheme == Globals.ControlScheme.Solo && playerHolder.PlayerKeyboardSoloMovement() != Vector3.zero)
            {
                kLkPulsing = false;
                kRkPulsing = false;

                if (isPlayer1)
                    player1ControlsChanged = false;
                else
                    player2ControlsChanged = false;
            }
        }

    }


    private ControlsAndInput SetControlsToUse(bool isPlayer1)
    {
        //ControlsAndInput controlsToUse = new ControlsAndInput();

        if (isPlayer1)
        {

            player1PreviousControls.controlScheme = Globals.Instance.player1Controls.controlScheme;
            player1PreviousControls.inputNameSelected = Globals.Instance.player1Controls.inputNameSelected;

            if (player1FutureControls.controlScheme != Globals.ControlScheme.None && player1FutureControls.inputNameSelected != Globals.InputNameSelected.None)
            {

                player1ControlsToUse.controlScheme = player1FutureControls.controlScheme;
                player1ControlsToUse.inputNameSelected = player1FutureControls.inputNameSelected;
            }
            else
            {
                player1ControlsToUse.controlScheme = Globals.Instance.player1Controls.controlScheme;
                player1ControlsToUse.inputNameSelected = Globals.Instance.player1Controls.inputNameSelected;
            }

            return player1ControlsToUse;
        }
        else
        {
            player2PreviousControls.controlScheme = Globals.Instance.player2Controls.controlScheme;
            player2PreviousControls.inputNameSelected = Globals.Instance.player2Controls.inputNameSelected;

            if (player2FutureControls.controlScheme != Globals.ControlScheme.None && player2FutureControls.inputNameSelected != Globals.InputNameSelected.None)
            {
                player2ControlsToUse.controlScheme = player2FutureControls.controlScheme;
                player2ControlsToUse.inputNameSelected = player2FutureControls.inputNameSelected;
            }
            else
            {
                player2ControlsToUse.controlScheme = Globals.Instance.player2Controls.controlScheme;
                player2ControlsToUse.inputNameSelected = Globals.Instance.player2Controls.inputNameSelected;
            }

            return player2ControlsToUse;
        }
    }


    void TweakControlScheme()//ControlsAndInput player1Controls, ControlsAndInput player2Controls)
    {
        player1ControlsToUse.controlScheme = player1ControlsToUse.inputNameSelected == player2ControlsToUse.inputNameSelected ? player1ControlsToUse.controlScheme : Globals.ControlScheme.Solo;
        player2ControlsToUse.controlScheme = player2ControlsToUse.inputNameSelected == player1ControlsToUse.inputNameSelected ? player2ControlsToUse.controlScheme : Globals.ControlScheme.Solo;

        if (player1ControlsToUse.inputNameSelected == player2ControlsToUse.inputNameSelected)
        {
            if (player1ControlsToUse.controlScheme == player2ControlsToUse.controlScheme)
            {
                //Check if swapping sides. If so, set control scheme to say so.
                if (player1ControlsToUse.controlScheme == Globals.Instance.player1Controls.controlScheme && player1ControlsToUse.inputNameSelected == Globals.Instance.player1Controls.inputNameSelected)
                {
                    if (player1ControlsToUse.controlScheme == Globals.ControlScheme.SharedLeft)
                        player1ControlsToUse.controlScheme = Globals.ControlScheme.SharedRight;
                    else if (player1ControlsToUse.controlScheme == Globals.ControlScheme.SharedRight)
                        player1ControlsToUse.controlScheme = Globals.ControlScheme.SharedLeft;
                }
                if (player2ControlsToUse.controlScheme == Globals.Instance.player2Controls.controlScheme && player2ControlsToUse.inputNameSelected == Globals.Instance.player2Controls.inputNameSelected)
                {
                    if (player2ControlsToUse.controlScheme == Globals.ControlScheme.SharedLeft)
                        player2ControlsToUse.controlScheme = Globals.ControlScheme.SharedRight;
                    else if (player2ControlsToUse.controlScheme == Globals.ControlScheme.SharedRight)
                        player2ControlsToUse.controlScheme = Globals.ControlScheme.SharedLeft;
                }
            }

            if (player1ControlsToUse.controlScheme == Globals.ControlScheme.Solo)
            {
                if (player2ControlsToUse.controlScheme == Globals.ControlScheme.SharedLeft)
                    player1ControlsToUse.controlScheme = Globals.ControlScheme.SharedRight;
                if (player2ControlsToUse.controlScheme == Globals.ControlScheme.SharedRight)
                    player1ControlsToUse.controlScheme = Globals.ControlScheme.SharedLeft;
            }
            if (player2ControlsToUse.controlScheme == Globals.ControlScheme.Solo)
            {
                if (player1ControlsToUse.controlScheme == Globals.ControlScheme.SharedLeft)
                    player2ControlsToUse.controlScheme = Globals.ControlScheme.SharedRight;
                if (player1ControlsToUse.controlScheme == Globals.ControlScheme.SharedRight)
                    player2ControlsToUse.controlScheme = Globals.ControlScheme.SharedLeft;
            }
        }

        p1UsingGlobalControls = (player1ControlsToUse.controlScheme == Globals.Instance.player1Controls.controlScheme && player1ControlsToUse.inputNameSelected == Globals.Instance.player1Controls.inputNameSelected) || (player1FutureControls.controlScheme == Globals.ControlScheme.None && player1FutureControls.inputNameSelected == Globals.InputNameSelected.None);
        p2UsingGlobalControls = (player2ControlsToUse.controlScheme == Globals.Instance.player2Controls.controlScheme && player2ControlsToUse.inputNameSelected == Globals.Instance.player2Controls.inputNameSelected) || (player2FutureControls.controlScheme == Globals.ControlScheme.None && player2FutureControls.inputNameSelected == Globals.InputNameSelected.None);
    }


    void PlayerInputFill(ControlsAndInput player1Controls, ControlsAndInput player2Controls)
    {
        float t = Time.deltaTime / duration;

        # region Tweak Control Scheme



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




      


        if (leftController.activeInHierarchy)
        {
            LcLeftThumbstickFillP1.transform.localScale = Vector3.Lerp(emptyFill, fullFill, LLF);
            LcLeftBaseFillP1.transform.localScale = Vector3.Lerp(emptyFill, fullFill, LLF);
           
            LcRightThumbstickFillP1.transform.localScale = Vector3.Lerp(emptyFill, fullFill, LRF);
            LcRightBaseFillP1.transform.localScale = Vector3.Lerp(emptyFill, fullFill, LRF);


            LcLeftThumbstickFillP2.transform.localScale = Vector3.Lerp(emptyFill, fullFill, LLF2);
            LcLeftBaseFillP2.transform.localScale = Vector3.Lerp(emptyFill, fullFill, LLF2);

            LcRightThumbstickFillP2.transform.localScale = Vector3.Lerp(emptyFill, fullFill, LRF2);
            LcRightBaseFillP2.transform.localScale = Vector3.Lerp(emptyFill, fullFill, LRF2);
        }
        if (rightController.activeInHierarchy)
        {
            RcLeftThumbstickFillP1.transform.localScale = Vector3.Lerp(emptyFill, fullFill, RLF);
            RcLeftBaseFillP1.transform.localScale = Vector3.Lerp(emptyFill, fullFill, RLF);

            RcRightThumbstickFillP1.transform.localScale = Vector3.Lerp(emptyFill, fullFill, RRF);
            RcRightBaseFillP1.transform.localScale = Vector3.Lerp(emptyFill, fullFill, RRF);


            RcLeftThumbstickFillP2.transform.localScale = Vector3.Lerp(emptyFill, fullFill, RLF2);
            RcLeftBaseFillP2.transform.localScale = Vector3.Lerp(emptyFill, fullFill, RLF2);

            RcRightThumbstickFillP2.transform.localScale = Vector3.Lerp(emptyFill, fullFill, RRF2);
            RcRightBaseFillP2.transform.localScale = Vector3.Lerp(emptyFill, fullFill, RRF2);
        }

        if (keyboard.activeInHierarchy)
        {
            //Player 1 Keyboard Fills
            LkUpFillP1.transform.localScale = Vector3.Lerp(emptyFill, fullFill, LKF);
            LkLeftFillP1.transform.localScale = Vector3.Lerp(emptyFill, fullFill, LKF);
            LkDownFillP1.transform.localScale = Vector3.Lerp(emptyFill, fullFill, LKF);
            LkRightFillP1.transform.localScale = Vector3.Lerp(emptyFill, fullFill, LKF);

            rkUpFillP1.transform.localScale = Vector3.Lerp(emptyFill, fullFill, RKF);
            rkLeftFillP1.transform.localScale = Vector3.Lerp(emptyFill, fullFill, RKF);
            rkDownFillP1.transform.localScale = Vector3.Lerp(emptyFill, fullFill, RKF);
            rkRightFillP1.transform.localScale = Vector3.Lerp(emptyFill, fullFill, RKF);

            //Player 2 Keyboard Fills
            LkUpFillP2.transform.localScale = Vector3.Lerp(emptyFill, fullFill, LKF2);
            LkLeftFillP2.transform.localScale = Vector3.Lerp(emptyFill, fullFill, LKF2);
            LkDownFillP2.transform.localScale = Vector3.Lerp(emptyFill, fullFill, LKF2);
            LkRightFillP2.transform.localScale = Vector3.Lerp(emptyFill, fullFill, LKF2);

            rkUpFillP2.transform.localScale = Vector3.Lerp(emptyFill, fullFill, RKF2);
            rkLeftFillP2.transform.localScale = Vector3.Lerp(emptyFill, fullFill, RKF2);
            rkDownFillP2.transform.localScale = Vector3.Lerp(emptyFill, fullFill, RKF2);
            rkRightFillP2.transform.localScale = Vector3.Lerp(emptyFill, fullFill, RKF2);
        }

        CheckForInputChange();
        ChangeInputSizes();

        CheckSplitLines();
        SetValuesToFalse();

    }

    void CheckSplitLines()
    {
        if (Globals.Instance.player1Controls.controlScheme != Globals.ControlScheme.Solo || Globals.Instance.player2Controls.controlScheme != Globals.ControlScheme.Solo)
        {
            if (Globals.Instance.player1Controls.inputNameSelected == Globals.InputNameSelected.LeftController || Globals.Instance.player2Controls.inputNameSelected == Globals.InputNameSelected.LeftController)
                leftControllerSplitLine.transform.localScale = fullFill;
            else
                leftControllerSplitLine.transform.localScale = emptyFill;

            if (Globals.Instance.player1Controls.inputNameSelected == Globals.InputNameSelected.RightController || Globals.Instance.player2Controls.inputNameSelected == Globals.InputNameSelected.RightController)
                rightControllerSplitLine.transform.localScale = fullFill;
            else
                rightControllerSplitLine.transform.localScale = emptyFill;

            if (Globals.Instance.player1Controls.inputNameSelected == Globals.InputNameSelected.Keyboard || Globals.Instance.player2Controls.inputNameSelected == Globals.InputNameSelected.Keyboard)
                keyboardSplitLine.transform.localScale = fullFill;
            else
                keyboardSplitLine.transform.localScale = emptyFill;
        }
        else
        {
            leftControllerSplitLine.transform.localScale = emptyFill;
            rightControllerSplitLine.transform.localScale = emptyFill;
            keyboardSplitLine.transform.localScale = emptyFill;
        }
            


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

    void CheckForInputChange(bool forceChange = false)
    {
        if (player1PreviousControls.controlScheme != Globals.Instance.player1Controls.controlScheme || player1PreviousControls.inputNameSelected != Globals.Instance.player1Controls.inputNameSelected || forceChange && !player1ControlsChanged)
        {
            player1ControlsChanged = true;
            switch (Globals.Instance.player1Controls.inputNameSelected)
            {
                case Globals.InputNameSelected.LeftController:
                    switch (Globals.Instance.player1Controls.controlScheme)
                    {
                        case Globals.ControlScheme.SharedLeft:
                            lcLtPulsing = true;
                            Helper.FirePulse(leftControllerLeftThumbstick.transform.position, pulseStats);
                            break;
                        case Globals.ControlScheme.SharedRight:
                            lcRtPulsing = true;
                            Helper.FirePulse(leftControllerRightThumbstick.transform.position, pulseStats);
                            break;
                        case Globals.ControlScheme.Solo:
                            lcLtPulsing = true;
                            lcRtPulsing = true;
                            Helper.FirePulse(leftControllerLeftThumbstick.transform.position, pulseStats);
                            Helper.FirePulse(leftControllerRightThumbstick.transform.position, pulseStats);
                            break;
                    }
                    break;
                case Globals.InputNameSelected.RightController:
                    switch (Globals.Instance.player1Controls.controlScheme)
                    {
                        case Globals.ControlScheme.SharedLeft:
                            rcLtPulsing = true;
                            Helper.FirePulse(rightControllerLeftThumbstick.transform.position, pulseStats);                            
                            break;
                        case Globals.ControlScheme.SharedRight:
                            rcRtPulsing = true;
                            Helper.FirePulse(rightControllerRightThumbstick.transform.position, pulseStats);
                            break;
                        case Globals.ControlScheme.Solo:
                            rcLtPulsing = true;
                            rcRtPulsing = true;
                            Helper.FirePulse(rightControllerLeftThumbstick.transform.position, pulseStats);
                            Helper.FirePulse(rightControllerRightThumbstick.transform.position, pulseStats);
                            break;
                    }
                    break;
                case Globals.InputNameSelected.Keyboard:
                    switch (Globals.Instance.player1Controls.controlScheme)
                    {
                        case Globals.ControlScheme.SharedLeft:
                            kLkPulsing = true;
                            Helper.FirePulse(keyboardLeftKeys.transform.position, pulseStats);
                            break;
                        case Globals.ControlScheme.SharedRight:
                            kRkPulsing = true;
                            Helper.FirePulse(keyboardRightKeys.transform.position, pulseStats);
                            break;
                        case Globals.ControlScheme.Solo:
                            kLkPulsing = true;
                            kRkPulsing = true;

                            Helper.FirePulse(keyboardLeftKeys.transform.position, pulseStats);
                            Helper.FirePulse(keyboardRightKeys.transform.position, pulseStats);
                            break;
                    }
                    break;
            }
        }




        if (player2PreviousControls.controlScheme != Globals.Instance.player2Controls.controlScheme || player2PreviousControls.inputNameSelected != Globals.Instance.player2Controls.inputNameSelected || forceChange && !player2ControlsChanged)
        {
            player2ControlsChanged = true;
            switch (Globals.Instance.player2Controls.inputNameSelected)
            {
                case Globals.InputNameSelected.LeftController:
                    switch (Globals.Instance.player2Controls.controlScheme)
                    {
                        case Globals.ControlScheme.SharedLeft:
                            lcLtPulsing = true;
                            Helper.FirePulse(leftControllerLeftThumbstick.transform.position, pulseStats);
                            break;
                        case Globals.ControlScheme.SharedRight:
                            lcRtPulsing = true;
                            Helper.FirePulse(leftControllerRightThumbstick.transform.position, pulseStats);
                            break;
                        case Globals.ControlScheme.Solo:
                            lcLtPulsing = true;
                            lcRtPulsing = true;
                            Helper.FirePulse(leftControllerLeftThumbstick.transform.position, pulseStats);
                            Helper.FirePulse(leftControllerRightThumbstick.transform.position, pulseStats);
                            break;
                    }
                    break;
                case Globals.InputNameSelected.RightController:
                    switch (Globals.Instance.player2Controls.controlScheme)
                    {
                        case Globals.ControlScheme.SharedLeft:
                            rcLtPulsing = true;
                            Helper.FirePulse(rightControllerLeftThumbstick.transform.position, pulseStats);
                            break;
                        case Globals.ControlScheme.SharedRight:
                            rcRtPulsing = true;
                            Helper.FirePulse(rightControllerRightThumbstick.transform.position, pulseStats);
                            break;
                        case Globals.ControlScheme.Solo:
                            rcLtPulsing = true;
                            rcRtPulsing = true;
                            Helper.FirePulse(rightControllerLeftThumbstick.transform.position, pulseStats);
                            Helper.FirePulse(rightControllerRightThumbstick.transform.position, pulseStats);
                            break;
                    }
                    break;
                case Globals.InputNameSelected.Keyboard:
                    switch (Globals.Instance.player2Controls.controlScheme)
                    {
                        case Globals.ControlScheme.SharedLeft:
                            kLkPulsing = true;
                            Helper.FirePulse(keyboardLeftKeys.transform.position, pulseStats);
                            break;
                        case Globals.ControlScheme.SharedRight:
                            kRkPulsing = true;
                            Helper.FirePulse(keyboardRightKeys.transform.position, pulseStats);
                            break;
                        case Globals.ControlScheme.Solo:
                            kLkPulsing = true;
                            kRkPulsing = true;

                            Helper.FirePulse(keyboardLeftKeys.transform.position, pulseStats);
                            Helper.FirePulse(keyboardRightKeys.transform.position, pulseStats);
                            break;
                    }
                    break;
            }
        }
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
                            if (LLF == 1 && LLF != previousLLF)
                            {
                                ChangePlayerOneInput(controlsAndInput);
                            }
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

        lcLtFilling = false;
        lcRtFilling = false;
        rcLtFilling = false;
        rcRtFilling = false;
        kLkFilling = false;
        kRkFilling = false;
    }

    
}

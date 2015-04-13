using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputFill : MonoBehaviour {


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

    private bool LLFilling = false;
    private bool LRFilling = false;
    private bool RLFilling = false;
    private bool RRFilling = false;
    private bool LKFilling = false;
    private bool RKFilling = false;

    private Color LLColor = new Color(70f / 255f, 130f / 255f, 192f / 255f);
    private Color LRColor = new Color(70f / 255f, 130f / 255f, 192f / 255f);
    private Color RLColor = new Color(70f / 255f, 130f / 255f, 192f / 255f);
    private Color RRColor = new Color(70f / 255f, 130f / 255f, 192f / 255f);
    private Color LKColor = new Color(70f / 255f, 130f / 255f, 192f / 255f);
    private Color RKColor = new Color(70f / 255f, 130f / 255f, 192f / 255f);


    private ControlsAndInput player1ControlsToUse = new ControlsAndInput { controlScheme = Globals.ControlScheme.None, inputNameSelected = Globals.InputNameSelected.None };
    ControlsAndInput player1CurrentControls = new ControlsAndInput { controlScheme = Globals.ControlScheme.None, inputNameSelected = Globals.InputNameSelected.None };
    public ControlsAndInput player1FutureControls = new ControlsAndInput { controlScheme = Globals.ControlScheme.None, inputNameSelected = Globals.InputNameSelected.None };

    private ControlsAndInput player2ControlsToUse = new ControlsAndInput { controlScheme = Globals.ControlScheme.None, inputNameSelected = Globals.InputNameSelected.None };
    ControlsAndInput player2CurrentControls = new ControlsAndInput { controlScheme = Globals.ControlScheme.None, inputNameSelected = Globals.InputNameSelected.None };
    public ControlsAndInput player2FutureControls = new ControlsAndInput { controlScheme = Globals.ControlScheme.None, inputNameSelected = Globals.InputNameSelected.None };



    private float duration = 5f;
    private float f = 0;
    private float e = 1;
    //private float 



    
    private Vector3 filledInController = new Vector3(3f, 3f, 1f);
    private Vector3 emptyController = new Vector3(0f, 3f, 1f);

    private Color player1Color = new Color(70f/255f, 130f/255f, 192f/255f);
    private Color player2Color = new Color(225f/255f, 123f/255f, 63f/255f);
    
	// Use this for initialization
	void Start () 
    {
       /* //Player 1
        switch(Globals.Instance.player1Controls.inputNameSelected)
        {
            case Globals.InputNameSelected.LeftController:
                switch(Globals.Instance.player1Controls.controlScheme)
                {
                    case Globals.ControlScheme.ControllerSharedLeft:
                        LCLTP1.transform.localScale = new Vector3(2.8f, 3f, 1f);
                        break;
                    case Globals.ControlScheme.ControllerSharedRight:
                        LCRTP1.transform.localScale = new Vector3(2.9f, 3f, 1f);
                        break;
                    case Globals.ControlScheme.ControllerSolo:
                        LCLTP1.transform.localScale = new Vector3(2.8f, 3f, 1f);
                        LCRTP1.transform.localScale = new Vector3(2.9f, 3f, 1f);
                        break;
                }

                break;

            case Globals.InputNameSelected.RightController:
                switch (Globals.Instance.player1Controls.controlScheme)
                {
                    case Globals.ControlScheme.ControllerSharedLeft:
                        RCLTP1.transform.localScale = new Vector3(2.8f, 3f, 1f);
                        break;
                    case Globals.ControlScheme.ControllerSharedRight:
                        RCRTP1.transform.localScale = new Vector3(2.9f, 3f, 1f);
                        break;
                    case Globals.ControlScheme.ControllerSolo:
                        RCLTP1.transform.localScale = new Vector3(2.8f, 3f, 1f);
                        RCRTP1.transform.localScale = new Vector3(2.9f, 3f, 1f);
                        break;
                }

                break;
        }
        //Player 2
        switch (Globals.Instance.player2Controls.inputNameSelected)
        {
            case Globals.InputNameSelected.LeftController:
                switch (Globals.Instance.player2Controls.controlScheme)
                {
                    case Globals.ControlScheme.ControllerSharedLeft:
                        LCLTP2.transform.localScale = new Vector3(2.8f, 3f, 1f);
                        break;
                    case Globals.ControlScheme.ControllerSharedRight:
                        LCRTP2.transform.localScale = new Vector3(2.9f, 3f, 1f);
                        break;
                    case Globals.ControlScheme.ControllerSolo:
                        LCLTP2.transform.localScale = new Vector3(2.8f, 3f, 1f);
                        LCRTP2.transform.localScale = new Vector3(2.9f, 3f, 1f);
                        break;
                }

                break;

            case Globals.InputNameSelected.RightController:
                switch (Globals.Instance.player2Controls.controlScheme)
                {
                    case Globals.ControlScheme.ControllerSharedLeft:
                        RCLTP2.transform.localScale = new Vector3(2.8f, 3f, 1f);
                        break;
                    case Globals.ControlScheme.ControllerSharedRight:
                        RCRTP2.transform.localScale = new Vector3(2.9f, 3f, 1f);inputnameandcontrolscheme
                        break;
                    case Globals.ControlScheme.ControllerSolo:
                        RCLTP2.transform.localScale = new Vector3(2.8f, 3f, 1f);
                        RCRTP2.transform.localScale = new Vector3(2.9f, 3f, 1f);
                        break;
                }

                break;
        }
        * */
       
        //Debug.Log(LCLTP2.GetComponentInChildren<Renderer>().material.color);
	}
	
	// Update is called once per frame
	void Update () 
    {
        GetCurrentPlayerControls();
        SetControlsToUse();

        Debug.Log(player1ControlsToUse.inputNameSelected);
        Player1InputFill(player1ControlsToUse, player2ControlsToUse);


        
        
        //Debug.Log(player1FutureControls.controlScheme);
	}

    private void GetCurrentPlayerControls()
    {
        player1CurrentControls.controlScheme = Globals.Instance.player1Controls.controlScheme;
        player1CurrentControls.inputNameSelected = Globals.Instance.player1Controls.inputNameSelected;

        player2CurrentControls.controlScheme = Globals.Instance.player2Controls.controlScheme;
        player2CurrentControls.inputNameSelected = Globals.Instance.player2Controls.inputNameSelected;
    }

    private void SetControlsToUse()
    {
        if (player1FutureControls.controlScheme != Globals.ControlScheme.None && player1FutureControls.inputNameSelected != Globals.InputNameSelected.None)
        {
            player1ControlsToUse.controlScheme = player1FutureControls.controlScheme;
            player1ControlsToUse.inputNameSelected = player1FutureControls.inputNameSelected;
        }
        else
        {
            player1ControlsToUse.controlScheme = player1CurrentControls.controlScheme;
            player1ControlsToUse.inputNameSelected = player1CurrentControls.inputNameSelected;
        }

        if (player2FutureControls.controlScheme != Globals.ControlScheme.None && player2FutureControls.inputNameSelected != Globals.InputNameSelected.None)
        {
            player2ControlsToUse.controlScheme = player2FutureControls.controlScheme;
            player2ControlsToUse.inputNameSelected = player2FutureControls.inputNameSelected;
        }
        else
        {
            player2ControlsToUse.controlScheme = player2CurrentControls.controlScheme;
            player2ControlsToUse.inputNameSelected = player2CurrentControls.inputNameSelected;
        }
    }


  /*  void Player1InputFill(ControlsAndInput controlsAndInput)
    {
        controlsAndInput.controlScheme = controlsAndInput.inputNameSelected == Globals.Instance.player2Controls.inputNameSelected ? controlsAndInput.controlScheme : Globals.ControlScheme.ControllerSolo;
        switch (controlsAndInput.inputNameSelected)
        {
            case Globals.InputNameSelected.LeftController:

                switch (controlsAndInput.controlScheme)
                {
                    case Globals.ControlScheme.ControllerSharedLeft:
                        LCLTP1.transform.localScale = Vector3.Lerp(emptyController, filledInLeftController, LCLTP1F);
                        LCLTP1F += Time.deltaTime / duration;
                        break;
                    case Globals.ControlScheme.ControllerSharedRight:
                        // Debug.Log(t);
                        LCRTP1.transform.localScale = Vector3.Lerp(emptyController, filledInRightController, LCRTP1F);
                        LCRTP1F += Time.deltaTime / duration;
                        break;
                    case Globals.ControlScheme.ControllerSolo:
                        LCLTP1.transform.localScale = Vector3.Lerp(emptyController, filledInLeftController, LCLTP1F);
                        LCRTP1.transform.localScale = Vector3.Lerp(emptyController, filledInRightController, LCRTP1F);
                        LCRTP1F += Time.deltaTime / duration;
                        LCLTP1F += Time.deltaTime / duration;
                        break;
                }

                break;

            case Globals.InputNameSelected.RightController:
                switch (controlsAndInput.controlScheme)
                {
                    case Globals.ControlScheme.ControllerSharedLeft:
                        RCLTP1.transform.localScale = Vector3.Lerp(emptyController, filledInLeftController, RCLTP1F);
                        RCLTP1F += Time.deltaTime / duration;
                        break;
                    case Globals.ControlScheme.ControllerSharedRight:
                        RCRTP1.transform.localScale = Vector3.Lerp(emptyController, filledInRightController, RCRTP1F);
                        RCRTP1F += Time.deltaTime / duration;
                        break;
                    case Globals.ControlScheme.ControllerSolo:
                        RCLTP1.transform.localScale = Vector3.Lerp(emptyController, filledInLeftController, RCLTP1F);
                        RCRTP1.transform.localScale = Vector3.Lerp(emptyController, filledInRightController, RCRTP1F);
                        RCRTP1F += Time.deltaTime / duration;
                        RCLTP1F += Time.deltaTime / duration;
                        break;
                }

                break;
        }
        //Player1InputEmpty(controlsAndInput);

       /* f += Time.deltaTime / duration;

        Debug.Log(f);
        e = 1f - f;
        Debug.Log("E: " + e);

    }*/

    void Player1InputFill(ControlsAndInput player1Controls, ControlsAndInput player2Controls)
    {
        float t = Time.deltaTime / duration;
        player1Controls.controlScheme = player1Controls.inputNameSelected == Globals.Instance.player2Controls.inputNameSelected ? player1Controls.controlScheme : Globals.ControlScheme.Solo;
        player2Controls.controlScheme = player2Controls.inputNameSelected == Globals.Instance.player1Controls.inputNameSelected ? player2Controls.controlScheme : Globals.ControlScheme.Solo;

        CheckControlScheme(player1Controls, 1);
        CheckControlScheme(player2Controls, 2);


        LLF += LLFilling ? t : -t;
        LRF += LRFilling ? t : -t;
        RLF += RLFilling ? t : -t;
        RRF += RRFilling ? t : -t;
        LKF += LKFilling ? t : -t;
        RKF += RKFilling ? t : -t;    

        LLF = Mathf.Clamp(LLF, 0, 1);
        LRF = Mathf.Clamp(LRF, 0, 1);
        RLF = Mathf.Clamp(RLF, 0, 1);
        RRF = Mathf.Clamp(RRF, 0, 1);
        LKF = Mathf.Clamp(LKF, 0, 1);
        RKF = Mathf.Clamp(RKF, 0, 1);

        if(LL.activeInHierarchy)
            LL.transform.localScale = Vector3.Lerp(emptyController, filledInController, LLF);
        if(LR.activeInHierarchy)
            LR.transform.localScale = Vector3.Lerp(emptyController, filledInController, LRF);
        if (RL.activeInHierarchy)
            RL.transform.localScale = Vector3.Lerp(emptyController, filledInController, RLF);
        if (RR.activeInHierarchy)
            RR.transform.localScale = Vector3.Lerp(emptyController, filledInController, RRF);
        if (LK.activeInHierarchy)
            LK.transform.localScale = Vector3.Lerp(emptyController, filledInController, LKF);
        if (RK.activeInHierarchy)
            RK.transform.localScale = Vector3.Lerp(emptyController, filledInController, RKF);

        if (LL.activeInHierarchy)
            LL.GetComponentInChildren<Renderer>().material.color = Color.Lerp(Color.gray, LLColor, LLF);
        if (LR.activeInHierarchy)
            LR.GetComponentInChildren<Renderer>().material.color = Color.Lerp(Color.gray, LRColor, LRF);
        if (RL.activeInHierarchy)
            RL.GetComponentInChildren<Renderer>().material.color = Color.Lerp(Color.gray, RLColor, RLF);
        if (RR.activeInHierarchy)
            RR.GetComponentInChildren<Renderer>().material.color = Color.Lerp(Color.gray, RRColor, RRF);
        if (LK.activeInHierarchy)
            LK.GetComponentInChildren<Renderer>().material.color = Color.Lerp(Color.gray, LKColor, RKF);
        if (RK.activeInHierarchy)
            RK.GetComponentInChildren<Renderer>().material.color = Color.Lerp(Color.gray, RKColor, LKF);

        SetValuesToFalse();

    }

    void CheckControlScheme(ControlsAndInput controlsAndInput, int playerNumber)
    {
       // controlsAndInput.controlScheme = controlsAndInput.inputNameSelected == Globals.Instance.player2Controls.inputNameSelected ? controlsAndInput.controlScheme : Globals.ControlScheme.ControllerSolo;

        switch (controlsAndInput.inputNameSelected)
        {
            case Globals.InputNameSelected.LeftController:

                switch (controlsAndInput.controlScheme)
                {
                    case Globals.ControlScheme.SharedLeft:
                        LLFilling = true;
                        if (playerNumber == 1)
                            LLColor = player1Color;
                        else if (playerNumber == 2)
                            LLColor = player2Color;                        
                        break;
                    case Globals.ControlScheme.SharedRight:
                        LRFilling = true;
                        if (playerNumber == 1)
                            LRColor = player1Color;
                        else if (playerNumber == 2)
                            LRColor = player2Color;
                        break;
                    case Globals.ControlScheme.Solo:
                        LLFilling = true;
                        LRFilling = true;
                        if (playerNumber == 1)
                        {
                            LLColor = player1Color;
                            LRColor = player1Color;
                        }
                        else if (playerNumber == 2)
                        {
                            LLColor = player2Color;
                            LRColor = player2Color;
                        }
                        break;
                }
                break;
            case Globals.InputNameSelected.RightController:
                switch (controlsAndInput.controlScheme)
                {
                    case Globals.ControlScheme.SharedLeft:
                        RLFilling = true;
                        if (playerNumber == 1)
                            RLColor = player1Color;
                        else if (playerNumber == 2)
                            RLColor = player2Color;
                        break;
                    case Globals.ControlScheme.SharedRight:
                        RRFilling = true;
                        if (playerNumber == 1)
                            RRColor = player1Color;
                        else if (playerNumber == 2)
                            RRColor = player2Color;
                        break;
                    case Globals.ControlScheme.Solo:
                        RLFilling = true;
                        RRFilling = true;
                        if (playerNumber == 1)
                        {
                            RLColor = player1Color;
                            RRColor = player1Color;
                        }
                        else if (playerNumber == 2)
                        {
                            RLColor = player2Color;
                            RRColor = player2Color;
                        }
                        break;
                }
                break;
            case Globals.InputNameSelected.Keyboard:
                switch (controlsAndInput.controlScheme)
                {
                    case Globals.ControlScheme.SharedLeft:
                        LKFilling = true;
                        if (playerNumber == 1)
                            LKColor = player1Color;
                        else if (playerNumber == 2)
                            LKColor = player2Color;
                        break;
                    case Globals.ControlScheme.SharedRight:
                        RKFilling = true;
                        if (playerNumber == 1)
                            RKColor = player1Color;
                        else if (playerNumber == 2)
                            RKColor = player2Color;
                        break;
                    case Globals.ControlScheme.Solo:
                        LKFilling = true;
                        RKFilling = true;
                        if (playerNumber == 1)
                        {
                            LKColor = player1Color;
                            RKColor = player1Color;
                        }
                        else if (playerNumber == 2)
                        {
                            LKColor = player2Color;
                            RKColor = player2Color;
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
    }

     /*void Player1InputEmpty(ControlsAndInput controlsAndInput)
    {
        if (controlsAndInput.controlScheme != Globals.ControlScheme.None && controlsAndInput.inputNameSelected != Globals.InputNameSelected.None)
        {
            controlsAndInput.controlScheme = controlsAndInput.inputNameSelected == Globals.Instance.player2Controls.inputNameSelected ? controlsAndInput.controlScheme : Globals.ControlScheme.ControllerSolo;
            switch (controlsAndInput.inputNameSelected)
            {
                case Globals.InputNameSelected.LeftController:
                   
                    switch (controlsAndInput.controlScheme)
                    {
                        case Globals.ControlScheme.ControllerSharedLeft:
                            foreach(GameObject go in player1ColorFills)
                            {
                                if(go != LCLTP1)//&& go.transform.localScale.x > .01)                                
                                    go.transform.localScale = Vector3.Lerp(emptyController, filledInLeftController, e);
                            }
                            break;
                        case Globals.ControlScheme.ControllerSharedRight:
                            foreach(GameObject go in player1ColorFills)
                            {
                                if (go != LCRTP1)// && go.transform.localScale.x > .01)
                                    go.transform.localScale = Vector3.Lerp(emptyController, filledInLeftController, e);
                            }
                            break;
                        case Globals.ControlScheme.ControllerSolo:
                            foreach(GameObject go in player1ColorFills)
                            {
                                if (go != LCLTP1 && go != LCRTP1)// && go.transform.localScale.x > .01)                                
                                    go.transform.localScale = Vector3.Lerp(emptyController,filledInLeftController, e);
                            }
                            break;
                    }

                    break;

                case Globals.InputNameSelected.RightController:
                    switch (controlsAndInput.controlScheme)
                    {
                        case Globals.ControlScheme.ControllerSharedLeft:
                            foreach(GameObject go in player1ColorFills)
                            {
                                if(go != RCLTP1 && go.transform.localScale.x > .01)                                
                                    go.transform.localScale = Vector3.Lerp(emptyController,filledInLeftController, e);
                            }
                            break;
                        case Globals.ControlScheme.ControllerSharedRight:
                            foreach (GameObject go in player1ColorFills)
                            {
                                if (go != RCRTP1 && go.transform.localScale.x > .01)
                                    go.transform.localScale = Vector3.Lerp(emptyController, filledInLeftController, e);
                            }
                            break;
                        case Globals.ControlScheme.ControllerSolo:
                            foreach (GameObject go in player1ColorFills)
                            {
                                if (go != RCLTP1 && go != RCRTP1 && go.transform.localScale.x > .01)
                                    go.transform.localScale = Vector3.Lerp(emptyController,filledInLeftController, e);
                            }
                            break;
                    }

                    break;
            }

        }

        
    }*/
}

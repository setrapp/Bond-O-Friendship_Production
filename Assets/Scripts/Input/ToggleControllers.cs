using UnityEngine;
using System.Collections;
using InControl;


public class ToggleControllers : MonoBehaviour {

    public GameObject rightController;
    public GameObject leftController;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Globals.Instance.leftControllerIndex == -3)
        {
            leftController.SetActive(false);
        }
        else
            leftController.SetActive(true);
        if (Globals.Instance.rightContollerIndex == -3)
        {
            rightController.SetActive(false);
        }
        else
            rightController.SetActive(true);

	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaitPadToggleActive : MonoBehaviour {

    public GameObject waitPad;

    public bool firstTime = true;

    [SerializeField]
    public List<GameObject> objectsToToggle;
	
	// Update is called once per frame
	void Update () {

        if(waitPad.GetComponent<WaitPad>().activated && firstTime)
        {
            foreach (GameObject go in objectsToToggle)
            {
                go.SetActive(!go.activeInHierarchy);
            }
            firstTime = false;
        }
	
	}
}

using UnityEngine;
using System.Collections;

public class ToggleMirrorObject : MonoBehaviour {

    public GameObject mirrorObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collide)
    {

            if (collide.gameObject.name == "Player 1" || collide.gameObject.name == "Player 2")
            {
                mirrorObject.GetComponent<MirrorMovement>().objectToMirror = collide.gameObject;
                //mirrorObject.transform.position = new Vector3(mirrorObject.transform.position.x, collide.gameObject.transform.position.y, mirrorObject.transform.position.z);
            }
        
       
    }

   // void OnTriggerExit(Collider collide)
    //{
     //   if (collide.gameObject.name == "Player 1" || collide.gameObject.name == "Player 2")
     //   {
     //       mirrorObject.GetComponent<MirrorMovement>().objectToMirror = null;
     //   }
   // }
}

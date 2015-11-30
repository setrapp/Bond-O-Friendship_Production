using UnityEngine;
using System.Collections;

public class Growthtrigger : MonoBehaviour {



    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter(Collider collide)
    {
        if(collide.gameObject.layer == 4)
        {
            BroadcastMessage("Activate",SendMessageOptions.DontRequireReceiver);
        }
    }
}

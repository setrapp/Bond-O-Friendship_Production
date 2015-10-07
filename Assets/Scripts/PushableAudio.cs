using UnityEngine;
using System.Collections;

public class PushableAudio : MonoBehaviour {

    public GameObject Pushable;
    public bool isDry;
    public bool wasDry;
    public AudioSource drySound;
    public AudioSource fullSound;

	// Use this for initialization
	void Start () {
        wasDry = false;
	}
	
	// Update is called once per frame
	void Update () {
        if(Pushable.GetComponent<StreamReactionList>().streamsTouched == 0)
        {
            isDry = true;
        }
        else if(Pushable.GetComponent<StreamReactionList>().streamsTouched > 0)
        {
            isDry = false;
            wasDry = true;
        }

        if (isDry && !wasDry)
        {
            fullSound.mute = true;
            drySound.mute = false;
        }
        else
        {
            fullSound.mute = false;
            drySound.mute = true;
        }
           

	
	}
}

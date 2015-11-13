using UnityEngine;
using System.Collections;

public class PushableAudio : MonoBehaviour {

    public GameObject Pushable;
    public bool isDry;
    public bool wasDry;
    public AudioSource drySound;
    public AudioSource fullSound;
	private StreamReactionList reactionList;

	// Use this for initialization
	void Start () {
		reactionList = Pushable.GetComponent<StreamReactionList> ();
        wasDry = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(reactionList.streamsTouched == 0)
        {
            isDry = true;
        }
		else if(reactionList.streamsTouched > 0)
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

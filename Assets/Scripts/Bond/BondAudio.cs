using UnityEngine;
using System.Collections;

public class BondAudio : MonoBehaviour {

    public bool wasbonded;
    public AudioSource bondOn;
    public AudioSource bondOff;
    public bool onwasPlayed;
    public bool offwasPlayed;
    public bool bonded;
    public bool unbonded;

	// Use this for initialization
	void Start () {

        wasbonded = false;
        onwasPlayed = false;
        offwasPlayed = false;
        bonded = false;
        unbonded = true;
	
	}

    // Update is called once per frame
    void Update() {

        if (Globals.Instance.playersBonded == true)
        {
            bonded = true;
            unbonded = false;
            wasbonded = true;
            //print("bonded");
        }
        else if (Globals.Instance.playersBonded == false)
        {
            bonded = false;
            unbonded = true;
            //print("unbonded");
        }

        if (bonded == true && onwasPlayed == false)
        {
			if (Globals.Instance.bondSoundPlayable)
			{
				bondOn.Play();
			}
            onwasPlayed = true;
            if(offwasPlayed == true)
            {
                offwasPlayed = false;
            }
           
        }

        if (unbonded == true && wasbonded == true && offwasPlayed == false)
        {
			if (Globals.Instance.bondSoundPlayable)
			{
				bondOff.Play();
			}
            offwasPlayed = true;
            if (onwasPlayed == true)
            {
                onwasPlayed = false;
            }          
        }
    }
	
	


}

using UnityEngine;
using System.Collections;

public class TutGrowth : MonoBehaviour {

    // Use this for initialization
    public Animator anim;
    public GameObject container;

	void Start () {
        anim = GetComponent<Animator >();
	
	}
	
	// Update is called once per frame
	void Update () {

        if (container.GetComponent<GrowthContainer>().activated == true)
        {
            PlayAnim();
        }

    }

    void PlayAnim()
    {
        anim.enabled = true;
        //anim.PlayQueued("grow", QueueMode.PlayNow);
        //anim.PlayQueued("wobble2", QueueMode.CompleteOthers);
    }
}

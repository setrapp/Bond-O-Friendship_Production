using UnityEngine;
using System.Collections;

public class PullTogether : MonoBehaviour {

    public bool activated = false;
   // public bool inverted;

    public GameObject center;

    public GameObject OtherHalf;
    
    public GameObject Island;
    public GameObject Dest;
   
    private Vector3 islePosition;
    private float maxDistance;
    private float maxIsleDistance;
    
	// Use this for initialization
	void Start () {

        activated = false;
        maxDistance = Vector3.Distance(gameObject.transform.position, center.transform.position);
        maxIsleDistance = Vector3.Distance(Island.transform.position, Dest.transform.position);

	}
	
	// Update is called once per frame
	void Update () {

        // TODO figure out where to put this




        if (activated && OtherHalf.GetComponent<PullTogether>().activated)
            GetComponent<SpringJoint>().spring = 0;

            //maxDistance = Vector3.Distance(gameObject.transform.position, center.transform.position);
            //islePosition.x += 1 - ((Vector3.Distance(gameObject.transform.position, center.transform.position) / maxDistance) * maxIsleDistance) / Vector3.Distance(Island.transform.position, Dest.transform.position);

            islePosition = Island.transform.position;
            islePosition.x = gameObject.transform.position.x;
            Island.transform.position = islePosition;
       

        //print(islePosition);

        if (activated)
        {
            islePosition.x += 0;
        }
	
	}

    	void OnCollisionEnter(Collision collide)
	{
		if(collide.gameObject.name == "Center")
		{
	        activated = true;
            print("activated");
		}

	}
	void OnCollisionExit(Collision collide)
	{
        if (collide.gameObject.name == "Center")
        {
            activated = false;
            print("deactivated");
        }
	}
}

using UnityEngine;
using System.Collections;

public class FinalPoint : MonoBehaviour {

	public bool pointMade;
	
	public AudioClip Gong;
	
	public GameObject lilPoint1;
	public GameObject lilPoint2;
	public GameObject lilPoint3;
	public GameObject lilPoint4;
	public GameObject lilPoint5;
	public GameObject lilPoint6;
	public GameObject lilPoint7;
	public GameObject lilPoint8;
	
	public float rotSpeed;
	private Vector3 rotVect;
	public float timeConst = 50;
	
	private float myAlpha;
	private float fadeConst = 0.2f;
	public bool fading = false;
	public bool bright = false;

	public bool advance = false;
	public GameObject creator;
	
	// Use this for initialization
	void Start () {
		
		rotSpeed = 5.0f;
		rotVect = new Vector3(0,0,1);

		myAlpha = 0;
		bright = true;
		renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, myAlpha);
	
	}
	
	// Update is called once per frame
	void Update () {
		
		renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, myAlpha);
		
		if(fading == true)
		{
			if(myAlpha >= 0)
				myAlpha -= Time.deltaTime * fadeConst;
		}
		
		if(bright == true)
		{
			if(myAlpha <=1)
				myAlpha += Time.deltaTime * fadeConst;
		}
		
		transform.Rotate(rotVect * rotSpeed * Time.deltaTime);
		
		if(lilPoint1.GetComponent<LongDetail>().isHit && lilPoint2.GetComponent<LongDetail>().isHit && lilPoint3.GetComponent<LongDetail>().isHit && lilPoint4.GetComponent<LongDetail>().isHit &&
		   lilPoint5.GetComponent<LongDetail>().isHit && lilPoint6.GetComponent<LongDetail>().isHit && lilPoint7.GetComponent<LongDetail>().isHit && lilPoint8.GetComponent<LongDetail>().isHit) 
		{
			renderer.material.color = Color.yellow;
			//print("Good Point");
			pointMade = true;
			rotSpeed = 200.0f;
			if(rotSpeed > 50.0f)
			{
				rotSpeed -= Time.deltaTime * timeConst;
			}
			audio.PlayOneShot(Gong);
			//rotVect.y = 2;
			BroadcastMessage("IsHitOff");


			PartnerLink creatorLink = creator.GetComponent<PartnerLink>();
			creatorLink.seekingPartner = false;
			LevelManager levelManager = GameObject.FindGameObjectWithTag("Globals").GetComponent<LevelManager>();
			if (levelManager != null)
			{
				levelManager.LevelEvent();
			}
			ConversationManager.Instance.EndConversation(creatorLink, creatorLink.Partner);
		}

		if(pointMade == true)
		{
			BroadcastMessage("TurnOff");
		}
		
	}

	void OnTriggerEnter(Collider collide)
	{
		if(collide.gameObject.tag == "Converser")
		{
			renderer.material.color = Color.cyan;
			advance = true;
		}
	}
	
	public void IsFading()
	{
		fading = true;
		bright = false;
		//print ("Is fading");
	}
	
	public void IsBright()
	{
		fading = false;
		bright = true;
		//print ("Is Bright");
	}
	
	
	
	
}

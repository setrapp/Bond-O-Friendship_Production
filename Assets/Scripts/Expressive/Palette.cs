using UnityEngine;
using System.Collections;

public class Palette : MonoBehaviour {

	public GameObject ripplePrefab;
	private GameObject rippleObj;
	private Vector3 mySize;
	public float sizeScale = 0;
	private Color palColor;
	public float r;
	public float g;
	public float b;
	public float a;

	
	// Use this for initialization
	void Start () {
		//sizeScale = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		palColor = new Color(r,g,b,a);
		gameObject.GetComponent<Renderer>().material.color = palColor;
		mySize = new Vector3(sizeScale,sizeScale,0.1f);
		transform.localScale = mySize;
		if(sizeScale < 0.2f)
		{
			sizeScale += Time.deltaTime*0.33f;
		}
	}

	void OnTriggerEnter(Collider collide)
	{
		if(collide.gameObject.name == "Player 1" || collide.gameObject.name == "Player 2")
		{
			Fire();
			//print("pal");
			collide.gameObject.GetComponent<Paint>().r = r;
			collide.gameObject.GetComponent<Paint>().g = g;
			collide.gameObject.GetComponent<Paint>().b = b;
			collide.gameObject.GetComponent<Paint>().a = a;
			collide.gameObject.GetComponent<Paint>().origColor = false;
			CharacterComponents characterCo = collide.GetComponent<CharacterComponents>();
			characterCo.midTrail.material.color = new Color(palColor.r * 0.8f, palColor.g * 0.8f, palColor.b * 0.8f, palColor.a);
			characterCo.leftTrail.material.color = characterCo.rightTrail.material.color = palColor;
		}
	}

	void Fire()
	{
		rippleObj = Instantiate(ripplePrefab,transform.position,Quaternion.identity) as GameObject;
		rippleObj.GetComponent<Collider>().enabled = false;
		rippleObj.GetComponent<RingPulse>().smallRing = false;
		rippleObj.GetComponent<RingPulse>().scaleRate = 40.0f;
		rippleObj.GetComponent<RingPulse>().lifeTime = 2.0f;
		rippleObj.GetComponent<RingPulse>().alpha = 1.0f;
		rippleObj.GetComponent<RingPulse>().alphaFade = 1.8f;

		//print ("fire!");
	}
}

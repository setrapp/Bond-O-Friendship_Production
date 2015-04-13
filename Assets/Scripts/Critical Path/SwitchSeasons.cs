using UnityEngine;
using System.Collections;

public class SwitchSeasons : MonoBehaviour {

	public ParticleSystem winterParticle;
	public ParticleSystem fallParticle;
	public ParticleSystem summerParticle;
	public float seasonDuration;

	private ParticleSystem winter;
	private ParticleSystem fall;
	private ParticleSystem summer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider col) {
		if(name == "Winter Switch" && winter == null)
		{
			winter = (ParticleSystem)Instantiate(winterParticle);
			winter.transform.position = transform.position + new Vector3(0, 0, -15.0f);
			Destroy(winter.gameObject, seasonDuration);
		}
		if(name == "Fall Switch" && fall == null)
		{
			fall = (ParticleSystem)Instantiate(fallParticle);
			fall.transform.position = transform.position + new Vector3(0, 0, -15.0f);
			Destroy(fall.gameObject, seasonDuration);
		}
		if(name == "Summer Switch" && summer == null)
		{
			summer = (ParticleSystem)Instantiate(summerParticle);
			summer.transform.position = transform.position + new Vector3(0, 0, -15.0f);
			Destroy(summer.gameObject, seasonDuration);
		}

	}
}

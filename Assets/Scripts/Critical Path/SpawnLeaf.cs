using UnityEngine;
using System.Collections;

public class SpawnLeaf : MonoBehaviour {

	public GameObject leaf1;
	public GameObject leaf2;
	public GameObject leaf3;
	public GameObject leaf4;
	public GameObject leaf5;
	public GameObject leaf6;

	private ParticleSystem leafParticle;
	private int leafPick;
	private GameObject newLeaf;
	private ParticleSystem.Particle[] allParticles;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnParticleCollision (GameObject other) {
		if(other.name == "Critical_Path_Floors")
		{
			allParticles = new ParticleSystem.Particle[GetComponent<ParticleSystem>().particleCount];
			GetComponent<ParticleSystem>().GetParticles(allParticles);
			foreach(ParticleSystem.Particle thisParticle in allParticles) 
			{	
				leafPick = Random.Range(1, 8);
				if(leafPick == 1)
					newLeaf = (GameObject)Instantiate(leaf1);
				if(leafPick == 2)
					newLeaf = (GameObject)Instantiate(leaf2);
				if(leafPick == 3)
					newLeaf = (GameObject)Instantiate(leaf3);
				if(leafPick == 4)
					newLeaf = (GameObject)Instantiate(leaf4);
				if(leafPick == 5)
					newLeaf = (GameObject)Instantiate(leaf5);
				if(leafPick == 6)
					newLeaf = (GameObject)Instantiate(leaf6);

				newLeaf.transform.parent = transform;
				newLeaf.transform.position = thisParticle.position + transform.position;
			}

		}
	}
}

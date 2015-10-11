using UnityEngine;
using System.Collections;

public class NodeWall : MonoBehaviour {

	public GameObject pairedWall;
	public ParticleSystem part;
	public float cooldownTime;
	public Color bondColor;

	private Color playerColor;
	private Color startColor;
	private Color wallColor;
	private bool fading;
	private float timer;
	private bool solved;
	private static int progressCounter = 0;
	private static int nodeCount;
	private bool progress;

	// Use this for initialization
	void Start () {
		nodeCount++;
		wallColor = pairedWall.GetComponent<Renderer>().material.color;
		startColor = GetComponent<Renderer>().material.color;
		timer = cooldownTime;
	}
	
	// Update is called once per frame
	void Update () {
		if(progressCounter >= nodeCount)
			solved = true;
		if(fading == true)
		{
			wallColor.a -= Time.deltaTime;
			pairedWall.GetComponent<Renderer>().material.color = wallColor;
			timer -= Time.deltaTime;
			if(timer <= 0 && solved == false)
			{
				timer = cooldownTime;
				pairedWall.GetComponent<Collider>().enabled = true;
				GetComponent<Renderer>().material.color = startColor;
				wallColor.a = 0.75f;
				pairedWall.GetComponent<Renderer>().material.color = wallColor;
				fading = false;
				progressCounter--;
				progress = false;
			}
		}
	}

	void OnTriggerEnter (Collider col) {
		if(col.gameObject.layer != LayerMask.NameToLayer("Fluff") && fading == false)
		{
			if (col == Globals.Instance.Player1.character.bodyCollider)
			{
				playerColor = Globals.Instance.Player1.character.colors.baseColor;
			}
			else if (col == Globals.Instance.Player2.character.bodyCollider)
			{
				playerColor = Globals.Instance.Player2.character.colors.baseColor;
			}
			else if (col.gameObject.layer == LayerMask.NameToLayer("Bond"))
			{
				playerColor = bondColor;
			}
			ParticleSystem nodePart = (ParticleSystem)Instantiate(part);
			nodePart.startColor = playerColor;
			nodePart.transform.position = transform.position;
			Destroy(nodePart.gameObject, 2.0f);

			GetComponent<Renderer>().material.color = playerColor;

			pairedWall.GetComponent<Collider>().enabled = false;
			fading = true;

			if(progress == false)
			{
				progressCounter++;
				progress = true;
			}
		}
	}
}

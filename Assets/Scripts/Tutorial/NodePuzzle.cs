using UnityEngine;
using System.Collections;

public class NodePuzzle : MonoBehaviour {

	public float cooldownTime = 5.0f;
	private float timer;
	private bool lit;
	public ParticleSystem nodeParticle;
	private static int litCount;
	private GameObject nodeGate;
	private LineRenderer line;
	public Material lineMaterial;
	private Color fadeColor;
	private float colorCheck;

	// Use this for initialization
	void Start () {
		nodeGate = GameObject.Find("NodeGate");
	}
	
	// Update is called once per frame
	void Update () {
		if(lit == true)
		{
			if(line == null && (transform.parent.childCount - 1) > litCount)
			{
				line = gameObject.AddComponent<LineRenderer>();
				line.SetWidth(0.2f, 0.02f);
				line.material = lineMaterial;
				line.SetPosition(0, transform.position);
				line.SetPosition(1, nodeGate.transform.position);
			}
			if((transform.parent.childCount - 1) > litCount)
				timer -= Time.deltaTime;
			if(timer <= 0)
			{
				Destroy(line);
				timer = cooldownTime;
				lit = false;
				litCount--;
				GetComponent<Renderer>().material.color = Color.white;
			}
			if((transform.parent.childCount - 1) <= litCount && line != null)
				Destroy(line);
		}

		if((transform.parent.childCount - 1) <= litCount)
		{
		//	GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, fadeColor, Time.deltaTime);
		//	colorCheck = GetComponent<Renderer>().material.color.a;
			fadeColor.a -= Time.deltaTime;
			GetComponent<Renderer>().material.color = fadeColor;
			if(nodeGate != null)
				Destroy(nodeGate);

			if(GetComponent<Renderer>().material.color.a <= 0.01f)
				Destroy(gameObject);
		}
	}

	void OnTriggerEnter (Collider col) {
		if(col.name == "Player 1")
		{
			if(lit != true)
				litCount++;
			GetComponent<Renderer>().material.color = Globals.Instance.player1.character.colors.attachmentColor;
			lit = true;
			timer = cooldownTime;
			ParticleSystem part = (ParticleSystem)Instantiate(nodeParticle);
			part.transform.position = transform.position;
			part.startColor = Globals.Instance.player1.character.colors.attachmentColor;
			Destroy(part.gameObject, 2.0f);
			fadeColor = GetComponent<Renderer>().material.color;
		}
		if(col.name == "Player 2")
		{
			if(lit != true)
				litCount++;
			GetComponent<Renderer>().material.color = Globals.Instance.player2.character.colors.attachmentColor;
			lit = true;
			timer = cooldownTime;
			ParticleSystem part = (ParticleSystem)Instantiate(nodeParticle);
			part.transform.position = transform.position;
			part.startColor = Globals.Instance.player2.character.colors.attachmentColor;
			Destroy(part.gameObject, 2.0f);
			fadeColor = GetComponent<Renderer>().material.color;
		}

	}
}

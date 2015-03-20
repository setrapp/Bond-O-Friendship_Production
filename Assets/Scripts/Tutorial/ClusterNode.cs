using UnityEngine;
using System.Collections;

public class ClusterNode : MonoBehaviour {

	[HideInInspector]
	public ClusterNodePuzzle targetPuzzle;
	public float cooldownTime = 5.0f;
	private float timer;
	public bool lit;
	
	private LineRenderer line;
	public Material lineMaterial;
	private Color fadeColor;
	private float colorCheck;
	private Color startingcolor;
	

	// Use this for initialization
	void Start () {
		startingcolor = GetComponent<Renderer>().material.color;
	}
	
	// Update is called once per frame
	void Update () {
		if(lit == true)
		{
			//TODO fix this.
			/*if(line == null && solved)
			{
				line = gameObject.AddComponent<LineRenderer>();
				line.SetWidth(0.2f, 0.02f);
				line.material = lineMaterial;
				line.SetPosition(0, transform.position);
				line.SetPosition(1, nodeGate.transform.position);
			}
			if(solved)
				timer -= Time.deltaTime;
			if(timer <= 0)
			{
				Destroy(line);
				timer = cooldownTime;
				lit = false;
				litCount--;
				GetComponent<Renderer>().material.color = startingcolor;
			}
			if(solved && line != null)
				Destroy(line);*/
		}

		/*if(solved)
		{
			fadeColor.a -= Time.deltaTime;
			GetComponent<Renderer>().material.color = fadeColor;
			if(GetComponent<Renderer>().material.color.a <= 0.01f)
				Destroy(gameObject);
		}*/
	}

	void OnTriggerEnter (Collider col) {
		
		Color playerColor = new Color();
		if (col == Globals.Instance.player1.character.collider)
		{
			playerColor = Globals.Instance.player1.character.colors.attachmentColor;
		}
		else if (col == Globals.Instance.player2.character.collider)
		{
			playerColor = Globals.Instance.player2.character.colors.attachmentColor;
		}
		else
		{
			return;
		}


		GetComponent<Renderer>().material.color = playerColor;
		
		timer = cooldownTime;
		ParticleSystem part = (ParticleSystem)Instantiate(targetPuzzle.nodeParticle);
		part.transform.position = transform.position;
		part.startColor = playerColor;
		Destroy(part.gameObject, 2.0f);
		fadeColor = GetComponent<Renderer>().material.color;

		if (!lit)
		{
			lit = true;
			targetPuzzle.NodeColored();
		}
	}
}

using UnityEngine;
using System.Collections;

public class ClusterNode : MonoBehaviour {

	[HideInInspector]
	public ClusterNodePuzzle targetPuzzle;
	public bool disappearOnSolve = true;
	public Renderer nodeRenderer = null;
	public float cooldownTime = -1;
	protected float timer;
	public bool lit;
	
	private LineRenderer line;
	public Material lineMaterial;
	protected Color fadeColor;
	protected float colorCheck;
	protected Color startingcolor;
	protected Collider lighter = null;


	// Use this for initialization
	virtual protected void Start () {
		if (nodeRenderer == null)
		{
			nodeRenderer = gameObject.GetComponent<Renderer>();
		}
		startingcolor = nodeRenderer.material.color;

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

			if (!targetPuzzle.solved && cooldownTime > 0 && lighter == null)
			{
				timer -= Time.deltaTime;
				if (timer <= 0)
				{
					lit = false;
					nodeRenderer.material.color = startingcolor;
				}
			}
		}

		if(targetPuzzle.solved && disappearOnSolve)
		{
			fadeColor.a -= Time.deltaTime;
			nodeRenderer.material.color = fadeColor;
			if (nodeRenderer.material.color.a <= 0.01f)
			{
				Destroy(gameObject);
			}
		}
	}

	virtual protected void OnTriggerEnter (Collider col)
	{
		if (targetPuzzle.solved)
		{
			return;
		}

		Color playerColor = new Color();
		if (col == Globals.Instance.player1.character.bodyCollider)
		{
			playerColor = Globals.Instance.player1.character.colors.attachmentColor;
		}
		else if (col == Globals.Instance.player2.character.bodyCollider)
		{
			playerColor = Globals.Instance.player2.character.colors.attachmentColor;
		}
		else
		{
			return;
		}


		nodeRenderer.material.color = playerColor;
		fadeColor = nodeRenderer.material.color;

		ParticleSystem part = (ParticleSystem)Instantiate(targetPuzzle.nodeParticle);
		part.transform.position = transform.position;
		part.startColor = playerColor;
		part.transform.parent = transform;
		part.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
		Destroy(part.gameObject, 2.0f);

		lighter = col;
		timer = cooldownTime;
		if (!lit)
		{
			lit = true;
			targetPuzzle.NodeColored();
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col == lighter)
		{
			lighter = null;
		}
	}
}

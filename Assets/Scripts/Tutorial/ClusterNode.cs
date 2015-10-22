using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClusterNode : MonoBehaviour {

	//[HideInInspector]
	public ClusterNodePuzzle targetPuzzle;
	public bool controlColor = true;
	public bool colorSet = false;
	public bool disappearOnSolve = true;
	public Renderer[] nodeRenderers = null;
	public float cooldownTime = -1;
	protected float timer;
	public bool lit;
	
	private LineRenderer line;
	public Material lineMaterial;
	protected Color fadeColor;
	protected float colorCheck;
	protected Color startingcolor;
	protected List<Collider> lighters = new List<Collider>();
	public Color bondColor;
	public bool particlesAtCollision = false;

	[Header("Optional Wall Pairing")]
	public GameObject pairedWall;
	public Renderer[] wallRenderers;
	private float startWallAlpha;
    public bool shrinking;
    private Vector3 pairedWallStartingSize;
    public float shrinkSpeed = 3.0f;
    private float shrinkTimer;
    public float growSpeed = 3.0f;

    // Use this for initialization
    virtual protected void Start () {
		if(nodeRenderers == null || nodeRenderers.Length < 1)
		{
			nodeRenderers = GetComponentsInChildren<Renderer>();
		}
		startingcolor = nodeRenderers[0].material.color;



		if (pairedWall != null)
		{
			wallRenderers = pairedWall.GetComponentsInChildren<Renderer>();
			startWallAlpha = wallRenderers[0].material.color.a;
            pairedWallStartingSize = pairedWall.transform.localScale;
        }
        colorSet = true;
	}
	
	// Update is called once per frame
	virtual protected void Update () {
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

			if (pairedWall != null && (wallRenderers[0].material.color.a > startWallAlpha / 2 || wallRenderers[0].material.color.a > 0 && targetPuzzle.solved))
			{
                if (targetPuzzle.individualBlockerFade == true)
                    shrinking = true;
				else if(controlColor)
				{
					for (int i = 0; i < wallRenderers.Length; i++)
					{
						Color newWallColor = wallRenderers[i].material.color;
						newWallColor.a -= Time.deltaTime;
						wallRenderers[i].material.color = newWallColor;
					}
				}
				if (wallRenderers[0].material.color.a <= 0)
				{
					pairedWall.GetComponent<Collider>().enabled = false;
				}
			}
			

			if (!targetPuzzle.solved && cooldownTime > 0 && lighters.Count < 1)
			{
				timer -= Time.deltaTime;
				if (timer <= 0)
				{
                    ResetNode();
				}
				
			}
            if (pairedWall != null && shrinking && pairedWall.transform.localScale.x > 0.05f)
            {
                pairedWall.transform.localScale -= new Vector3(1, 1, 1) * shrinkSpeed * Time.deltaTime;
                if (pairedWall.transform.localScale.x <= 0.1f)
                    pairedWall.GetComponent<Collider>().enabled = false;
            }
        }
        if (pairedWall != null && !shrinking && pairedWall.transform.localScale.x < pairedWallStartingSize.x && targetPuzzle.individualBlockerFade == true)
        {
            pairedWall.transform.localScale += new Vector3(1, 1, 1) * growSpeed * Time.deltaTime;
            if (pairedWall.transform.localScale.x >= 0.01f)
                pairedWall.GetComponent<Collider>().enabled = true;
            if (pairedWall.transform.localScale.x > pairedWallStartingSize.x)
                pairedWall.transform.localScale = pairedWallStartingSize;
        }
        else if (pairedWall != null && wallRenderers[0].material.color.a < startWallAlpha)
		{
			if(controlColor)
			{
				for (int i = 0; i < wallRenderers.Length; i++)
				{
					Color newWallColor = wallRenderers[i].material.color;
					newWallColor.a += Time.deltaTime * 3;
					wallRenderers[i].material.color = newWallColor;
				}
			}
			pairedWall.GetComponent<Collider>().enabled = true;
		}

		if(targetPuzzle.solved && disappearOnSolve && (pairedWall == null || !pairedWall.GetComponent<Collider>().enabled))
		{
			fadeColor.a -= Time.deltaTime;
			if(controlColor)
			{
				for (int i = 0; i < nodeRenderers.Length; i++)
				{
					nodeRenderers[i].material.color = fadeColor;
				}
			}
			if (fadeColor.a <= 0.01f)
			{
				Destroy(gameObject);
			}

		}
	}

    public void ResetNode()
    {
        lit = false;
        if (targetPuzzle.individualBlockerFade == true)
            shrinking = false;
        if (controlColor)
        {
            for (int i = 0; i < nodeRenderers.Length; i++)
            {
                nodeRenderers[i].material.color = startingcolor;
            }
        }
    }
   

	virtual protected void OnCollisionEnter(Collision col)
	{
		CheckCollision(col.collider);
	}

	virtual protected void OnTriggerEnter(Collider col)
	{
		CheckCollision(col);
	}

	virtual public void CheckCollision (Collider col)
	{
		if (targetPuzzle.solved || (targetPuzzle.streamReaction != null && targetPuzzle.streamReaction.reactionProgress <= 0))
		{
			return;
		}

		Color playerColor = new Color();
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
		else
		{
			return;
		}



		for (int i = 0; i < nodeRenderers.Length; i++)
		{
			nodeRenderers[i].material.color = playerColor;
		}
		fadeColor = nodeRenderers[0].material.color;

		if (targetPuzzle.nodeParticle != null)
		{
			ParticleSystem part = (ParticleSystem)Instantiate(targetPuzzle.nodeParticle);
			part.transform.position = (particlesAtCollision) ? part.transform.position = col.transform.position : transform.position;
			part.startColor = playerColor;
			part.transform.parent = transform;
			part.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
			Destroy(part.gameObject, 2.0f);
		}

		lighters.Add(col);
		timer = cooldownTime;
		if (!lit)
		{
			lit = true;
			targetPuzzle.NodeColored();

			/*if (pairedWall != null)
			{
				pairedWall.GetComponent<Collider>().enabled = false;
			}*/
		}
	}

	void OnColliderExit(Collision col)
	{
		for (int i = 0; i < lighters.Count; i++)
		{
			if (lighters[i] == col.collider || lighters[i] == null)
			{
				lighters.Remove(lighters[i]);
				i--;
			}
		}
		
	}

	void OnTriggerExit(Collider col)
	{
		for (int i = 0; i < lighters.Count; i++)
		{
			if (lighters[i] == col || lighters[i] == null)
			{
				lighters.Remove(lighters[i]);
				i--;
			}
		}
	}
}

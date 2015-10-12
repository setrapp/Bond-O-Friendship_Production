using UnityEngine;
using System.Collections;

public class Paint : MonoBehaviour {

	public GameObject paintPrefab;
	private PaintCircle paintCircle;
	private float paintTime;
	private Vector3 paintPos;
	private float paintJitter;
	private float colorJitter;
	public bool painting;
	public Color paintColor;
	public int randRot;
	public float alpha;
	private float painttimeFloat;
	private float zJitter;
	public bool origColor;
	public float r;
	public float g;
	public float b;
	public float a;
    public bool eraserOn;

	public CanvasBehavior paintCanvas;
	
	// Use this for initialization
	void Start () {
		origColor = true;
		paintTime = 0.05f;
		paintJitter = 0.5f;
		paintJitter = 0.05f;
		alpha = 1.0f;
		painttimeFloat = 0.05f;
		painting = false;
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!eraserOn)
        {
            if (Globals.Instance.Player1.character.bondAttachable.IsBondMade(Globals.Instance.Player2.character.bondAttachable))
            {
                painttimeFloat = 0.09f;
            }
            else
            {
                painttimeFloat = 0.06f;
            }

            paintJitter = Random.Range(-0.5f, 0.5f);
            colorJitter = Random.Range(-0.05F, 0.05F);
            zJitter = Random.Range(0.5f, 0.7f);
            randRot = Random.Range(0, 360);

            if (origColor == true)
            {
                CharacterComponents characterCo = GetComponent<CharacterComponents>();
                if (gameObject.name == "Player 1")
                {
                    paintColor = new Color(0.1f + colorJitter, 0.4f + colorJitter, 0.9f + colorJitter, alpha);
                    characterCo.midTrail.material.color = GetComponent<CharacterColors>().trailMaterial.color;
                    characterCo.leftTrail.material.color = characterCo.rightTrail.material.color = GetComponent<CharacterColors>().sideTrailMaterial.color;
                }
                if (gameObject.name == "Player 2")
                {
                    paintColor = new Color(0.9f + colorJitter, 0.6f + colorJitter, 0.1f + colorJitter, alpha);
                    characterCo.midTrail.material.color = GetComponent<CharacterColors>().trailMaterial.color;
                    characterCo.leftTrail.material.color = characterCo.rightTrail.material.color = GetComponent<CharacterColors>().sideTrailMaterial.color;
                }
            }
            else
                paintColor = new Color(r + colorJitter, g + colorJitter, b + colorJitter, a);

            paintPos = new Vector3(transform.position.x + paintJitter, transform.position.y + paintJitter, transform.position.z + zJitter);

			if(name != "Paint Copier")
			{
	            if (painting == true && gameObject.GetComponent<CharacterComponents>().mover.velocity.sqrMagnitude != 0)
	            {
	                if (paintTime == painttimeFloat)
	                {
	                    blot(false);
	                }
	                paintTime -= Time.deltaTime;
	            }
			}
			/*else
			{
				if (painting == true && transform.parent.GetComponent<CanvasBehavior>().pairedPlayer.GetComponent<CharacterComponents>().mover.velocity.sqrMagnitude != 0)
				{
					if (paintTime == painttimeFloat)
					{
						blot(true);
					}
					paintTime -= Time.deltaTime;
				}
			}*/
            if (paintTime <= 0.0f)
            {
                paintTime = painttimeFloat;
            }
        }
        else
        {
            Erase();
        }
	}

	public void blot(bool inMirror, float baseRadius = -1)
	{
		// Attempt to set a radius of paint based on proximity to a node.
		if (baseRadius < 0)
		{
			// Find the nearest node on this canvas or the paired canvas.
			MirroringClusterNode nearestNode, nearestPairNode;
			float nearestDist = -1, nearestPairDist = -1;
			if (paintCanvas != null && paintCanvas.nodeCollisionTest != null)
			{
				nearestNode = paintCanvas.nodeCollisionTest.FindNearestNode(transform.position);
				if (nearestNode != null)
				{
					nearestDist = (transform.position - nearestNode.transform.position).magnitude;
				}
			}
			if (paintCanvas != null && paintCanvas.pairedCanvas != null && paintCanvas.pairedCanvas.nodeCollisionTest != null)
			{
				nearestPairNode = paintCanvas.pairedCanvas.nodeCollisionTest.FindNearestNode(transform.position - paintCanvas.mirrorDistance);
				if (nearestPairNode != null)
				{
					nearestPairDist = ((transform.position - paintCanvas.mirrorDistance) - nearestPairNode.transform.position).magnitude;
				}
			}

			if (nearestDist >= 0 && nearestPairDist >= 0)
			{
				nearestDist = Mathf.Min(nearestDist, nearestPairDist);
			}
			else if (nearestPairDist >= 0)
			{
				nearestDist = nearestPairDist;
			}

			if (nearestDist >= 0)
			{
				float radiusFactor = Mathf.Clamp01(nearestDist / paintCanvas.maxPaintRadius);
				baseRadius = ((1 - radiusFactor) * paintCanvas.maxPaintRadius) + (radiusFactor * paintCanvas.minPaintRadius);
			}
		}
		

		paintCircle = ((GameObject)Instantiate(paintPrefab, paintPos, Quaternion.Euler(0,0,randRot))).GetComponent<PaintCircle>();
		paintCircle.GetComponent<Renderer>().material.color = paintColor;
		paintCircle.paintCircColor = paintColor;
		if(Globals.Instance.Player1.character.bondAttachable.IsBondMade(Globals.Instance.Player2.character.bondAttachable))
		{
			paintCircle.rLifemin = 6.0f;
			paintCircle.rLifemax = 7.0f;
			paintCircle.rSizemin = 1.0f;
			paintCircle.rSizemax = 5.0f;
		}
		else
		{
			paintCircle.rLifemin = 2.0f;
			paintCircle.rLifemax = 3.0f;
			paintCircle.rSizemin = 0.5f;
			paintCircle.rSizemax = 3.0f;
		}

		if (baseRadius >= 0)
		{
			paintCircle.rSizemax = baseRadius;
			paintCircle.rSizemin = baseRadius;
		}

		if (paintCanvas.pairedCanvas != null && paintCanvas.paintCopier == null && paintCanvas.pairedCanvas.paintCopier != null)
		{
			paintCanvas.pairedCanvas.paintCopier.GetComponent<Paint>().blot(true, baseRadius);
		}

		if (inMirror)
			paintCanvas.gameObject.GetComponent<PaintAndNodeCollisionTest>().CheckPaintAndNodeCollision(paintCircle.GetComponent<PaintCircle>());
	}

    void Erase()
    {
        
        var hits = Physics.RaycastAll(transform.position, Vector3.forward, Mathf.Infinity);

        foreach (var hit in hits)
        {
            if (hit.transform.gameObject.GetComponent<PaintCircle>() != null)
            {
                hit.transform.gameObject.GetComponent<PaintCircle>().erased = true;
                hit.transform.gameObject.GetComponent<PaintCircle>().myLife = 0;
            }
        }
    }
}

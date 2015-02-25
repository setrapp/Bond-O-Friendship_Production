using UnityEngine;
using System.Collections;

public class Paint : MonoBehaviour {

	public GameObject paintPrefab;
	private GameObject paintCircle;
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
            if (Globals.Instance.player1.character.bondAttachable.IsBondMade(Globals.Instance.player2.character.bondAttachable))
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

            if (painting == true && gameObject.GetComponent<CharacterComponents>().mover.velocity.sqrMagnitude != 0)
            {
                if (paintTime == painttimeFloat)
                {
                    blot();
                }
                paintTime -= Time.deltaTime;
            }
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

	void blot()
	{
		paintCircle = Instantiate(paintPrefab, paintPos, Quaternion.Euler(0,0,randRot)) as GameObject;
		paintCircle.GetComponent<Renderer>().material.color = paintColor;
		paintCircle.GetComponent<PaintCircle>().paintCircColor = paintColor;
		if(Globals.Instance.player1.character.bondAttachable.IsBondMade(Globals.Instance.player2.character.bondAttachable))
		{
			paintCircle.GetComponent<PaintCircle>().rLifemin = 6.0f;
			paintCircle.GetComponent<PaintCircle>().rLifemax = 7.0f;
			paintCircle.GetComponent<PaintCircle>().rSizemin = 1.0f;
			paintCircle.GetComponent<PaintCircle>().rSizemax = 5.0f;
		}
		else
		{
			paintCircle.GetComponent<PaintCircle>().rLifemin = 2.0f;
			paintCircle.GetComponent<PaintCircle>().rLifemax = 3.0f;
			paintCircle.GetComponent<PaintCircle>().rSizemin = 0.5f;
			paintCircle.GetComponent<PaintCircle>().rSizemax = 3.0f;
		}
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

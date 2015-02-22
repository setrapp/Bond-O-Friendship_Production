using UnityEngine;
using System.Collections;

public class FluffPaint : MonoBehaviour {

	public GameObject paintPrefab;
	private GameObject paintCircle;
	private float paintTime;
	private Vector3 paintPos;
	private float paintJitter;
	public Color paintColor;
	public int randRot;
	private float painttimeFloat;
	private float zJitter;
	
	// Use this for initialization
	void Start () {
		paintTime = 0.05f;
		paintJitter = 0.05f;
		painttimeFloat = 0.05f;
		
	}
	
	// Update is called once per frame
	void Update () {

		painttimeFloat = 0.05f;

		paintJitter = Random.Range(-0.05f,0.05f);
		zJitter = Random.Range(0.5f,0.7f);
		randRot = Random.Range(0,360);
		Fluff fluff = GetComponent<Fluff>();
		Paint creatorPaint = null;
		if (fluff != null && fluff.creator != null)
		{
			creatorPaint = fluff.creator.GetComponent<Paint>();
		}

		if (creatorPaint != null)
		{
			paintColor = creatorPaint.paintColor;
		}
		paintPos = new Vector3(transform.position.x+paintJitter, transform.position.y+paintJitter, transform.position.z+zJitter);

		if ((creatorPaint != null && creatorPaint.painting) && (fluff != null && fluff.moving))
		{
			if(paintTime == painttimeFloat)
			{
				blot ();
			}
			paintTime -= Time.deltaTime;
		}
		if (paintTime <= 0.0f)
		{
			paintTime = painttimeFloat;
		}

	}
	
	void blot()
	{
		paintCircle = Instantiate(paintPrefab, paintPos, Quaternion.Euler(0,0,randRot)) as GameObject;
		paintCircle.GetComponent<Renderer>().material.color = paintColor;
		paintCircle.GetComponent<PaintCircle>().paintCircColor = paintColor;

		paintCircle.GetComponent<PaintCircle>().rLifemin = 1.0f;
		paintCircle.GetComponent<PaintCircle>().rLifemax = 2.0f;
		paintCircle.GetComponent<PaintCircle>().rSizemin = 0.1f;
		paintCircle.GetComponent<PaintCircle>().rSizemax = 0.5f;

	}
}
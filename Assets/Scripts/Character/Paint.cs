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
	
	// Use this for initialization
	void Start () {
		origColor = true;
		paintTime = 0.05f;
		paintJitter = 0.5f;
		paintJitter = 0.05f;
		alpha = 1.0f;
		painttimeFloat = 0.03f;
		painting = false;
	
	}
	
	// Update is called once per frame
	void Update () {
		paintJitter = Random.Range(-0.5f,0.5f);
		colorJitter = Random.Range(-0.05F,0.05F);
		zJitter = Random.Range(0.5f,0.7f);
		randRot = Random.Range(0,360);

		if(origColor == true)
		{
		if(gameObject.name == "Player 1")
		{
		paintColor = new Color(0.1f+colorJitter,0.4f+colorJitter,0.9f+colorJitter,alpha);
		}
		if(gameObject.name == "Player 2")
		{
			paintColor = new Color(0.9f+colorJitter,0.6f+colorJitter,0.1f+colorJitter,alpha);
		}
		}
		else
			paintColor = new Color(r+colorJitter,g+colorJitter,b+colorJitter,a);

		paintPos = new Vector3(transform.position.x+paintJitter, transform.position.y+paintJitter, transform.position.z+zJitter);

		if (painting == true && gameObject.GetComponent<CharacterComponents>().mover.velocity.sqrMagnitude != 0)
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
	}
}

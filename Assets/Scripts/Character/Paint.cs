using UnityEngine;
using System.Collections;

public class Paint : MonoBehaviour {

	public GameObject paintPrefab;
	private GameObject paintCircle;
	private float paintTime;
	private Vector3 paintPos;
	private float paintJitter;
	public bool painting;
	public Color paintColor;
	public int randRot;

	// Use this for initialization
	void Start () {
		paintTime = 0.02f;
		paintJitter = 0.5f;
		painting = false;
	
	}
	
	// Update is called once per frame
	void Update () {
		paintJitter = Random.Range(-0.5f,0.5f);
		randRot = Random.Range(0,360);
		paintColor = new Color(0.5f, 0.6f, 0.05f);
		paintPos = new Vector3(transform.position.x+paintJitter, transform.position.y+paintJitter, transform.position.z+5.0f);

		if (painting == true)
		{
			if(paintTime == 0.02f)
			{
				paintCircle = Instantiate(paintPrefab, paintPos, Quaternion.Euler(0,0,randRot)) as GameObject;
				paintCircle.GetComponent<Renderer>().material.color = paintColor;
			}
			paintTime -= Time.deltaTime;
		}
		if (paintTime <= 0.0f)
		{
			paintTime = 0.02f;
		}
	}
}

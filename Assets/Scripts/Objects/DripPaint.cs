using UnityEngine;
using System.Collections;

public class DripPaint : MonoBehaviour {

    public bool isPainting;
    private bool isPaintingPrevious = false;

	public GameObject paintPrefab;

    private GameObject paintCircle;
	
	private float paintJitter = 0.5f;
	private float colorJitter = 0.05f;
	public float paintDepthOffset = 1;
	
    public Color referenceColor;
    private Color paintColor;

    private Vector3 paintPos;
	
	// Update is called once per frame
	void Update () 
    {
        if (isPainting)
        {
            paintJitter = Random.Range(-0.5f, 0.5f);
            colorJitter = Random.Range(-0.05F, 0.05F);

            paintColor = new Color(referenceColor.r + colorJitter, referenceColor.g + colorJitter, referenceColor.b + colorJitter, referenceColor.a);
            paintPos = new Vector3(transform.position.x + paintJitter, transform.position.y + paintJitter, transform.position.z + paintDepthOffset);
        }

        if (isPainting != isPaintingPrevious)
        {
            if (isPainting)
                InvokeRepeating("Blot", 0.1f, 0.1f);
            else
                CancelInvoke("Blot");

            isPaintingPrevious = isPainting;
        }       
	}

    void Blot()
    {
        if (gameObject.GetComponent<CharacterComponents>().mover.velocity.sqrMagnitude != 0)
        {
            paintCircle = Instantiate(paintPrefab, paintPos, Quaternion.Euler(0, 0, 0)) as GameObject;

            paintCircle.GetComponent<Renderer>().material.color = paintColor;
            paintCircle.GetComponent<DripPaintCircle>().rLifemin = 2.0f;
            paintCircle.GetComponent<DripPaintCircle>().rLifemax = 3.0f;
            paintCircle.GetComponent<DripPaintCircle>().rSizemin = 0.5f;
            paintCircle.GetComponent<DripPaintCircle>().rSizemax = 2.0f;
        }

    }
}



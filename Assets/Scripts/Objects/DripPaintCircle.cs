using UnityEngine;
using System.Collections;

public class DripPaintCircle : MonoBehaviour
{


    public float sizeRand;
    public float rSizemin;
    public float rSizemax;

    public float myLife;
    public float rLifemin;
    public float rLifemax;

    private Vector3 mySize;
    private Vector3 startingSize;

    private float t;
    private float duration = 1.0f;

    // Use this for initialization
    void Start()
    {
        sizeRand = Random.Range(rSizemin, rSizemax);
        myLife = Random.Range(rLifemin, rLifemax);
        mySize = new Vector3(sizeRand, sizeRand, 0.001f);

        transform.localScale = new Vector3(0f, 0f, 0.001f);
        startingSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

        if (t != 1)
        {
            t = Mathf.Clamp(t + (Time.deltaTime / duration), 0.0f, 1.0f);
            transform.localScale = Vector3.Lerp(startingSize, mySize, t);
        }
        else
        {
            mySize = new Vector3(sizeRand, sizeRand, 0.001f);
            transform.localScale = mySize;
            myLife -= Time.deltaTime;
            if (myLife <= 0)
                sizeRand -= Time.deltaTime * 2.0f;
            if (sizeRand <= 0)
                Destroy(gameObject);
        }
    }
}

	


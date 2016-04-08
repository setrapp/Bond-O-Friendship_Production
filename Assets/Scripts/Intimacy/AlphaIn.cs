using UnityEngine;
using System.Collections;

public class AlphaIn : MonoBehaviour {

    public GameObject targethreader;
    private Color mycolor;
    private bool threaderSolved;
    private float r;
    private float g;
    private float b;
    private float a;
    private float con;
    private float fadeRate;

    // Use this for initialization
    void Start () {
        threaderSolved = false;
        r = 54.0f;
        g = 3.0f;
        b = 92.0f;
        a = 0.0f;
        con = 255.0f;
        fadeRate = 0.3f;

    }
	
	// Update is called once per frame
	void Update () {
        mycolor = new Color(r/con, g/con, b/con, a);
        GetComponent<Renderer>().material.color = mycolor;
        if (targethreader.GetComponent<ThreadParent>().solved == true)
        {
            threaderSolved = true;
        }
        if (threaderSolved == true)
        {
            if (a < 1.0f)
            {
                a += Time.deltaTime * fadeRate;
            }

        }
    
	
	}
}

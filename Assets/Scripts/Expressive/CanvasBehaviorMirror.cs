using UnityEngine;
using System.Collections;

public class CanvasBehaviorMirror : MonoBehaviour
{
    public GameObject mirrorPaint;
    //private Color canvasColor;
    private float alpha;



    // Use this for initialization

    void Start()
    {
        alpha = 0.0f;
        //canvasColor = new Color(0.0f, 0.0f, 0.0f, alpha);
        //gameObject.GetComponent<Renderer>().material.color = canvasColor;

    }

    // Update is called once per frame
    void Update()
    {
        //canvasColor = Color.white;
       // gameObject.GetComponent<Renderer>().material.color = canvasColor;
        //if (alpha < 1.0f)
        //{
        //    alpha += Time.deltaTime * 0.5f;
       // }

    }

    void OnTriggerEnter(Collider collide)
    {
        
        if (collide.gameObject.name == "MirrorPaint")
        {
            //Debug.Log("In Here");
            mirrorPaint = collide.gameObject;
            mirrorPaint.GetComponent<MirrorPaint>().painting = true;

        }
    }

    void OnTriggerExit(Collider collide)
    {
        if (collide.gameObject.name == "MirrorPaint")
        {
            mirrorPaint = collide.gameObject;
            mirrorPaint.GetComponent<MirrorPaint>().painting = false;
            //print ("Paintfalse");
        }
    }
}

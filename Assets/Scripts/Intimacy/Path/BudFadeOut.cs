using UnityEngine;
using System.Collections;

public class BudFadeOut : MonoBehaviour
{

    private float timer;
    private Color myColor;
    public GameObject myShadow;
    private Color shadow;
    public bool fadeNow = false;

    // Use this for initialization
    void Start()
    {
        timer = 1.0f;

    }

    // Update is called once per frame
    void Update()
    {
        myColor = new Color(107.0f / 255.0f, 53.0f / 255.0f, 201.0f / 255.0f, timer);

        if (myShadow != null)
        {
            shadow = new Color(139.0f / 255.0f, 139.0f / 255.0f, 139.0f / 255.0f, timer);
            myShadow.GetComponent<Renderer>().material.color = shadow;
        }

        GetComponent<Renderer>().material.color = myColor;
        if (fadeNow == true)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            Destroy(gameObject);
        }

    }
}

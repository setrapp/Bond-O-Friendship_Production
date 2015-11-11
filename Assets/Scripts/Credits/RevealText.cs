using UnityEngine;
using System.Collections;

public class RevealText : MonoBehaviour {

    public bool revealed;
    public float revealSpeed;

    private Color textColor;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (revealed && GetComponent<Renderer>().material.color.a < 1)
        {
            textColor = GetComponent<Renderer>().material.color;
            textColor.a += revealSpeed * Time.deltaTime;
            GetComponent<Renderer>().material.color = textColor;
        }
	}

    void OnTriggerEnter (Collider col)
    {
        if (col.transform.parent.name == "River Particles")
            revealed = true;
        if (col.name == "Player 1" || col.name == "Player 2" || col.name == "Bond")
        {
            if (revealed && transform.GetChild(0) != null)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(0).GetComponent<Light>().color = GetComponentInParent<Renderer>().material.color;
            }
        }

    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindClosestLuminus : MonoBehaviour {

    [HideInInspector] public GameObject[] luminusArray;
    public GameObject l1, l2;   //Closest to P1, P2
    [HideInInspector] public GameObject p1, p2;   //Players
    [HideInInspector] public SetShaderData_DarkAlphaMasker darkMaskScript;    //on the alpha masking plane
    [HideInInspector] public float l1p1_sqMag, l2p2_sqMag;    //shortest distances (useful in shader)

	// Use this for initialization
	void Start () 
    {
        p1 = Globals.Instance.player1.gameObject;
        p2 = Globals.Instance.player2.gameObject;
        luminusArray = GameObject.FindGameObjectsWithTag("Luminus");
        darkMaskScript = GameObject.Find("DarkMask").GetComponent<SetShaderData_DarkAlphaMasker>();
        //Find which ones are closest at the beginning
        FindClosestLumini();
        SendDataToDarkMask();
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Update closest lumini every frame
        FindClosestLumini();
        SendDataToDarkMask();
	}

    void FindClosestLumini()
    {
        l1 = l2 = luminusArray[0];
        Vector3 l1p1 = p1.transform.position - l1.transform.position;
        Vector3 l2p2 = p2.transform.position - l2.transform.position;
        Vector3 temp;

        l1p1_sqMag = l1p1.sqrMagnitude;
        l2p2_sqMag = l2p2.sqrMagnitude;

        for (int i = 1; i < luminusArray.Length; i++)
        {
            //Closest to p1
            temp = p1.transform.position - luminusArray[i].transform.position;
            //if a different luminus is closer
            if (temp.sqrMagnitude < l1p1_sqMag)
            {
                l1 = luminusArray[i];
                l1p1_sqMag = temp.sqrMagnitude;
            }

            //Closest to P2
            temp = p2.transform.position - luminusArray[i].transform.position;
            //if a different luminus is closer
            if (temp.sqrMagnitude < l2p2_sqMag)
            {
                l2 = luminusArray[i];
                l2p2_sqMag = temp.sqrMagnitude;
            }
            
        }
    }

    void SendDataToDarkMask()
    {
        darkMaskScript.l1 = l1;
        darkMaskScript.l2 = l2;
        darkMaskScript.l1p1_sqMag = l1p1_sqMag;
        darkMaskScript.l2p2_sqMag = l2p2_sqMag;
    }
}

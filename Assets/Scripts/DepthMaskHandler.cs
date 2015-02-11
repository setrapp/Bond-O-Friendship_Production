using UnityEngine;
using System.Collections;

public class DepthMaskHandler : MonoBehaviour {

    [HideInInspector]
    public GameObject depthMask;

	// Use this for initialization
	void Start () {
        if (Globals.Instance != null)
        {
            Vector3 startPos = new Vector3(transform.position.x, transform.position.y, -10.0f);
            depthMask = (GameObject)Instantiate(Globals.Instance.depthMaskPrefab, startPos, Quaternion.identity);
            depthMask.transform.parent = DepthMaskHolder.Instance.transform;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (depthMask != null)
        {
            depthMask.transform.position = new Vector3(transform.position.x, transform.position.y, -10.0f);
        }
	}
}

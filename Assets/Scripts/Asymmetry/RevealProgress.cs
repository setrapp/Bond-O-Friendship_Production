using UnityEngine;
using System.Collections;

public class RevealProgress : MonoBehaviour {
	
	public  GameObject pairedPuzzle;
	private Vector3 targetScale;
	private int arrayLength = 0;
	private MirroringClusterNode[] nodes;
	private float revealedProgress;
    private int arrayCounter = 0;
	
	// Use this for initialization
	void Start () {
		nodes = pairedPuzzle.GetComponentsInChildren<MirroringClusterNode>();
		/*for(int i = 0; i < pairedPuzzle.transform.childCount; i++)
		{
			if(pairedPuzzle.transform.GetChild(i).gameObject.name == "Node1" || pairedPuzzle.transform.GetChild(i).gameObject.name == "Node2")
				arrayLength++;
		}
		nodes = new MirroringClusterNode[arrayLength];
		for(int i = 0; i < pairedPuzzle.transform.childCount; i++)
		{
            if (pairedPuzzle.transform.GetChild(i).gameObject.name == "Node1" || pairedPuzzle.transform.GetChild(i).gameObject.name == "Node2")
            {
                nodes[arrayCounter] = pairedPuzzle.transform.GetChild(i).GetComp;
                arrayCounter++;
            }
		}*/
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < nodes.Length; i++)
		{
			if(nodes[i] != null && nodes[i].revealed == true)
			{
				//Debug.Log(1/nodes.Length);
				revealedProgress += 1.0f/nodes.Length;
			}
		}

		if (pairedPuzzle != null && transform.localScale.x < 1.0f) {
			targetScale = new Vector3(revealedProgress, revealedProgress, revealedProgress);
			transform.localScale = Vector3.Lerp (transform.localScale, targetScale, Time.deltaTime);
		}
		if (transform.localScale.x > 0.99f)
			transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
		revealedProgress = 0;
	}
}

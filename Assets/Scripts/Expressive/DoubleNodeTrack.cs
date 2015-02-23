using UnityEngine;
using System.Collections;

public class DoubleNodeTrack : MonoBehaviour {

	public int nodeActivated;
	public int nodeNumber;
	public bool activated = false;
	private Vector3 canvasPos;
	
	// Use this for initialization
	void Start () {
		nodeActivated = 0;
		//canvasPos = new Vector3(transform.position.x,transform.position.y,transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.childCount != 0)
		{
				
			nodeNumber = transform.childCount;
			DoubleNodeContainer[] childnodes = transform.GetComponentsInChildren<DoubleNodeContainer>();
			nodeActivated = 0;
			for(int i = nodeNumber-1;i >= 0;i--)
			{
				if(childnodes[i].bothActivated == true)
				{
					nodeActivated++;
				}
			}
			if(nodeActivated == nodeNumber)
			activated = true;
			if(activated == true)
			{
			 //print ("track activated");
			}
		}
	}
}
using UnityEngine;
using System.Collections;

public class DoubleNodeMonitor : MonoBehaviour {

	public GameObject canvas1;
	public GameObject canvas;
	public int nodeActivated;
	public int nodeNumber;
	public bool activated = false;
	private Vector3 canvasPos;

	public int doubleNodeNum;
	public int exNodeNum;

	// Use this for initialization
	void Start () {
		nodeActivated = 0;
		//canvasPos = new Vector3(transform.position.x,transform.position.y,transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		if(canvas1 == null)
		{
			if(transform.childCount != 0)
			{
				
				nodeNumber = transform.childCount;
				DoubleNodeTrack[] childDoublenodes = transform.GetComponentsInChildren<DoubleNodeTrack>();
				doubleNodeNum = childDoublenodes.Length;
				ExNode[] childExnodes = transform.GetComponentsInChildren<ExNode>();
				exNodeNum = childExnodes.Length;

				nodeActivated = 0;

				for(int i = exNodeNum -1;i >= 0;i--)
				{
					if(childExnodes[i].activated == true)
					{
						nodeActivated++;
					}
				}

				for(int i = doubleNodeNum -1;i >= 0;i--)
				{
					if(childDoublenodes[i].activated == true)
					{
						nodeActivated++;
					}
				}


				if(nodeActivated == nodeNumber)
					activated = true;
			}
			
			if(activated == true)
			{
				
				canvas1 = Instantiate(canvas) as GameObject;
				canvas1.transform.parent = transform;
				canvas1.transform.localPosition = new Vector3(0, 0, 5);
			}
		}
		
	}
}
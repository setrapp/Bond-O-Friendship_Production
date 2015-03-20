using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClusterNodePuzzle : MonoBehaviour {

	public List<ClusterNode> nodes;
	public List<GameObject> listeners;
	public ParticleSystem nodeParticle;
	public bool solved;


	void Awake()
	{
		for (int i = 0; i < nodes.Count; i++)
		{
			nodes[i].lit = false;
			nodes[i].targetPuzzle = this;
		}
	}
	
	public void NodeColored()
	{
		bool allLit = true;
		for (int i = 0; i < nodes.Count && allLit; i++)
		{
			if (!nodes[i].lit)
			{
				allLit = false;
			}
		}

		if (allLit && !solved)
		{
			solved = true;
			for (int i = 0; i < listeners.Count; i++)
			{
				listeners[i].SendMessage("ClusterNodesSolved", this, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}

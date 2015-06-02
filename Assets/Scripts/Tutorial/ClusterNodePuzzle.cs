using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClusterNodePuzzle : MonoBehaviour {

	public List<ClusterNode> nodes;
	public List<GameObject> listeners;
	public ParticleSystem nodeParticle;
	public bool solved;

	public GameObject streamBlocker;
	public GameObject streamBlocker2;

	private float startingSize;
	private int litCount;
    public float progress = 0;


	void Awake()
	{
		for (int i = 0; i < nodes.Count; i++)
		{
			if (nodes[i] != null)
			{
				nodes[i].lit = false;
				nodes[i].targetPuzzle = this;
			}
			else
			{
				Debug.LogError("Node Puzzle \'" + gameObject.name + "\' has a null referenced node. Removing node for play session. Please remove from list permanently while in edit mode.");
				nodes.RemoveAt(i);
				i--;
			}
			
		}

		if(streamBlocker != null && streamBlocker2 != null)
			startingSize = streamBlocker.transform.localScale.y;
	}
	
	public void NodeColored()
	{
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].lit)
                litCount++;
        }

        if (nodes.Count > 0)
        {
            progress = Mathf.Max((float)litCount / nodes.Count, progress);
        }
        
		if (streamBlocker != null)
		{
			Renderer[] blockerRenderers = streamBlocker.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < blockerRenderers.Length; i++)
			{
				if (blockerRenderers[i] != null && blockerRenderers[i].enabled && blockerRenderers[i].material != null)
				{
					Color blockerColor = blockerRenderers[i].material.color;
					blockerColor.a = (1 - progress) + 0.2f;
					blockerRenderers[i].material.color = blockerColor;
				}
			}
		}
		if (streamBlocker2 != null)
		{
			Renderer[] blockerRenderers = streamBlocker2.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < blockerRenderers.Length; i++)
			{
				if (blockerRenderers[i] != null && blockerRenderers[i].enabled && blockerRenderers[i].material != null)
				{
					Color blockerColor = blockerRenderers[i].material.color;
					blockerColor.a = (1 - progress) + 0.2f;
					blockerRenderers[i].material.color = blockerColor;
				}
			}
		}
		/*if(streamBlocker != null && streamBlocker2 != null)
		{

			streamBlocker.transform.localScale = new Vector3(streamBlocker.transform.localScale.x, Mathf.Min(startingSize - (startingSize / nodes.Count) * litCount, streamBlocker.transform.localScale.y), streamBlocker.transform.localScale.z);
			streamBlocker2.transform.localScale = new Vector3(streamBlocker2.transform.localScale.x, Mathf.Min(startingSize - (startingSize / nodes.Count) * litCount, streamBlocker2.transform.localScale.y), streamBlocker2.transform.localScale.z);
		}*/
		litCount = 0;

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
			if(streamBlocker != null)
				Destroy(streamBlocker);
			if(streamBlocker2 != null)
				Destroy(streamBlocker2);
			for (int i = 0; i < listeners.Count; i++)
			{
				listeners[i].SendMessage("ClusterNodesSolved", this, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}

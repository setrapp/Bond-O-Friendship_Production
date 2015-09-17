using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OptionsMenu : MonoBehaviour {


	public ClusterNodePuzzle soundOnPuzzle;
	public ClusterNodePuzzle soundOffPuzzle;

	public List<ClusterNode> soundOnNodes;
	public List<ClusterNode> soundOffNodes;	

	public bool soundChecked = false;

	public void CheckSoundSettings(bool useBondColor = true)
	{
		soundOffPuzzle.solved = Globals.Instance.mute;
		soundOnPuzzle.solved = !Globals.Instance.mute;

		foreach (ClusterNode node in soundOffNodes) {
			if (Globals.Instance.mute)
				node.GetComponent<Renderer> ().material.color = useBondColor ? node.bondColor : node.GetComponent<Renderer> ().material.color;
			else
				node.GetComponent<Renderer> ().material.color = Color.white;

			node.lit = Globals.Instance.mute;
		}


		foreach (ClusterNode node in soundOnNodes) {
			if (!Globals.Instance.mute)
				node.GetComponent<Renderer> ().material.color = useBondColor ? node.bondColor : node.GetComponent<Renderer> ().material.color;
			else
				node.GetComponent<Renderer> ().material.color = Color.white;

			node.lit = !Globals.Instance.mute;
		}

		soundChecked = true;
		
	}



	void Update()
	{
		if (Globals.Instance.mute) 
		{
			if(soundOnPuzzle.solved)
			{
				Globals.Instance.mute = false;
				CheckSoundSettings();
			}
		}

		if (!Globals.Instance.mute) 
		{
			if(soundOffPuzzle.solved)
			{
				Globals.Instance.mute = true;
				CheckSoundSettings();
			}
		}
	}

}

using UnityEngine;
using System.Collections;

public class CanvasMembraneSwitcher : MonoBehaviour {
	public NodeMonitor canvasMaker;
	public MembraneShell membraneToDestroy;
	public MembraneShell membraneToCreate;

	void Update()
	{
		if (canvasMaker != null && canvasMaker.canvas1 != null)
		{
			if (membraneToDestroy != null)
			{
				membraneToDestroy.SilentBreak();
			}
			if (membraneToCreate != null)
			{
				membraneToCreate.CreateShell();
			}
			canvasMaker = null;
		}
	}
}

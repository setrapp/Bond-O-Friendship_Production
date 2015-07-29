using UnityEngine;
using System.Collections;

public class MirroringClusterNode : ClusterNode {

    public ClusterNode nodeToMirror;
	public bool revealToNode = true;
	public bool revealToPaint = false;
    public bool revealed;

    int numOfRenderers;

    protected override void Start()
    {
        base.Start();

        numOfRenderers = nodeRenderers.Length;
        HideNode();
    }

	// Update is called once per frame
    protected override void Update() 
    {
        base.Update();

		//Reveal 
		if (nodeToMirror.lit && revealToNode)
			RevealNode();
	}

    void MirrorNode()
    {
        //revealed = nodeToMirror.lit;  
    }

	void OnTriggerEnter(Collider col)
	{
		base.OnTriggerEnter (col);

		//checking of collision with paint
		if (col.name == "PaintCircle(Clone)" && revealToPaint) {
			RevealNode ();
		}
	}

	void OnCollisionEnter(Collision col)
	{
		base.OnCollisionEnter (col);
		
		//checking of collision with paint
		if (col.gameObject.name == "PaintCircle(Clone)" && revealToPaint) {
			RevealNode ();
		}
	}

	public void RevealNode()
	{
		revealed = true;

        //enable rendering
        for (int i = 0; i < numOfRenderers; i++)
        {
            nodeRenderers[i].enabled = true;
        }
	}

    public void HideNode()
	{
		revealed = false;

        //disable rendering
        for (int i = 0; i < numOfRenderers; i++)
        {
            nodeRenderers[i].enabled = false;
        }
	}
}

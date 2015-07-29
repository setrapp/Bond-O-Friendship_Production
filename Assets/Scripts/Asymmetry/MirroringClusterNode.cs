using UnityEngine;
using System.Collections;

public class MirroringClusterNode : ClusterNode {

    public ClusterNode nodeToMirror;
	public bool revealToNode = true;
	public bool revealToPaint = false;
    public bool revealed;

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
	}

	public void HideNode()
	{
		revealed = false;
	}
}

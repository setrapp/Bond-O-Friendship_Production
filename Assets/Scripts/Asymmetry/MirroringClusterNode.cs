using UnityEngine;
using System.Collections;

public class MirroringClusterNode : ClusterNode {

    [HideInInspector] public ClusterNode nodeToMirror;
	[HideInInspector] public bool revealToNode = false;
    [HideInInspector] public bool revealToPaint = true;
    public bool revealed;
    public float revealDuration = -1;

    private float revealedAt = -1;

    private int numOfRenderers;

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
        if (revealToNode && nodeToMirror.lit)
        {
            RevealNode();
            revealedAt = Time.time;
        }

        if (revealed)
        {
			if (revealDuration > 0 && Time.time - revealedAt > revealDuration)
			{
				HideNode();
			}
        }
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
        revealedAt = Time.time;

        //enable rendering
        for (int i = 0; i < numOfRenderers; i++)
        {
            nodeRenderers[i].enabled = true;
        }
	}

    public void HideNode()
	{
        if (!lit)
        {
            revealed = false;
            //disable rendering
            for (int i = 0; i < numOfRenderers; i++)
            {
                nodeRenderers[i].enabled = false;
            }
        }
	}

	override public void CheckCollision (Collider col)
	{
		if (revealed)
		{
			base.CheckCollision(col);
		}
	}
}

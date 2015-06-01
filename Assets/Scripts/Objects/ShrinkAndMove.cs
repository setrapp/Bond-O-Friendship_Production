using UnityEngine;
using System.Collections;

public class ShrinkAndMove : ClusterNodeColorSpecific 
{
    public ShrinkAndMove otherPlayerNode;
    public bool isPlayer1 = false;

    public bool moving = false;
    public bool atTarget = false;

    public bool fullSize = false;

    private GameObject player;

    public Renderer child;

    private Vector3 playerPosNoZ;
    private Vector3 nodePosNoZ;

    public float playerNodeDis = 1.0f;
    public float toggleDis = 1.0f;

    public Transform target;
    private Vector3 startPos;
    private Vector3 endPos;

    public float duration = 2.5f;
    public float moveBackDur = 2.5f;
    private float t = 0.0f;

    private Color fullColor;
    private Color transparentColor;

    private Vector3 childStartSize;
    private Vector3 childEndSize;

	public AllowPlayerBond bondAllow;
	public AllowPlayerBond bondDisallow;

	// Use this for initialization
	override protected void Start () 
    {
        base.Start();
        startPos = transform.position;
        endPos = target.position;

        fullColor = renderer.material.color;
        transparentColor = new Color(fullColor.a, fullColor.b, fullColor.g, 0.0f);
        childStartSize = child.transform.localScale;
        childEndSize = Vector3.zero;

        if (isPlayer1)
            player = Globals.Instance.player1.gameObject;
        else
            player = Globals.Instance.player2.gameObject;
	}
	
	// Update is called once per frame
    override protected void Update() 
    {
        base.Update();

        

        if(player == null)
        {
            if (isPlayer1)
                player = Globals.Instance.player1.gameObject;
            else
                player = Globals.Instance.player2.gameObject;
        }

        playerPosNoZ = new Vector3(player.transform.position.x, player.transform.position.y, 0.0f);
        nodePosNoZ = new Vector3(transform.position.x, transform.position.y, 0.0f);

        playerNodeDis = Vector3.SqrMagnitude(playerPosNoZ - nodePosNoZ);

        if (playerNodeDis < Mathf.Pow(toggleDis, 2))
        {
            moving = true;
        }
        else
            moving = false;

        if (fullSize)
        {
            
            t = moving ? Mathf.Clamp(t + Time.deltaTime / duration, 0.0f, 1.0f) : Mathf.Clamp(t - Time.deltaTime / moveBackDur, 0.0f, 1.0f);
            transform.position = Vector3.Lerp(startPos, endPos, t);
            renderer.material.color = Color.Lerp(fullColor, transparentColor, t);
            child.transform.localScale = Vector3.Lerp(childStartSize, childEndSize, t);
            atTarget = t >= .8f;

            //if (!moving)
            //{
            //    atTarget = false;
            //}

			if (atTarget && otherPlayerNode.atTarget)
			{
				bondAllow.AllowBond();
			}
			else if (!Globals.Instance.playersBonded)
			{
				bondDisallow.AllowBond();
			}

            if (Globals.Instance.playersBonded && !lit)
            {
                lit = true;
                targetPuzzle.NodeColored();
            }
        }
	}

	public void BecomeFullSize()
	{
		fullSize = true;
		/*startPos = transform.position;
		endPos = target.position;
		childStartSize = child.transform.localScale;
		childEndSize = Vector3.zero;*/
	}

    override protected void OnTriggerEnter(Collider collide)
    {
       
    }

    void OnTriggerStay(Collider collide)
    {
       /* if (Globals.Instance.playersBonded)
        {
            CheckCollision(collide);
			bondAllow.AllowBond();
			if (bondAllow != null)
			{
				Destroy(bondAllow.gameObject);
			}
			if (bondDisallow != null)
			{
				Destroy(bondDisallow.gameObject);
			}
			
        }*/
    }
    override protected void OnCollisionEnter(Collision col)
    {
    }

    void OnTriggerExit(Collider collide)
    {
        /*if (collide.gameObject.name == "Player 1" && isPlayer1)
        {
            moving = false;
        }
        if (collide.gameObject.name == "Player 2" && !isPlayer1)
        {
            moving = false;
        }*/
    }
}

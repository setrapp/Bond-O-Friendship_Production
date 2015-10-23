using UnityEngine;
using System.Collections;

public class ShrinkAndMove : ClusterNodeColorSpecific 
{
    public ShrinkAndMove otherPlayerNode;
    public bool isPlayer1 = false;

    public ShrinkMoveDirection moving = ShrinkMoveDirection.NONE;
    public bool atTarget = false;

    public bool fullSize = false;

    private GameObject player;

    public CrumpleMesh child;
	public float activeCrumpleSpeed;
	public Vector3 crumpleReachOffset;
	public float crumpleReachSpeed;

    private Vector3 playerPosNoZ;
    private Vector3 nodePosNoZ;

    public float playerNodeDis = 1.0f;
    public float toggleDis = 1.0f;

    public Transform target;
	private Vector3 startPos;
    private Vector3 endPos;

    public float duration = 2.5f;
    public float moveBackDur = 2.5f;
    public float t = 0.0f;
	public float centerMaxRadius = 3;
	public float centerMinRadius = 1;
	private Vector3 centerStartScale;
	public TimedCameraControl endingCameraControl;

    private Color fullColor;
    public Color transparentColor;

    private Vector3 childStartSize;
    public Vector3 childEndSize;

	public AllowPlayerBond bondAllow;
	public AllowPlayerBond bondDisallow;

	public enum ShrinkMoveDirection
	{
		NONE = 0,
		FORWARD,
		BACKWARD
	}

	// Use this for initialization
	override protected void Start () 
    {
        base.Start();
        startPos = transform.position;
        endPos = target.position;

        fullColor = GetComponent<Renderer>().material.color;
        //transparentColor = new Color(fullColor.a, fullColor.b, fullColor.g, 0.25f);

        if (isPlayer1)
            player = Globals.Instance.Player1.gameObject;
        else
            player = Globals.Instance.Player2.gameObject;
	}
	
	// Update is called once per frame
    override protected void Update() 
    {
        base.Update();

        

        if(player == null)
        {
            if (isPlayer1)
                player = Globals.Instance.Player1.gameObject;
            else
                player = Globals.Instance.Player2.gameObject;
        }

        playerPosNoZ = new Vector3(player.transform.position.x, player.transform.position.y, 0.0f);
        nodePosNoZ = new Vector3(transform.position.x, transform.position.y, 0.0f);

        playerNodeDis = Vector3.SqrMagnitude(playerPosNoZ - nodePosNoZ);

		/*TODO Slow node movement when between extremes, rather than stoping completely?*/
        if (playerNodeDis < Mathf.Pow(toggleDis, 2))
        {
            moving = ShrinkMoveDirection.FORWARD;
			if (fullSize)
			{
				child.scale = activeCrumpleSpeed;
			}
        }
		else if (playerNodeDis < Mathf.Pow(toggleDis * 1.5f, 2))
		{
			moving = ShrinkMoveDirection.NONE;
		}
		else
		{
			moving = ShrinkMoveDirection.BACKWARD;
		}

        if (fullSize)
        {
            if (moving == ShrinkMoveDirection.FORWARD)
			{
				// If the other node is also ready to move, move towards the other node.
				//if(otherPlayerNode == null || otherPlayerNode.moving == ShrinkMoveDirection.FORWARD)
				{
					t = Mathf.Clamp(t + Time.deltaTime / duration, 0.0f, 1.0f);
				}

				// Reach child crumple towards the other node.
				child.centerOffset += crumpleReachOffset.normalized * crumpleReachSpeed * Time.deltaTime;
				if (child.centerOffset.sqrMagnitude > crumpleReachOffset.sqrMagnitude)
				{
					child.centerOffset = crumpleReachOffset;
				}
			}
			else if (moving == ShrinkMoveDirection.BACKWARD)
			{
				// Move away from the other node.
				t = Mathf.Clamp(t - Time.deltaTime / moveBackDur, 0.0f, 1.0f);

				// Return child to resting crumple center.
				if (child.centerOffset.sqrMagnitude < Mathf.Pow(crumpleReachSpeed * Time.deltaTime, 2))
				{
					child.centerOffset = Vector3.zero;
				}
				else
				{
					child.centerOffset -= crumpleReachOffset.normalized * crumpleReachSpeed * Time.deltaTime;
				}
			}
            transform.position = Vector3.Lerp(startPos, endPos, t);
            GetComponent<Renderer>().material.color = Color.Lerp(fullColor, transparentColor, t);
            child.transform.localScale = Vector3.Lerp(childStartSize, childEndSize, t);
			atTarget = t >= 0.8f;

            //if (!moving)
            //{
            //    atTarget = false;
            //}

			//TODO scale while atTarget
			if (t >= 1 || (!Globals.Instance.playersBonded && transform.localScale.sqrMagnitude < centerStartScale.sqrMagnitude))
			{
				Vector3 betweenPlayers = Globals.Instance.Player1.transform.position - Globals.Instance.Player2.transform.position;
				betweenPlayers.z = 0;
				float centerScaleProgress = Mathf.Clamp01(1 - ((betweenPlayers.magnitude - centerMinRadius) / (centerMaxRadius - centerMinRadius)));
				transform.localScale = (1 - centerScaleProgress) * centerStartScale;
			}

			if (atTarget && otherPlayerNode.atTarget)
			{
				bondAllow.AllowBond();
			}
			else if (!Globals.Instance.playersBonded && Globals.Instance.bondAllowed)
			{
				Globals.Instance.bondAllowed = false;
				bondDisallow.AllowBond();
			}

            if (Globals.Instance.playersBonded && !lit)
            {
                lit = true;
                targetPuzzle.NodeColored();
				if (endingCameraControl != null)
				{
					endingCameraControl.InitiateCameraControl();
				}
            }
        }
	}

	public void BecomeFullSize()
	{
		fullSize = true;
		startPos = transform.position;
		endPos = target.position;
		centerStartScale = transform.localScale;
		childStartSize = child.transform.localScale;
		childEndSize = child.transform.localScale / 2;
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

using UnityEngine;
using System.Collections;

public class CastPoints : MonoBehaviour {

	public SimpleMover mover;

	public GameObject points;

	private GameObject createdPoints;

	private Vector3 pointsPos;

	public bool isCreated = false;

	private bool n = false;
	private bool s = false;
	private bool e = false;
	private bool w = false;
	private bool ne = false;
	private bool se = false;
	private bool sw = false;
	private bool nw = false;

	// Use this for initialization
	void Start () {
		if (mover == null)
		{
			mover = GetComponent<SimpleMover>();
		}
	}
	
	// Update is called once per frame
	void Update () {

		// coordinates
		if (mover.velocity.x > 0 && mover.velocity.y == 0)
			e = true;
		else
			e = false;

		if (mover.velocity.x < 0 && mover.velocity.y == 0)
			w = true;
		else
			w = false;

		if (mover.velocity.x == 0 && mover.velocity.y > 0)
			n = true;
		else
			n = false;

		if (mover.velocity.x == 0 && mover.velocity.y < 0)
			s = true;
		else
			s = false;

		if (mover.velocity.x > 0 && mover.velocity.y > 0)
			ne = true;
		else
			ne = false;

		if (mover.velocity.x > 0 && mover.velocity.y < 0)
			se = true;
		else
			se = false;

		if (mover.velocity.x < 0 && mover.velocity.y < 0)
			sw = true;
		else
			sw = false;

		if (mover.velocity.x < 0 && mover.velocity.y > 0)
			nw = true;
		else
			nw = false;

		pointsPos = gameObject.transform.position;
	
	}

	void StartPoints()
	{
		if(!isCreated)
		{
			isCreated = true;
			if(n)
				createdPoints = (GameObject)Instantiate(points, pointsPos, Quaternion.Euler(0,0,270));

			if(s)
				createdPoints = (GameObject)Instantiate(points, pointsPos, Quaternion.Euler(0, 0, 90));

			if(e)
				createdPoints = (GameObject)Instantiate(points, pointsPos, Quaternion.Euler(0, 0, 180));

			if(w)
				createdPoints = (GameObject)Instantiate(points, pointsPos, Quaternion.Euler(0, 0, 0));

			if(ne)
				createdPoints = (GameObject)Instantiate(points, pointsPos, Quaternion.Euler(0, 0, 225));

			if(se)
				createdPoints = (GameObject)Instantiate(points, pointsPos, Quaternion.Euler(0, 0, 135));

			if(sw)
				createdPoints = (GameObject)Instantiate(points, pointsPos, Quaternion.Euler(0, 0, 45));

			if(nw)
				createdPoints = (GameObject)Instantiate(points, pointsPos, Quaternion.Euler(0, 0, 315));
			
			// Store creator of points.
			Detail[] details = createdPoints.GetComponentsInChildren<Detail>();
			for (int i = 0; i < details.Length; i++)
			{
				details[i].creator = gameObject;
			}
			MiniPoint[] miniPoints = createdPoints.GetComponentsInChildren<MiniPoint>();
			for (int i = 0; i < miniPoints.Length; i++)
			{
				miniPoints[i].creator = gameObject;
			}
			LongDetail[] longdetails = createdPoints.GetComponentsInChildren<LongDetail>();
			for (int i = 0; i < longdetails.Length; i++)
			{
				longdetails[i].creator = gameObject;
			}
			LongPoint[] longPoints = createdPoints.GetComponentsInChildren<LongPoint>();
			for (int i = 0; i < longPoints.Length; i++)
			{
				longPoints[i].creator = gameObject;
			}
			DeepDetail[] deepdetails = createdPoints.GetComponentsInChildren<DeepDetail>();
			for (int i = 0; i < deepdetails.Length; i++)
			{
				deepdetails[i].creator = gameObject;
			}
			DeepPoint[] deepPoints = createdPoints.GetComponentsInChildren<DeepPoint>();
			for (int i = 0; i < deepPoints.Length; i++)
			{
				deepPoints[i].creator = gameObject;
			}
			FinalPoint[] finalPoints = createdPoints.GetComponentsInChildren<FinalPoint>();
			for (int i = 0; i < finalPoints.Length; i++)
			{
				finalPoints[i].creator = gameObject;
			}
		}
	}

	public void CanCreatePoints()
	{
		isCreated = false;
	}


}


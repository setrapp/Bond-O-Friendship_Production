using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerLooping : MonoBehaviour {
	
	public GameObject player;

	private float moveDistanceVertical = 0;
	private float moveDistanceHorizontal = 0;

	public enum ColliderLocation{Top,Bottom,Left,Right};

	public float worldWidth = 200f;
	public float worldHeight = 200f;

	void Start()
	{
		ChangeWorldSize(worldWidth, worldHeight);
	}

	public void ChangeWorldSize(float worldWidth, float worldHeight)
	{
		foreach(Transform child in transform)
		{
			
			switch(child.GetComponent<Boundary>().colliderLocation)
			{
				case ColliderLocation.Top:
					//Resize Collider
					child.GetComponent<BoxCollider>().size = new Vector3(worldWidth, 1.0f, 1.0f);
					child.transform.localPosition = new Vector3(0f, worldHeight/2, 0f);
					break;
				case ColliderLocation.Bottom:
					//Resize Collider
					child.GetComponent<BoxCollider>().size = new Vector3(worldWidth, 1.0f, 1.0f);
					child.transform.localPosition = new Vector3(0f, -worldHeight/2, 0f);
					break;
				case ColliderLocation.Left:
					//Resize Collider
					child.GetComponent<BoxCollider>().size = new Vector3(1.0f, worldHeight, 1.0f);
					child.transform.localPosition = new Vector3(-worldWidth/2, 0f, 0f);
					break;
				case ColliderLocation.Right:
					//Resize Collider
					child.GetComponent<BoxCollider>().size = new Vector3(1.0f, worldHeight, 1.0f);
					child.transform.localPosition = new Vector3(worldWidth/2, 0f, 0f);
					break;
			}
		}

		moveDistanceHorizontal = worldWidth - 10f;
		moveDistanceVertical = worldHeight - 10f;
	}


	public void MoveWorld(ColliderLocation location)
	{
		Vector3 moveDistance = Vector3.zero;

		switch(location)
		{
			case ColliderLocation.Top:
				moveDistance = new Vector3(0f, moveDistanceVertical, 0f);
				break;
			case ColliderLocation.Bottom:
			moveDistance = new Vector3(0f, -moveDistanceVertical, 0f);
				break;
			case ColliderLocation.Left:
			moveDistance = new Vector3(-moveDistanceHorizontal, 0f, 0f);
				break;
			case ColliderLocation.Right:
			moveDistance = new Vector3(moveDistanceHorizontal,0f,0f);
				break;
		}

		//Move the boundaries
		transform.position += moveDistance;

		//Get all gameobjects in the scene and then filter to get only loopable objects//
		GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
		List<GameObject> loopableObjects = new List<GameObject>();
		foreach(GameObject go in allObjects)
		{
			if(go.GetComponent<LoopTag>() != null)
					loopableObjects.Add(go);
		}

		foreach(GameObject lo in loopableObjects)
		{
			
			//Check if the object is on screen
			if (lo != null && !OnScreen(lo) && (lo.GetComponent<LoopTag>().stayOutsideBounds || !OutSideBounds(lo,location,moveDistance)))
			{
				Vector3 oldPosition = lo.transform.position;

				//Debug.Log(OnScreen(lo));
				moveDistance += MoveOffScreen(lo, Vector3.zero, location);

				if (lo.GetComponent<LoopTag>().moveRoot)
					lo.transform.root.position += moveDistance;
				else
					lo.transform.position += moveDistance;

				GameObject tracerGameObject = lo;
				Tracer tracer = lo.GetComponent<Tracer>();
				while (tracer == null && tracerGameObject != null && tracerGameObject.transform.parent != null)
				{
					tracerGameObject = tracerGameObject.transform.parent.gameObject;
					tracer = tracerGameObject.GetComponent<Tracer>();
				}
				if (tracer != null)
				{
					tracer.MoveVertices(lo.transform.position - oldPosition);
				}
			}
		}

		
	}

	public void MoveIndividual(ColliderLocation location, GameObject individual, Tracer tracer)
	{
			if (!OnScreen(individual))
			{
				Vector3 moveDistance = Vector3.zero;
				switch(location)
				{
				case ColliderLocation.Top:
					moveDistance = new Vector3(0f, -moveDistanceVertical, 0f);
					break;
				case ColliderLocation.Bottom:
					moveDistance = new Vector3(0f, moveDistanceVertical, 0f);
					break;
				case ColliderLocation.Left:
					moveDistance = new Vector3(moveDistanceHorizontal, 0f, 0f);
					break;
				case ColliderLocation.Right:
					moveDistance = new Vector3(+moveDistanceHorizontal,0f,0f);
					break;
				}

				//Tracer otherTracer = individual.GetComponent<Tracer>();
				Vector3 oldPosition = individual.transform.position;

				moveDistance += MoveOffScreen(individual.gameObject, moveDistance, location);
				if (individual.gameObject.GetComponent<LoopTag>().moveRoot)
				{
					individual.gameObject.transform.root.position += moveDistance;
				}
				else
				{
					individual.gameObject.transform.position += moveDistance;
				}

				if (tracer != null)
				{
					tracer.MoveVertices(individual.transform.position - oldPosition);
					//tracer.DestroyLine();
					//tracer.SendMessage("TailStartFollow", SendMessageOptions.DontRequireReceiver);
				}
			}
	}

	private bool OutSideBounds(GameObject lo, ColliderLocation colliderLocation, Vector3 offset)
	{
		switch (colliderLocation)
		{
		case ColliderLocation.Top:
			return lo.transform.position.y > transform.position.y + offset.y;
		case ColliderLocation.Bottom:
			return lo.transform.position.y < transform.position.y + offset.y;
		case ColliderLocation.Left:
			return lo.transform.position.x < transform.position.x + offset.y;
		case ColliderLocation.Right:
			return lo.transform.position.x > transform.position.x + offset.y;
		}

		return false;
	}

	private bool OnScreen(GameObject lo)
	{
		return OnScreen(lo, Vector3.zero);
	}

	private bool OnScreen(GameObject lo, Vector3 offset)
	{
		Vector3 newPos = Camera.main.WorldToViewportPoint(lo.transform.position + offset);

		bool onScreen = false;
		
		if(newPos.x > -0.01f && newPos.x < 1.01f && newPos.y > -0.01f && newPos.y < 1.01f)
			onScreen = true;
		return onScreen;
	}

	private Vector3 MoveOffScreen(GameObject lo, Vector3 offset, ColliderLocation colliderLocation)
	{
		Vector3 offScreenMove= Vector3.zero;

		if (OnScreen(lo, offset))
		{
			var camHeight = Camera.main.orthographicSize;
			var camWidth = camHeight * Camera.main.aspect;
			//Switch to see which direction to move by viewport size
			switch (colliderLocation)
			{
			case ColliderLocation.Top:
				//Debug.Log (camHeight);
				//Debug.Log(lo.transform.position);
				offScreenMove += new Vector3(0, camHeight, 0);
				//Debug.Log(lo.transform.position);
				break;
			case ColliderLocation.Bottom:
				offScreenMove -= new Vector3(0,camHeight, 0);
				break;
			case ColliderLocation.Left:
				offScreenMove -= new Vector3(camWidth, 0, 0);
				break;
			case ColliderLocation.Right:
				offScreenMove += new Vector3(camWidth, 0, 0);
				break;
			}
		}

		if(OnScreen(lo, offset + offScreenMove))
		{
			offScreenMove += MoveOffScreen(lo, offset + offScreenMove, colliderLocation);
		}

		return offScreenMove;
	}


}

using UnityEngine;
using System.Collections;

public class DistanceLine : MonoBehaviour {

	public Transform target;
	private LineRenderer linerenderer;

	void Awake()
	{
		linerenderer = gameObject.GetComponent<LineRenderer>();
	}
	/*void OnDrawGizmosSelected() {
		if (target != null) {

			//if(Vector3.Distance(transform.position, target.position) > 20)
			//{
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(transform.position, target.position);
			//}

			if(Vector3.Distance(transform.position, target.position) > 30)
				target = null;
		}
	}*/

	void Update()
	{
		if (target != null) 
		{
			linerenderer.SetPosition(0, transform.position);
			linerenderer.SetPosition(1, target.position);

			if(Vector3.Distance(transform.position, target.position) > 30)
			{

				linerenderer.SetPosition(0, new Vector3(0,0,0));
				linerenderer.SetPosition(1, new Vector3(0,0,0));
				target = null;

			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		//Debug.Log("Hit");
		target = other.gameObject.transform;
	}


}

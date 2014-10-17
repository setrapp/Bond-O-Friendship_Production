using UnityEngine;
using System.Collections;

public class SimpleConnection : MonoBehaviour {
	public bool connected = false;
	public LineRenderer lineRenderer1;
	public LineRenderer lineRenderer2;
	public GameObject attachPoint1;
	public GameObject attachPoint2;
	public float distancePerDrain;
	public float drained = 0;
	public float endsWidth;
	public float midWidth;

	void Update()
	{
		// Only enable line renderers while connected;
		lineRenderer1.enabled = lineRenderer2.enabled = connected;

		if (connected)
		{
			// Determine how much has been drained from the partners.
			float maxDistance = (distancePerDrain) * ((attachPoint1.transform.localScale + attachPoint2.transform.localScale) / 2).magnitude;
			float actualDrain = (attachPoint2.transform.position - attachPoint1.transform.position).magnitude / maxDistance;
			drained = Mathf.Clamp(actualDrain, 0, 1);

			// Base the width of the connection on how much has been drained beyond the partners' capacity.
			float actualMidWidth = midWidth * Mathf.Clamp(1 - (actualDrain - drained), 0, 1);

			// Set connection points.
			Vector3 midpoint = (attachPoint1.transform.position + attachPoint2.transform.position) / 2;
			lineRenderer1.SetPosition(0, attachPoint1.transform.position);
			lineRenderer1.SetPosition(1, midpoint);
			lineRenderer2.SetPosition(0, attachPoint2.transform.position);
			lineRenderer2.SetPosition(1, midpoint);
			lineRenderer1.SetWidth(endsWidth, actualMidWidth);
			lineRenderer2.SetWidth(endsWidth, actualMidWidth);

			// Disconnect if too far apart.
			if (actualMidWidth <= 0)
			{
				connected = false;
			}
		}
	}
}

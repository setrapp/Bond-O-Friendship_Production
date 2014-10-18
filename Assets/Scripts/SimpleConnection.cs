using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleConnection : MonoBehaviour {
	public bool connected = false;
	public LineRenderer lineRenderer1;
	public LineRenderer lineRenderer2;
	public PartnerLink partner1;
	public PartnerLink partner2;
	public float minDistanceToDrain;
	public float distancePerDrain;
	public float drained = 0;
	public float endsWidth;
	public float midWidth;
	public float pulseSpeed;
	public float pulseAmplitude;
	public List<PulsePoint> pulsePoints;
	public int nextFluctuationDirection = 1;

	void Start()
	{
		pulsePoints = new List<PulsePoint>();
	}

	void Update()
	{
		// Only enable line renderers while connected;
		lineRenderer1.enabled = lineRenderer2.enabled = connected;

		if (connected)
		{
			// Update all pulse points.
			int vertexCount1 = 1, vertexCount2 = 1;
			Vector3 fluctuation = Vector3.Cross((partner2.transform.position - partner1.transform.position).normalized, Vector3.forward) * pulseAmplitude;
			for (int i = 0; i < pulsePoints.Count; i++)
			{
				pulsePoints[i].position += (pulsePoints[i].target.attachPoint.transform.position - pulsePoints[i].position).normalized * pulseSpeed;

				if (Vector3.Dot(pulsePoints[i].position - pulsePoints[i].target.attachPoint.transform.position, pulsePoints[i].target.attachPoint.transform.position - pulsePoints[i].target.transform.position) < 0)
				{
					pulsePoints.RemoveAt(i);
					i--;
				}
				else
				{
					if ((pulsePoints[i].position - partner1.transform.position).sqrMagnitude < (pulsePoints[i].position - partner2.transform.position).sqrMagnitude)
					{
						vertexCount1++;
						lineRenderer1.SetVertexCount(vertexCount1);
						lineRenderer1.SetPosition(vertexCount1 - 1, pulsePoints[i].position + (fluctuation * pulsePoints[i].fluctuationDirection));
					}
					else
					{
						vertexCount2++;
						lineRenderer2.SetVertexCount(vertexCount2);
						lineRenderer2.SetPosition(vertexCount2 - 1, pulsePoints[i].position + (fluctuation * pulsePoints[i].fluctuationDirection));
					}
				}
			}
			vertexCount1++;
			lineRenderer1.SetVertexCount(vertexCount1);
			vertexCount2++;
			lineRenderer2.SetVertexCount(vertexCount2);

			// Determine how much has been drained from the partners.
			float maxDistance = (distancePerDrain) * ((partner1.attachPoint.transform.localScale + partner2.attachPoint.transform.localScale) / 2).magnitude;
			float actualDrain = ((partner2.attachPoint.transform.position - partner1.attachPoint.transform.position).magnitude - minDistanceToDrain) / maxDistance;
			partner1.fillScale = Mathf.Clamp(partner1.fillScale - (actualDrain - drained), 0, 1);
			partner2.fillScale = Mathf.Clamp(partner2.fillScale - (actualDrain - drained), 0, 1);
			drained = Mathf.Clamp(actualDrain, 0, 1);

			// Base the width of the connection on how much has been drained beyond the partners' capacity.
			float actualMidWidth = midWidth * Mathf.Clamp(1 - (actualDrain - drained), 0, 1);

			// Set connection points.
			Vector3 midpoint = (partner1.attachPoint.transform.position + partner2.attachPoint.transform.position) / 2;
			lineRenderer1.SetPosition(0, partner1.attachPoint.transform.position);
			lineRenderer1.SetPosition(vertexCount1 - 1, midpoint);
			lineRenderer2.SetPosition(0, midpoint);
			lineRenderer2.SetPosition(vertexCount2 - 1, partner2.attachPoint.transform.position);
			lineRenderer1.SetWidth(endsWidth, actualMidWidth);
			lineRenderer2.SetWidth(actualMidWidth, endsWidth);

			// Disconnect if too far apart.
			if (actualMidWidth <= 0)
			{
				connected = false;
			}
		}
	}

	public bool SendPulse(PartnerLink start, PartnerLink target)
	{
		// Only allow pulse between partners sharing this connection.
		if ((start != partner1 || target != partner2) && (start != partner2 || target != partner1))
		{
			return false;
		}
		
		// Create new pulse moving from start to target.
		PulsePoint newPulse = new PulsePoint();
		newPulse.position = start.attachPoint.transform.position;
		newPulse.target = target;
		newPulse.fluctuationDirection = nextFluctuationDirection;
		nextFluctuationDirection *= -1;

		// Add pulse point to line.
		if (start == partner1)
		{
			pulsePoints.Insert(0, newPulse);
		}
		else
		{
			pulsePoints.Add(newPulse);
		}

		return true;
	}

	/*public SortPulsePoints()
	{
		for (int i = 0; i < pulsePoints.Count; i++)
		{
			bool iSorted = false;
			for (int j = i  + 1; i < pulsePoints.Count && !iSorted; j++)
			{
				//pulsePoints.
			}
		}
	}*/
}

public class PulsePoint
{
	public Vector3 position;
	public float partner1SqrDist;
	public PartnerLink target;
	public int fluctuationDirection;
}

/*public class PulsePointComparer : IComparer<PulsePoint>
{
	//public int Compare()
}*/

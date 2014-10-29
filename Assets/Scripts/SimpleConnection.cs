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
	public float scalePerPulse;
	public float drained = 0;
	public float endsWidth;
	public float midWidth;
	public float pulseSpeed;
	public float pulseAmplitude;
	public List<PulsePoint> pulsePoints;
	public int nextFluctuationDirection = 1;
	public bool bouncePulseOnReject = false;
	public float partnerDist;
	public GameObject Shield;
	private GameObject bondCollider;


	void Start()
	{
		pulsePoints = new List<PulsePoint>();
	}

	void Update()
	{
		partnerDist = Vector3.Distance(partner1.transform.position,partner2.transform.position);
		Shield.transform.position = (partner1.transform.position + partner2.transform.position) * 0.5f;
		// Only enable line renderers while connected;
		lineRenderer1.enabled = lineRenderer2.enabled = connected;

		if (connected)
		{
			// Update all pulse points.
			int vertexCount1 = 1, vertexCount2 = 1;
			Vector3 fluctuation = Vector3.Cross((partner2.transform.position - partner1.transform.position).normalized, Vector3.forward) * pulseAmplitude;
			for (int i = 0; i < pulsePoints.Count; i++)
			{
				// Move based on pulse speed.
				pulsePoints[i].position += (pulsePoints[i].target.attachPoint.transform.position - pulsePoints[i].position).normalized * pulseSpeed * Time.deltaTime;
				// Prevent pulses from lagging behind partners moving, but positioning relative to partner1.
				Vector3 relativePosition = Helper.ProjectVector(partner2.transform.position - partner1.transform.position, pulsePoints[i].position - partner1.transform.position);
				pulsePoints[i].position = partner1.transform.position + relativePosition;
				// Update tracked square distance from partner1.
				pulsePoints[i].partner1SqrDist = relativePosition.sqrMagnitude;	
			}

			// Sort pulse points on based how far they are from partner1.
			pulsePoints.Sort(new PulsePointComparer());

			// Added points to line renderers.
			/*for (int i = 0; i < pulsePoints.Count; i++)
			{
				if (Vector3.Dot(pulsePoints[i].position - pulsePoints[i].target.attachPoint.transform.position, pulsePoints[i].target.attachPoint.transform.position - pulsePoints[i].target.transform.position) < 0)
				{
					// Remove pulse when it reaches target.
					bool accepted = AcceptPulse(pulsePoints[i].target, pulsePoints[i].bounced, !pulsePoints[i].bounced);
					if (!accepted && bouncePulseOnReject && !pulsePoints[i].bounced)
					{
						pulsePoints[i].bounced = true;
						pulsePoints[i].fluctuationDirection *= -1;
						if (pulsePoints[i].target = partner1)
						{
							pulsePoints[i].target = partner2;
						}
						else
						{
							pulsePoints[i].target = partner1;
						}
					}
					else
					{
						pulsePoints.RemoveAt(i);
						i--;
					}
				}
				else
				{
					// Move pulse through the line it is nearest to.
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
			}*/

			// Add vertex space for end vertex.
			vertexCount1++;
			lineRenderer1.SetVertexCount(vertexCount1);
			vertexCount2++;
			lineRenderer2.SetVertexCount(vertexCount2);

			// Determine how much has been drained from the partners.
			float maxDistance = (distancePerDrain) * (Mathf.Min(partner1.transform.localScale.x, partner2.transform.localScale.x));
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

			if(partnerDist < 1.5f)
			{
				Shield.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
			}
			else
				Shield.SendMessage("DeActivate", SendMessageOptions.DontRequireReceiver);
		
			if(bondCollider == null)
			{
				bondCollider = new GameObject();
				bondCollider.name = "Bond Collider";
				bondCollider.AddComponent<BoxCollider>();
			}
			bondCollider.transform.position = (partner1.transform.position + partner2.transform.position)/2;
			bondCollider.transform.localScale = new Vector3(Vector3.Distance(partner1.transform.position, partner2.transform.position), 0.5f, 10.0f);
			bondCollider.transform.up = Vector3.Cross(partner1.transform.position - partner2.transform.position, Vector3.forward);
		}

		if(!connected && bondCollider != null)
			Destroy(bondCollider);

		if(partnerDist > 1)
		{

		}
	}

	/*public bool SendPulse(PartnerLink start, PartnerLink target)
	{
		//float maxDistance = (distancePerDrain) * (Mathf.Min(partner1.transform.localScale.x, partner2.transform.localScale.x));

		// Only allow pulse when start can afford it and the link will not become breakable.
		float newMaxDistance = distancePerDrain * start.transform.localScale.x;
		if (start.transform.localScale.x - scalePerPulse < start.minScale || Mathf.Pow(newMaxDistance, 2) < (start.attachPoint.transform.position - target.attachPoint.transform.position).sqrMagnitude)
		{
			return false;
		}

		// Only allow pulse between partners sharing this connection.
		if ((start != partner1 || target != partner2) && (start != partner2 || target != partner1))
		{
			return false;
		}
		
		// Create new pulse moving from start to target.
		PulsePoint newPulse = new PulsePoint();
		newPulse.position = start.attachPoint.transform.position;
		newPulse.partner1SqrDist = (newPulse.position - partner1.transform.position).sqrMagnitude;
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

		// Scale the sender down.
		Vector3 localScale = start.transform.localScale;
		start.transform.localScale = new Vector3(localScale.x - scalePerPulse, localScale.y - scalePerPulse, localScale.z - scalePerPulse);
		
		return true;
	}

	private bool AcceptPulse(PartnerLink target, bool forceAccept, bool exceedMax)
	{
		// Only accept if the target is connected.
		if (target != partner1 && target != partner2)
		{
			return false;
		}

		// Only accept if the target is prepared to accept or acceptance is forced.
		//if (!target.preparingPulse && !forceAccept)
		//{
		//	return false;
		//}

		// Scale target up.
		Vector3 localScale = target.transform.localScale;
		target.transform.localScale = new Vector3(localScale.x + scalePerPulse, localScale.y + scalePerPulse, localScale.z + scalePerPulse);

		if (!exceedMax)
		{
			target.transform.localScale = new Vector3(Mathf.Min(target.transform.localScale.x, target.normalScale), Mathf.Min(target.transform.localScale.y, target.normalScale), Mathf.Min(target.transform.localScale.z, target.normalScale));
		}

		return true;
	}*/
}

public class PulsePoint
{
	public Vector3 position;
	public float partner1SqrDist;
	public PartnerLink target;
	public int fluctuationDirection;
	public bool bounced;
}

public class PulsePointComparer : IComparer<PulsePoint>
{
	public int Compare(PulsePoint a, PulsePoint b)
	{
		if (a == null && b == null)
		{
			return 0;
		}
		else if (a == null)
		{
			return -1;
		}
		else if (b == null)
		{
			return 1;
		}
		else if (a.partner1SqrDist < b.partner1SqrDist)
		{
			return -1;
		}
		else if (b.partner1SqrDist < a.partner1SqrDist)
		{
			return 1;
		}
		return 0;
	}
}

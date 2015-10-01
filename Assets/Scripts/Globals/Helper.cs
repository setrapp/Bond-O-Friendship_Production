using UnityEngine;
using System.Collections;

public class Helper {
	public static Vector3 ProjectVector(Vector3 baseDirection, Vector3 projectee)
	{
		if (baseDirection.sqrMagnitude != 1)
		{
			baseDirection.Normalize();
		}
		
		float projecteeMag = projectee.magnitude;
		float projecteeDotBase = 0;
		if (projecteeMag > 0)
		{
			projecteeDotBase = Vector3.Dot(projectee / projecteeMag, baseDirection);
		}
		Vector3 projection = baseDirection * projecteeMag * projecteeDotBase;
		return projection;
	}

	public static Vector3 RotateVector(Vector3 baseVector, Vector3 targetVector, Vector3 axis)
	{
		float angle = Helper.AngleDegrees(baseVector, targetVector, axis);
		Quaternion rotation = Quaternion.AngleAxis(angle, axis);
		return rotation * baseVector;
	}

	public static float AngleDegrees(Vector3 baseVector, Vector3 targetVector, Vector3 axis)
	{
		float angle = Vector3.Angle(baseVector, targetVector);
		if (Vector3.Dot(Vector3.Cross(baseVector, targetVector), axis) < 0)
		{
			angle *= -1;
		}
		return angle;
	}

	public static float AngleRadians(Vector3 baseVector, Vector3 targetVector, Vector3 axis)
	{
		return AngleDegrees(baseVector, targetVector, axis) * Mathf.Deg2Rad;
	}

	public static Vector3 RotateTowards2D(Vector3 current, Vector3 desired, float maxRadiansDelta, float maxMagnitudeDelta, Vector3 sideTest = new Vector3())
	{
		Vector3 newCurrent = current;
		if (desired.sqrMagnitude > 0 && desired != current)
		{
			if (Vector3.Dot(desired, current) < 0)
			{
				Vector3 newDesire = Vector3.Cross(current, -Vector3.forward);
				float desireDotNew = Vector3.Dot(desired, newDesire);
				if (sideTest.sqrMagnitude > 0 && (desireDotNew < 0 || (desireDotNew == 0 && Vector3.Dot(sideTest, newDesire) < 0)))
				{
					newDesire *= -1;
				}
				desired = newDesire;
			}
			newCurrent = Vector3.RotateTowards(current, desired, maxRadiansDelta, maxMagnitudeDelta);
		}
		return newCurrent;
	}

	public static void DrawCircle(LineRenderer renderer, GameObject parentObject, Vector3 offset, float radius, float thetaDelta = 0.01f)
	{
		Vector3 center = offset;
		if (parentObject != null)
		{
			center = parentObject.transform.TransformPoint(offset);
		}

		int vertexCount = (int)((Mathf.PI * 2) / thetaDelta) + 2;
		renderer.SetVertexCount(vertexCount);
		for (int i = 0; i < vertexCount - 1; i++)
		{
			float theta = i * thetaDelta;
			Vector3 unitCirclePoint = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);

			renderer.SetPosition(i, center + (unitCirclePoint * radius));
		}
		renderer.SetPosition(vertexCount - 1, center + new Vector3(radius, 0, 0));
	}

	public static RingPulse FirePulse(Vector3 position, PulseStats pulseStats, RingPulse alternativePulsePrefab = null)
	{
		RingPulse pulsePrefab = alternativePulsePrefab;
		if (pulsePrefab == null)
		{
			pulsePrefab = Globals.Instance.defaultPulsePrefab;
		}
		if (pulsePrefab == null)
		{
			return null;
		}

		RingPulse pulse = ((GameObject)GameObject.Instantiate(pulsePrefab.gameObject, position, Quaternion.identity)).GetComponent<RingPulse>();
		pulse.scaleRate = pulseStats.scaleRate;
		pulse.lifeTime = pulseStats.lifeTime;
		pulse.fadeAlpha = pulseStats.fadeAlpha;
		pulse.alpha = pulseStats.alpha;
		pulse.smallRing = pulseStats.smallRing;
		pulse.pauseInterval = pulseStats.pauseInterval;
		pulse.retractInterval = pulseStats.retractInterval;

		return pulse;
	}

	/*public static RingPulse FirePulseArray(int pulseCount, float pulseInterval, Vector3 position, PulseStats pulseStats, RingPulse alternativePulsePrefab = null)
	{
		
	}

	private IEnumerable ProcessPulseArray(int pulseCount, float pulseInterval, Vector3 position, PulseStats pulseStats, RingPulse alternativePulsePrefab = null)
	{
		int pulsesFired = 0;

		while(pulsesFired < pulseCount)
		{
			FirePulse(position, pulseStats, alternativePulsePrefab);
			yield return null;
		}

	}*/
}

[System.Serializable]
public class PulseStats
{
	public float scaleRate = 10;
	public float lifeTime = 2;
	public bool fadeAlpha = true;
	public float alpha = 1;
	public bool smallRing;
	[Header("Optional Retraction")]
	public float pauseInterval = -1;
	public float retractInterval = -1;
}

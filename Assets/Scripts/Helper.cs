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
}

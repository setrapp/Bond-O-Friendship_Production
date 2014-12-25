using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class ShapingPoint : MonoBehaviour {
	public Rigidbody body;
	public float shapingForce = -1;
	[HideInInspector]
	public float lodShapingForce = -1;
}

[System.Serializable]
public class ShapingPointStats
{
	public string pointName = null;
	public Vector2 position;
	public float shapingForce = -1.0f;
}

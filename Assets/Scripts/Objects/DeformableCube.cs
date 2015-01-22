using UnityEngine;
using System.Collections;

public class DeformableCube : MonoBehaviour {
	public CubeBits pushableBits;
	public CubeBits nonPushableBits;

	void Update()
	{
		Vector3 bottomLeftPos = pushableBits.bottomLeft.transform.position;
		Vector3 bottomRightPos = pushableBits.bottomRight.transform.position;
		Vector3 topLeftPos = pushableBits.topLeft.transform.position;
		Vector3 topRightPos = pushableBits.topRight.transform.position;

		transform.position = (bottomLeftPos + bottomRightPos + topLeftPos + topRightPos) / 4;

		pushableBits.bottomLeft.transform.position = bottomLeftPos;
		pushableBits.bottomRight.transform.position = bottomRightPos;
		pushableBits.topLeft.transform.position = topLeftPos;
		pushableBits.topRight.transform.position = topRightPos;


		nonPushableBits.bottomLeft.transform.position = pushableBits.bottomLeft.transform.position;
		nonPushableBits.bottomRight.transform.position = pushableBits.bottomRight.transform.position;
		nonPushableBits.topLeft.transform.position = pushableBits.topLeft.transform.position;
		nonPushableBits.topRight.transform.position = pushableBits.topRight.transform.position;
	}
}

[System.Serializable]
public class CubeBits
{
	public GameObject bottomLeft;
	public GameObject bottomRight;
	public GameObject topLeft;
	public GameObject topRight;
}

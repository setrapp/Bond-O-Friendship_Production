using UnityEngine;
using System.Collections;

public class SpinPadWallSync : MonoBehaviour {

	public GameObject wallEnd1;
	public GameObject wallEnd2;
	public GameObject rotatee;
	//public GameObject centerBlock;
	public float spinRadius = 5.5f;
	private Vector3 center;
	private Vector3 oldWallEndPos1;
	private Vector3 oldWallEndPos2;
	public float currentRotation;

	void Start()
	{
		center = (wallEnd1.transform.position + wallEnd2.transform.position) / 2;
		rotatee.transform.position = center;
		oldWallEndPos1 = wallEnd1.transform.position;
		oldWallEndPos2 = wallEnd2.transform.position;
	}

	void Update()
	{
		/*float blockRadius = centerBlock.transform.localScale.x / 2;
		float endRadius1 = wallEnd1.transform.localScale.x / 2;
		float endRadius2 = wallEnd2.transform.localScale.x / 2;*/

		Vector3 toWallEnd1 = wallEnd1.transform.position - center;
		toWallEnd1.z = 0;
		toWallEnd1 = toWallEnd1.normalized * (spinRadius);

		Vector3 toWallEnd2 = wallEnd2.transform.position - center;
		toWallEnd2.z = 0;
		toWallEnd2 = toWallEnd2.normalized * (spinRadius);



		Vector3 newWallEndPos1 = center + toWallEnd1;
		Vector3 newWallEndPos2 = center + toWallEnd2;

		if ((newWallEndPos2 - oldWallEndPos2).sqrMagnitude < (newWallEndPos1 - oldWallEndPos1).sqrMagnitude)
		{
			newWallEndPos1 = center - toWallEnd2;
		}
		else
		{
			newWallEndPos2 = center - toWallEnd1;
		}

		/*float rotationCosine = Vector3.Dot(newWallEndPos1.normalized, oldWallEndPos1.normalized);
		if (newWallEndPos1 != oldWallEndPos1)
		{
			currentRotation += Mathf.Acos(rotationCosine) * Mathf.Rad2Deg;
		}*/
		rotatee.transform.LookAt(wallEnd1.transform, -Vector3.forward) ;

		wallEnd1.transform.position = new Vector3(newWallEndPos1.x, newWallEndPos1.y, wallEnd1.transform.position.z);
		wallEnd2.transform.position = new Vector3(newWallEndPos2.x, newWallEndPos2.y, wallEnd2.transform.position.z);

		oldWallEndPos1 = newWallEndPos1;
		oldWallEndPos2 = newWallEndPos2;
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BondingFunnel : MonoBehaviour {

	public Rigidbody pushee;
	public Rigidbody outPushee;
	public GameObject pusheeCap1;
	public GameObject pusheeCap2;
	public GameObject bearingPusher;
	public GameObject scaleBearing1;
	public GameObject scaleBearing2;
	public GameObject scaleBackupBearing1;
	public GameObject scaleBackupBearing2;
	public Rigidbody noBacktrackBody1;
	public Rigidbody noBacktrackBody2;
	public bool solved = false;
	[SerializeField]
	public List<GameObject> solveListeners;
	public float scaleModification = 0;
	public float shrinkBearingDistance = 3.5f;
	public float destroyBearingDistance = 0.5f;
	public float destroyBackupDistance = 24;
	private float pusheeLocalY = 0;
	private bool funnelingOut = false;
	private float capXZRatio = 1;

	void Start()
	{
		pusheeLocalY = pushee.transform.localPosition.y;
		noBacktrackBody1.centerOfMass = Vector3.zero;
		noBacktrackBody2.centerOfMass = Vector3.zero;
	}

	void Update()
	{
		if (scaleBearing1 != null && scaleBearing2 != null)
		{
			Vector3 pusheeScale = pushee.transform.localScale;
			pusheeScale.y = Mathf.Abs(scaleBearing2.transform.localPosition.y - scaleBearing1.transform.localPosition.y) + scaleModification;

			if (!funnelingOut && pusheeScale.y <= shrinkBearingDistance)
			{
				float bearingScale = (pusheeScale.y / 2);
				scaleBearing1.transform.localScale = new Vector3(bearingScale, scaleBearing1.transform.localScale.y, bearingScale);
				scaleBearing2.transform.localScale = new Vector3(bearingScale, scaleBearing2.transform.localScale.y, bearingScale);

				if (pusheeScale.y <= destroyBearingDistance)
				{
					/*Vector3 bearingPusherPos = bearingPusher.transform.localPosition;
					Vector3 bearingPusherScale = bearingPusher.transform.localScale;
					bearingPusher.transform.parent = outPushee.transform;
					bearingPusher.transform.localPosition = bearingPusherPos;
					bearingPusher.transform.localScale = bearingPusherScale;

					Vector3 cap1Pos = pusheeCap1.transform.localPosition;
					pusheeCap1.transform.parent = outPushee.transform;
					pusheeCap1.transform.localPosition = cap1Pos;
					pusheeCap1.layer = outPushee.gameObject.layer;

					Vector3 cap2Pos = pusheeCap2.transform.localPosition;
					pusheeCap2.transform.parent = outPushee.transform;
					pusheeCap2.transform.localPosition = cap2Pos;
					pusheeCap2.layer = outPushee.gameObject.layer;
	
					Destroy(pushee.gameObject);
					pushee = outPushee;
					outPushee = null;

					Destroy(scaleBearing1);
					scaleBearing1 = scaleBackupBearing1;
					scaleBackupBearing1 = null;

					Destroy(scaleBearing2);
					scaleBearing2 = scaleBackupBearing2;
					scaleBackupBearing2 = null;

					SpringJoint[] nonCollisionSprings = GetComponentsInChildren<SpringJoint>();
					for (int i = 0; i < nonCollisionSprings.Length; i++)
					{
						if (nonCollisionSprings[i].connectedBody == pushee)
						{
							Destroy(nonCollisionSprings[i]);
						}
					}*/

					Destroy(pushee.gameObject);
					pushee = null;
					Destroy(scaleBearing1);
					scaleBearing1 = null;
					Destroy(scaleBearing2);
					scaleBearing2 = null;
					Destroy(bearingPusher);
					bearingPusher = null;

					funnelingOut = true;
				}
			}
			else if (funnelingOut && pusheeScale.y >= destroyBackupDistance)
			{
				Destroy(scaleBearing1);
				scaleBearing1 = null;
				Destroy(scaleBearing2);
				scaleBearing2 = null;
				Destroy(bearingPusher);
				bearingPusher = null;

				SpringJoint[] nonCollisionSprings = GetComponentsInChildren<SpringJoint>();
				for (int i = 0; i < nonCollisionSprings.Length; i++)
				{
					if (nonCollisionSprings[i].connectedBody == pushee)
					{
						Destroy(nonCollisionSprings[i]);
					}
				}
			}

			if (pushee != null)
			{
				pushee.transform.localScale = pusheeScale;

				Vector3 capScale = pusheeCap1.transform.localScale;
				capScale.z = (capScale.x * pusheeScale.x) / pusheeScale.y;
				pusheeCap1.transform.localScale = capScale;
				pusheeCap2.transform.localScale = capScale;
			}

			
		}

		if (!solved && pushee != null && pushee.isKinematic)
		{
			solved = true;
			for (int i = 0; i < solveListeners.Count; i++)
			{
				solveListeners[i].SendMessage("FunnelSolved", this, SendMessageOptions.DontRequireReceiver);
			}
		}

		if (pushee != null)
		{
			Vector3 pusheePos = pushee.transform.localPosition;
			pusheePos.y = pusheeLocalY;
		}
	}

	public void StopBackTracking()
	{
		noBacktrackBody1.isKinematic = false;
		noBacktrackBody2.isKinematic = false;
	}
}

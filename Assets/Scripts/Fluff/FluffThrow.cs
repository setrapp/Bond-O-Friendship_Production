using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FluffThrow : MonoBehaviour {
	public CharacterComponents character;
	public FluffThrow lastFluffAccepted;
	public bool volleyOnlyFirst = true;
	public int volleys;
	public FluffThrow volleyPartner;
	public FloatMoving floatMove;
	public float floatPushBack;
	public FluffHandler fluffHandler;
	public int minShotCount;
	public int maxShotCount;
	public float shotSpread;
	public float minShotFactor;
	public float passForce = 1000;
	public float movingBonusFactor = 0.3f;

	void Start()
	{
		if (character == null)
		{
			character = GetComponent<CharacterComponents>();
		}
		if (floatMove == null)
		{
			floatMove = GetComponent<FloatMoving>();
		}
		if (fluffHandler == null)
		{
			fluffHandler = GetComponent<FluffHandler>();
		}
	}

	public void Throw(Vector3 passDirection, Vector3 velocityBoost)
	{
		if (Time.deltaTime <= 0)
		{
			return;
		}

		int passFluffCount = Mathf.Min(Random.Range(minShotCount, maxShotCount), fluffHandler.fluffs.Count);


		if (fluffHandler == null || fluffHandler.fluffs.Count < 1 || passFluffCount < 1)
		{
			return;
		}

		passDirection.Normalize();
		List<int> passFluffIndices = new List<int>();
		List<GameObject> passFluffs = new List<GameObject>();
		List<float> maxFluffDotPasses = new List<float>();
		for (int i = 0; i < fluffHandler.fluffs.Count; i++)
		{
			float fluffDotPass = Vector3.Dot(fluffHandler.fluffs[i].transform.up, passDirection);
			if ((maxFluffDotPasses.Count < passFluffCount || fluffDotPass > maxFluffDotPasses[passFluffCount - 1]) && fluffHandler.fluffs[i] != fluffHandler.spawnedFluff)
			{
				maxFluffDotPasses.Add(fluffDotPass);
				passFluffs.Add(fluffHandler.fluffs[i].gameObject);
				passFluffIndices.Add(i);
				if (maxFluffDotPasses.Count > passFluffCount)
				{
					float minMaxFluffDotPass = maxFluffDotPasses[0];
					//int minMaxFluffIndex = 0;
					for (int j = 1; j < maxFluffDotPasses.Count; j++)
					{
						float maxFluffDotPass = maxFluffDotPasses[j];
						if (maxFluffDotPass < minMaxFluffDotPass)
						{
							minMaxFluffDotPass = maxFluffDotPasses[j];
							//minMaxFluffIndex = j;
						}
					}
				}
				
			}

			if (maxFluffDotPasses.Count > passFluffCount)
			{
				maxFluffDotPasses.RemoveAt(passFluffCount);
				passFluffs.RemoveAt(passFluffCount);
				passFluffIndices.RemoveAt(passFluffCount);
			}
		}

		
		float shotAngle = -shotSpread / 2;
		if (passFluffs.Count == 1)
		{
			shotAngle = 0;
		}

		for (int i = passFluffs.Count - 1; i >= 0; i--)
		{
			fluffHandler.fluffs.RemoveAt(passFluffIndices[i]);

			Vector3 rotatedPassDir = Quaternion.Euler(0, 0, shotAngle) * passDirection;

			Fluff fluff = passFluffs[i].GetComponent<Fluff>();
            fluff.transform.rotation = Quaternion.LookRotation(rotatedPassDir, Vector3.Cross(rotatedPassDir, -Vector3.forward));
            fluff.transform.parent = transform.parent;
            fluff.volleyPartner = lastFluffAccepted;
			shotAngle += shotSpread / passFluffCount;
            fluff.transform.position = transform.position;

            fluff.Pass((rotatedPassDir * passForce * Random.Range(minShotFactor, 1.0f)) + (velocityBoost / Time.deltaTime) * movingBonusFactor, gameObject);
		}
		

		// If only the first fluff can be volleyed to create a bond, ignore last fluff accepted for future shots.
		if (volleyOnlyFirst)
		{
			lastFluffAccepted = null;
		}

		// If floating propel away from fluff.
		if (floatMove.Floating && passFluffs.Count > 0)
		{
			Vector3 recoilForce = -passDirection * floatPushBack;
			character.mover.body.AddForce(recoilForce);
			character.mover.velocity += recoilForce * Time.deltaTime;
		}
	}
}

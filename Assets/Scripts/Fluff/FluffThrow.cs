using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FluffThrow : MonoBehaviour {
	public CharacterComponents character;
	public float floatPushBack;
	public int minShotCount;
	public int maxShotCount;
	public float shotSpread;
	public float minShotFactor;
	public float passForce = 1000;
	public float movingBonusFactor = 0.3f;
	public float preventFluffAttractTime = 0.5f;

	void Start()
	{
		if (character == null)
		{
			character = GetComponent<CharacterComponents>();
		}
	}

	public void Throw(Vector3 passDirection, Vector3 velocityBoost)
	{
		if (Globals.Instance != null && !Globals.Instance.fluffsThrowable)
		{
			return;
		}

		if (Time.deltaTime <= 0)
		{
			return;
		}

		int passFluffCount = Mathf.Min(Random.Range(minShotCount, maxShotCount), character.fluffHandler.fluffs.Count);

		if (character.fluffHandler.fluffs == null)
		{
			return;
		}
		else if (character.fluffHandler.fluffs.Count < 1 || passFluffCount < 1)
		{
			if (character.flufflessPass != null)
			{
				character.flufflessPass.Play();
			}
		}

		passDirection.Normalize();
		List<int> passFluffIndices = new List<int>();
		List<GameObject> passFluffs = new List<GameObject>();
		List<float> maxFluffDotPasses = new List<float>();
		for (int i = 0; i < character.fluffHandler.fluffs.Count; i++)
		{
			float fluffDotPass = Vector3.Dot(character.fluffHandler.fluffs[i].transform.up, passDirection);
			if ((maxFluffDotPasses.Count < passFluffCount || fluffDotPass > maxFluffDotPasses[passFluffCount - 1]) && character.fluffHandler.fluffs[i] != character.fluffHandler.spawnedFluff)
			{
				maxFluffDotPasses.Add(fluffDotPass);
				passFluffs.Add(character.fluffHandler.fluffs[i].gameObject);
				passFluffIndices.Add(i);
				if (maxFluffDotPasses.Count > passFluffCount)
				{
					float minMaxFluffDotPass = maxFluffDotPasses[0];
					for (int j = 1; j < maxFluffDotPasses.Count; j++)
					{
						float maxFluffDotPass = maxFluffDotPasses[j];
						if (maxFluffDotPass < minMaxFluffDotPass)
						{
							minMaxFluffDotPass = maxFluffDotPasses[j];
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
			character.fluffHandler.fluffs.RemoveAt(passFluffIndices[i]);

			Vector3 rotatedPassDir = Quaternion.Euler(0, 0, shotAngle) * passDirection;

			Fluff fluff = passFluffs[i].GetComponent<Fluff>();
			fluff.transform.rotation = Quaternion.LookRotation(rotatedPassDir, Vector3.Cross(rotatedPassDir, -Vector3.forward));
			if (OrphanFluffHolder.Instance != null)
			{
				fluff.transform.parent = OrphanFluffHolder.Instance.transform;
			}
			else
			{
				fluff.transform.parent = transform.parent;
			}
			shotAngle += shotSpread / passFluffCount;
			fluff.transform.position = transform.position;

			fluff.Pass((rotatedPassDir * passForce * Random.Range(minShotFactor, 1.0f)) + (velocityBoost / Time.deltaTime) * movingBonusFactor, gameObject, preventFluffAttractTime);
		}
		
		// If floating propel away from fluff.
		/*if (character.floatMove.Floating && passFluffs.Count > 0)
		{
			Vector3 recoilForce = -passDirection * floatPushBack;
			character.mover.body.AddForce(recoilForce);
			character.mover.velocity += recoilForce * Time.deltaTime;
		}*/
	}
}

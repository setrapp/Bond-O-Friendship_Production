using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Letter {NONE = -1, A = 0, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z};

public class LetterManager : MonoBehaviour {

	private static LetterManager instance;
	public static LetterManager Instance { get { return instance; } }

	public bool editorRandomReceivers = false;
	public LetterRegion[] letterRegions;
	[SerializeField]
	public List<LetterReceiverList> letterReceiverLists;
	public int nearRegionCount = 3;
	public Material[] letterMaterials;
	public float nearbyAttractForce;
	public float lerpEndPortion = 0.5f;
	public Color attachmentColor;
	public float attachmentOffset = -0.2f;
	public float attachmentScale = 4;

	void Awake()
	{
		instance = this;

		// Create a list of each letter of letter receivers.
		letterReceiverLists = new List<LetterReceiverList> ();
		for (int i = 0; i < letterMaterials.Length; i++)
		{
			LetterReceiverList letterList = new LetterReceiverList ();
			letterList.letter = (char)((int)'a' + i);
			letterList.receivers = new List<LetterReceiver> ();
			letterReceiverLists.Add (letterList);
		}

		// Find all letter receivers.
		GameObject[] allReceivers = GameObject.FindGameObjectsWithTag("Letter Receiver");

		// Find all letter regions.
		letterRegions = gameObject.GetComponentsInChildren<LetterRegion>();

		// Randomize receiver letters in editor.
		if (Application.isEditor && editorRandomReceivers)
		{
			for (int i = 0; i < allReceivers.Length; i++)
			{
				allReceivers[i].GetComponent<LetterReceiver>().receiveLetter = (Letter)Random.Range((int)Letter.A, (int)Letter.Z);
			}
		}

		// Add receivers to specific letter lists.
		for (int i = 0; i < allReceivers.Length; i++)
		{
			LetterReceiver receiver = allReceivers[i].GetComponent<LetterReceiver>();
			letterReceiverLists[(int)receiver.receiveLetter].receivers.Add(receiver);
		}

		// Find existing letters in each region
		for (int i = 0; i < letterRegions.Length; i++)
		{
			letterRegions[i].FindLetters();
		}

		// Ensure that every letter receiver has at least one corresponding letter.
		for (int i = 0; i < letterReceiverLists.Count; i++)
		{
			LetterReceiverList receiverList = letterReceiverLists[i];
			for (int j = 0; j < receiverList.receivers.Count; j++)
			{
				LetterReceiver receiver = receiverList.receivers[j];
				receiver.nearbyRegions = FindNearbyLetterRegions(receiver);
				bool letterAssigned = false;
				for (int k = 0; k < receiver.nearbyRegions.Length && !letterAssigned; k++)
				{
					letterAssigned = receiver.nearbyRegions[k].AssignLetter(receiver);
				}

				if (!letterAssigned)
				{
					Debug.LogError("No letters could be assigned nearby the receiver " + receiver.gameObject.name + " of " + receiver.transform.parent.gameObject.name + ". Please add letters to a nearby region");
				}
			}
		}

		// Give random letter values to the letters that have not yet been set.
		for (int i = 0; i < letterRegions.Length; i++)
		{
			letterRegions[i].RadomizeLetters();
		}
	}

	private LetterRegion[] FindNearbyLetterRegions(LetterReceiver receiver)
	{
		LetterRegion[] nearRegions = new LetterRegion[nearRegionCount];
		float[] regionSqrDists = new float[nearRegionCount];
		for (int i = 0; i < letterRegions.Length; i++)
		{
			bool placed = false;
			for (int j = 0; j < nearRegions.Length && !placed; j++)
			{
				float sqrDist = (letterRegions[i].centroid - receiver.transform.position).sqrMagnitude;
				if (nearRegions[j] == null || sqrDist < regionSqrDists[j])
				{
					for (int k = regionSqrDists.Length - 1; k > j; k--)
					{
						regionSqrDists[k] = regionSqrDists[k-1];
						nearRegions[k] = nearRegions[k-1];
					}
					regionSqrDists[j] = sqrDist;
					nearRegions[j] = letterRegions[i];
					placed = true;
				}
			}
		}
		return nearRegions;
	}
}

[System.Serializable]
public class LetterReceiverList
{
	public char letter;
	public List<LetterReceiver> receivers;
}

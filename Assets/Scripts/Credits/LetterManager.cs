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
		letterReceiverLists = new List<LetterReceiverList>();
		for (int i = 0; i < letterMaterials.Length; i++)
		{
			LetterReceiverList letterList = new LetterReceiverList();
			letterList.letter = (char)((int)'a' + i);
			letterList.receivers = new List<LetterReceiver>();
			letterReceiverLists.Add(letterList);
		}

		// Find all letter receivers.
		GameObject[] allReceivers = GameObject.FindGameObjectsWithTag ("Letter Receiver");

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

		// Find all existing letters and prepare for assignment.
		GameObject[] allLetters = GameObject.FindGameObjectsWithTag ("Letter");
		List<CreditsLetter> availableLetters = new List<CreditsLetter>();
		List<CreditsLetter> predeterminedLetters = new List<CreditsLetter>();
		for (int i = 0; i < allLetters.Length; i++)
		{
			CreditsLetter letter = allLetters[i].GetComponent<CreditsLetter>();
			if (letter != null)
			{
				// Separate letters between available to be set and predetermined.
				if (letter.letterValue == Letter.NONE)
				{
					availableLetters.Add(letter);
				}
				else
				{
					predeterminedLetters.Add(letter);
				}
			}
		}



		// Ensure that every letter receiver has at least one corresponding letter.
		for (int i = 0; i < letterReceiverLists.Count; i++)
		{
			LetterReceiverList receiverList = letterReceiverLists[i];
			for (int j = 0; j < receiverList.receivers.Count; j++)
			{
				LetterReceiver receiver = receiverList.receivers[j];
				receiverList.receivers[j].nearbyRegions = FindNearbyLetterRegions();
				bool letterAssigned = false;
				for (int k = 0; k < receiverList.receivers[j].nearbyRegions.Length && !letterAssigned; k++)
				{
					letterAssigned = receiverList.receivers[j].nearbyRegions[k].AssignLetter(receiverList.receivers[j]);
				}

				if (!letterAssigned)
				{
					Debug.LogError("No letters could be assigned nearby the receiver " + receiverList.receivers[j].gameObject.name + ". Please add letters to a nearby region");
					receiverList.receivers[j].gameObject.name = "[Error] " + receiverList.receivers[j].gameObject.name;
				}
			}
		}

		// Give random letter values to the letters that have not yet been set.
		for (int i = 0; i < availableLetters.Count; i++)
		{
			availableLetters[i].letterValue = (Letter)Random.Range((int)Letter.A, (int)Letter.Z);
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
				float sqrDist = letterRegions[i].transform.position - receiver.transform.position;
				if (sqrDist < regionSqrDists[j])
				{
					for (int k = j + 1; k < regionSqrDists.Length; k++)
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
	}
}

[System.Serializable]
public class LetterReceiverList
{
	public char letter;
	public List<LetterReceiver> receivers;
}

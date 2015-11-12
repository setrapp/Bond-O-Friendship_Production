using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Letter {NONE = -1, A = 0, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z};

public class LetterManager : MonoBehaviour {

	private static LetterManager instance;
	public static LetterManager Instance { get { return instance; } }

	[SerializeField]
	public List<LetterReceiverList> letterReceiverLists;
	public Material[] letterMaterials;
	public Color letterEndColor;

	void Awake()
	{
		instance = this;

		// Create a list of each letter of letter receivers.
		letterReceiverLists = new List<LetterReceiverList>();
		for (int i = 0; i < 26; i++)
		{
			LetterReceiverList letterList = new LetterReceiverList();
			letterList.letter = (char)((int)'a' + i);
			letterList.receivers = new List<LetterReceiver>();
			letterReceiverLists.Add(letterList);
		}

		// Add receivers to specific letter lists.
		GameObject[] allReceivers = GameObject.FindGameObjectsWithTag ("Letter Receiver");
		for (int i = 0; i < allReceivers.Length; i++)
		{
			LetterReceiver receiver = allReceivers[i].GetComponent<LetterReceiver>();
			letterReceiverLists[(int)receiver.receiveLetter].receivers.Add(receiver);
		}

		// Find all existing letters and prepare for assignment.
		GameObject[] allLetters = GameObject.FindGameObjectsWithTag ("Letter");
		List<CreditsLetter> availableLetters = new List<CreditsLetter>();
		for (int i = 0; i < allLetters.Length; i++)
		{
			CreditsLetter letter = allLetters[i].GetComponent<CreditsLetter>();
			// Ignore letters that have already had their values set.
			if (letter != null && letter.letterValue == Letter.NONE)
			{
				availableLetters.Add(letter);
			}
		}

		// Ensure that every letter receiver has at least one corresponding letter.
		for (int i = 0; i < letterReceiverLists.Count; i++)
		{
			LetterReceiverList receiverList = letterReceiverLists[i];
			for (int j = 0; j < receiverList.receivers.Count; j++)
			{
				if (availableLetters.Count > 0)
				{
					int letterIndex = Random.Range(0, availableLetters.Count);
					availableLetters[letterIndex].letterValue = receiverList.receivers[j].receiveLetter;
					availableLetters.RemoveAt(letterIndex);
				}
				else
				{
					Debug.LogError("Not enough letters exist to ensure that each reciever has a letter");
				}
			}
		}

		// Give random letter values to the letters that have not yet been set.
		for (int i = 0; i < availableLetters.Count; i++)
		{
			availableLetters[i].letterValue = (Letter)Random.Range((int)Letter.A, (int)Letter.Z);
		}
	}
}

[System.Serializable]
public class LetterReceiverList
{
	public char letter;
	public List<LetterReceiver> receivers;
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LetterRegion : MonoBehaviour {

	[HideInInspector]
	public List<CreditsLetter> availableLetters;
	[HideInInspector]
	public List<CreditsLetter> predeterminedLetters;

	// Find all existing letters in preparation for assignment.
	public void FindLetters()
	{
		GameObject[] allLetters = gameObject.GetComponentsInChildren<CreditsLetter>();
		availableLetters = new List<CreditsLetter>();
		predeterminedLetters = new List<CreditsLetter>();
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
	}

	// Attempt to assign a letter that is able to fill the given receiver.
	public bool AssignLetter(LetterReceiver receiver)
	{
		// First, check if any predetermined letters will fit.
		bool readyLetterFound = false;
		for (int k = 0; k < predeterminedLetters.Count && !readyLetterFound; k++)
		{
			if (predeterminedLetters[k].letterValue == receiver.receiveLetter)
			{
				predeterminedLetters.RemoveAt(k);
				readyLetterFound = true;
			}
		}
		
		// If one does not already exist, attempt to assign a letter to the needed value.
		if (!readyLetterFound)
		{
			if (availableLetters.Count > 0)
			{
				int letterIndex = Random.Range(0, availableLetters.Count);
				availableLetters[letterIndex].letterValue = receiver.receiveLetter;
				availableLetters.RemoveAt(letterIndex);
			}
			else
			{
				return false;
			}
		}

		return true;
	}


}

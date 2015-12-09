using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LetterRegion : MonoBehaviour {

	[HideInInspector]
	public List<CreditsLetter> availableLetters;
	[HideInInspector]
	public List<CreditsLetter> predeterminedLetters;
	[HideInInspector]
	public List<CreditsLetter> randomizableLetters;
	public Vector3 centroid;

	// Find all existing letters in preparation for assignment.
	public void FindLetters()
	{
		CreditsLetter[] allLetters = gameObject.GetComponentsInChildren<CreditsLetter>();
		availableLetters = new List<CreditsLetter>();
		predeterminedLetters = new List<CreditsLetter>();
		centroid = Vector3.zero;
		for (int i = 0; i < allLetters.Length; i++)
		{
			CreditsLetter letter = allLetters[i];
			centroid += letter.transform.position;

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

		// Place centroid of contained letters.
		centroid /= allLetters.Length;
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
				randomizableLetters.Add(predeterminedLetters[k]);
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
				randomizableLetters.Add(availableLetters[letterIndex]);
				availableLetters.RemoveAt(letterIndex);
			}
			else
			{
				return false;
			}
		}

		return true;
	}

	// Radomize all remaining letters by repeating values needed by recievers.
	public void RadomizeLetters()
	{
		for (int i = 0; i < availableLetters.Count; i++)
		{
			if (randomizableLetters.Count > 0)
			{
				int randomIndex = Random.Range(0, randomizableLetters.Count);
				availableLetters[i].letterValue = randomizableLetters[randomIndex].letterValue;
			}
		}
	}

}

using UnityEngine;
using System.Collections;

public class ClusterNodePuzzleGroup : MonoBehaviour {
	public int puzzleCount = 0;
	public int donePuzzleCount = 0;
	public bool solved = false;

	public void ClusterNodesSolved(ClusterNodePuzzle puzzle)
	{
		if (puzzle.solved && donePuzzleCount < puzzleCount)
		{
			donePuzzleCount++;
		}

		if (donePuzzleCount >= puzzleCount)
		{
			donePuzzleCount = puzzleCount;
			solved = true;
		}

	}
}

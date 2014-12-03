using UnityEngine;
using System.Collections;

public class StageCrash : MonoBehaviour {
	public GameObject stageOne;
	public GameObject stageTwo;
	private bool moveStages;
	public float moveSpeed;
	public float stopDistance;
	public GameObject barrierOne;
	public GameObject barrierTwo;
	public GameObject fluffGenerator;
	public float breakBarrierDistance;
	public bool open = false;

	void Update()
	{
		if (moveStages)
		{
			float stageDistance = Vector3.Distance(stageOne.transform.position, stageTwo.transform.position);

			// Move stages closer togther until they approach the stopDistance, then snap to position and stop.
			if (stageDistance - (moveSpeed * Time.deltaTime * 2) > stopDistance)
			{
				stageOne.transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
				stageTwo.transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
			}
			else
			{
				moveStages = false;
				float midPointX = (stageOne.transform.position.x + stageTwo.transform.position.x) / 2;
				stageOne.transform.position = new Vector3(midPointX - stopDistance / 2, stageOne.transform.position.y, stageOne.transform.position.z);
				stageTwo.transform.position = new Vector3(midPointX + stopDistance / 2, stageTwo.transform.position.y, stageTwo.transform.position.z);
			}

			// Break barries when close enough.
			if (stageDistance <= breakBarrierDistance)
			{
				if (barrierOne)
				{
					Destroy(barrierOne);
					barrierOne = null;
				}
				if (barrierTwo)
				{
					Destroy(barrierTwo);
					barrierTwo = null;
				}
				if(fluffGenerator)
				{
					Destroy(fluffGenerator);
					fluffGenerator = null;
				}
				open = true;
			}
		}
	}

	private void TriggerEmpty(GameObject trigger)
	{
		moveStages = true;
	}
}

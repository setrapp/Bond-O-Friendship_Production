using UnityEngine;
using System.Collections;

public class LevelShrink : MonoBehaviour {
	public WaitPad triggerPad;
	public GameObject padObject;
	private bool activated;
	public Island targetLevel;
	public float shrinkTime = 10.0f;
	private float timeElapsed = 0;
	private Vector3 startScale = Vector3.zero;
	public Transform nonShrinkParent;
	private GameObject focalPoint;

	void Update()
	{
		if (!activated && triggerPad.activated)
		{
			transform.parent = nonShrinkParent;
			Globals.Instance.player1.transform.parent = transform;
			Globals.Instance.player2.transform.parent = transform;
			padObject.transform.parent = transform;
			targetLevel.transform.position = new Vector3(targetLevel.transform.position.x, targetLevel.transform.position.y, transform.position.z);

			focalPoint = new GameObject();
			focalPoint.transform.position = transform.position;
			focalPoint.transform.parent = targetLevel.transform;
			activated = true;

			if (targetLevel.container != null && targetLevel.container.atmosphere != null)
			{
				targetLevel.container.atmosphere.SilentBreak();
			}

			startScale = targetLevel.transform.localScale;
			StartCoroutine(Shrink());
		}
	}

	private IEnumerator Shrink()
	{
		while(timeElapsed < shrinkTime)
		{
			Vector3 levelScale = Vector3.Lerp(startScale, Vector3.zero, timeElapsed / shrinkTime);
			targetLevel.transform.localScale = levelScale;
			targetLevel.transform.position += (transform.position - focalPoint.transform.position);
			timeElapsed += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		Destroy(targetLevel.gameObject);
	}
}

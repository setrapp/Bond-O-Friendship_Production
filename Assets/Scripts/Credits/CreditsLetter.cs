using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreditsLetter : MonoBehaviour {
	
	public Letter letterValue = Letter.NONE;
	public Renderer backgroundRenderer;
	public Renderer letterRenderer;
	public float attachDistance;
	public bool attachedToReceiver;
	public float colorChangeRange;

	private float tempDistance;
	private Color startColor;
	private Vector3 oldPosition;
	
	void Start () {
		if (backgroundRenderer == null)
		{
			backgroundRenderer =  GetComponent<Renderer>();
		}

		startColor = backgroundRenderer.material.color;

		if (letterValue != Letter.NONE && letterRenderer != null)
		{
			letterRenderer.material = LetterManager.Instance.letterMaterials[(int)letterValue];
			gameObject.name = "Letter " + (char)((int)'a' + (int)letterValue);
		}

		CheckForAttachment();
		oldPosition = transform.position;
	}

	void Update () {
		
		if(!attachedToReceiver && oldPosition != transform.position)
		{
			CheckForAttachment();
		}
		oldPosition = transform.position;
	}

	private void CheckForAttachment()
	{
		List<LetterReceiver> possibleReceivers = LetterManager.Instance.letterReceiverLists[(int)letterValue].receivers;
		LetterReceiver receiver = null;
		float minDistance = -1;
		for(int i = 0; i < possibleReceivers.Count; i++)
		{
			if(letterValue == possibleReceivers[i].receiveLetter)
			{
				tempDistance = Vector2.Distance(transform.position, possibleReceivers[i].transform.position);
				if (minDistance > tempDistance || minDistance < 0)
				{
					minDistance = tempDistance;
					receiver = possibleReceivers[i];
				}
			}
		}
		if(receiver != null)
		{
			Color endColor = LetterManager.Instance.letterEndColor;
			
			if (minDistance < colorChangeRange)
			{
				backgroundRenderer.material.color = Color.Lerp(startColor, endColor, 1 - minDistance / (colorChangeRange - attachDistance));
			}
			else
			{
				backgroundRenderer.material.color = startColor;
			}
			if (minDistance < attachDistance)
			{
				transform.position = receiver.transform.position + new Vector3(0, 0, -1);
				Destroy(gameObject.GetComponent<Collider>());
				Destroy(gameObject.GetComponent<Rigidbody>());
				receiver.GetComponentInChildren<ParticleSystem>().Play();
				attachedToReceiver = true;
				possibleReceivers.Remove(receiver);
				backgroundRenderer.material.color = endColor;
			}
		}
	}
}

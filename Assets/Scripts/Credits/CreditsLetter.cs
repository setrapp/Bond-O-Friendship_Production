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
	public float attractRange;

	private float tempDistance;
	private Color startColor;
	private float startScale;
	private Rigidbody body;
	private Vector3 oldPosition;
	private LetterReceiver receiver;
	
	void Start () {
		if (backgroundRenderer == null)
		{
			backgroundRenderer =  GetComponent<Renderer>();
		}
		if (body == null)
		{
			body = GetComponent<Rigidbody>();
		}

		startColor = backgroundRenderer.material.color;
		startScale = transform.localScale.x;

		if (letterValue != Letter.NONE && letterRenderer != null)
		{
			letterRenderer.material = LetterManager.Instance.letterMaterials[(int)letterValue];
			gameObject.name = "Letter " + (char)((int)'a' + (int)letterValue);
		}

		CheckForAttachment();
		oldPosition = transform.position;
	}

	void Update () {
		
		if(!attachedToReceiver && (oldPosition != transform.position || (receiver != null && receiver.attachedLetter != null && receiver.attachedLetter != this)))
		{
			CheckForAttachment();
		}
		oldPosition = transform.position;
	}

	private void CheckForAttachment()
	{
		List<LetterReceiver> possibleReceivers = LetterManager.Instance.letterReceiverLists[(int)letterValue].receivers;
		LetterReceiver foundReceiver = null;
		float minDistance = -1;
		for(int i = 0; i < possibleReceivers.Count; i++)
		{
			if(letterValue == possibleReceivers[i].receiveLetter)
			{
				tempDistance = Vector2.Distance(transform.position, possibleReceivers[i].transform.position);
				if (minDistance > tempDistance || minDistance < 0)
				{
					minDistance = tempDistance;
					foundReceiver = possibleReceivers[i];
				}
			}
		}
		if(foundReceiver != null)
		{	
			if (receiver != null)
			{
				receiver.nearestToLetter = false;
			}
			foundReceiver.nearestToLetter = true;
			receiver = foundReceiver;

			if (minDistance < colorChangeRange)
			{
				Color lerpEndColor = startColor + ((LetterManager.Instance.attachmentColor - startColor) * LetterManager.Instance.lerpEndPortion);
				float lerpEndScale = startScale + ((LetterManager.Instance.attachmentScale - startScale) * LetterManager.Instance.lerpEndPortion);
				float lerpProgress = 1 - (minDistance / colorChangeRange);

				backgroundRenderer.material.color = Color.Lerp(startColor, lerpEndColor, lerpProgress);
				receiver.attachRenderer.material.color = Color.Lerp(receiver.startColor, lerpEndColor, lerpProgress);

				float lerpScale = Mathf.Lerp(startScale, lerpEndScale, lerpProgress);
				transform.localScale = new Vector3(lerpScale, transform.localScale.y, lerpScale);
			}
			else
			{
				backgroundRenderer.material.color = startColor;
				transform.localScale = new Vector3(startScale, transform.localScale.y, startScale);
			}

			if (minDistance < attractRange && body != null)
			{
				float lerpProgress = 1 - (minDistance / attractRange);
				Vector3 attractForce = (receiver.transform.position - transform.position).normalized * LetterManager.Instance.nearbyAttractForce * lerpProgress;
				body.AddForce(attractForce);
			}

			if (minDistance < attachDistance)
			{
				AttachToReceiver(receiver);
				possibleReceivers.Remove(receiver);
			}
		}
	}

	private void AttachToReceiver(LetterReceiver receiver)
	{
		transform.position = receiver.transform.position + new Vector3(0, 0, LetterManager.Instance.attachmentOffset);
		transform.localScale = new Vector3(LetterManager.Instance.attachmentScale, transform.localScale.y, LetterManager.Instance.attachmentScale);
		Destroy(gameObject.GetComponent<Collider>());
		Destroy(body);
		receiver.GetComponentInChildren<ParticleSystem>().Play();
		Destroy(receiver.GetComponentInChildren<ParticleSystem>(), 5);
		receiver.attachedLetter = this;
		attachedToReceiver = true;
		backgroundRenderer.material.color = LetterManager.Instance.attachmentColor;
	}
}

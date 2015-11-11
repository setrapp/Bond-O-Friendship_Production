using UnityEngine;
using System.Collections;

public enum Letter {A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z};

public class CreditsLetters : MonoBehaviour {

	public Letter letterValue;
	public GameObject receiversParent;
	public float attachDistance;
	public bool attachedToReceiver;
	public float colorChangeRange;

	private float tempDistance;
	private float minDistance = 1000;
	public Color startColor;
	private Color endColor;

	// Use this for initialization
	void Start () {
		startColor = GetComponent<Renderer>().material.color;
		endColor = receiversParent.GetComponentInChildren<Renderer>().material.color;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!attachedToReceiver)
		{
			Transform receiver = null;
			for(int i = 0; i < receiversParent.transform.childCount; i++)
			{
				if(letterValue == receiversParent.transform.GetChild(i).GetComponent<LetterReceivers>().receiveLetter)
				{
					tempDistance = Vector2.Distance(transform.position, receiversParent.transform.GetChild(i).transform.position);
					if (minDistance > tempDistance)
					{
						minDistance = tempDistance;
						receiver = receiversParent.transform.GetChild(i);
					}
				}
			}
			if(receiver != null)
			{
				if (minDistance < colorChangeRange)
					GetComponent<Renderer>().material.color = Color.Lerp(startColor, endColor, 1 - minDistance / (colorChangeRange - attachDistance));
				else
					GetComponent<Renderer>().material.color = startColor;
				if (minDistance < attachDistance)
				{
					transform.position = receiver.position + new Vector3(0, 0, -1);
					GetComponent<Collider>().enabled = false;
					GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
					receiver.GetComponentInChildren<ParticleSystem>().Play();
					attachedToReceiver = true;
					GetComponent<Renderer>().material.color = endColor;
				}
			}
			minDistance = 1000;
		}
	}
}

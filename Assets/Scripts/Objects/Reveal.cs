using UnityEngine;
using System.Collections;

public class Reveal : MonoBehaviour {

	public GameObject trigger1;
	public GameObject trigger2;
	public GameObject stopBlock;
	public float moveSpeed;
	public float returnSpeed;
	public Vector3 originalPos;
	public Vector3 pushPos;
	public Rigidbody body = null;
	public float limit;
	
	// Use this for initialization
	void Start () {
		moveSpeed = 4.0f;
		returnSpeed = 0.5f;
		originalPos = transform.localPosition;
		pushPos = originalPos;
		body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.deltaTime <= 0)
		{
			return;
		}

		transform.localPosition = pushPos;
		if(trigger1.GetComponent<triggerBlock>().triggered || trigger2.GetComponent<triggerBlock>().triggered)
		{
			if(pushPos.y<limit)
				pushPos.y += moveSpeed*Time.deltaTime;
		}
		else
		{
			if(pushPos.y > originalPos.y)
			{
				pushPos.y -= returnSpeed*Time.deltaTime;
			}
		}
		if(stopBlock.GetComponent<triggerBlock>().stopped)
		{
			returnSpeed = 0.0f;
			//print("Stopped");
		}
		else
		{
			//print ("returning");
			returnSpeed = 0.5f;
		}
	}
}

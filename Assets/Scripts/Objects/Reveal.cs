using UnityEngine;
using System.Collections;

public class Reveal : MonoBehaviour {

	public GameObject trigger1;
	public GameObject trigger2;
	public float moveSpeed;
	public float returnSpeed;
	public Vector3 originalPos;
	public Vector3 pushPos;
	
	// Use this for initialization
	void Start () {
		moveSpeed = 0.5f;
		returnSpeed = 0.1f;
		originalPos = transform.position;
		pushPos = originalPos;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = pushPos;
	if(trigger1.GetComponent<triggerBlock>().triggered)
		{
			if(pushPos.x > originalPos.x - 0.8)
			{
				pushPos.x -= moveSpeed*Time.deltaTime;
			}
		}
		else
		{
			if(pushPos.x < originalPos.x)
			{
				pushPos.x += returnSpeed*Time.deltaTime;
			}
		}
	if(trigger2.GetComponent<triggerBlock>().triggered)
		{
			pushPos = transform.position;
			returnSpeed = 0;
			trigger1.SendMessage("AllDone",SendMessageOptions.DontRequireReceiver);
			trigger2.SendMessage("AllDone",SendMessageOptions.DontRequireReceiver);
			renderer.material.color = Color.cyan;
		}
	}
}

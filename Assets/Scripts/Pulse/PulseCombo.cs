using UnityEngine;
using System.Collections;

public class PulseCombo : MonoBehaviour {

	public GameObject pulse;
	public GameObject pulsePrefab;
	public bool pulseOne = false;
	public bool pulseTwo = false;
	public Vector3 pulseOnePos;
	public Vector3 pulseTwoPos;
	private Vector3 newPos;
	public Quaternion p1Quat;
	public Quaternion p2Quat;
	private Quaternion newQuat;
	public Vector3 p1For;
	public Vector3 p2For;
	private Vector3 forAv;
	private Quaternion forQuat;
	public Vector3 p1Targ;
	public Vector3 p2Targ;
	private Vector3 newTarg;
	public Vector3 p1scale;
	public Vector3 p2scale;
	private Vector3 newScale;
	public float p1Cap;
	public float p2Cap;
	private float newCap;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(pulseOne == true && pulseTwo == true)
		{
			newPos = (pulseOnePos + pulseTwoPos)*0.5f;
			forAv = (p1For+p2For)*0.5f;
			forQuat = Quaternion.LookRotation(forAv);
			newQuat = p1Quat * p2Quat;
			newTarg = (p1Targ + p2Targ)*0.5f;
			newScale = (p1scale + p2scale)*0.5f;
			newCap = (p1Cap+p2Cap)*0.5f;
			NewPulse(newPos,newTarg,forQuat);
			pulseOne = false;
			pulseTwo = false;
		}
	
	}
	void NewPulse(Vector3 newpulseVect, Vector3 pulseTarget, Quaternion newQuatern)
	{
		pulse = Instantiate(pulsePrefab, newpulseVect, newQuatern) as GameObject;
		//pulse.transform.localScale += new Vector3(pulseScale, pulseScale, pulseScale);
		
		MovePulse movePulse = pulse.GetComponent<MovePulse>();
		movePulse.target = pulseTarget;
		movePulse.pulseCreator = gameObject;
		movePulse.capacity = newCap;
		pulse.transform.localScale = newScale;
		pulse.GetComponent<Renderer>().material.color = Color.magenta;
	}
}

using UnityEngine;
using System.Collections;

public class SpinPadCompleteOnJoin : MonoBehaviour {

	public JoinTogetherGroup triggerJoinGroup;
	public SpinPad targetPad;
	public bool completeOnIn = false;
	public bool completeOnOut = false;

	void Start()
	{
		if (triggerJoinGroup == null)
		{
			triggerJoinGroup = GetComponent<JoinTogetherGroup>();
		}
		if (targetPad == null)
		{
			targetPad = GetComponent<SpinPad>();
		}
	}
	void Update()
	{
		if (triggerJoinGroup != null && targetPad != null && triggerJoinGroup.solved)
		{
			targetPad.completeOnIn = completeOnIn;
			targetPad.completeOnOut = completeOnOut;
			enabled = false;
		}
	}
}

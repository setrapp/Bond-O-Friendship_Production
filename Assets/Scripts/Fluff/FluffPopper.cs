using UnityEngine;
using System.Collections;

public class FluffPopper : MonoBehaviour {

	private void AttachFluff(Fluff fluff)
	{
		if (fluff != null)
		{
			fluff.PopFluff();
		}
	}
}

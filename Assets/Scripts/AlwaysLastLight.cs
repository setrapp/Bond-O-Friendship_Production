using UnityEngine;
using System.Collections;

public class AlwaysLastLight : MonoBehaviour {
	public Light lastLight;

	void OnPreRender()
	{
		lastLight.gameObject.SetActive(true);
		StartCoroutine(TurnOffLight());
	}

	void OnPostRender()
	{
		//lastLight.gameObject.SetActive(false);
	}

	private IEnumerator TurnOffLight()
	{
		yield return null;
		lastLight.gameObject.SetActive(false);
	}
}

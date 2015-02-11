using UnityEngine;
using System.Collections;

public class CreditsLink : MonoBehaviour {
	public GameObject creditsLinkPrefab;
	private GameObject creditsLink;

	void Start()
	{
		creditsLink = (GameObject)Instantiate(creditsLinkPrefab, transform.position, Quaternion.identity);
		creditsLink.transform.parent = this.transform.parent;
	}

	void Update()
	{
		creditsLink.transform.position = transform.position;
	}
}

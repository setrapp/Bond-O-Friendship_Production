using UnityEngine;
using System.Collections;

public class TranslevelMessage : MonoBehaviour {
	public string messageName;
	public string message;

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}
}

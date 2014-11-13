using UnityEngine;
using System.Collections;

public class TextureSet : MonoBehaviour {
	public Texture2D forcedTexture;

	void Start()
	{
		GetComponent<Renderer>().material.SetTexture(0, forcedTexture);
	}
}

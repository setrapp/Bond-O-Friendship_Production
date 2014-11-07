using UnityEngine;
using System.Collections;

public class TextureSet : MonoBehaviour {
	public Texture2D forcedTexture;

	void Start()
	{
		renderer.material.SetTexture(0, forcedTexture);
	}
}

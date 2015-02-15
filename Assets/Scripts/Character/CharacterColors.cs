using UnityEngine;
using System.Collections;

public class CharacterColors : MonoBehaviour {
	public bool updateColors = false;
	public CharacterComponents character;
	[Header("Colors")]
	public Color attachmentColor;
	[Header("Materials")]
	public Material headMaterial;
	public Material fillMaterial;
	public Material trailMaterial;
	public Material sideTrailMaterial;
	public Material fluffMaterial;

	void Awake()
	{
		if (character == null)
		{
			character = GetComponent<CharacterComponents>();
		}
		character.FindComponents();
		UpdateCharacterColors();
	}

	void Update()
	{
		if (updateColors)
		{
			UpdateCharacterColors();
			updateColors = false;
		}
	}

	private void UpdateCharacterColors()
	{
		character.bondAttachable.attachmentColor = attachmentColor;
		character.headRenderer.material = headMaterial;
		character.fillRenderer.material = fillMaterial;
		character.flashRenderer.color = attachmentColor;
		character.leftTrail.material = sideTrailMaterial;
		character.midTrail.material = trailMaterial;
		character.rightTrail.material = sideTrailMaterial;
		character.fluffHandler.fluffMaterial = fluffMaterial;
	}
}

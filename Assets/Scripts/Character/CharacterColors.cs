using UnityEngine;
using System.Collections;

public class CharacterColors : MonoBehaviour {
	public bool updateColors = false;
	[Header("Colors")]
	public Color attachmentColor;
	[Header("Materials")]
	public Material headMaterial;
	public Material fillMaterial;
	public Material trailMaterial;
	public Material fluffMaterial;
	[Header("Objects")]
	public BondAttachable connectionAttachable;
	public MeshRenderer headRenderer;
	public MeshRenderer fillRenderer;
	public SpriteRenderer flashRenderer;
	public TrailRenderer leftTrailRenderer;
	public TrailRenderer midTrailRenderer;
	public TrailRenderer rightTrailRenderer;
	public FluffSpawn fluffSpawn;

	void Awake()
	{
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
		connectionAttachable.attachmentColor = attachmentColor;
		headRenderer.material = headMaterial;
		fillRenderer.material = fillMaterial;
		flashRenderer.color = attachmentColor;
		leftTrailRenderer.material = trailMaterial;
		midTrailRenderer.material = trailMaterial;
		rightTrailRenderer.material = trailMaterial;
		fluffSpawn.fluffMaterial = fluffMaterial;
	}
}

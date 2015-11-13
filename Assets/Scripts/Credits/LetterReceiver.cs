using UnityEngine;
using System.Collections;

public class LetterReceiver : MonoBehaviour {

    public Letter receiveLetter;
	public Renderer attachRenderer;
	public CreditsLetter attachedLetter;
	[HideInInspector]
	public Color startColor;
	[HideInInspector]
	public bool nearestToLetter = false;
	public float fadeRate = 1;

	void Awake()
	{
		if (attachRenderer == null)
		{
			attachRenderer = GetComponent<Renderer>();
		}
		startColor = attachRenderer.material.color;
	}

	void Update()
	{
		if (!nearestToLetter && attachRenderer.material.color != startColor)
		{
			Color curColor = attachRenderer.material.color;
			Color fadeColor = curColor + (startColor - LetterManager.Instance.attachmentColor) * fadeRate * Time.deltaTime;
			if (((Vector4)(LetterManager.Instance.attachmentColor - fadeColor)).sqrMagnitude / ((Vector4)(LetterManager.Instance.attachmentColor - startColor)).sqrMagnitude < 1)
			{
				attachRenderer.material.color = fadeColor;
			}
			else
			{
				attachRenderer.material.color = startColor;
			}
		}
	}
}

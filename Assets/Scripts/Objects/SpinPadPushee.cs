using UnityEngine;
using System.Collections;

public class SpinPadPushee : MonoBehaviour {
	public SpinPadSide targetSidePad;
	private bool exitting = false;
	public Collider pusheeCollider;
	public Renderer pusheeRenderer;
	public CopyPlayerColor pusheeColorer;
	public float pushingTintFactor = 0.6f;
	public bool pushing = false;
	public RingPulse ripplePrefab;
	private bool rippleFired = false;

	void Start()
	{
		if (pusheeCollider == null)
		{
			pusheeCollider = GetComponent<Collider>();
		}
		if (pusheeRenderer == null)
		{
			pusheeRenderer = GetComponent<Renderer>();
		}
		if (pusheeColorer == null)
		{
			if (pusheeRenderer != null)
			{
				pusheeColorer = pusheeRenderer.GetComponent<CopyPlayerColor>();
			}
			else
			{
				pusheeColorer = GetComponent<CopyPlayerColor>();
			}
		}
	}

	void Update()
	{
		if (!pusheeCollider.collider.enabled)
		{
			pusheeCollider.collider.enabled = true;
		}
	}

	public void DestroyAndRipple()
	{
		if (rippleFired)
		{
			return;
		}

		if (ripplePrefab != null)
		{
			GameObject rippleObj = Instantiate(ripplePrefab.gameObject, transform.position, Quaternion.identity) as GameObject;
			RingPulse ripple = rippleObj.GetComponent<RingPulse>();
			ripple.scaleRate = 8;
			ripple.lifeTime = 1;
			ripple.alpha = 1;
			ripple.alphaFade = 1;
			ripple.smallRing = false;
		}

		rippleFired = true;
		StartCoroutine(FadeToDestroy());
	}

	private IEnumerator FadeToDestroy()
	{
		pusheeRenderer.material.color = Color.white;
		Color pusheeColor = pusheeRenderer.material.color;
		while(pusheeColor.a > 0)
		{
			pusheeColor.a -= Time.deltaTime;
			pusheeRenderer.material.color = pusheeColor;
			yield return null;
		}
		pusheeCollider.gameObject.SetActive(false);
		gameObject.SetActive(false);
	}

	void OnTriggerEnter(Collider col)
	{
		if (targetSidePad.targetPlayer == PlayerInput.Player.Player1 && col.gameObject == Globals.Instance.player1.gameObject)
		{
			if (pusheeColorer != null)
			{
				pusheeColorer.ApplyTint(pushingTintFactor);
			}
			pushing = true;
		}
		else if (targetSidePad.targetPlayer == PlayerInput.Player.Player2 && col.gameObject == Globals.Instance.player2.gameObject)
		{
			if (pusheeColorer != null)
			{
				pusheeColorer.ApplyTint(pushingTintFactor);
			}
			pushing = true;
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (targetSidePad.targetPlayer == PlayerInput.Player.Player1 && col.gameObject == Globals.Instance.player1.gameObject)
		{
			if (pusheeColorer != null)
			{
				pusheeColorer.ApplyTint(1);
			}
			pushing = false;
		}
		else if (targetSidePad.targetPlayer == PlayerInput.Player.Player2 && col.gameObject == Globals.Instance.player2.gameObject)
		{
			if (pusheeColorer != null)
			{
				pusheeColorer.ApplyTint(1);
			}
			pushing = false;
		}
		pusheeCollider.enabled = false;
	}
}

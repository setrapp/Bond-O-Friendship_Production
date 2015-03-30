using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterComponents : MonoBehaviour {
	private bool componentsFound = false;
	public CharacterColors colors;
	public GameObject geometryContainer;
	public MeshRenderer headRenderer;
	public Renderer fillRenderer;
	public SpriteRenderer flashRenderer;
	public TrailRenderer leftTrail;
	public TrailRenderer midTrail;
	public TrailRenderer rightTrail;
	public FluffHandler fluffHandler;
	public FluffThrow fluffThrow;
	public FluffStick fluffStick;
	public BondAttachable bondAttachable;
	public SimpleMover mover;
	public FloatMoving floatMove;
	public SphereCollider bodyCollider;
	public Rigidbody body;
	public Attractor attractor;
	public FlufflessPass flufflessPass;
	[HideInInspector]
	public float fillScale = 1;
	public float flashFadeTime = 1;
	
	void Awake()
	{
		FindComponents();
	}

	void Start()
	{
		SetFlashAndFill(new Color(0, 0, 0, 0));
	}
	
	void Update()
	{
		// Dim indication flash renderer over time.
		if (flashRenderer.color.a > 0)
		{
			Color newFlashColor = flashRenderer.color;
			newFlashColor.a = Mathf.Max(newFlashColor.a - (Time.deltaTime / flashFadeTime), 0);
			SetFlashAndFill(newFlashColor);
			if (bondAttachable.volleyPartner != null)
			{
				Vector3 toPartner = bondAttachable.volleyPartner.transform.position - transform.position;
				toPartner.z = 0;
				flashRenderer.transform.parent.up = toPartner;
			}
		}

		fillRenderer.transform.localScale = new Vector3(fillScale, fillScale, fillScale);

		// Scale collider to smaller of the geometry scale directions, to accomodate squash and stretch;
		bodyCollider.radius = Mathf.Min(geometryContainer.transform.localScale.x, geometryContainer.transform.localScale.z) / 2;
	}

	// Set color of indication flash and fluff spawn fill.
	public void SetFlashAndFill(Color newFlashColor)
	{
		flashRenderer.color = newFlashColor;
		Color newFillColor = fillRenderer.material.color;
		newFillColor.a = 1 - newFlashColor.a;
		fillRenderer.material.color = newFillColor;
	}

	public void FindComponents()
	{
		if (!componentsFound)
		{
			if (fluffHandler == null)
			{
				fluffHandler = GetComponent<FluffHandler>();
			}
			if (fluffThrow == null)
			{
				fluffThrow = GetComponent<FluffThrow>();
			}
			if (fluffStick == null)
			{
				fluffStick = GetComponent<FluffStick>();
			}
			if (bondAttachable == null)
			{
				bondAttachable = GetComponent<BondAttachable>();
			}
			if (mover == null)
			{
				mover = GetComponent<SimpleMover>();
			}
			if (floatMove == null)
			{
				floatMove = GetComponent<FloatMoving>();
			}
			if (bodyCollider == null)
			{
				bodyCollider = GetComponent<SphereCollider>();
			}
			if (body == null)
			{
				body = GetComponent<Rigidbody>();
			}
			if (attractor == null)
			{
				attractor = GetComponent<Attractor>();
			}
		}
		componentsFound = true;
	}
}

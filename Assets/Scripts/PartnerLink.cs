using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartnerLink : MonoBehaviour {
	public bool isPlayer = false;
	public Renderer fillRenderer;
	public SpriteRenderer flashRenderer;
	public float flashFadeTime = 1;
	public TrailRenderer leftTrail;
	public TrailRenderer midTrail;
	public TrailRenderer rightTrail;
	public FluffThrow fluffThrow;
	public ConnectionAttachable connectionAttachable;
	public float partnerLineSize = 0.25f;
	[HideInInspector]
	public SimpleMover mover;
	[HideInInspector]
	public Tracer tracer;
	[HideInInspector]
	public float fillScale = 1;
	public bool empty;
	public float minScale;
	public float maxScale;
	public float normalScale;
	public float preChargeScale;
	public float scaleRestoreRate;
	public float endChargeRestoreRate;
	public bool connectionAbsorb = true;
	public float connectionOffsetFactor = 0.5f;
	public float absorbStrength = 5;
	public bool absorbing = false;
	private bool slowing = false;
	private bool wasSlowing = false;
	public float absorbSpeedFactor = 0;
	public int volleysToConnect = 2;
	private List<Fluff> fluffsToAdd;
	private FloatMoving floatMove;

	void Awake()
	{
		if (mover == null)
		{
			mover = GetComponent<SimpleMover>();
		}
		if (tracer == null)
		{
			tracer = GetComponent<Tracer>();
		}
		if (fluffThrow == null)
		{
			fluffThrow = GetComponent<FluffThrow>();
		}
		if (connectionAttachable == null)
		{
			connectionAttachable = GetComponent<ConnectionAttachable>();
		}

		floatMove = GetComponent<FloatMoving>();
	}

	void Start()
	{
		SetFlashAndFill(new Color(0, 0, 0, 0));
	}
	
	void Update()
	{
		slowing = absorbing && !floatMove.Floating;
		if (slowing != wasSlowing)
		{
			if (slowing)
			{
				mover.externalSpeedMultiplier += absorbSpeedFactor;
			}
			else
			{
				mover.externalSpeedMultiplier -= absorbSpeedFactor;
			}
			wasSlowing = slowing;
		}


		if (flashRenderer.color.a > 0)
		{
			Color newFlashColor = flashRenderer.color;
			newFlashColor.a = Mathf.Max(newFlashColor.a - (Time.deltaTime / flashFadeTime), 0);
			SetFlashAndFill(newFlashColor);
			if (connectionAttachable.volleyPartner != null)
			{
				Vector3 toPartner = connectionAttachable.volleyPartner.transform.position - transform.position;
				toPartner.z = 0;
				flashRenderer.transform.parent.up = toPartner;
			}
		}

		if (fluffsToAdd != null)
		{
			// Spawn fluffs that look like clones of the ones being added.
			for (int i = fluffsToAdd.Count - 1; i >=0 ; i--)
			{
				Material fluffMaterial = null;
				MeshRenderer fluffMesh = fluffsToAdd[i].GetComponentInChildren<MeshRenderer>();
				if (fluffMesh != null)
				{
					fluffMaterial = fluffMesh.material;
				}

				fluffThrow.fluffSpawn.SpawnFluff(true, fluffMaterial);

				Destroy(fluffsToAdd[i].gameObject);
				fluffsToAdd.RemoveAt(i);
			}

			fluffsToAdd.Clear();
			fluffsToAdd = null;
		}

		fillRenderer.transform.localScale = new Vector3(fillScale, fillScale, fillScale);
	}

	public void AttachFluff(Fluff fluff)
	{
		if (fluff != null && (absorbing || fluff.moving) && (fluff.attachee == null || fluff.attachee.gameObject == gameObject || !fluff.attachee.possessive))
		{
			connectionAttachable.AttemptConnection(fluff.creator, fluff.transform.position);

			if (fluff.creator != null && fluff.creator != connectionAttachable)
			{
				SetFlashAndFill(fluff.creator.attachmentColor);
			}

			if (fluffsToAdd == null)
			{
				fluffsToAdd = new List<Fluff>();
			}
			fluffsToAdd.Add(fluff);
		}	
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == "Converser")
		{
			ConnectionAttachable partner = collision.collider.GetComponent<ConnectionAttachable>();
			if (floatMove.Floating && partner != null && !connectionAttachable.IsConnectionMade(partner))
			{
				connectionAttachable.AttemptConnection(partner, transform.position, true);
			}
		}
	}

	public void SetFlashAndFill(Color newFlashColor)
	{
		flashRenderer.color = newFlashColor;
		Color newFillColor = fillRenderer.material.color;
		newFillColor.a = 1 - newFlashColor.a;
		fillRenderer.material.color = newFillColor;
	}

	void OnDrawGizmos()
	{
		if (absorbing)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, absorbStrength);
		}
	}
}

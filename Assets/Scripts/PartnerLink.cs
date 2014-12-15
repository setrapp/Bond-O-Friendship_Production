using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartnerLink : MonoBehaviour {
	public bool isPlayer = false;
	public Renderer headRenderer;
	public Renderer fillRenderer;
	public Renderer flashRenderer;
	public float flashFadeTime = 1;
	public TrailRenderer trail;
	public PulseShot pulseShot;
	public ConnectionAttachable connectionAttachable;
	public float partnerLineSize = 0.25f;
	[HideInInspector]
	public SimpleMover mover;
	[HideInInspector]
	public Tracer tracer;
	public GameObject connectionPrefab;
	[SerializeField]
	public List<Connection> connections;
	[HideInInspector]
	public float fillScale = 1;
	public bool empty;
	public float minScale;
	public float maxScale;
	public float normalScale;
	public float preChargeScale;
	public float scaleRestoreRate;
	public float endChargeRestoreRate;
	public bool absorbing = false;
	private bool slowing = false;
	private bool wasSlowing = false;
	public float absorbSpeedFactor = 0;
	public int volleysToConnect = 2;
	private List<MovePulse> fluffsToAdd;
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
		if (pulseShot == null)
		{
			pulseShot = GetComponent<PulseShot>();
		}
		if (connectionAttachable == null)
		{
			connectionAttachable = GetComponent<ConnectionAttachable>();
		}

		floatMove = GetComponent<FloatMoving>();
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


		if (flashRenderer.material.color.a > 0)
		{
			Color newFlashColor = flashRenderer.material.color;
			newFlashColor.a = Mathf.Max(newFlashColor.a - (Time.deltaTime / flashFadeTime), 0);
			SetFlashAndFill(newFlashColor);
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

				pulseShot.fluffSpawn.SpawnFluff(true, fluffMaterial);

				Destroy(fluffsToAdd[i].gameObject);
				fluffsToAdd.RemoveAt(i);
			}

			fluffsToAdd.Clear();
			fluffsToAdd = null;
		}

		fillRenderer.transform.localScale = new Vector3(fillScale, fillScale, fillScale);

		trail.startWidth = transform.localScale.x;
	}

	public void AttachFluff(MovePulse pulse)
	{
		if (pulse != null && (absorbing || pulse.moving) && (pulse.attachee == null || pulse.attachee.gameObject == gameObject || !pulse.attachee.possessive))
		{
			connectionAttachable.AttemptConnection(pulse);

			if (pulse.creator != null && pulse.creator != connectionAttachable)
			{
				SetFlashAndFill(pulse.creator.attachmentColor);
			}

			if (fluffsToAdd == null)
			{
				fluffsToAdd = new List<MovePulse>();
			}
			fluffsToAdd.Add(pulse);
		}	
	}

	public void SetFlashAndFill(Color newFlashColor)
	{
		flashRenderer.material.color = newFlashColor;
		Color newFillColor = fillRenderer.material.color;
		newFillColor.a = 1 - newFlashColor.a;
		fillRenderer.material.color = newFillColor;
	}

	private void OnDestroy()
	{
		for (int i = 0; i < connections.Count;)
		{
			connections[i].BreakConnection();
		}
	}

}

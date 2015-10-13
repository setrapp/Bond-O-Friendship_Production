using UnityEngine;
using System.Collections;

public class SetShaderData_DarkAlphaMasker : MonoBehaviour {

	public GameObject p1, p2;
	public Luminus l1, l2;
	public Luminus oldL1, oldL2;
	public float l1p1_sqMag, l2p2_sqMag;    //shortest distances (useful in shader)
	private Vector4 mul_sameLuminus, mul_diffLuminus;
	public Renderer maskRenderer;
	public bool fadeIn = false;
	public float fadeTime = 1;
	public DarknessTrigger trigger = null;

	//Imaginary height of the light source
	public float height = 1.5f;

	// Use this for initialization
	void Start () {
		if (maskRenderer == null)
		{
			maskRenderer = GetComponent<Renderer>();
		}
		mul_sameLuminus = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
		mul_diffLuminus = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
		p1 = Globals.Instance.Player1.gameObject;
		p2 = Globals.Instance.Player2.gameObject;

		if (trigger != null)
		{
			fadeIn = trigger.isActiveAndEnabled;
		}

		Color maskColor = maskRenderer.material.color;
		if (fadeIn && maskColor.a < 1)
		{
			if (fadeTime > 0)
			{
				maskColor.a += Time.deltaTime / fadeTime;
			}
			else
			{
				maskColor.a = 1;
			}
		}
		else if (!fadeIn && maskColor.a > 0)
		{
			if (fadeTime > 0)
			{
				maskColor.a -= Time.deltaTime / fadeTime;
			}
			else
			{
				maskColor.a = 0;
			}
		}
		maskColor.a = Mathf.Clamp01(maskColor.a);
		maskRenderer.material.color = maskColor;

		//every frame, update players positions on material (for shader)
		Vector3 pos = new Vector3(p1.transform.position.x, p1.transform.position.y, transform.position.z - height);
		maskRenderer.material.SetVector("_P1Pos", pos);
		pos = new Vector3(p2.transform.position.x, p2.transform.position.y, transform.position.z - height);
		maskRenderer.material.SetVector("_P2Pos", pos);

		//repeat for the two closest lumini
		if (l1 != null)
		{
			pos = new Vector3(l1.transform.position.x, l1.transform.position.y, transform.position.z - height);
			maskRenderer.material.SetVector("_L1Pos", pos);
		}
		if (l2 != null)
		{
			pos = new Vector3(l2.transform.position.x, l2.transform.position.y, transform.position.z - height);
			maskRenderer.material.SetVector("_L2Pos", pos);
		}

		// Determine if both players are near the same luminus.
		Vector4 lightingMultiples = mul_diffLuminus;
		if (l1 == l2)
		{
			lightingMultiples = mul_sameLuminus;
		}
        

		// Ignore luminus if it has no intensity.
		if (l1 != null && l1.intensity <= 0)
		{
			lightingMultiples.z = 0;
		}
		if (l2 != null && l2.intensity <= 0)
		{
			lightingMultiples.w = 0;
		}

		// Apply the players' actual intensities.
		if (Globals.Instance != null)
		{
			lightingMultiples.x *= Globals.Instance.playerLuminIntensity;
			lightingMultiples.y *= Globals.Instance.playerLuminIntensity;
		}

		// Apply the lumini's actual intensities.
		if (l1 != null)
		{
			lightingMultiples.z *= l1.intensity;
		}
		if (l2 != null)
		{
			lightingMultiples.w *= l2.intensity;
		}

		maskRenderer.material.SetVector("_Mul", lightingMultiples);

	}
}

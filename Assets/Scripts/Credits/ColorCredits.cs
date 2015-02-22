using UnityEngine;
using System.Collections;

public class ColorCredits : MonoBehaviour {

	public GameObject fluffPrefab;
	public Material player1Material;
	public Material player2Material;
	private int fluffCount;
	public int maxFluffsPerLetter;
	public bool colored;
	public static Color[] letterColors;
	
	void Start () {
		player1Material = Globals.Instance.player1.character.fluffHandler.fluffMaterial;
		player2Material = Globals.Instance.player2.character.fluffHandler.fluffMaterial;
		letterColors = new Color[transform.parent.childCount];
	}

	void OnCollisionEnter (Collision col) {
		if(LayerMask.LayerToName(col.gameObject.layer) == "CreditsHit")
		{
			if(colored == false && transform.childCount > maxFluffsPerLetter/3.0f)
			{
				for(int i = 0; i < letterColors.Length; i++)
				{
					if(letterColors[0].r + letterColors[0].g + letterColors[0].b == 0)
					{
						GetComponent<Renderer>().material.color = new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f), 0.8f);
						letterColors[0] = GetComponent<Renderer>().material.color;
						break;
					}
					else if(letterColors[0].r + letterColors[0].g + letterColors[0].b >= 1.6f && letterColors[i].r + letterColors[i].g + letterColors[i].b == 0)
					{
						GetComponent<Renderer>().material.color = new Color(letterColors[i-1].r - 0.05f, letterColors[i-1].g - 0.05f, letterColors[i-1].b - 0.05f,  0.8f);
						letterColors[i] = GetComponent<Renderer>().material.color;
						break;
					}
					else if(letterColors[i].r + letterColors[i].g + letterColors[i].b == 0)
					{
						GetComponent<Renderer>().material.color = new Color(letterColors[i-1].r + 0.05f, letterColors[i-1].g + 0.05f, letterColors[i-1].b + 0.05f,  0.8f);
						letterColors[i] = GetComponent<Renderer>().material.color;
						break;
					}
				}
				colored = true;
			}
			if (transform.childCount >= maxFluffsPerLetter)
			{
				Destroy(transform.GetChild(0).gameObject);

			}
				GameObject fluff = (GameObject)Instantiate(fluffPrefab, col.contacts[0].point, Quaternion.identity);
			fluff.transform.parent = transform;
			int randomColor = Random.Range(1, 3);
			fluffCount++;
			fluff.transform.up = -col.contacts[0].normal;
			Material fluffMat = player1Material;
			if (randomColor == 2)
			{
				fluffMat = player2Material;
			}

			MeshRenderer[] meshRenderers = fluff.GetComponentsInChildren<MeshRenderer>();
			for (int i = 0; i < meshRenderers.Length; i++)
			{
				meshRenderers[i].material = fluffMat;
			}
		}

	}

	/*void OnTriggerEnter (Collider col) {
		if(LayerMask.LayerToName(col.gameObject.layer) == "Bond" || col.gameObject.tag == "Character")
		{
			int random = Random.Range(0, 15);
			if(random == 1)
			{
				GameObject fluff = (GameObject)Instantiate(fluffPrefab, col.contacts[0].point, Quaternion.identity);
				fluff.transform.up = col.contacts[0].normal;
			}
		}

	}*/
}

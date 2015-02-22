using UnityEngine;
using System.Collections;

public class DarknessLayers : MonoBehaviour {
	public Renderer layerOriginal;
	public bool skipFirst = true;
	public int layerCount = 10;
	public float alphaInterval = 0.1f;
	public float depthInterval = 0.1f;

	void Start()
	{
		if (layerOriginal != null)
		{
			for (int i = (!skipFirst) ? 0 : 1; i < layerCount; i++)
			{
				GameObject newLayer = (GameObject)Instantiate(layerOriginal.gameObject, transform.position, Quaternion.identity);
				newLayer.transform.parent = transform;
				newLayer.transform.position -= new Vector3(0, 0, depthInterval * i);
				Material layerMaterial = newLayer.GetComponent<Renderer>().material;
				if (layerMaterial != null)
				{
					Color layerColor = layerMaterial.color;
					layerColor.a = Mathf.Max(layerColor.a - (alphaInterval * i), 0);
					layerMaterial.color = layerColor;
				}
			}
		}
	}
}

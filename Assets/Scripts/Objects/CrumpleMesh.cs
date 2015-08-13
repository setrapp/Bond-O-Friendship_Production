using UnityEngine;
using System.Collections;

public class CrumpleMesh : MonoBehaviour
{
    // This script is placed in public domain. The author takes no responsibility for any possible harm.

    public bool x = true;
    public bool y = true;
    public bool z = true;

	public bool useRandomValues = true;

    public float scale = 1.0f;
    public float speed = 1.0f;
    bool recalculateNormals = false;

    public bool useOtherValues = false;

    public bool wilin = true;
    public bool wasWilin = false;

    public bool firstWil = true;

    public CrumpleMesh valuesToUse;

    private Vector3[] baseVertices;
    //private var noise : Perlin;

    void Start()
    {
        //noise = new Perlin ();
        if (useOtherValues && valuesToUse != null)
        {
            scale = valuesToUse.scale;
            speed = valuesToUse.speed;
        }
        else
        {
			if (useRandomValues)
			{
				scale = Random.Range(.1f, .3f);
				speed = Random.Range(1.0f, 2.0f);
			}
        }
    }

    void Update()
    {
        if (useOtherValues && valuesToUse != null)
        {
            scale = valuesToUse.scale;
            speed = valuesToUse.speed;
            wilin = valuesToUse.wilin;
        }
        if (wilin || firstWil)
        {          

            Mesh mesh = GetComponent<MeshFilter>().mesh;

            if (baseVertices == null)
                baseVertices = mesh.vertices;

            var vertices = new Vector3[baseVertices.Length];

            float timex = Time.time * speed + 0.1365143f;
            float timey = Time.time * speed + 1.21688f;
            float timez = Time.time * speed + 2.5564f;
            for (var i = 0; i < vertices.Length; i++)
            {
                var vertex = baseVertices[i];

                if (x)
                    vertex.x += (Mathf.PerlinNoise(timex + vertex.x, timex + vertex.z) - 0.5f) * scale;
                if (y)
                    vertex.y += (Mathf.PerlinNoise(timey + vertex.x, timey + vertex.y) - 0.5f) * scale;
                if (z)
					vertex.z += (Mathf.PerlinNoise(timez + vertex.x, timez + vertex.z) - 0.5f) * scale;

                vertices[i] = vertex;
            }

            mesh.vertices = vertices;

            if (recalculateNormals)
                mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            firstWil = false;
        }
    }

}

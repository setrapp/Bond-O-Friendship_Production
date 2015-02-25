using UnityEngine;
using System.Collections;

public class MirrorPaint : MonoBehaviour
{
    public GameObject playerToMirror;
    public GameObject paintPrefab;
    public bool eraserOn;
    private GameObject paintCircle;
    private float paintTime;
    private Vector3 paintPos;
    private float paintJitter;
    public bool painting;
    public Color paintColor;
    public int randRot;
    private float painttimeFloat;
    private float zJitter;

    // Use this for initialization
    void Start()
    {
        paintTime = 0.05f;
        paintJitter = 0.5f;
        paintJitter = 0.05f;
        painttimeFloat = 0.05f;
        painting = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (playerToMirror != null)
        {
            if (!eraserOn)
            {
                paintJitter = Random.Range(-0.5f, 0.5f);
                zJitter = Random.Range(0.5f, 0.7f);
                randRot = Random.Range(0, 360);

                paintPos = new Vector3(transform.position.x + paintJitter, transform.position.y + paintJitter, transform.position.z + zJitter);

                if (painting == true && playerToMirror.GetComponent<CharacterComponents>().mover.velocity.sqrMagnitude != 0)
                {
                    if (paintTime == painttimeFloat)
                    {
                        Blot();
                    }
                    paintTime -= Time.deltaTime;
                }
                if (paintTime <= 0.0f)
                {
                    paintTime = painttimeFloat;
                }
            }
            else
                Erase();
        }
    }

    void Blot()
    {
        paintCircle = Instantiate(paintPrefab, paintPos, Quaternion.Euler(0, 0, randRot)) as GameObject;
        paintCircle.GetComponent<Renderer>().material.color = paintColor;
		paintCircle.GetComponent<PaintCircle>().paintCircColor = paintColor;
        paintCircle.GetComponent<PaintCircle>().rLifemin = 5.0f;
        paintCircle.GetComponent<PaintCircle>().rLifemax = 10.0f;
        paintCircle.GetComponent<PaintCircle>().rSizemin = 0.5f;
        paintCircle.GetComponent<PaintCircle>().rSizemax = 3.0f;
    }

    void Erase()
    {

        var hits = Physics.RaycastAll(transform.position, Vector3.forward, Mathf.Infinity);

        foreach (var hit in hits)
        {
            if (hit.transform.gameObject.GetComponent<PaintCircle>() != null)
            {
                hit.transform.gameObject.GetComponent<PaintCircle>().erased = true;
                hit.transform.gameObject.GetComponent<PaintCircle>().myLife = 0;
            }
        }
    }
}

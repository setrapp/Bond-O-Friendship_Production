using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FadeOptions: MonoBehaviour {

	public List<Renderer> soundOffRenderers;
    public List<Renderer> soundOnRenderers;
	private List<Color> optionsColorsEmpty = new List<Color>();
	private List<Color> optionsColorsFull = new List<Color>();

    private Color solvedColorFull;
    private Color solvedColorEmpty;
    private Color unsolvedColorFull;
    private Color unsolvedColorEmpty;

    public bool colorsSet = false;

	public float f = 0.0f;
	public float duration = 2.0f;
	public bool fadeOut;
	public bool fadeIn;

    private Vector3 posNoZ;
    private Vector3 player1NoZ;
    private Vector3 player2NoZ;

    public float distance = 2.0f;
    private float distancePow = 0.0f;

    private float disToPlayer1;
    private float disToPlayer2;

    public bool player1Toggled = false;
    public bool player2Toggled = false;

	// Use this for initialization
	void Awake () 
	{
		/*foreach (Renderer renderer in optionsRenderers)
		{
			renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.0f);
			optionsColorsEmpty.Add(renderer.material.color);
			optionsColorsFull.Add(new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1.0f));
		}*/

        solvedColorFull = soundOnRenderers[0].GetComponent<ClusterNode>().bondColor;
        solvedColorEmpty = new Color(solvedColorFull.r, solvedColorFull.g, solvedColorFull.b, 0.0f);
        unsolvedColorFull = Color.white;
        unsolvedColorEmpty = new Color(unsolvedColorFull.r, unsolvedColorFull.g, unsolvedColorFull.b, 0.0f);

        distancePow = Mathf.Pow(distance, 2);
	}
	
	public void FadeIn()
    {
        if (f != 1)
        {
            f = Mathf.Clamp(f + Time.deltaTime / duration, 0.0f, 1.0f);

            if (Globals.Instance.mute)
            {
                for (int i = 0; i < soundOffRenderers.Count; i++)
                {
                    soundOffRenderers[i].material.color = Color.Lerp(solvedColorEmpty, solvedColorFull, f);
                }

                for (int i = 0; i < soundOnRenderers.Count; i++)
                {
                    soundOnRenderers[i].material.color = Color.Lerp(unsolvedColorEmpty, unsolvedColorFull, f);
                }
            }
            else
            {
                for (int i = 0; i < soundOffRenderers.Count; i++)
                {
                    soundOffRenderers[i].material.color = Color.Lerp(unsolvedColorEmpty, unsolvedColorFull, f);
                }

                for (int i = 0; i < soundOnRenderers.Count; i++)
                {
                    soundOnRenderers[i].material.color = Color.Lerp(solvedColorEmpty, solvedColorFull, f);
                }
            }

        }
	}
	
	public void FadeOut()
	{
		if (f != 0) {
			f = Mathf.Clamp (f - Time.deltaTime / duration, 0.0f, 1.0f);
            if (Globals.Instance.mute)
            {
                for (int i = 0; i < soundOffRenderers.Count; i++)
                {
                    soundOffRenderers[i].material.color = Color.Lerp(solvedColorEmpty, solvedColorFull, f);
                }

                for (int i = 0; i < soundOnRenderers.Count; i++)
                {
                    soundOnRenderers[i].material.color = Color.Lerp(unsolvedColorEmpty, unsolvedColorFull, f);
                }
            }
            else
            {
                for (int i = 0; i < soundOffRenderers.Count; i++)
                {
                    soundOffRenderers[i].material.color = Color.Lerp(unsolvedColorEmpty, unsolvedColorFull, f);
                }

                for (int i = 0; i < soundOnRenderers.Count; i++)
                {
                    soundOnRenderers[i].material.color = Color.Lerp(solvedColorEmpty, solvedColorFull, f);
                }
            }
			
		} 
	}

    void Update()
    {
        if (!colorsSet)
            SetTextColors();

         posNoZ = new Vector3(transform.position.x, transform.position.y, 0.0f);


         if (Player1InRange() || Player2InRange())
         {
             FadeIn();
         }
         else
         {
             FadeOut();
         }
    }

    private bool Player1InRange()
    {
        player1NoZ = new Vector3(Globals.Instance.Player1.transform.position.x, Globals.Instance.Player1.transform.position.y, 0.0f);
        disToPlayer1 = Vector3.SqrMagnitude(player1NoZ - posNoZ);
        player1Toggled = disToPlayer1 < distancePow;
        return player1Toggled;
    }

    private bool Player2InRange()
    {
        player2NoZ = new Vector3(Globals.Instance.Player2.transform.position.x, Globals.Instance.Player2.transform.position.y, 0.0f);
        disToPlayer2 = Vector3.SqrMagnitude(player2NoZ - posNoZ);
        player2Toggled = disToPlayer2 < distancePow;
        return player2Toggled;
    }

    private void SetTextColors()
    {
        int colorSetCount = 0;

        foreach (Renderer renderer in soundOnRenderers)
        {
            if (renderer.GetComponent<ClusterNode>().colorSet)
            {
                renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.0f);
                optionsColorsEmpty.Add(renderer.material.color);
                optionsColorsFull.Add(new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1.0f));
                colorSetCount++;
            }
        }

        foreach (Renderer renderer in soundOffRenderers)
        {
            if (renderer.GetComponent<ClusterNode>().colorSet)
            {
                renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.0f);
                optionsColorsEmpty.Add(renderer.material.color);
                optionsColorsFull.Add(new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1.0f));
                colorSetCount++;
            }
        }
        var optionRenderersCount = soundOnRenderers.Count + soundOffRenderers.Count;

        if (colorSetCount == optionRenderersCount)
            colorsSet = true;


    }

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FadeQuitGame : MonoBehaviour {

	public Renderer prompt;
	private Color promptFull;
	private Color promptEmpty;

	public List<Renderer> textRenderers;
	private List<Color> textColorsEmpty = new List<Color>();
	private List<Color> textColorsFull = new List<Color>();

	public float f = 0.0f;
	public float duration = 1.0f;

	private bool colorsSet = false;

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
	void Start () 
	{
		prompt.material.color = new Color (prompt.material.color.r, prompt.material.color.g, prompt.material.color.b, 0.0f);
		promptEmpty =  prompt.material.color;
		promptFull = new Color (prompt.material.color.r, prompt.material.color.g, prompt.material.color.b, 1.0f);	

		foreach (Renderer renderer in textRenderers) 
		{
			renderer.material.color = new Color (renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.0f);
			textColorsEmpty.Add (renderer.material.color);
			textColorsFull.Add (new Color (renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1.0f));
		}

        distancePow = Mathf.Pow(distance, 2);
	}

	public void FadeIn()
	{
		if (f != 1) 
		{
			f = Mathf.Clamp (f + Time.deltaTime / duration, 0.0f, 1.0f);
			//Text Prompt
			prompt.material.color = Color.Lerp(promptEmpty, promptFull, f);
			//Text Interactable
			for (int i = 0; i < textRenderers.Count; i++) 
			{
				textRenderers [i].material.color = Color.Lerp (textColorsEmpty [i], textColorsFull [i], f);
			}

		}
	}

	public void FadeOut()
	{
		if (f != 0) 
		{
			f = Mathf.Clamp (f - Time.deltaTime / duration, 0.0f, 1.0f);
			//Text Prompt
			prompt.material.color = Color.Lerp(promptEmpty, promptFull, f);
			//Interactable Text 
			for (int l = 0; l < textRenderers.Count; l++) 
			{
				textRenderers [l].material.color = Color.Lerp (textColorsEmpty [l], textColorsFull [l], f);
				textRenderers[l].GetComponent<ClusterNode>().lit = false;
			}

		}
	}

    void Update()
    {
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



	
	
}

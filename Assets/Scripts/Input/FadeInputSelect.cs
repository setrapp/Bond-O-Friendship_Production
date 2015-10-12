using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FadeInputSelect : MonoBehaviour {

	public List<Renderer> inputSelectRenderers;
	private List<Color> inputSelectColorsEmpty = new List<Color>();
	private List<Color> inputSelectColorsFull = new List<Color>();


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

    public FollowPlayerInputKeyboard followKeyboardInput;

	// Use this for initialization
	void Awake () 
	{
		foreach (Renderer renderer in inputSelectRenderers)
		{

			renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.0f);
			inputSelectColorsEmpty.Add(renderer.material.color);
			inputSelectColorsFull.Add(new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1.0f));
		}

        distancePow = Mathf.Pow(distance, 2);
	
	}

    void Update ()
    {
        posNoZ = new Vector3(transform.position.x, transform.position.y, 0.0f);


        if(Player1InRange() || Player2InRange())
        {
            FadeIn();
        }
        else
        {
            followKeyboardInput.setColor = false;
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

	public void FadeIn()
	{	if (f != 1) 
	    {
			f = Mathf.Clamp (f + Time.deltaTime / duration, 0.0f, 1.0f);
			for (int i = 0; i < inputSelectRenderers.Count; i++) {		
				inputSelectRenderers [i].material.color = Color.Lerp (inputSelectColorsEmpty [i], inputSelectColorsFull [i], f);
			}
			
		}
    else
    {
        followKeyboardInput.setColor = true;
    }
	}
	
	public void FadeOut()
	{
		if (f != 0) 
        {
			f = Mathf.Clamp (f - Time.deltaTime / duration, 0.0f, 1.0f);
			for (int i = 0; i < inputSelectRenderers.Count; i++) {				
				inputSelectRenderers [i].material.color = Color.Lerp (inputSelectColorsEmpty [i], inputSelectColorsFull [i], f);
			}
			
		} 
	}

}

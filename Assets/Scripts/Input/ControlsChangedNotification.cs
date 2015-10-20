using UnityEngine;
using System.Collections;

public class ControlsChangedNotification : MonoBehaviour 
{
     
    public RectTransform keyboardPlayer1;
    public RectTransform gamepadPlayer1;
    public RectTransform gamepadPlayer2;
    public RectTransform keyboardPlayer2;
    public RectTransform gamepadSharedPlayer1Left;
    public RectTransform gamepadSharedPlayer2Left;
    public RectTransform keyboardSharedPlayer1Left;
    public RectTransform keyboardSharedPlayer2Left;


    private float duration = 1.0f;
    public float f = 1.0f;

    private Vector2 keyboardPlayer1v2;
    private Vector2 gamepadPlayer1v2;
    private Vector2 gamepadPlayer2v2;
    private Vector2 keyboardPlayer2v2;
    private Vector2 gamepadSharedPlayer1Leftv2;
    private Vector2 gamepadSharedPlayer2Leftv2;
    private Vector2 keyboardSharedPlayer1Leftv2;
    private Vector2 keyboardSharedPlayer2Leftv2;

	// Use this for initialization
    void Start()
    {
        keyboardPlayer1v2 = keyboardPlayer1.anchoredPosition;
        gamepadPlayer1v2 = gamepadPlayer1.anchoredPosition;
        gamepadPlayer2v2 = gamepadPlayer2.anchoredPosition;
        keyboardPlayer2v2 = keyboardPlayer2.anchoredPosition;
        gamepadSharedPlayer1Leftv2 = gamepadSharedPlayer1Left.anchoredPosition;
        gamepadSharedPlayer2Leftv2 = gamepadSharedPlayer2Left.anchoredPosition;
        keyboardSharedPlayer1Leftv2 = keyboardSharedPlayer1Left.anchoredPosition;
        keyboardSharedPlayer2Leftv2 = keyboardSharedPlayer2Left.anchoredPosition;
    }
	
	// Update is called once per frame
	void Update () {
        if (Globals.Instance.notifyControlsChangeOnDisconnect)
            MoveControlsDown();
	}

    private void MoveControlsDown()
    {
        if (f != 0)
        {
            f = Mathf.Clamp(f - Time.deltaTime / duration, 0.0f, 1.0f);

           keyboardPlayer1.anchoredPosition = Vector2.Lerp(keyboardPlayer1v2, new Vector2(keyboardPlayer1v2.x, 200.0f), f);
           gamepadPlayer1.anchoredPosition = Vector2.Lerp(gamepadPlayer1v2, new Vector2(gamepadPlayer1v2.x, 200.0f), f);

           gamepadPlayer2.anchoredPosition = Vector2.Lerp(gamepadPlayer2v2, new Vector2(gamepadPlayer2v2.x, 200.0f), f);
           keyboardPlayer2.anchoredPosition = Vector2.Lerp(keyboardPlayer2v2, new Vector2(keyboardPlayer2v2.x, 200.0f), f);

           gamepadSharedPlayer1Left.anchoredPosition = Vector2.Lerp(gamepadSharedPlayer1Leftv2, new Vector2(gamepadSharedPlayer1Leftv2.x, 200.0f), f);
           gamepadSharedPlayer2Left.anchoredPosition = Vector2.Lerp(gamepadSharedPlayer2Leftv2, new Vector2(gamepadSharedPlayer2Leftv2.x, 200.0f), f);

           keyboardSharedPlayer1Left.anchoredPosition = Vector2.Lerp(keyboardSharedPlayer1Leftv2, new Vector2(keyboardSharedPlayer1Leftv2.x, 200.0f), f);
           keyboardSharedPlayer2Left.anchoredPosition = Vector2.Lerp(keyboardSharedPlayer2Leftv2, new Vector2(keyboardSharedPlayer2Leftv2.x, 200.0f), f);

        }
        else
        {
            f = 1;
            Globals.Instance.notifyControlsChangeOnDisconnect = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class StartScript : MonoBehaviour {

    public int playerId;
    private Rewired.Player player;

    int menuState;
    bool axisActive;
    float axisValue;

    public GameObject arrow;

    public AudioClip buttonClick;
    AudioSource buttonClickSound;

    void Start()
    {
        player = ReInput.players.GetPlayer(playerId);

        menuState = 0;
        axisActive = false;
        axisValue = 0f;

        buttonClickSound = GetComponent<AudioSource>();

        //(-261.4, -61.9) (-261.4, -222.5) (-261.4, -387)
    }

    void Update()
    {
        //Makes the axis like a button and only moved once
        if (player.GetAxis("LSV") != 0)
        {
            if (!axisActive)
            {
                MoveCursor();
                axisActive = true;
            }
        }
        if (player.GetAxis("LSV") == 0)
        {
            axisActive = false;
            axisValue = 0f;
        }

        if (player.GetButtonDown("A"))
        {
            switch (menuState)
            {
                case 0:
                    PlayGame();
                    break;

                case 1:
                    GameOptions();
                    break;

                case 2:
                    QuitGame();
                    break;
            }
        }
    }

    void MoveCursor()
    {
        buttonClickSound.PlayOneShot(buttonClick, 1f);

        //Check which way the cursor should move
        axisValue = player.GetAxis("LSV");
        if (axisValue > 0)
        {
            menuState--;
        }

        if (axisValue < 0)
        {
            menuState++;
        }

        //Move the cusor
        switch (menuState)
        {
            case -1:
                menuState = 0;
                break;
            case 0:
                Debug.Log("Play");
                arrow.transform.localPosition = new Vector2(-261.4f, -61.9f);
                break;

            case 1:
                Debug.Log("Options");
                arrow.transform.localPosition = new Vector2(-261.4f, -222.5f);
                break;

            case 2:
                Debug.Log("Quit");
                arrow.transform.localPosition = new Vector2(-261.4f, -387f);
                break;
            case 3:
                menuState = 2;
                break;
        }
    }

    void PlayGame()
    {
        buttonClickSound.PlayOneShot(buttonClick, 1f);
        SceneManager.LoadScene("Player_Login_Screen");
    }

    void GameOptions()
    {
        buttonClickSound.PlayOneShot(buttonClick, 1f);
        SceneManager.LoadScene("Options_Screen");
    }

    void QuitGame()
    {
        buttonClickSound.PlayOneShot(buttonClick, 1f);
        Application.Quit();
    }
}

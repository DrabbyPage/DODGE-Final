using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class EndScript : MonoBehaviour {

    public int playerId1 = 0;
    public int playerId2 = 1;
    public int playerId3 = 2;
    public int playerId4 = 3;

    private Rewired.Player player1;
    private Rewired.Player player2;
    private Rewired.Player player3;
    private Rewired.Player player4;

    //public static int numberOfPlayers = 0;
    //public static int p1CharacterClass = 0, p2CharacterClass = 0, p3CharacterClass = 0, p4CharacterClass = 0;
    int numberOfReadyPlayers;

    public int p1State, p2State, p3State, p4State;
    bool p1Ready, p2Ready, p3Ready, p4Ready;
    public GameObject p1ReadyText, p2ReadyText, p3ReadyText, p4ReadyText;

    public AudioClip buttonClick;
    AudioSource buttonClickSound;

    void Start()
    {
        player1 = ReInput.players.GetPlayer(playerId1);
        player2 = ReInput.players.GetPlayer(playerId2);
        player3 = ReInput.players.GetPlayer(playerId3);
        player4 = ReInput.players.GetPlayer(playerId4);

        p1State = 0;
        p2State = 0;
        p3State = 0;
        p4State = 0;

        numberOfReadyPlayers = 0;

        p1Ready = false;
        p2Ready = false;
        p3Ready = false;
        p4Ready = false;

        buttonClickSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        PlayerViewEndStats(player1, playerId1);
        PlayerViewEndStats(player2, playerId2);
        PlayerViewEndStats(player3, playerId3);
        PlayerViewEndStats(player4, playerId4);
    }

    void PlayerViewEndStats(Rewired.Player player, int playerId)
    {
        //Advances the player through the menu
        if (player.GetButtonDown("A"))
        {
            buttonClickSound.PlayOneShot(buttonClick, 1f);

            Debug.Log(playerId + " Advance");
            switch (playerId)
            {
                //Player 1
                case 0:
                    p1State++;
                    break;

                //Player 2
                case 1:
                    p2State++;
                    break;

                //Player 3
                case 2:
                    p3State++;
                    break;

                //Player 4
                case 3:
                    p4State++;
                    break;
            }
            Player1State();
            Player2State();
            Player3State();
            Player4State();
        }

        //Regresses the player through the menu
        if (player.GetButtonDown("B"))
        {
            buttonClickSound.PlayOneShot(buttonClick, 1f);

            //Debug.Log(playerId + " Back");
            switch (playerId)
            {
                //Player 1
                case 0:
                    p1State--;
                    break;

                //Player 2
                case 1:
                    p2State--;
                    break;

                //Player 3
                case 2:
                    p3State--;
                    break;

                //Player 4
                case 3:
                    p4State--;
                    break;
            }
            Player1State();
            Player2State();
            Player3State();
            Player4State();
        }

        CheckReadyPlayers();
    }

    void Player1State()
    {
        switch (p1State)
        {
            //Sets a minimum for the states so that the player can advance immediately
            case -1:
                p1State = 0;
                break;

            //Viewing Stats State
            case 0:
                //Debug.Log("State: " + p1State);
                //show match stats
                p1ReadyText.SetActive(false);
                p1Ready = false;
                break;

            //Ready state
            case 1:
                //Debug.Log("State: " + p1State);
                //show ready text and advance if all joined players are
                p1ReadyText.SetActive(true);
                p1Ready = true;
                break;

            //Sets a maximum for the states so that the player can regress immediately
            case 2:
                p1State = 1;
                break;
        }
    }
            
    void Player2State()
    {
        switch (p2State)
        {
            //Sets a minimum for the states so that the player can advance immediately
            case -1:
                p2State = 0;
                break;

            //Viewing Stats State
            case 0:
                //Debug.Log("State: " + p1State);
                //show match stats
                p2ReadyText.SetActive(false);
                p2Ready = false;
                break;

            //Ready state
            case 1:
                //Debug.Log("State: " + p1State);
                //show ready text and advance if all joined players are
                p2ReadyText.SetActive(true);
                p2Ready = true;
                break;

            //Sets a maximum for the states so that the player can regress immediately
            case 2:
                p2State = 1;
                break;
        }
    }
            
    void Player3State()
    {
        switch (p3State)
        {
            //Sets a minimum for the states so that the player can advance immediately
            case -1:
                p3State = 0;
                break;

            //Viewing Stats State
            case 0:
                //Debug.Log("State: " + p1State);
                //show match stats
                p3ReadyText.SetActive(false);
                p3Ready = false;
                break;

            //Ready state
            case 1:
                //Debug.Log("State: " + p1State);
                //show ready text and advance if all joined players are
                p3ReadyText.SetActive(true);
                p3Ready = true;
                break;

            //Sets a maximum for the states so that the player can regress immediately
            case 2:
                p3State = 1;
                break;
        }
    }
            
    void Player4State()
    {
        switch (p4State)
        {
            //Sets a minimum for the states so that the player can advance immediately
            case -1:
                p4State = 0;
                break;

            //Viewing Stats State
            case 0:
                //Debug.Log("State: " + p1State);
                //show match stats
                p4ReadyText.SetActive(false);
                p4Ready = false;
                break;

            //Ready state
            case 1:
                //Debug.Log("State: " + p1State);
                //show ready text and advance if all joined players are
                p4ReadyText.SetActive(true);
                p4Ready = true;
                break;

            //Sets a maximum for the states so that the player can regress immediately
            case 2:
                p4State = 1;
                break;
        }
    }

    void CheckReadyPlayers()
    {
        switch (ManagerScript.playerAmount)
        {
            case 1:
                if (p1Ready)
                {
                    RestartGame();
                }
                break;

            case 2:
                if (p1Ready && p2Ready)
                {
                    RestartGame();
                }
                break;

            case 3:
                if (p1Ready && p2Ready && p3Ready)
                {
                    RestartGame();
                }
                break;

            case 4:
                if (p1Ready && p2Ready && p3Ready && p4Ready)
                {
                    RestartGame();
                }
                break;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Start_Menu");
        //SceneManager.LoadScene("Player_Login_Scene");
    }
}

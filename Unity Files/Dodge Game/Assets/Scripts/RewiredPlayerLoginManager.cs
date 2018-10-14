using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class RewiredPlayerLoginManager : MonoBehaviour
{

    public int playerId1 = 0;
    public int playerId2 = 1;
    public int playerId3 = 2;
    public int playerId4 = 3;

    private Rewired.Player player1;
    private Rewired.Player player2;
    private Rewired.Player player3;
    private Rewired.Player player4;

    public static int numberOfPlayers = 0;
    public static int p1CharacterClass = 0, p2CharacterClass = 0, p3CharacterClass = 0, p4CharacterClass = 0;
    int numberOfReadyPlayers;

    public int p1State, p2State, p3State, p4State;

    public GameObject p1JoinText, p2JoinText, p3JoinText, p4JoinText;
    public GameObject p1Panel, p2Panel, p3Panel, p4Panel;
    public GameObject p1ReadyText, p2ReadyText, p3ReadyText, p4ReadyText;

    public GameObject p1StrikerCharacter, p2StrikerCharacter, p3StrikerCharacter, p4StrikerCharacter;
    public GameObject p1BlockerCharacter, p2BlockerCharacter, p3BlockerCharacter, p4BlockerCharacter;
    public Graphic p1CharacterRightSelectButton, p2CharacterRightSelectButton, p3CharacterRightSelectButton, p4CharacterRightSelectButton;
    public Graphic p1CharacterLeftSelectButton, p2CharacterLeftSelectButton, p3CharacterLeftSelectButton, p4CharacterLeftSelectButton;

    Color32 p1Color, p1SelectedColor;
    Color32 p2Color, p2SelectedColor;
    Color32 p3Color, p3SelectedColor;
    Color32 p4Color, p4SelectedColor;

    bool p1Join, p2Join, p3Join, p4Join;
    bool p1Ready, p2Ready, p3Ready, p4Ready;

    bool p1AxisActive, p2AxisActive, p3AxisActive, p4AxisActive;

    public GameObject startTextPanel;

    public AudioClip buttonClick;
    AudioSource buttonClickSound;

    int p1Team, p2Team, p3Team, p4Team;
    int p1TeamPlayerNumber, p2TeamPlayerNumber, p3TeamPlayerNumber, p4TeamPlayerNumber;
    bool T1P1, T1P2, T1P3, T2P1, T2P2, T2P3, T3P1, T3P2, T3P3, T4P1, T4P2, T4P3;

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

        //Sets colors for the character select "buttons"
        p1Color = new Color32(103, 88, 255, 255);
        p1SelectedColor = new Color32(217, 214, 255, 255);

        p2Color = new Color32(255, 143, 0, 255);
        p2SelectedColor = new Color32(255, 220, 176, 255);

        p3Color = new Color32(0, 244, 255, 255);
        p3SelectedColor = new Color32(218, 252, 253, 255);

        p4Color = new Color32(249, 17, 17, 255);
        p4SelectedColor = new Color32(238, 172, 172, 255);

        p1Join = false;
        p2Join = false;
        p3Join = false;
        p4Join = false;

        p1Ready = false;
        p2Ready = false;
        p3Ready = false;
        p4Ready = false;

        p1AxisActive = false;
        p2AxisActive = false;
        p3AxisActive = false;
        p4AxisActive = false;

        p1Team = 1;
        p2Team = 2;
        p3Team = 3;
        p4Team = 4;

        p1TeamPlayerNumber = 11;
        p2TeamPlayerNumber = 21;
        p3TeamPlayerNumber = 31;
        p4TeamPlayerNumber = 41;

        T1P1 = true;
        T1P2 = false;
        T1P3 = false;
        T2P1 = true;
        T2P2 = false;
        T2P3 = false;
        T3P1 = true;
        T3P2 = false;
        T3P3 = false;
        T4P1 = true;
        T4P2 = false;
        T4P3 = false;

        startTextPanel.SetActive(false);

        buttonClickSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        PlayerLogIn(player1, playerId1);
        PlayerLogIn(player2, playerId2);
        PlayerLogIn(player3, playerId3);
        PlayerLogIn(player4, playerId4);

        //Hot Key to start game
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SceneManager.LoadScene("Level_Select");
        }
    }

    void PlayerLogIn(Rewired.Player player, int playerId)
    {
        //Advances the player through the menu
        if (player.GetButtonDown("A"))
        {
            buttonClickSound.PlayOneShot(buttonClick, 1f);

            //Debug.Log(playerId + " Advance");
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

        //Starts the game once players are ready
        if (CheckReadyPlayers() && player.GetButtonDown("Start"))
        {
            Debug.Log("Start Game with " + numberOfPlayers + " Players");

            if (numberOfPlayers == 1)
            {

                GameObject.Find("GameManager").GetComponent<ManagerScript>().SetPlayerAbleToSpawn("Player1", true);
                GameObject.Find("GameManager").GetComponent<ManagerScript>().SetPlayerAbleToSpawn("Player2", true);
                GameObject.Find("GameManager").GetComponent<ManagerScript>().SetPlayerAbleToSpawn("Player3", true);
                GameObject.Find("GameManager").GetComponent<ManagerScript>().SetPlayerAbleToSpawn("Player4", true);

                GameObject.Find("GameManager").GetComponent<ManagerScript>().SetPlayerClass("Player1", p1CharacterClass);
                GameObject.Find("GameManager").GetComponent<ManagerScript>().SetPlayerClass("Player2", p2CharacterClass);
                GameObject.Find("GameManager").GetComponent<ManagerScript>().SetPlayerClass("Player3", p3CharacterClass);
                GameObject.Find("GameManager").GetComponent<ManagerScript>().SetPlayerClass("Player4", p4CharacterClass);


            }
            else
            {
                GameObject.Find("GameManager").GetComponent<ManagerScript>().SetPlayerAbleToSpawn("Player1", p1Join);
                GameObject.Find("GameManager").GetComponent<ManagerScript>().SetPlayerAbleToSpawn("Player2", p2Join);
                GameObject.Find("GameManager").GetComponent<ManagerScript>().SetPlayerAbleToSpawn("Player3", p3Join);
                GameObject.Find("GameManager").GetComponent<ManagerScript>().SetPlayerAbleToSpawn("Player4", p4Join);

                GameObject.Find("GameManager").GetComponent<ManagerScript>().SetPlayerClass("Player1", p1CharacterClass);
                GameObject.Find("GameManager").GetComponent<ManagerScript>().SetPlayerClass("Player2", p2CharacterClass);
                GameObject.Find("GameManager").GetComponent<ManagerScript>().SetPlayerClass("Player3", p3CharacterClass);
                GameObject.Find("GameManager").GetComponent<ManagerScript>().SetPlayerClass("Player4", p4CharacterClass);

            }

            SceneManager.LoadScene("Level_Select");
        }

        //Returns "buttons" to their original color
        if (player.GetAxis("LSH") == 0)
        {
            if (playerId == 0)
            {
                //Debug.Log("PlayerID 0");
                p1AxisActive = false;

                p1CharacterLeftSelectButton.GetComponent<Graphic>().color = p1Color;
                p1CharacterRightSelectButton.GetComponent<Graphic>().color = p1Color;
            }

            if (playerId == 1)
            {
                //Debug.Log("PlayerID 1");
                p2AxisActive = false;

                p2CharacterLeftSelectButton.GetComponent<Graphic>().color = p2Color;
                p2CharacterRightSelectButton.GetComponent<Graphic>().color = p2Color;
            }

            if (playerId == 2)
            {
                //Debug.Log("PlayerID 2");
                p3AxisActive = false;

                p3CharacterLeftSelectButton.GetComponent<Graphic>().color = p3Color;
                p3CharacterRightSelectButton.GetComponent<Graphic>().color = p3Color;
            }

            if (playerId == 3)
            {
                //Debug.Log("PlayerID 3");
                p4AxisActive = false;

                p4CharacterLeftSelectButton.GetComponent<Graphic>().color = p4Color;
                p4CharacterRightSelectButton.GetComponent<Graphic>().color = p4Color;
            }
        }


        /*
        //Makes the axis like a button and only moved once
        if (player.GetAxis("LSV") != 0)
        {
            if (!axisActive)
            {
                MoveCursor();
                axisActive = true;
            }
        }
        */

        //character select right
        if (player.GetAxis("LSH") == -1 || player.GetAxis("LSH") == 1)
        {
            //Debug.Log(playerId + " Switch Character");
            switch (playerId)
            {
                //Player 1
                case 0:
                    //Debug.Log("State: " + p1State);
                    if (!p1AxisActive)
                    {
                        //Only let the player change their character in the character select state
                        if (p1State == 1)
                        {
                            //Changes the color of the correct button
                            if (player.GetAxis("LSH") == -1 && playerId == 0)
                            {
                                p1CharacterLeftSelectButton.GetComponent<Graphic>().color = p1SelectedColor;
                                buttonClickSound.PlayOneShot(buttonClick, 1f);
                            }
                            if (player.GetAxis("LSH") == 1 && playerId == 0)
                            {
                                p1CharacterRightSelectButton.GetComponent<Graphic>().color = p1SelectedColor;
                                buttonClickSound.PlayOneShot(buttonClick, 1f);
                            }

                            //Changes the player's character
                            if (p1CharacterClass == 0) //switching from the striker to the blocker
                            {
                                p1CharacterClass = 1;
                                p1StrikerCharacter.SetActive(false);
                                p1BlockerCharacter.SetActive(true);
                            }
                            else if (p1CharacterClass == 1) //switching from the blocker to the striker
                            {
                                p1CharacterClass = 0;
                                p1StrikerCharacter.SetActive(true);
                                p1BlockerCharacter.SetActive(false);
                            }
                        }
                        p1AxisActive = true;
                    }
                    break;

                //Player 2
                case 1:
                    if (!p2AxisActive)
                    {
                        //Only let the player change their character in the character select state
                        if (p2State == 1)
                        {
                            //Changes the color of the correct button
                            if (player.GetAxis("LSH") == -1 && playerId == 1)
                            {
                                p2CharacterLeftSelectButton.GetComponent<Graphic>().color = p2SelectedColor;
                                buttonClickSound.PlayOneShot(buttonClick, 1f);
                            }
                            if (player.GetAxis("LSH") == 1 && playerId == 1)
                            {
                                p2CharacterRightSelectButton.GetComponent<Graphic>().color = p2SelectedColor;
                                buttonClickSound.PlayOneShot(buttonClick, 1f);
                            }

                            //Changes the player's character
                            if (p2CharacterClass == 0) //switching from the striker to the blocker
                            {
                                p2CharacterClass = 1;
                                p2StrikerCharacter.SetActive(false);
                                p2BlockerCharacter.SetActive(true);
                            }
                            else if (p2CharacterClass == 1) //switching from the blocker to the striker
                            {
                                p2CharacterClass = 0;
                                p2StrikerCharacter.SetActive(true);
                                p2BlockerCharacter.SetActive(false);
                            }
                        }
                        p2AxisActive = true;
                    }
                    break;

                //Player 3
                case 2:
                    if (!p3AxisActive)
                    {
                        //Only let the player change their character in the character select state
                        if (p3State == 1)
                        {
                            //Changes the color of the correct button
                            if (player.GetAxis("LSH") == -1 && playerId == 2)
                            {
                                p3CharacterLeftSelectButton.GetComponent<Graphic>().color = p3SelectedColor;
                                buttonClickSound.PlayOneShot(buttonClick, 1f);
                            }
                            if (player.GetAxis("LSH") == 1 && playerId == 2)
                            {
                                p3CharacterRightSelectButton.GetComponent<Graphic>().color = p3SelectedColor;
                                buttonClickSound.PlayOneShot(buttonClick, 1f);
                            }

                            //Changes the player's character
                            if (p3CharacterClass == 0) //switching from the striker to the blocker
                            {
                                p3CharacterClass = 1;
                                p3StrikerCharacter.SetActive(false);
                                p3BlockerCharacter.SetActive(true);
                            }
                            else if (p1CharacterClass == 1) //switching from the blocker to the striker
                            {
                                p3CharacterClass = 0;
                                p3StrikerCharacter.SetActive(true);
                                p3BlockerCharacter.SetActive(false);
                            }
                        }
                        p3AxisActive = true;
                    }
                    break;

                //Player 4
                case 3:
                    if (!p4AxisActive)
                    {
                        //Only let the player change their character in the character select state
                        if (p4State == 1)
                        {
                            //Changes the color of the correct button
                            if (player.GetAxis("LSH") == -1 && playerId == 3)
                            {
                                p4CharacterLeftSelectButton.GetComponent<Graphic>().color = p4SelectedColor;
                                buttonClickSound.PlayOneShot(buttonClick, 1f);
                            }
                            if (player.GetAxis("LSH") == 1 && playerId == 3)
                            {
                                p4CharacterRightSelectButton.GetComponent<Graphic>().color = p4SelectedColor;
                                buttonClickSound.PlayOneShot(buttonClick, 1f);
                            }

                            //Changes the player's character
                            if (p4CharacterClass == 0) //switching from the striker to the blocker
                            {
                                p4CharacterClass = 1;
                                p4StrikerCharacter.SetActive(false);
                                p4BlockerCharacter.SetActive(true);
                            }
                            else if (p4CharacterClass == 1) //switching from the blocker to the striker
                            {
                                p4CharacterClass = 0;
                                p4StrikerCharacter.SetActive(true);
                                p4BlockerCharacter.SetActive(false);
                            }
                        }
                        p4AxisActive = true;
                    }
                    break;
            }
        }

        //Switch Player Teams
        if (player.GetButtonDown("RB"))
        {
            buttonClickSound.PlayOneShot(buttonClick, 1f);

            //Debug.Log(playerId + " Back");
            switch (playerId)
            {
                //Player 1
                case 0:
                    ResetTeamPlayers(p1TeamPlayerNumber);
                    p1Team++;
                    break;

                //Player 2
                case 1:
                    ResetTeamPlayers(p2TeamPlayerNumber);
                    p2Team++;
                    break;

                //Player 3
                case 2:
                    ResetTeamPlayers(p3TeamPlayerNumber);
                    p3Team++;
                    break;

                //Player 4
                case 3:
                    ResetTeamPlayers(p4TeamPlayerNumber);
                    p4Team++;
                    break;
            }
            Player1Team();
            Player2Team();
            Player3Team();
            Player4Team();

            Debug.Log("Player 1: Team  " + p1Team + " Number " + p1TeamPlayerNumber);
            Debug.Log("Player 2: Team  " + p2Team + " Number " + p2TeamPlayerNumber);
            Debug.Log("Player 3: Team  " + p3Team + " Number " + p3TeamPlayerNumber);
            Debug.Log("Player 4: Team  " + p4Team + " Number " + p4TeamPlayerNumber);
        }
        if (player.GetButtonDown("LB"))
        {
            buttonClickSound.PlayOneShot(buttonClick, 1f);

            //Debug.Log(playerId + " Back");
            switch (playerId)
            {
                //Player 1
                case 0:
                    ResetTeamPlayers(p1TeamPlayerNumber);
                    p1Team--;
                    break;

                //Player 2
                case 1:
                    ResetTeamPlayers(p2TeamPlayerNumber);
                    p2Team--;
                    break;

                //Player 3
                case 2:
                    ResetTeamPlayers(p3TeamPlayerNumber);
                    p3Team--;
                    break;

                //Player 4
                case 3:
                    ResetTeamPlayers(p4TeamPlayerNumber);
                    p4Team--;
                    break;
            }
            Player1Team();
            Player2Team();
            Player3Team();
            Player4Team();

            Debug.Log("Player 1: Team  " + p1Team + " Number " + p1TeamPlayerNumber);
            Debug.Log("Player 2: Team  " + p2Team + " Number " + p2TeamPlayerNumber);
            Debug.Log("Player 3: Team  " + p3Team + " Number " + p3TeamPlayerNumber);
            Debug.Log("Player 4: Team  " + p4Team + " Number " + p4TeamPlayerNumber);
        }
    }

    void Player1State()
    {
        switch (p1State)
        {
            //Sets a minimum for the states so that the player can advance immediately
            case -1:
                p1State = 0;
                SceneManager.LoadScene("Start_Menu");
                break;

            //Waiting to join state
            case 0:
                //Debug.Log("State: " + p1State);
                //show "Press A to Join"
                p1JoinText.SetActive(true);
                p1Panel.SetActive(false);
                p1Join = false;
                break;

            //Character select state
            case 1:
                //Debug.Log("State: " + p1State);
                //show character panel
                p1JoinText.SetActive(false);
                p1Panel.SetActive(true);
                p1ReadyText.SetActive(false);
                p1Join = true;
                p1Ready = false;
                break;

            //Ready state
            case 2:
                //Debug.Log("State: " + p1State);
                //show ready text and advance if all joined players are
                p1ReadyText.SetActive(true);
                numberOfPlayers = 1;
                p1Ready = true;
                GameObject.Find("GameManager").GetComponent<ManagerScript>().SetPlayerAmount(numberOfPlayers);
                break;

            //Sets a maximum for the states so that the player can regress immediately
            case 3:
                p1State = 2;
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

            //Waiting to join state
            case 0:
                //Debug.Log("State: " + p1State);
                //show "Press A to Join"
                p2JoinText.SetActive(true);
                p2Panel.SetActive(false);
                p2Join = false;
                break;

            //Character select state
            case 1:
                //Debug.Log("State: " + p1State);
                //show character panel
                p2JoinText.SetActive(false);
                p2Panel.SetActive(true);
                p2ReadyText.SetActive(false);
                p2Join = true;
                p2Ready = false;
                break;

            //Ready state
            case 2:
                //Debug.Log("State: " + p1State);
                //show ready text and advance if all joined players are
                p2ReadyText.SetActive(true);
                p2Ready = true;
                numberOfPlayers = 2;
                GameObject.Find("GameManager").GetComponent<ManagerScript>().SetPlayerAmount(numberOfPlayers);
                break;

            //Sets a maximum for the states so that the player can regress immediately
            case 3:
                p1State = 2;
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

            //Waiting to join state
            case 0:
                //Debug.Log("State: " + p1State);
                //show "Press A to Join"
                p3JoinText.SetActive(true);
                p3Panel.SetActive(false);
                p3Join = false;
                break;

            //Character select state
            case 1:
                //Debug.Log("State: " + p1State);
                //show character panel
                p3JoinText.SetActive(false);
                p3Panel.SetActive(true);
                p3ReadyText.SetActive(false);
                p3Join = true;
                p3Ready = false;
                break;

            //Ready state
            case 2:
                //Debug.Log("State: " + p1State);
                //show ready text and advance if all joined players are
                p3ReadyText.SetActive(true);
                p3Ready = true;
                numberOfPlayers = 3;
                GameObject.Find("GameManager").GetComponent<ManagerScript>().SetPlayerAmount(numberOfPlayers);
                break;

            //Sets a maximum for the states so that the player can regress immediately
            case 3:
                p3State = 2;
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

            //Waiting to join state
            case 0:
                //Debug.Log("State: " + p1State);
                //show "Press A to Join"
                p4JoinText.SetActive(true);
                p4Panel.SetActive(false);
                p4Join = false;
                break;

            //Character select state
            case 1:
                //Debug.Log("State: " + p1State);
                //show character panel
                p4JoinText.SetActive(false);
                p4Panel.SetActive(true);
                p4ReadyText.SetActive(false);
                p4Join = true;
                p4Ready = false;
                break;

            //Ready state
            case 2:
                //Debug.Log("State: " + p1State);
                //show ready text and advance if all joined players are
                p4ReadyText.SetActive(true);
                p4Ready = true;
                numberOfPlayers = 4;
                GameObject.Find("GameManager").GetComponent<ManagerScript>().SetPlayerAmount(numberOfPlayers);
                break;

            //Sets a maximum for the states so that the player can regress immediately
            case 3:
                p4State = 2;
                break;
        }
    }

    void Player1Team()
    {
        switch (p1Team)
        {
            //sets team to 4 to cycle through the teams
            case 0:
                p1Team = 4;
                p1TeamPlayerNumber = CheckTeamPlayers(p1Team); //returns which player they are on the team to give them a unique color
                break;

            case 1:
                p1Team = 1;
                p1TeamPlayerNumber = CheckTeamPlayers(p1Team); //returns which player they are on the team to give them a unique color
                break;

            case 2:
                p1Team = 2;
                p1TeamPlayerNumber = CheckTeamPlayers(p1Team); //returns which player they are on the team to give them a unique color
                break;

            case 3:
                p1Team = 3;
                p1TeamPlayerNumber = CheckTeamPlayers(p1Team); //returns which player they are on the team to give them a unique color
                break;

            case 4:
                p1Team = 4;
                p1TeamPlayerNumber = CheckTeamPlayers(p1Team); //returns which player they are on the team to give them a unique color
                break;

            //sets teams to 1 to cylce through the teams
            case 5:
                p1Team = 1;
                p1TeamPlayerNumber = CheckTeamPlayers(p1Team); //returns which player they are on the team to give them a unique color
                break;
        }
    }

    void Player2Team()
    {
        switch (p2Team)
        {
            //sets team to 4 to cycle through the teams
            case 0:
                p2Team = 4;
                p2TeamPlayerNumber = CheckTeamPlayers(p2Team); //returns which player they are on the team to give them a unique color
                break;

            case 1:
                p2Team = 1;
                p2TeamPlayerNumber = CheckTeamPlayers(p2Team); //returns which player they are on the team to give them a unique color
                break;

            case 2:
                p2Team = 2;
                p2TeamPlayerNumber = CheckTeamPlayers(p2Team); //returns which player they are on the team to give them a unique color
                break;

            case 3:
                p2Team = 3;
                p2TeamPlayerNumber = CheckTeamPlayers(p2Team); //returns which player they are on the team to give them a unique color
                break;

            case 4:
                p2Team = 4;
                p2TeamPlayerNumber = CheckTeamPlayers(p2Team); //returns which player they are on the team to give them a unique color
                break;

            //sets teams to 1 to cylce through the teams
            case 5:
                p2Team = 1;
                p2TeamPlayerNumber = CheckTeamPlayers(p2Team); //returns which player they are on the team to give them a unique color
                break;
        }
    }

    void Player3Team()
    {
        switch (p2Team)
        {
            //sets team to 4 to cycle through the teams
            case 0:
                p3Team = 4;
                p3TeamPlayerNumber = CheckTeamPlayers(p3Team); //returns which player they are on the team to give them a unique color
                break;

            case 1:
                p3Team = 1;
                p3TeamPlayerNumber = CheckTeamPlayers(p3Team); //returns which player they are on the team to give them a unique color
                break;

            case 2:
                p3Team = 2;
                p3TeamPlayerNumber = CheckTeamPlayers(p3Team); //returns which player they are on the team to give them a unique color
                break;

            case 3:
                p3Team = 3;
                p3TeamPlayerNumber = CheckTeamPlayers(p3Team); //returns which player they are on the team to give them a unique color
                break;

            case 4:
                p3Team = 4;
                p3TeamPlayerNumber = CheckTeamPlayers(p3Team); //returns which player they are on the team to give them a unique color
                break;

            //sets teams to 1 to cylce through the teams
            case 5:
                p3Team = 1;
                p3TeamPlayerNumber = CheckTeamPlayers(p3Team); //returns which player they are on the team to give them a unique color
                break;
        }
    }

    void Player4Team()
    {
        switch (p2Team)
        {
            //sets team to 4 to cycle through the teams
            case 0:
                p4Team = 4;
                p4TeamPlayerNumber = CheckTeamPlayers(p4Team); //returns which player they are on the team to give them a unique color
                break;

            case 1:
                p4Team = 1;
                p4TeamPlayerNumber = CheckTeamPlayers(p4Team); //returns which player they are on the team to give them a unique color
                break;

            case 2:
                p4Team = 2;
                p4TeamPlayerNumber = CheckTeamPlayers(p4Team); //returns which player they are on the team to give them a unique color
                break;

            case 3:
                p4Team = 3;
                p4TeamPlayerNumber = CheckTeamPlayers(p4Team); //returns which player they are on the team to give them a unique color
                break;

            case 4:
                p4Team = 4;
                p4TeamPlayerNumber = CheckTeamPlayers(p4Team); //returns which player they are on the team to give them a unique color
                break;

            //sets teams to 1 to cylce through the teams
            case 5:
                p4Team = 1;
                p4TeamPlayerNumber = CheckTeamPlayers(p4Team); //returns which player they are on the team to give them a unique color
                break;
        }
    }

    bool IsSomeonePlaying()
    {
        if (p1Join || p2Join || p3Join || p4Join)
        {
            return true;
        }

        return false;
    }

    bool CheckReadyPlayers()
    {
        //if numplayersready == numplayers, then show "Press start to play"
        if (p1Join || p2Join || p3Join || p4Join)
        {
            if (p1Join == p1Ready && p2Join == p2Ready && p3Join == p3Ready && p4Join == p4Ready)
            {
                startTextPanel.SetActive(true);
                return true;
            }

            else
            {
                startTextPanel.SetActive(false);
                return false;
            }
        }

        else
        {
            startTextPanel.SetActive(false);
            return false;
        }
    }

    int CheckTeamPlayers(int playerTeam)
    {
        if (playerTeam == 1)
        {
            if (!T1P1)
            {
                T1P1 = true;
                return 11;
            }
            else if (!T1P2)
            {
                T1P2 = true;
                return 12;
            }
            else if (!T1P3)
            {
                T1P3 = true;
                return 13;
            }
            else
                return 0;
        }

        else if (playerTeam == 2)
        {
            if (!T2P1)
            {
                T2P1 = true;
                return 21;
            }
            else if (!T2P2)
            {
                T2P2 = true;
                return 22;
            }
            else if (!T2P3)
            {
                T2P3 = true;
                return 23;
            }
            else
                return 0;
        }

        else if (playerTeam == 3)
        {
            if (!T3P1)
            {
                T3P1 = true;
                return 31;
            }
            else if (!T3P2)
            {
                T3P2 = true;
                return 32;
            }
            else if (!T3P3)
            {
                T3P3 = true;
                return 33;
            }
            else
                return 0;
        }

        else if (playerTeam == 4)
        {
            if (!T4P1)
            {
                T4P1 = true;
                return 41;
            }
            else if (!T4P2)
            {
                T4P2 = true;
                return 42;
            }
            else if (!T4P3)
            {
                T4P3 = true;
                return 43;
            }
            else
                return 0;
        }

        else
            return 0;
    }

    //Called after a player switches teams to keep track of active teams
    void ResetTeamPlayers(int playerTeamNumber)
    {
        switch (playerTeamNumber)
        {
            case 11:
                T1P1 = false;
                break;
            case 12:
                T1P2 = false;
                break;
            case 13:
                T1P3 = false;
                break;
            case 21:
                T2P1 = false;
                break;
            case 22:
                T2P2 = false;
                break;
            case 23:
                T2P3 = false;
                break;
            case 31:
                T3P1 = false;
                break;
            case 32:
                T3P2 = false;
                break;
            case 33:
                T3P3 = false;
                break;
            case 41:
                T4P1 = false;
                break;
            case 42:
                T4P2 = false;
                break;
            case 43:
                T4P3 = false;
                break;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class GameOptionsManager : MonoBehaviour
{
    public int playerId;
    private Rewired.Player player;

    /*
    public int playerId1 = 0;
    public int playerId2 = 1;
    public int playerId3 = 2;
    public int playerId4 = 3;

    private Rewired.Player player1;
    private Rewired.Player player2;
    private Rewired.Player player3;
    private Rewired.Player player4;
    */

    int menuState;

    public static int numberOfRounds = 1;
    public Text numberOfRoundsText;

    public Graphic roundRightArrow, roundLeftArrow;
    Color32 roundArrowNormalColor, roundArrowHighlightedColor, roundArrowClickedColor;

    int currentLevel;
    string currentLevelName;

    public Graphic level1, level2, level3;
    Color32 unselectedLevelColor, selectedLevelColor;

    bool p1AxisActive;

    public AudioClip buttonClick;
    AudioSource buttonClickSound;

    void Start()
    {
        player = ReInput.players.GetPlayer(playerId);

        menuState = 0;

        roundArrowNormalColor = new Color32(169, 169, 169, 255);
        roundArrowHighlightedColor = new Color32(255, 255, 255, 255);
        roundArrowClickedColor = new Color32(128, 128, 128, 255);

        currentLevel = 1;
        currentLevelName = "Level1";

        unselectedLevelColor = new Color32(118, 118, 118, 255);
        selectedLevelColor = new Color32(255, 255, 255, 255);

        p1AxisActive = false;

        buttonClickSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        //Things to check every loop
        numberOfRoundsText.text = numberOfRounds.ToString();
        GameObject.Find("GameManager").GetComponent<ManagerScript>().SetMaxRounds(numberOfRounds);
        //Debug.Log(currentLevel + " " + currentLevelName);

        //Advances the player through the menu
        if (player.GetButtonDown("A"))
        {
            buttonClickSound.PlayOneShot(buttonClick, 1f);
            menuState++;
            MenuState();
        }

        //Regress the player through the menu
        if (player.GetButtonDown("B"))
        {
            buttonClickSound.PlayOneShot(buttonClick, 1f);
            menuState--;
            MenuState();
        }

        //Returns "buttons" to their original color
        if (player.GetAxis("LSH") == 0)
        {
            p1AxisActive = false;

            roundRightArrow.GetComponent<Graphic>().color = roundArrowNormalColor;
            roundLeftArrow.GetComponent<Graphic>().color = roundArrowNormalColor;

            if (menuState == 1)
            {
                switch(currentLevel)
                {
                    case 1:
                        level2.GetComponent<Graphic>().color = unselectedLevelColor;
                        level3.GetComponent<Graphic>().color = unselectedLevelColor;
                        break;

                    case 2:
                        level1.GetComponent<Graphic>().color = unselectedLevelColor;
                        level3.GetComponent<Graphic>().color = unselectedLevelColor;
                        break;

                    case 3:
                        level1.GetComponent<Graphic>().color = unselectedLevelColor;
                        level2.GetComponent<Graphic>().color = unselectedLevelColor;
                        break;
                }
            }
        }

        //Selects next button
        if (player.GetAxis("LSH") == -1 || player.GetAxis("LSH") == 1)
        {
            if (!p1AxisActive)
            {
                //Only let the player change the round number
                if (menuState == 0)
                {
                    //Changes the color of the correct button
                    if (player.GetAxis("LSH") == -1)// && playerId == 0)
                    {
                        LessRounds();
                        roundLeftArrow.GetComponent<Graphic>().color = roundArrowHighlightedColor;
                    }
                    if (player.GetAxis("LSH") == 1)// && playerId == 0)
                    {
                        MoreRounds();
                        roundRightArrow.GetComponent<Graphic>().color = roundArrowHighlightedColor;
                    }
                }

                //Only let the player change the level
                if (menuState == 1)
                {
                    //Changes the color of the correct level
                    if (player.GetAxis("LSH") == -1)// && playerId == 0)
                    {
                        currentLevel--;
                        SetLevel();
                    }
                    if (player.GetAxis("LSH") == 1)// && playerId == 0)
                    {
                        currentLevel++;
                        SetLevel();
                    }
                }

                p1AxisActive = true;
            }
        }

        /*
        PlayerLogIn(player1, playerId1);
        PlayerLogIn(player2, playerId2);
        PlayerLogIn(player3, playerId3);
        PlayerLogIn(player4, playerId4);
        */
    }

    //Not being used
    void PlayerLogIn(Rewired.Player player, int playerId)
    {
        //Advances the player through the menu
        if (player.GetButtonDown("A"))
        {
            buttonClickSound.PlayOneShot(buttonClick, 1f);
            menuState++;
            MenuState();
        }

        //Regress the player through the menu
        if (player.GetButtonDown("B"))
        {
            buttonClickSound.PlayOneShot(buttonClick, 1f);
            menuState--;
            MenuState();
        }

        //Returns "buttons" to their original color
        if (player.GetAxis("LSH") == 0)
        {
            p1AxisActive = false;

            roundRightArrow.GetComponent<Graphic>().color = roundArrowNormalColor;
            roundLeftArrow.GetComponent<Graphic>().color = roundArrowNormalColor;
        }

        //Selects next button
        if (player.GetAxis("LSH") == -1 || player.GetAxis("LSH") == 1)
        {
            if (!p1AxisActive)
            {
                //Only let the player change the round number
                if (menuState == 0)
                {
                    //Changes the color of the correct button
                    if (player.GetAxis("LSH") == -1)// && playerId == 0)
                    {
                        roundLeftArrow.GetComponent<Graphic>().color = roundArrowHighlightedColor;
                        buttonClickSound.PlayOneShot(buttonClick, 1f);
                    }
                    if (player.GetAxis("LSH") == 1)// && playerId == 0)
                    {
                        roundRightArrow.GetComponent<Graphic>().color = roundArrowHighlightedColor;
                        buttonClickSound.PlayOneShot(buttonClick, 1f);
                    }
                }

                //Only let the player change the level
                if (menuState == 1)
                {

                }

                p1AxisActive = true;
            }
        }
    }

    void MenuState()
    {
        switch (menuState)
        {
            //Sets a minimum for the states so that the player can advance immediately
            case -1:
                menuState = 0;
                SceneManager.LoadScene("Player_Login_Screen");
                break;

            //Round Selection
            case 0:
                Debug.Log("Round Selection");
                level1.GetComponent<Graphic>().color = unselectedLevelColor;
                level2.GetComponent<Graphic>().color = unselectedLevelColor;
                level3.GetComponent<Graphic>().color = unselectedLevelColor;
                break;

            //Level Selection
            case 1:
                //Debug.Log("Level Selection");
                currentLevel = 1;
                currentLevelName = "Level1";
                level1.GetComponent<Graphic>().color = selectedLevelColor;
                break;

            //Player selected level and continues
            case 2:
                SceneManager.LoadScene(currentLevelName);
                break;
        }
    }

    public void MoreRounds()
    {
        buttonClickSound.PlayOneShot(buttonClick, 1f);

        //Odd numbers only
        numberOfRounds++;
        numberOfRounds++;

        if (numberOfRounds > 15)
        {
            numberOfRounds = 1;
        }

        GameObject.Find("GameManager").GetComponent<ManagerScript>().SetMaxRounds(numberOfRounds);
    }

    public void LessRounds()
    {
        buttonClickSound.PlayOneShot(buttonClick, 1f);

        //Odd numbers only
        numberOfRounds--;
        numberOfRounds--;

        if (numberOfRounds < 1)
        {
            numberOfRounds = 15;
        }

        GameObject.Find("GameManager").GetComponent<ManagerScript>().SetMaxRounds(numberOfRounds);
    }

    public void SetLevel()
    {
        buttonClickSound.PlayOneShot(buttonClick, 1f);

        switch(currentLevel)
        {
            case 0:
                currentLevel = 1;
                currentLevelName = "Level1";
                level1.GetComponent<Graphic>().color = selectedLevelColor;
                level2.GetComponent<Graphic>().color = unselectedLevelColor;
                level3.GetComponent<Graphic>().color = unselectedLevelColor;
                break;

            case 1:
                currentLevel = 1;
                currentLevelName = "Level1";
                level1.GetComponent<Graphic>().color = selectedLevelColor;
                level2.GetComponent<Graphic>().color = unselectedLevelColor;
                level3.GetComponent<Graphic>().color = unselectedLevelColor;
                break;

            case 2:
                currentLevel = 2;
                currentLevelName = "Test_Level_2";
                level2.GetComponent<Graphic>().color = selectedLevelColor;
                level1.GetComponent<Graphic>().color = unselectedLevelColor;
                level3.GetComponent<Graphic>().color = unselectedLevelColor;
                break;

            case 3:
                currentLevel = 3;
                currentLevelName = "Test_Level_3";
                level3.GetComponent<Graphic>().color = selectedLevelColor;
                level1.GetComponent<Graphic>().color = unselectedLevelColor;
                level2.GetComponent<Graphic>().color = unselectedLevelColor;
                break;

            case 4:
                currentLevel = 3;
                currentLevelName = "Test_Level_3";
                level3.GetComponent<Graphic>().color = selectedLevelColor;
                level1.GetComponent<Graphic>().color = unselectedLevelColor;
                level2.GetComponent<Graphic>().color = unselectedLevelColor;
                break;
        }
    }
}
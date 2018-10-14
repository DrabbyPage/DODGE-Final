using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndSceneUIData : MonoBehaviour {

    int player1KillNum;
    int player2KillNum;
    int player3KillNum;
    int player4KillNum;

    int player1DeadNum;
    int player2DeadNum;
    int player3DeadNum;
    int player4DeadNum;

    public Text player1StatText;
    public Text player2StatText;
    public Text player3StatText;
    public Text player4StatText;
    public Text roundEndText;


    // Use this for initialization
    void Start()
    {
        player1KillNum = 0;
        player2KillNum = 0;
        player3KillNum = 0;
        player4KillNum = 0;

        player1DeadNum = 0;
        player2DeadNum = 0;
        player3DeadNum = 0;
        player4DeadNum = 0;


    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("End_Canvas"))
            UpdateTexts();

    }

    void UpdateTexts()
    {
        player1StatText.text = "Kills: " + player1KillNum.ToString() + " \n" +
                               "Deaths: " + player1DeadNum.ToString();
        player2StatText.text = "Kills: " + player2KillNum.ToString() + " \n" +
                               "Deaths: " + player2DeadNum.ToString();
        player3StatText.text = "Kills: " + player3KillNum.ToString() + " \n" +
                               "Deaths: " + player3DeadNum.ToString();
        player4StatText.text = "Kills: " + player4KillNum.ToString() + " \n" +
                               "Deaths: " + player4DeadNum.ToString();

    }

    void CheckPlayer()
    {
        if (SceneManager.GetActiveScene().name == "Level1" || SceneManager.GetActiveScene().name == "Test_Level_2"
            || SceneManager.GetActiveScene().name == "Test_Level_3" || SceneManager.GetActiveScene().name == "Test_Level_4")
        {
           
        }
    }

    public void SetKills(int p1, int p2, int p3, int p4)
    {
        player1KillNum = p1;
        player2KillNum = p2;
        player3KillNum = p3;
        player4KillNum = p4;
    }

    public void SetDeaths(int p1, int p2, int p3, int p4)
    {
        player1DeadNum = p1;
        player2DeadNum = p2;
        player3DeadNum = p3;
        player4DeadNum = p4;
    }


    public void SetWinningText(string text)
    {
        roundEndText.text = text;
    }
}

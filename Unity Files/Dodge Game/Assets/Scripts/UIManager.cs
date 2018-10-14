using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    int redTeamScore;
    int blueTeamScore;
    int roundNum;

    public Text redTeamScoreText;
    public Text blueTeamScoreText;
    public Text roundNumText;
    public Text roundEndText;

    // Use this for initialization
    void Start () {
        DisableRoundEndText();
        roundNum = 1;
        redTeamScore = 0;
        blueTeamScore = 0;
	}
	
	// Update is called once per frame
	void Update () {
        CheckUI();
	}

    void UpdateTexts()
    {
        redTeamScoreText.text = redTeamScore.ToString();
        blueTeamScoreText.text = blueTeamScore.ToString();
        roundNumText.text = roundNum.ToString();
    }

    void CheckUI()
    {
        if (SceneManager.GetActiveScene().name == "Level1" || SceneManager.GetActiveScene().name == "Test_Level_2"
            || SceneManager.GetActiveScene().name == "Test_Level_3" || SceneManager.GetActiveScene().name == "Test_Level_4")
        {
            redTeamScoreText = GameObject.Find("RedTeamScore").GetComponent<Text>();
            blueTeamScoreText = GameObject.Find("BlueTeamScore").GetComponent<Text>();
            roundNumText = GameObject.Find("RoundNum").GetComponent<Text>();
            roundEndText = GameObject.Find("RoundWinText").GetComponent<Text>();
            UpdateTexts();
        }
    }

    public void SetRedTeamNum(int newRedNum)
    {
        redTeamScore = newRedNum;
    }

    public void SetBlueTeamNum(int newBlueNum)
    {
        blueTeamScore = newBlueNum;
    }

    public void SetRoundNum(int newRoundNum)
    {
        roundNum = newRoundNum;
    }

    public void EnableRoundEndText(string newText)
    {
        roundEndText.text = newText;
    }

    public void DisableRoundEndText()
    {
        if (roundEndText != null)
            roundEndText.text = "";
    }
}
